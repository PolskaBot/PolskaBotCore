using System;
using System.Collections;
using System.Text;

namespace Flash.External
{
	/// <summary>
	/// Value object containing information about an ExternalInterface call
	/// sent between a .NET application and a Shockwave Flash object.
	/// </summary>
	public class ExternalInterfaceCall
	{
		#region Private Fields

		private string _functionName;
		private ArrayList _arguments;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new ExternalInterfaceCall instance with the specified 
		/// function name.
		/// </summary>
		/// <param name="functionName">The name of the function as provided
		/// by Flash Player</param>
		public ExternalInterfaceCall(string functionName)
		{
			_functionName = functionName;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// The name of the function call provided by Flash Player
		/// </summary>
		public string FunctionName
		{
			get { return _functionName; }
		}

		/// <summary>
		/// The function parameters associated with this function call.
		/// </summary>
		public object[] Arguments
		{
			get { return (object[])_arguments.ToArray(typeof(object)); }
		}

		#endregion

		#region Public Methods

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			result.AppendFormat("Function Name: {0}{1}", _functionName, Environment.NewLine);
			if (_arguments != null && _arguments.Count > 0)
			{
				result.AppendFormat("Arguments:{0}", Environment.NewLine);
				foreach (object arg in _arguments)
				{
					result.AppendFormat("\t{0}{1}", arg, Environment.NewLine);
				}
			}
			return result.ToString();
		}

		#endregion

		#region Internal Methods

		internal void AddArgument(object argument)
		{
			if (_arguments == null)
			{
				_arguments = new ArrayList();
			}
			_arguments.Add(argument);
		}

		#endregion
	}
}