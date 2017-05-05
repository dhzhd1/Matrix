using System;
using log4net.Config;
using log4net;
using System.IO;
using System.Configuration;
using MatrixLibrary;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using MySql;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Data;



namespace Server
{
	

	class Server
	{
		static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		static public Dictionary<String, String> dbInfo = new Dictionary<string, string> ();
		static public Dictionary<String, String> serviceInfo = new Dictionary<string, string> ();
		static public Database dbInst = null;

		public static void Main (string[] args)
		{
			
			new MatrixLibrary.MatrixLibrary();
			// Initiate the backend log
			XmlConfigurator.Configure(new FileInfo(ConfigurationManager.AppSettings["log4net.Config"]));


			// Load Server configuration and parse. Pass the database information into the variable.
			// [Configuration] will Detect Server side configuration file. Default path is /etc/matrix/server.conf
			try {
				MatrixLibrary.Configuration config = new MatrixLibrary.Configuration (ConstValues.DEFAULT_SERVER_CONFIG_PATH);
				dbInfo = config.GetDbInfo ();
				serviceInfo = config.GetServiceInfo ();
			} catch (Exception ex) {
				log.Error (ex.Message);
				log.Debug (ex.StackTrace);
				return;
			}

			// Create database connection
			dbInst = new Database(dbInfo);
			dbInst.Connect ();

			// Check if the server's UUID is registed in the database.
			if (!dbInst.IfServerRegisted (serviceInfo ["serial_number"])) {
				log.Error ("This Matrix Server didn't registed or deployed successuful");
				log.Info ("Exiting...");
				return;
			}


			Thread serverThread = new Thread (new ThreadStart (StartServer));
			serverThread.Start ();
		}

		static void StartServer(){
			IPAddress serverIP = serviceInfo ["service_ip"] == "0.0.0.0" ? IPAddress.Any : IPAddress.Parse (serviceInfo ["service_ip"]);
			Int32 tcpPort = Int32.Parse (serviceInfo ["service_port"]);
			try {
				TcpListener serverDaemon = new TcpListener (serverIP, tcpPort);
				serverDaemon.Start ();
				log.Info("Server Daemon listening on " + serverIP + ":" + tcpPort.ToString());
				while (true) {
					TcpClient client = serverDaemon.AcceptTcpClient ();
					Protocol serverListenProtocol = new Protocol (client);
					serverListenProtocol.dbInst = dbInst;
					serverListenProtocol.Start ();
				}
			} catch (Exception ex) {
				log.Error (ex.Message);
				log.Debug (ex.StackTrace);
				return;
			}
		}


	}
}
