using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Diagnostics;


namespace MatrixLibrary
{
	public class Resource
	{
		public List<Dictionary<String, String>> GpuInfoList = new List<Dictionary<string, string>> ();
		public List<Dictionary<String, String>> FabricInfoList = new List<Dictionary<string, string>> ();
		public Resource ()
		{

		}

		static public List<Dictionary<String, String>> CollectGpuInfo(String agentId, String cudaPath, String nvidiaSmiPath){
			String gpuXmlOutput = String.Empty;
			if (Configuration.GpuDependencyCheck (cudaPath, nvidiaSmiPath)) {
				SystemCall sysCall = new SystemCall ();
				sysCall.CommandText = @"/bin/bash";
				sysCall.Parameters = String.Format(@" -c ""{0}  -q -x | tail -n +3""",nvidiaSmiPath);
				MatrixLibrary.log.Debug (sysCall.Parameters);
				if (sysCall.CommandExecute ()) {
					gpuXmlOutput = sysCall.StandOutput;
					//MatrixLibrary.log.Debug (gpuXmlOutput);
					//MatrixLibrary.log.Debug (sysCall.StandError);
				}
			} else {
				MatrixLibrary.log.Info ("Info: Cuda Path or Nvidia-SMI doesn't find");
			}
			return ParseGpuXmlInfo (agentId, gpuXmlOutput);
		}

		static List<Dictionary<String, String>> ParseGpuXmlInfo(String agentId, String xmlContent)
		{
			Int32 gpuId = 0;
			//string agent_id = Utility.GetSystemUUID ();
			List<Dictionary<String,String> > gpuInfoList = new List<Dictionary<string, string>> ();
			//MatrixLibrary.log.Info (xmlContent.ToString ());
			//Console.WriteLine (xmlContent);
			XmlDocument xmlGpuInfo = new XmlDocument ();
			xmlGpuInfo.LoadXml (xmlContent);
			foreach (XmlNode gpuNode in xmlGpuInfo.SelectNodes("/nvidia_smi_log/gpu"))
			{
				var prodName = gpuNode.SelectSingleNode ("product_name").InnerText;
				var gpuUUID = gpuNode.SelectSingleNode ("uuid").InnerText;
				Dictionary<String, String> gpuInfo = new Dictionary<string, string> ();
				gpuInfo.Add ("gpuid", gpuId.ToString ());
				gpuInfo.Add ("agent_id", agentId);
				gpuInfo.Add ("prodname", prodName);
				gpuInfo.Add ("uuid", gpuUUID);
				gpuInfo.Add ("xml", gpuNode.OuterXml.ToString ());
				gpuInfoList.Add (gpuInfo);
				gpuId++;
			}
			return gpuInfoList;
		}

		static public List<Dictionary<String, String>> CollectFabricInfo(String agentId){
			List<Dictionary<String, String>> fabricList = new List<Dictionary<string, string>> ();
			NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces ();
			//string agent_id = Utility.GetSystemUUID ();
			foreach (NetworkInterface adapter in networkInterfaces) {
				MatrixLibrary.log.Info (adapter.Id);
				if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
					Dictionary<String, String> fabricInfo = new Dictionary<string, string> ();
					List<String> unicastIpList = new List<string> ();
					var ipaddrCollection = adapter.GetIPProperties ().UnicastAddresses;
					foreach (var ipaddress in ipaddrCollection) {
						if (ipaddress.Address.AddressFamily == AddressFamily.InterNetwork) {
							unicastIpList.Add (ipaddress.Address.ToString ());
						}
					}
					fabricInfo.Add ("agent_id", agentId);
					fabricInfo.Add ("nic_name", adapter.Id);
					fabricInfo.Add ("mac_addr", adapter.GetPhysicalAddress().ToString());
					fabricInfo.Add ("ip_addr", String.Join(",", unicastIpList));
					fabricInfo.Add ("op_status", adapter.OperationalStatus.ToString());
					fabricInfo.Add ("gateway_ip", adapter.GetIPProperties ().GatewayAddresses.ToString());
					fabricInfo.Add ("link_speed", adapter.Speed.ToString());
					fabricList.Add (fabricInfo);
				}
			}
			return fabricList;
		}

	}
}

