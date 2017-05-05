using System;
using log4net;
using log4net.Config;
using System.IO;
using System.Configuration;
using MatrixLibrary;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Xml;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace Agent
{
	class Agent
	{
		static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		static public Dictionary<String, String> agentInfo = new Dictionary<string, string>();

		public static void Main (string[] args)
		{
			new MatrixLibrary.MatrixLibrary ();
			// Initialize log instance
			XmlConfigurator.Configure(new FileInfo(ConfigurationManager.AppSettings["log4net.Config"]));

			// Loading agent information from configuration file
			try {
				MatrixLibrary.Configuration config = new MatrixLibrary.Configuration (ConstValues.DEFAULT_AGENT_CONFIG_PATH);
				agentInfo = config.GetAgentInfo();
			} catch (Exception ex) {
				Debug.WriteLine (ex.Message);
				log.Error (ex.Message);
				log.Debug (ex.StackTrace);
				return;
			}

			// verify if the agent's serial number has been registed
			if (!CheckAgentRegistration ()) {
				Debug.WriteLine ("Unregisted Agent");
				log.Error ("This agent didn't registed");
				return;
			} else {
				// Start Daemon service on the agent node
				Thread agentDaemonThread = new Thread(new ThreadStart(StartDaemon));
				agentDaemonThread.Start ();

				// Load rCuda Runtime Configuration.
				if (LoadRcudaConfiguration ()) {
					Debug.WriteLine ("rCuda Configuration File Loaded!");
					log.Info ("rCuda Configuration File Loaded!");
				} else {
					Debug.WriteLine ("Load rCuda configuration failed!");
					log.Error ("Load rCuda configuration failed!");
				}

				// Update HWinfo GPU List and Interface
				if (UpdateHardwareInfo ()) {
					Debug.WriteLine ("Hardware Information Update Successful.");
					log.Info ("Hardware Information Update Successful.");
				} else {
					Debug.WriteLine ("Update Hardware information failed.");
					log.Error ("Update Hardware information failed.");
				}
			}

		}

		static bool CheckAgentRegistration(){
			String serialNumber = agentInfo ["serial_number"];
			object content = serialNumber;
			Type contentType = serialNumber.GetType ();
			try {
				TcpClient client = new TcpClient (agentInfo ["registered_server_ip"], Int32.Parse (agentInfo ["registered_server_port"]));
				Protocol checkAgentRegStatus = new Protocol (client, ConstValues.PROTOCOL_FN_AGENT_AUTHENTICATION_CHECK, contentType, content);
				checkAgentRegStatus.Start ();
				if ((String)checkAgentRegStatus.ResultObject == "True") {
					return true;
				} else {
					return false;
				}
			} catch (Exception ex) {
				log.Error (ex.Message);
				log.Debug (ex.StackTrace);
				return false;
			}
		}

		static void StartDaemon(){
			IPAddress daemonIP = agentInfo ["agent_ip"] == "0.0.0.0" ? IPAddress.Any : IPAddress.Parse (agentInfo ["agent_ip"]);
			Int32 tcpPort = Int32.Parse (agentInfo ["agent_port"]);
			try {
				TcpListener agentDaemon = new TcpListener (daemonIP, tcpPort);
				agentDaemon.Start ();
				log.Info("Agent Daemon Listening on " + daemonIP + ":" + tcpPort.ToString());
				while (true) {
					TcpClient client = agentDaemon.AcceptTcpClient ();
					Protocol agentServerDaemon = new Protocol (client);
					agentServerDaemon.Start ();
				}
			} catch (Exception ex) {
				log.Error (ex.Message);
				log.Debug (ex.StackTrace);
				return;
			}
		}


		static bool LoadRcudaConfiguration(){
			string agentId = agentInfo ["serial_number"];
			string agentType = agentInfo ["config_type"];
			object content = agentId;
			bool result = true;
			Type contentType = content.GetType ();
			try {
				// For rCuda client configuration file need be write to a local file for each session use
				if(agentType == "ClientAgent"){
					TcpClient client = new TcpClient (agentInfo ["registered_server_ip"], Int32.Parse (agentInfo ["registered_server_port"]));
					Protocol getRcudaConfig = new Protocol (client, ConstValues.PROTOCOL_FN_REQUEST_ASSIGNMENT_CONFIG, contentType, content);
					getRcudaConfig.Start ();
					var rCudaRuntimeConfig = getRcudaConfig.ReceivedContent as List<String>;
					if(rCudaRuntimeConfig != null){
						if(!MatrixLibrary.Configuration.rCUDACommand(rCudaRuntimeConfig)){
							result = false;
						}
						SaveRuntimeConfig(rCudaRuntimeConfig);

					}
				}else if(agentType == "ResAgent"){
					TcpClient client = new TcpClient (agentInfo ["registered_server_ip"], Int32.Parse (agentInfo ["registered_server_port"]));
					Protocol getRcudaConfig = new Protocol (client, ConstValues.PROTOCOL_FN_REQUEST_RCUDA_SERVER_CONFIG, contentType, content);
					getRcudaConfig.Start ();
					var rCudaRuntimeConfig = getRcudaConfig.ReceivedContent as List<String>;
					if(rCudaRuntimeConfig != null){
						if(!MatrixLibrary.Configuration.rCUDACommand(rCudaRuntimeConfig)){
							result = false;
						}
					}
				}else if(agentType == "MFAgent"){
					TcpClient client = new TcpClient (agentInfo ["registered_server_ip"], Int32.Parse (agentInfo ["registered_server_port"]));
					Protocol getRcudaConfig = new Protocol (client, ConstValues.PROTOCOL_FN_REQUEST_RCUDA_SERVER_CONFIG, contentType, content);
					getRcudaConfig.Start ();
					var rCudaRuntimeConfig = getRcudaConfig.ReceivedContent as List<String>;
					if(rCudaRuntimeConfig!= null){
						if(!MatrixLibrary.Configuration.rCUDACommand(rCudaRuntimeConfig)){
							result = false;
						}
					}
					TcpClient client2 = new TcpClient (agentInfo ["registered_server_ip"], Int32.Parse (agentInfo ["registered_server_port"]));
					Protocol getRcudaConfig2 = new Protocol (client2, ConstValues.PROTOCOL_FN_REQUEST_ASSIGNMENT_CONFIG, contentType, content);
					getRcudaConfig2.Start ();
					var rCudaRuntimeConfig2 = getRcudaConfig2.ReceivedContent as List<String>;
					if(rCudaRuntimeConfig2 != null){
						if(!MatrixLibrary.Configuration.rCUDACommand(rCudaRuntimeConfig2)){
							result = false;
						}
						//save the configuration file to /opt/amax/matrix/load_runtime_config
						SaveRuntimeConfig(rCudaRuntimeConfig2);
					}
				}
			} catch (Exception ex) {
				log.Error (ex.Message);
				log.Debug (ex.StackTrace);
				result = false;
			}
			return result;
		}


		static void SaveRuntimeConfig(List<String> config){
			System.IO.File.WriteAllLines (ConstValues.DEFAULT_CLIENT_RUNTIME_CONFIG, config);
		}


		static bool UpdateHardwareInfo (){
			bool result = true;
			try {
				List<Dictionary<String,String>> gpuInfo = Resource.CollectGpuInfo (agentInfo["serial_number"],agentInfo ["cuda_path"], agentInfo ["nvidia_smi"]);
				object contentGpuInfo = gpuInfo;
				Type contentTypeGpuInfo = gpuInfo.GetType ();
				//printList(gpuInfo);

				List<Dictionary<String,String>> fabricInfo = Resource.CollectFabricInfo (agentInfo["serial_number"]);
				object contentFabricInfo = fabricInfo;
				Type contentTypeFabricInfo = fabricInfo.GetType ();
				//printList(fabricInfo);

				TcpClient client = new TcpClient (agentInfo ["registered_server_ip"], Int32.Parse (agentInfo ["registered_server_port"]));
				Protocol uploadGpuInfo = new Protocol (client, ConstValues.PROTOCOL_FN_UPLOAD_GPU_INFO, contentTypeGpuInfo, contentGpuInfo);
				uploadGpuInfo.Start ();
				if ((String)uploadGpuInfo.ResultObject != "True") {
					log.Error ("Upload GPU Infomation to Server failed!");
					result = false;
				} else {
					log.Info ("Upload GPU Information to Server successful");
				}
				TcpClient client2 = new TcpClient (agentInfo ["registered_server_ip"], Int32.Parse (agentInfo ["registered_server_port"]));
				Protocol uploadFabricInfo = new Protocol (client2, ConstValues.PROTOCOL_FN_UPLOAD_FABRIC_INFO, contentTypeFabricInfo, contentFabricInfo);
				uploadFabricInfo.Start ();
				if ((String)uploadFabricInfo.ResultObject != "True") {
					log.Error ("Upload Fabric Infomation to Server failed!");
					result = false;
				} else {
					log.Info ("Upload Fabric Information to Server successful");
				}
			} catch (Exception ex) {
				result = false;
				log.Error (ex.Message);
				log.Debug (ex.StackTrace);
			}
			return result;
		}


		static void printList(List<Dictionary<String,String>> info){
			foreach (var item in info) {
				foreach (var dictItem in item) {
					Console.WriteLine ("Key:" + dictItem.Key.ToString ());
					Console.WriteLine ("Value:" + dictItem.Value.ToString ());
				}
			}
		}
	}
}
