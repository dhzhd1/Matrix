using System;
using System.Xml;
using log4net;
using log4net.Config;
using System.Configuration;
using System.IO;

namespace MatrixLibrary
{
	public class MatrixLibrary
	{
		public static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public MatrixLibrary ()
		{
			// setup log entry
			XmlConfigurator.Configure(new FileInfo(ConfigurationManager.AppSettings["log4net.Config"]));

		}
	}
}

