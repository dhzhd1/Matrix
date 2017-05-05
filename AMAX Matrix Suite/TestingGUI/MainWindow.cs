using System;
using Gtk;
using System.Net.Sockets;
using System.Net;
using MatrixLibrary;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.NetworkInformation;





public partial class MainWindow: Gtk.Window
{
	public TcpListener matrixSrv = null;
	public TcpListener matrixAgent = null;
	//public String serverIP = "192.168.0.7";
	public String serverIP = "208.108.11.141";



	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButton4Clicked (object sender, EventArgs e)
	{
		Thread startSrv = new Thread (new ThreadStart (StartServer));
		startSrv.Start ();


	}

	protected void OnButton2Clicked (object sender, EventArgs e)
	{
		// GUI Say Hello to Server Below function will be used for GUI first time connect to the server
		//TcpClient client = new TcpClient ("208.108.11.141", 14928);
		try {
			TcpClient client = new TcpClient (serverIP, 14928);
			String Content = "Hello";
			Type ContentType = Content.GetType ();
			Protocol clientTalk = new Protocol (client, ConstValues.PROTOCOL_FN_GUI_HANDSHAKE_WITH_SERVER, ContentType, Content);
			clientTalk.Start ();
			Gdk.Threads.AddIdle (0, () => {
				textview1.Buffer.Text += (String)(clientTalk.ReceivedContent) + "\n";
				return false;
			});
		} catch (Exception ex) {
			Debug.WriteLine (ex.Message);
			Gdk.Threads.AddIdle (0, () => {
				textview1.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}

	}

	void StartServer(){
		// Start Matrix Server 
		try {
			matrixSrv = new TcpListener (IPAddress.Any, 14928);
			matrixSrv.Start ();
			Gdk.Threads.AddIdle (0, () => {
				textview2.Buffer.Text += "Listen on 0.0.0.0 : 14928 \n";
				return false;
			});
			Debug.WriteLine ("Server Listen on 0.0.0.0 : 14928");
			while (true) {
				var client = matrixSrv.AcceptTcpClient ();
				Protocol matrixSrvProtocol = new Protocol (client);
				matrixSrvProtocol.Start ();
			}
		} catch (Exception ex) {
			Debug.WriteLine (ex.Message);
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}
	}



	protected void OnButton5Clicked (object sender, EventArgs e)
	{
		Thread agentSrvThread = new Thread (new ThreadStart (StartAgentServer));
		agentSrvThread.Start ();
	}

	void StartAgentServer(){
		// Start Matrix Agent Daemon
		try {
			matrixAgent = new TcpListener (IPAddress.Any, 14729);
			matrixAgent.Start ();
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += "Listen on 0.0.0.0 : 14729 \n";
				return false;
			});
			Debug.WriteLine ("Agent Listen on 0.0.0.0 : 14729");
			while (true) {
				var client = matrixAgent.AcceptTcpClient ();
				Protocol matrixAgentProtocol = new Protocol (client);
				matrixAgentProtocol.Start ();
			}
		} catch (Exception ex) {
			Debug.WriteLine (ex.Message);
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}
	}



	protected void OnButton6Clicked (object sender, EventArgs e)
	{
		//Check agent itself if registed, if not could not start the server
		try {
			String agentId = string.Empty;
			#if DEBUG
			agentId = "0000000000";
			#endif 
			object content = agentId;
			Type contentType = agentId.GetType ();
			TcpClient client = new TcpClient (serverIP, 14928);
			Protocol checkRegStatus = new Protocol (client, ConstValues.PROTOCOL_FN_AGENT_AUTHENTICATION_CHECK, contentType, content);
			checkRegStatus.Start ();
			if ((String)checkRegStatus.ResultObject == "True") {
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += "This agent has been registed!\n";
					return false;
				});
			} else {
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += "This agent didn't registed!\n";
					return false;
				});
			}
		} catch (Exception ex) {
			Debug.WriteLine (ex.Message);
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}
	}


	protected void OnButton61Clicked (object sender, EventArgs e)
	{
		// update agent status
		try {
			TcpClient client = new TcpClient (serverIP, 14928);
			String agentId = string.Empty;
			String status = string.Empty;
			#if DEBUG
			agentId = "0000000000";
			status = "Online";
			#endif 
			List<String> paramList = new List<String> ();
			paramList.Add (agentId);
			paramList.Add (status);
			object content = paramList;
			Type contentType = paramList.GetType ();
			Protocol updateAgentStatus = new Protocol (client, ConstValues.PROTOCOL_FN_UPDATE_AGENT_STATUS, contentType, content);
			updateAgentStatus.Start ();
			if ((String)updateAgentStatus.ResultObject == "True") {
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += "Agent Status update to online\n";
					return false;
				});
			} else {
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += "Agent status update failed\n";
					return false;
				});
			}
		} catch (Exception ex) {
			Debug.WriteLine (ex.Message);
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}
	}


	protected void OnButton155Clicked (object sender, EventArgs e)
	{
//		List<String> configList = new List<string> ();
//		configList.Add ("export LD_LIBRARY_PATH=/usr/local/cuda/lib64:/home/amax/cuda/lib64\n");
//		configList.Add ("export RCUDAPROTO=TCP\n");
//		configList.Add ("cd /home/amax/rCUDAv16.11.05alpha2-CUDA8.0/bin/\n");
//		configList.Add ("killall rCUDAd\n");
//		configList.Add ("./rCUDAd\n");
		try {
			TcpClient client = new TcpClient (serverIP, 14928);
			String agentId = string.Empty;
			#if DEBUG
			agentId = "00000000";
			#endif
			object content = agentId;
			Type contentType = agentId.GetType ();
			Protocol getRcudaServerConfigList = new Protocol (client, ConstValues.PROTOCOL_FN_REQUEST_RCUDA_SERVER_CONFIG, contentType, content);
			getRcudaServerConfigList.Start ();
			List<String> configList = (List<String>)getRcudaServerConfigList.ResultObject;
			foreach (var configLine in configList) {
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += configLine + "\n";
					return false;
				});
			}
		} catch (Exception ex) {
			Debug.WriteLine (ex.Message);
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}
			
	}

	protected void OnButton194Clicked (object sender, EventArgs e)
	{
		// Get rCuda Server configuration/command
		try {
			TcpClient client = new TcpClient (serverIP, 14928);
			String agentId = String.Empty;
			#if DEBUG
			agentId = "00000000";
			#endif
			object content = agentId;
			Type contentType = agentId.GetType ();
			Protocol getRcudaClientConfigList = new Protocol (client, ConstValues.PROTOCOL_FN_REQUEST_ASSIGNMENT_CONFIG, contentType, content);
			getRcudaClientConfigList.Start ();
			List<String> configList = (List<String>)getRcudaClientConfigList.ResultObject;
			foreach (var configLine in configList) {
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += configLine + "\n";
					return false;
				});
			}
		} catch (Exception ex) {
			Debug.WriteLine (ex.Message);
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}
	}


	protected void OnButton3Clicked (object sender, EventArgs e)
	{
		// Get rCuda Client Configuration / Commands
		try {
			String configStr = "export LD_LIBRARY_PATH=/usr/local/cuda/lib64:/home/amax/cuda/lib64\nexport RCUDAPROTO=TCP\ncd /home/amax/rCUDAv16.11.05alpha2-CUDA8.0/bin/\nkillall rCUDAd\n./rCUDAd\n";
			List<String> configList = new List<string> ();
			foreach (var configLine in configStr.Split('\n')) {
				configList.Add (configLine);
			}
			String agentId = "00000000";
			String agentIP = "208.108.11.141";
			object content = configList;
			Type contentType = configList.GetType ();
			TcpClient client = new TcpClient (agentIP, 14729);
			Protocol pushAssignment = new Protocol (client, ConstValues.PROTOCOL_FN_PUSH_NEW_ASSIGNMENT, contentType, content);
			pushAssignment.Start ();
			Gdk.Threads.AddIdle (0, () => {
				textview2.Buffer.Text += (String)pushAssignment.ResultObject + "\n";
				return false;
			});
		} catch (Exception ex) {
			Gdk.Threads.AddIdle (0, () => {
				textview2.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}


	}

	protected void OnButton195Clicked (object sender, EventArgs e)
	{
		// Upload Agent's GPU information to server and make record
		List<Dictionary<String, String>> gpuInfoList = new List<Dictionary<string, string>>();
		Dictionary<String, String> gpuInfo = new Dictionary<string, string> ();
		gpuInfo.Add ("gpuid", "0");
		gpuInfo.Add ("agent_id", "00000001");
		gpuInfo.Add ("prodname", "TitanXP");
		gpuInfo.Add ("uuid", "1234567890");
		gpuInfo.Add ("xml", "<xml>omitted info</xml>");
		gpuInfoList.Add (gpuInfo);

		gpuInfo = new Dictionary<string, string> ();
		gpuInfo.Add ("gpuid", "1");
		gpuInfo.Add ("agent_id", "00000001");
		gpuInfo.Add ("prodname", "TitanXP");
		gpuInfo.Add ("uuid", "9876543210");
		gpuInfo.Add ("xml", "<xml>omitted info 002</xml>");
		gpuInfoList.Add (gpuInfo);

		try {
			TcpClient client = new TcpClient (serverIP, 14928);
			object content = gpuInfoList;
			Type contentType = gpuInfoList.GetType ();
			Protocol upsertGpuInfo = new Protocol (client, ConstValues.PROTOCOL_FN_UPLOAD_GPU_INFO, contentType, content);
			upsertGpuInfo.Start ();
			var result = (String)upsertGpuInfo.ResultObject;
			if(result == "True"){
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += "GPU Info Uploaded succesful\n";
					return false;
				});
			}else{
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += "GPU Info Uploaded failed\n";
					return false;
				});
			}

		} catch (Exception ex) {
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}
	}

	


	protected void OnButton196Clicked (object sender, EventArgs e)
	{
		List<Dictionary<String,String>> fabricInfoList = new List<Dictionary<string, string>> ();
		Dictionary<String, String> fabricInfo = new Dictionary<string, string> ();
		fabricInfo.Add ("agent_id", "00000001");
		fabricInfo.Add ("nic_name", "eth0");
		fabricInfo.Add ("mac_addr", "eaebeced0012");
		fabricInfo.Add ("ip_addr", "208.108.11.141");
		fabricInfo.Add ("op_status", "Up");
		fabricInfo.Add ("gateway_ip", "208.108.15.254");
		fabricInfo.Add ("link_speed", "1000000");
		fabricInfoList.Add (fabricInfo);

		fabricInfo = new Dictionary<string, string> ();
		fabricInfo.Add ("agent_id", "00000001");
		fabricInfo.Add ("nic_name", "eth1");
		fabricInfo.Add ("mac_addr", "eaebeced0013");
		fabricInfo.Add ("ip_addr", "208.108.11.142");
		fabricInfo.Add ("op_status", "Up");
		fabricInfo.Add ("gateway_ip", "208.108.15.254");
		fabricInfo.Add ("link_speed", "1000000");
		fabricInfoList.Add (fabricInfo);

		fabricInfo = new Dictionary<string, string> ();
		fabricInfo.Add ("agent_id", "00000002");
		fabricInfo.Add ("nic_name", "eth0");
		fabricInfo.Add ("mac_addr", "eaebeced0014");
		fabricInfo.Add ("ip_addr", "208.108.11.145");
		fabricInfo.Add ("op_status", "Up");
		fabricInfo.Add ("gateway_ip", "208.108.15.254");
		fabricInfo.Add ("link_speed", "1000000");
		fabricInfoList.Add (fabricInfo);

		try {
			TcpClient client = new TcpClient (serverIP, 14928);
			object content = fabricInfoList;
			Type contentType = fabricInfoList.GetType ();
			Protocol upsertFabric = new Protocol (client, ConstValues.PROTOCOL_FN_UPLOAD_FABRIC_INFO, contentType, content);
			upsertFabric.Start ();
			var result = (String)upsertFabric.ResultObject;
			if (result == "True") {
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += "Fabric Info Uploaded succesful\n";
					return false;
				});
			} else {
				Gdk.Threads.AddIdle (0, () => {
					textview3.Buffer.Text += "Fabric Info Uploaded failed\n";
					return false;
				});
			}
		} catch (Exception ex) {
			Gdk.Threads.AddIdle (0, () => {
				textview3.Buffer.Text += ex.Message + "\n";
				return false;
			});
		}
	}




	protected void OnButton1Clicked (object sender, EventArgs e)
	{
		//Generate UUID
		var guid = Guid.NewGuid().ToString();
		Gdk.Threads.AddIdle (0, () => {
			textview1.Buffer.Text += guid + "\n";
			return false;
		});
	}
}
