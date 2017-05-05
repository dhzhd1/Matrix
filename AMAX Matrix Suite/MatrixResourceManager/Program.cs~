using System;
using Gtk;
using log4net.Config;
using log4net;
using System.IO;
using System.Configuration;
using System.Xml;

namespace MatrixResourceManager
{
	class MainClass
	{
		public static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static void Main (string[] args)
		{
			XmlConfigurator.Configure(new FileInfo(ConfigurationManager.AppSettings["log4net.Config"]));

			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
