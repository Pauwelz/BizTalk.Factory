//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Be.Stateless.BizTalk.Orchestrations.Dummy
{
	using System;
	using System.CodeDom.Compiler;
	using Be.Stateless.BizTalk.Dsl.Binding;
	using Be.Stateless.BizTalk.Orchestrations.Dummy;
	
	
	[GeneratedCodeAttribute("Be.Stateless.BizTalk.Dsl", "1.0.0.0")]
	internal partial class ProcessOrchestrationBinding : OrchestrationBindingBase<Process>
	{
		
		public ProcessOrchestrationBinding()
		{
		}
		
		public ProcessOrchestrationBinding(Action<ProcessOrchestrationBinding> orchestrationBindingConfigurator)
		{
			orchestrationBindingConfigurator(this);
			((Be.Stateless.BizTalk.Dsl.Binding.ISupportValidation)(this)).Validate();
		}
		
		public ISendPort SendPort { get; set; } //;
		
		public IReceivePort ReceivePort { get; set; } //;
		
		public IReceivePort RequestResponsePort { get; set; } //;
		
		public ISendPort SolicitResponsePort { get; set; } //;
	}
}
