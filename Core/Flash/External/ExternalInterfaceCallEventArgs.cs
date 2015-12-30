using System;

namespace Flash.External
{
	public delegate object ExternalInterfaceCallEventHandler(object sender, ExternalInterfaceCallEventArgs e);

	/// <summary>
	/// Event arguments for the ExternalInterfaceCallEventHandler.
	/// </summary>
	public class ExternalInterfaceCallEventArgs : System.EventArgs
	{
		private ExternalInterfaceCall _functionCall;

		public ExternalInterfaceCallEventArgs(ExternalInterfaceCall functionCall)
			: base()
		{
			_functionCall = functionCall;
		}

		public ExternalInterfaceCall FunctionCall
		{
			get { return _functionCall; }
		}
	}
}