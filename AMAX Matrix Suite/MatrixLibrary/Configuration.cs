//
//  Copyright 2017  AMAX Information Technologies, Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace MatrixLibrary
{
	public class Configuration
	{
		XmlDocument configXml = null;
		String configPath = string.Empty;


		public Configuration (String configPath)
		{
			this.configPath = configPath;
			LoadConfigurationFromFile ();
		}

		private void LoadConfigurationFromFile(){
			if (Utility.FileExist (configPath)) {
				LoadConfigToXml (configPath);
			}
		}


		public Dictionary<String, String> GetDbInfo()
		{
			Dictionary<String, String> dbConfig = new Dictionary<string, string> ();
			XmlNode configNode = configXml.FirstChild;
			foreach (XmlNode node in configNode) {
				switch (((XmlElement)node).Name) {
				case "database_host":
					dbConfig.Add ("database_host", ((XmlElement)node).InnerText);
					break;
				case "database_user":
					dbConfig.Add ("database_user", ((XmlElement)node).InnerText);
					break;
				case "database_pass":
					dbConfig.Add ("database_pass", ((XmlElement)node).InnerText);
					break;
				case "database_name":
					dbConfig.Add ("database_name", ((XmlElement)node).InnerText);
					break;
				case "database_port":
					dbConfig.Add ("database_port", ((XmlElement)node).InnerText);
					break;
				default:
					break;
				}
			}
			return dbConfig;
		}

		Boolean LoadConfigToXml(String xmlFilePath)
		{
			XmlDocument xmlContent = new XmlDocument ();
			try{
				xmlContent.Load (xmlFilePath);
				configXml = xmlContent;
				return true;
			}
			catch (XmlException xmlE)
			{
				Debug.WriteLine ("Exception", xmlE);
				configXml = null;
				return false;
			}
		}



		public Dictionary<String, String> GetServiceInfo()
		{
			Dictionary<String, String> serviceConfig = new Dictionary<string, string> ();
			XmlNode configNode = configXml.FirstChild;
			foreach (XmlNode node in configNode) {
				switch (((XmlElement)node).Name) {
				case "service_port":
					serviceConfig.Add ("service_port", ((XmlElement)node).InnerText);
					break;
				case "service_ip":
					serviceConfig.Add ("service_ip", ((XmlElement)node).InnerText);
					break;
				case "serial_number":
					serviceConfig.Add ("serial_number", ((XmlElement)node).InnerText);
					break;
				default:
					break;
				}
			}
			return serviceConfig;
		}

		public Dictionary<String, String> GetAgentInfo()
		{
			Dictionary<String, String> agentInfo = new Dictionary<string, string> ();
			XmlNode configNode = configXml.FirstChild;
			foreach (XmlNode node in configNode) {
				switch (((XmlElement)node).Name) {
				case "registered_server_ip":
					agentInfo.Add("registered_server_ip",((XmlElement)node).InnerText);
					break;
				case "registered_server_port":
					agentInfo.Add("registered_server_port",((XmlElement)node).InnerText);
					break;
				case "serial_number":
					agentInfo.Add ("serial_number", ((XmlElement)node).InnerText);
					break;
				case "config_type":
					agentInfo.Add ("config_type", ((XmlElement)node).InnerText);
					break;
				case "agent_ip":
					agentInfo.Add ("agent_ip", ((XmlElement)node).InnerText);
					break;
				case "agent_port":
					agentInfo.Add ("agent_port", ((XmlElement)node).InnerText);
					break;
				case "cuda_path":
					agentInfo.Add ("cuda_path", ((XmlElement)node).InnerText);
					break;
				case "nvidia_smi":
					agentInfo.Add ("nvidia_smi", ((XmlElement)node).InnerText);
					break;
				default:
					break;
				}
			}
			return agentInfo;
		}

		static public bool rCUDACommand(List<string> commandList){
			UnsetRcudaEnv ();
			bool result = true;
			SystemCall runrCuda = new SystemCall ();
//			foreach (var cmd in commandList) {
//				runrCuda.CommandText = @"/bin/bash";
//				runrCuda.Parameters = String.Format (@" -c ""{0}""", cmd);
//				runrCuda.CommandExecute ();
//				if (runrCuda.StandError.Trim () != "") {
//					result = false;
//				}
//			}
			string cmdStr = String.Join("\n",commandList);
			runrCuda.CommandText = @"/bin/bash";
			runrCuda.Parameters = String.Format (@"-c ""{0}""", cmdStr);
			runrCuda.CommandExecute ();
			Console.WriteLine (runrCuda.StandError);
			if (runrCuda.StandError.Trim () != "") {
				result = false;
			}
			return result;
		}

		static public void UnsetRcudaEnv(){
			SystemCall getRcudaEnv = new SystemCall ();
			getRcudaEnv.CommandText = @"/bin/bash";
			getRcudaEnv.Parameters = @"-c ""printenv | grep RCUDA | awk -F= '{ print $1 }'""";
			getRcudaEnv.CommandExecute ();
			String output = getRcudaEnv.StandOutput;
			var variables = output.Split ('\n');
			foreach (var variable in variables) {
				if (variable.Trim () != "") {
					SystemCall unsetEnv = new SystemCall ();
					unsetEnv.CommandText = @"/bin/bash";
					unsetEnv.Parameters = String.Format (@"-c ""unset {0}""", variable);
				}
			}
		}

		public static Boolean GpuDependencyCheck(String cudaPath, String nvidiaSmiPath){
			Boolean result = true;
			if (!Directory.Exists (cudaPath)) {
				Debug.WriteLine (String.Format (ConstValues.MSG_ERR_CUDA_PATH_CHECK_FAILED, cudaPath));
				result = false;
			}
			if (!File.Exists (nvidiaSmiPath)) {
				Debug.WriteLine (String.Format (ConstValues.MSG_ERR_NVIDIA_SMI_PATH_CHECK_FAILED, nvidiaSmiPath));
				result = false;
			}
			return result;
		}


		public static List<String> GenerateServerConfig(Dictionary<string, string> systemInfo){
			List<string> configContent = new List<string> ();
			configContent.Add("<config>");
			configContent.Add("<config_type>server</config_type>");
			configContent.Add("<serial_number>" + systemInfo ["serial_number"] + "</serial_number>");
			configContent.Add("<database_host>" + systemInfo ["database_host"] + "</database_host>");
			configContent.Add("<database_user>" + systemInfo ["database_user"] + "</database_user>");
			configContent.Add("<database_pass>" + systemInfo ["database_pass"] + "</database_pass>");
			configContent.Add("<database_name>" + systemInfo ["database_name"] + "</database_name>");
			configContent.Add("<database_port>" + systemInfo ["database_port"] + "</database_port>");
			configContent.Add("<service_ip>" + systemInfo["service_ip"] + "</service_ip>");
			configContent.Add("<service_port>" + systemInfo["service_port"] + "</service_port>");
			configContent.Add("</config>");
			return configContent;
		}

		public static List<String> GenerateAgentConfig(Dictionary<String,String> agentInfo){
			List<String> configContent = new List<string>();
			configContent.Add("<config>");
			configContent.Add("<config_type>"+ agentInfo["config_type"] + "</config_type>");
			configContent.Add ("<serial_number>" + agentInfo ["serial_number"] + "</serial_number>");
			configContent.Add ("<registered_server_ip>" + agentInfo ["registered_server_ip"] + "</registered_server_ip>");
			configContent.Add ("<registered_server_port>" + agentInfo ["registered_server_port"] + "</registered_server_port>");
			configContent.Add ("<agent_ip>" + agentInfo ["agent_ip"] + "</agent_ip>");
			configContent.Add ("<agent_port>" + agentInfo ["agent_port"] + "</agent_port>");
			configContent.Add ("<cuda_path>" + agentInfo ["cuda_path"] + "</cuda_path>");
			configContent.Add ("<nvidia_smi>" + agentInfo ["nvidia_smi"] + "</nvidia_smi>");
			configContent.Add("</config>");
			return configContent;
		}

		public static List<String> GenerateRcudaClientRuntimeConfig(List<Dictionary<String,String>> assignmentList){
			int gpuCount = assignmentList.Count;
			List<String> config = new List<string> ();
			if (gpuCount == 0) {
				config.Add(@"echo ""No GPU card assigned to this client""");
			} else {
				config.Add (String.Format(@"declare -x MATRIX_AGENT_ID={0}",assignmentList[0]["agent_id"]));
				config.Add (@"declare -x RCUDAPROTO=TCP");
				config.Add (String.Format(@"declare -x RCUDA_DEVICE_COUNT=""{0}""", gpuCount.ToString ()));
				int id = 0;
				foreach (var assignment in assignmentList) {
					config.Add (String.Format (@"declare -x RCUDA_DEVICE_{0}=""{1}:{2}""", id, assignment["resource_ip"], assignment["gpu_id"]));
					id++;
				}
			}

			return config;
		}

		public static List<String> GenerateRcudaServerRuntimeConfig(String agentId, int gpuCount){
			List<String> config = new List<string>();
			config.Add (String.Format(@"declare -x MATRIX_AGENT_ID={0}", agentId));
			config.Add (String.Format(@"declare -x RCUDA_DEVICE_COUNT=""{0}""", gpuCount));
			config.Add (@"killall rCUDAd");
			config.Add (@"declare -x RCUDAPROTO=TCP");
			config.Add (@"declare -x LD_LIBRARY_PATH=""/usr/local/cuda/lib64:/opt/amax/matrix/rCuda/lib/cudnn:/opt/amax/matrix/rCuda/lib:$LD_LIBRARY_PATH""");
			config.Add (@"pushd /opt/amax/matrix/rCuda/bin/");
			config.Add (@"./rCUDAd");
			config.Add (@"popd");
			return config;
		}

	}
}


