#region Copyright & License

# Copyright © 2012 - 2017 François Chabot
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
# http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

#endregion

function Get-QuartzAgentInstallationPath
{
    [CmdletBinding()]
    Param()

    if ($script:quartzAgentInstallationPath -eq $null) {
        $path = GetQuartzAgentExecutableAbsolutePath
        $script:quartzAgentInstallationPath = Split-Path -Path $path -Parent
    }
    $script:quartzAgentInstallationPath
}

function Get-QuartzScheduler
{
    [CmdletBinding()]
    Param()

    if ($script:scheduler -eq $null) {
        Write-Verbose -Message 'Loading Quartz scheduler settings from service host''s config file.'
        $configuration = [System.Configuration.ConfigurationManager]::OpenExeConfiguration((GetQuartzAgentExecutableAbsolutePath))
        $section = $configuration.GetSection("quartz")
        # convert AppSettings into a NameValueCollection
        $settings = New-Object -TypeName System.Collections.Specialized.NameValueCollection -ArgumentList $section.Settings.Count
        # filter out irrelevant/unnecessary settings, whose some would entail scheduler's initialization errors when performed from within PowerShell
        $section.Settings `
            | Where-Object { $_.Key -notmatch 'quartz\.plugin\.xml\.' } `
            | ForEach-Object { $settings[$_.Key] = $_.Value }

        Write-Verbose -Message 'Constructing Quartz scheduler instance.'
        $schedulerFactory = New-Object -TypeName Quartz.Impl.StdSchedulerFactory
        $schedulerFactory.Initialize($settings)
        $script:scheduler = $schedulerFactory.GetScheduler()
    }
    $script:scheduler
}

function Get-QuartzJobs
{
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$false)]
        [switch]
        $Detailed
    )

    $jobKeys = GetQuartzJobKeys
    if ($Detailed) {
        $jobKeys | ForEach-Object { $scheduler.GetJobDetail($_) }
    }
    else {
        $jobKeys
    }
}

function Get-QuartzJob
{
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true,ParameterSetName="vector",Position=0,ValueFromPipeline=$true)]
        [Quartz.JobKey]
        $JobKey,

        [Parameter(Mandatory=$true,ParameterSetName="scalar",Position=0,ValueFromPipeline=$false)]
        [string]
        $Name,

        [Parameter(Mandatory=$false,ParameterSetName="scalar",Position=1,ValueFromPipeline=$false)]
        [string]
        $Group = $null,

        [Parameter(Mandatory=$false,ParameterSetName="vector")]
        [Parameter(Mandatory=$false,ParameterSetName="scalar")]
        [switch]
        $Detailed
    )

    process {
        if ($PsCmdlet.ParameterSetName -eq 'scalar') {
            $JobKey = New-Object -TypeName Quartz.JobKey -ArgumentList $Name, $Group
        }
        $scheduler = Get-QuartzScheduler
        if ($scheduler.CheckExists($JobKey)) {
            if ($Detailed) {
                $scheduler.GetJobDetail($JobKey)
            }
            else {
                $JobKey
            }
        }
        else {
            Write-Host ('''{0}'' job not found.' -f (GetQuartzJobDisplayName $JobKey))
        }
    }
}

function Remove-QuartzJobs
{
    [CmdletBinding(SupportsShouldProcess=$true)]
    Param()

    GetQuartzJobKeys | Remove-QuartzJob
}

function Remove-QuartzJob
{
    [CmdletBinding(SupportsShouldProcess=$true)]
    Param(
        [Parameter(Mandatory=$true,ParameterSetName="vector",Position=0,ValueFromPipeline=$true)]
        [Quartz.JobKey]
        $JobKey,
    
        [Parameter(Mandatory=$true,ParameterSetName="scalar",Position=0,ValueFromPipeline=$false)]
        [string]
        $Name,

        [Parameter(Mandatory=$false,ParameterSetName="scalar",Position=1,ValueFromPipeline=$false)]
        [string]
        $Group = $null
    )

    process {
        $scheduler = Get-QuartzScheduler
        if ($scheduler.CheckExists($JobKey) -and $PsCmdlet.ShouldProcess((GetQuartzJobDisplayName $JobKey), "Delete Job")) {
            if ($scheduler.DeleteJob($_)) {
                Write-Verbose -Message ('''{0}'' job has been deleted.' -f (GetQuartzJobDisplayName $JobKey))
            } else {
                Write-Error -Message ('''{0}'' job could not be deleted.' -f (GetQuartzJobDisplayName $JobKey))
            }
        }
    }
}

<#
 # Helpers
 #>
function AssertQuartzAgent
{
    [CmdletBinding()]
    param()

    $agent = Get-CimInstance -ClassName Win32_Service -Filter "Name='QuartzAgent'"
    if ($agent -eq $null) {
        throw "Quart.NET Scheduler Agent is not available on this machine."
    }
}

function GetQuartzAgentExecutableAbsolutePath
{
    [CmdletBinding()]
    Param()

    if ($script:quartzAgentExecutableAbsolutePath -eq $null) {
        $path = Get-CimInstance -ClassName Win32_Service -Filter "Name='QuartzAgent'" | Select-Object -ExpandProperty PathName
        $script:quartzAgentExecutableAbsolutePath = $path.Trim('"')
    }
    $script:quartzAgentExecutableAbsolutePath
}

function GetQuartzJobDisplayName
{
    [CmdletBinding()]
    Param(
        [Quartz.JobKey]
        $JobKey
    )

    if ($JobKey.Group.Length -gt 0) {
        ('[{0}:{1}]' -f $JobKey.Group, $JobKey.Name)
    } else {
        ('[{0}]' -f $JobKey.Name)
    }
}

function GetQuartzJobKeys
{
    [CmdletBinding()]
    Param()
    
    $scheduler = Get-QuartzScheduler
	# https://stackoverflow.com/questions/11975130/generic-type-of-generic-type-in-powershell
    $groupMatcher = [Type]('Quartz.Impl.Matchers.GroupMatcher[[{0}]]' -f [Quartz.JobKey].fullname)
    $anyGroupMatcher = $groupMatcher::AnyGroup()
    $scheduler.GetJobKeys($anyGroupMatcher)
}

function LoadQuartzDependentAssemblies
{
    [CmdletBinding()]
    param()

    # https://stackoverflow.com/questions/7997725/the-type-initializer-for-quartz-impl-stdschedulerfactory-threw-an-exception
    # the list is in reversed depency order
    $assemblyNameList = @('log4net.dll', 'Common.Logging.Core.dll', 'Common.Logging.dll', 'Quartz.dll', 'Be.Stateless.Quartz.dll')
    foreach($assemblyName in $assemblyNameList) {
        Add-Type -Path (Join-Path -Path (Get-QuartzAgentInstallationPath) -ChildPath $assemblyName)
    }
}

<#
 # Main
 #>

# register clean up handler should the module be removed from the session
$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
    $script:scheduler = $null
}

$quartzAgentExecutableAbsolutePath = $null
$quartzAgentInstallationPath = $null
$scheduler = $null

AssertQuartzAgent
LoadQuartzDependentAssemblies

Export-ModuleMember -Alias * -Function '*-*'
