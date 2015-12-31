using System;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

namespace Flash.External
{
	/// <summary>
	/// Provides methods to convert the XML communication format to .NET classes. Typically
	/// this class will not be used directly; it supports the operations of the
	/// ExternalInterfaceProxy class.
	/// </summary>
	public class ExternalInterfaceSerializer
	{
		#region Constructor

		private ExternalInterfaceSerializer()
		{
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Encodes a function call to be sent to Flash.
		/// </summary>
		/// <param name="functionName">The name of the function to call.</param>
		/// <param name="arguments">Zero or more parameters to pass in to the ActonScript function.</param>
		/// <returns>The XML string representation of the function call to pass to Flash</returns>
		public static string EncodeInvoke(string functionName, object[] arguments)
		{
			StringBuilder sb = new StringBuilder();

			XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));

			// <invoke name="functionName" returntype="xml">
			writer.WriteStartElement("invoke");
			writer.WriteAttributeString("name", functionName);
			writer.WriteAttributeString("returntype", "xml");

			if (arguments != null && arguments.Length > 0)
			{
				// <arguments>
				writer.WriteStartElement("arguments");

				// individual arguments
				foreach (object value in arguments)
				{
					WriteElement(writer, value);
				}

				// </arguments>
				writer.WriteEndElement();
			}

			// </invoke>
			writer.WriteEndElement();

			writer.Flush();
			writer.Close();

			return sb.ToString();
		}

		/// <summary>
		/// Encodes a value to send to Flash as the result of a function call from Flash.
		/// </summary>
		/// <param name="value">The value to encode.</param>
		/// <returns>The XML string representation of the value.</returns>
		public static string EncodeResult(object value)
		{
			StringBuilder sb = new StringBuilder();

			XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));

			WriteElement(writer, value);

			writer.Flush();
			writer.Close();

			return sb.ToString();
		}


		/// <summary>
		/// Decodes a function call from Flash.
		/// </summary>
		/// <param name="xml">The XML string representing the function call.</param>
		/// <returns>An ExternalInterfaceCall object representing the function call.</returns>
		public static ExternalInterfaceCall DecodeInvoke(string xml)
		{
			XmlTextReader reader = new XmlTextReader(xml, XmlNodeType.Document, null);

			reader.Read();

			string functionName = reader.GetAttribute("name");
			ExternalInterfaceCall result = new ExternalInterfaceCall(functionName);

			reader.ReadStartElement("invoke");   
			reader.ReadStartElement("arguments");

			while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "arguments")
			{
				result.AddArgument(ReadElement(reader));
			}

			reader.ReadEndElement();
			reader.ReadEndElement();

			return result;
		}


		/// <summary>
		/// Decodes the result of a function call to Flash
		/// </summary>
		/// <param name="xml">The XML string representing the result.</param>
		/// <returns>A <see cref="System.Object"/> containing the result</returns>
		public static object DecodeResult(string xml)
		{
			XmlTextReader reader = new XmlTextReader(xml, XmlNodeType.Document, null);
			reader.Read();
			return ReadElement(reader);
		}

		#endregion

		#region Writers

		private static void WriteElement(XmlTextWriter writer, object value)
		{
			if (value == null)
			{
				writer.WriteStartElement("null");
				writer.WriteEndElement();
			}
			else if (value is string)
			{
				writer.WriteStartElement("string");
				writer.WriteString(value.ToString());
				writer.WriteEndElement();
			}
			else if (value is bool)
			{
				writer.WriteStartElement((bool)value ? "true" : "false");
				writer.WriteEndElement();
			}
			else if (value is Single || value is Double || value is int || value is uint)
			{
				writer.WriteStartElement("number");
				writer.WriteString(value.ToString());
				writer.WriteEndElement();
			}
			else if (value is ArrayList)
			{
				WriteArray(writer, (ArrayList)value);
			}
			else if (value is Hashtable)
			{
				WriteObject(writer, (Hashtable)value);
			}
			else
			{
				// null is the default when ActionScript can't serialize an object
				writer.WriteStartElement("null");
				writer.WriteEndElement();
			}
		}


		private static void WriteArray(XmlTextWriter writer, ArrayList array)
		{
			writer.WriteStartElement("array");

			int len = array.Count;

			for (int i = 0; i < len; i++)
			{
				writer.WriteStartElement("property");
				writer.WriteAttributeString("id", i.ToString());
				WriteElement(writer, array[i]);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}


		private static void WriteObject(XmlTextWriter writer, Hashtable table)
		{
			writer.WriteStartElement("object");

			foreach (DictionaryEntry entry in table)
			{
				writer.WriteStartElement("property");
				writer.WriteAttributeString("id", entry.Key.ToString());
				WriteElement(writer, entry.Value);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		#endregion

		#region Readers

		private static object ReadElement(XmlTextReader reader)
		{
			if (reader.NodeType != XmlNodeType.Element)
			{
				throw new XmlException();
			}

			if (reader.Name == "true")
			{
				reader.Read();
				return true;
			}

			if (reader.Name == "false")
			{
				reader.Read();
				return false;
			}

			if (reader.Name == "null" || reader.Name == "undefined")
			{
				reader.Read();
				return null;
			}

			if (reader.IsStartElement("number"))
			{
				reader.ReadStartElement("number");
				double value = Double.Parse(reader.Value);
				reader.Read();
				reader.ReadEndElement();
				return value;
			}

			if (reader.IsStartElement("string"))
			{
				reader.ReadStartElement("string");
				string value = reader.Value;
				reader.Read();
				reader.ReadEndElement();
				return value;
			}

			if (reader.IsStartElement("array"))
			{
				reader.ReadStartElement("array");
				ArrayList value = ReadArray(reader);
				reader.ReadEndElement();
				return value;
			}

			if (reader.IsStartElement("object"))
			{
				reader.ReadStartElement("object");
				Hashtable value = ReadObject(reader);
				reader.ReadEndElement();
				return value;
			}
			throw new XmlException();
		}


		private static ArrayList ReadArray(XmlTextReader reader)
		{
			ArrayList result = new ArrayList();

			while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "array")
			{
				int id = int.Parse(reader.GetAttribute("id"));
				reader.ReadStartElement("property");
				result.Add(ReadElement(reader));
				reader.ReadEndElement();
			}

			return result;
		}


		private static Hashtable ReadObject(XmlTextReader reader)
		{
			Hashtable result = new Hashtable();

			while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "object")
			{
				string id = reader.GetAttribute("id");
				reader.ReadStartElement("property");
				result.Add(id, ReadElement(reader));
				reader.ReadEndElement();
			}

			return result;
		}

		#endregion
	}
}