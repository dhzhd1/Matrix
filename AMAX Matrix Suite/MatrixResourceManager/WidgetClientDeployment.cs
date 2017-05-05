using System;
using Gtk;
using System.ComponentModel;
using MatrixLibrary;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace MatrixResourceManager
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class WidgetClientDeployment : Gtk.Bin
	{
		public bool PreCheckErrorFlag = false;
		public bool DeployErrorFlag = false;
		public String ClientIP = string.Empty;
		public String LoginUser = string.Empty;
		public String LoginPass = string.Empty;
		public String ClientTcpPort = String.Empty;
		public String AgentType = String.Empty;
		public String UUID;
		public String ServerIp=String.Empty;
		public String TcpPort = String.Empty;

		public WidgetClientDeployment ()
		{
			this.Build ();
			#if DEBUG
			entry1.Text = "157.22.244.230";
			entry2.Text = "root";
			entry3.Text = "amax1565";
			entry4.Text = "14729";
			#endif
		}

		protected void OnButton634Clicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}


		protected void OnButton632Clicked (object sender, EventArgs e)
		{
			//Pre-Check Button
			PreCheckErrorFlag = false;
			textview1.Buffer.Text = "";
			ClientIP = entry1.Text.Trim ();
			LoginUser = entry2.Text.Trim ();
			LoginPass = entry3.Text.Trim ();
			ClientTcpPort = entry4.Text.Trim ();
			if (radiobutton1.Active) {
				AgentType = "ClientAgent";
			} else if (radiobutton2.Active) {
				AgentType = "ResAgent";
			} else {
				AgentType = "MFAgent";
			}

			BackgroundWorker preCheckWorker = new BackgroundWorker ();
			preCheckWorker.WorkerReportsProgress = true;
			preCheckWorker.WorkerSupportsCancellation = true;
			preCheckWorker.DoWork += PreCheckWorker_DoWork;
			preCheckWorker.RunWorkerAsync ();
		}

		void PreCheckWorker_DoWork (object sender, DoWorkEventArgs e)
		{
			UpdateLog (textview1, "[INFO] Start to Check client server @" + DateTime.Now.ToString());
			//Check target server
			CheckLoginCredential ();
			GenerateGUID ();
			DeployFolderCheck (ConstValues.DEFAULT_DEPLOY_PATH);
			DeployFolderCheck (ConstValues.DEFAULT_MATRIX_LOG_PATH);
			DeployFolderCheck (ConstValues.DEFAULT_MATRIX_CONFIG_PATH);
			CudaRelatedPathCheck ();

			if (PreCheckErrorFlag) {
				UpdateLog (textview1, "\n[INFO] Deploy Pre-Chech didn't pass. Please fix the issue and run the pre-check again.");

			} else {
				button633.Sensitive = true;
				UpdateLog (textview1, "\n[INFO] Deployment Precheck passed. Please click the 'Deploy' button start to deploy Client");
			}

		}

		void UpdateLog(TextView textview, String msg){
			Gdk.Threads.AddIdle (0, () => {
				textview.Buffer.Text += msg + "\n";
				return false;
			});
		}

		void CheckLoginCredential (){
			// This function will login the system get uptime to approve the system is working
			SystemCall getUptime = new SystemCall();
			getUptime.CommandText = @"sshpass";
			getUptime.Parameters = String.Format(@" -p {0} ssh -o StrictHostKeyChecking=no {1}@{2} uptime ",  LoginPass, LoginUser, ClientIP);
			getUptime.CommandExecute ();
			if (getUptime.StandError != "") {
				MainClass.log.Error (getUptime.StandError);
				UpdateLog (textview1, "[ERROR] Check Login Credential Failed");
				PreCheckErrorFlag = true;
			} else {
				string uptime = getUptime.StandOutput.Split (',') [0].Trim();
				UpdateLog (textview1, "[INFO] Login Credential validated!");
				UpdateLog (textview1, "[INFO] System is alive. \n[INFO] Uptime: " + uptime);
			}
		}

		void GenerateGUID(){
			UUID = Utility.GenerateUUID ();
			UpdateLog (textview1, "[INFO] Generate New GUID for Server: " + UUID);
		}

		void DeployFolderCheck(String folderPath){
			SystemCall checkFolder = new SystemCall ();
			checkFolder.CommandText = @"sshpass";
			checkFolder.Parameters = String.Format (@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} ls -d {3}", LoginPass, LoginUser, ClientIP, folderPath);
			checkFolder.CommandExecute ();
			if (checkFolder.StandOutput.Trim () == "") {
				UpdateLog (textview1, String.Format (@"[INFO] Folder {0} doesn't exist. It will be created during the deployment stage.", folderPath));
			} else {
				UpdateLog (textview1, String.Format(@"[WARN] The folder {0} existed. If you decide continue to deploy on this server, the previouse deployment will be wipped.", folderPath));
			}
		}

		void CudaRelatedPathCheck(){
			SystemCall verifyCuda = new SystemCall ();
			verifyCuda.CommandText = @"sshpass";
			verifyCuda.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} ls -d {3}", LoginPass, LoginUser, ClientIP, ConstValues.DEFAULT_CUDA_FOLDER);
			verifyCuda.CommandExecute ();
			if (verifyCuda.StandOutput.Trim () == "") {
				UpdateLog (textview1, String.Format (@"[INFO] No CUDA installation found. This client only can be deployed to a computer client."));
				Gdk.Threads.AddIdle (0, () => {
					radiobutton1.Active = true;
					return false;
				});
			} else {
				UpdateLog (textview1, String.Format (@"[INFO] CUDA installation found."));
			}
		}


		protected void OnButton633Clicked (object sender, EventArgs e)
		{
			textview2.Buffer.Text = "";
			DeployErrorFlag = false;
			BackgroundWorker deployWorker = new BackgroundWorker ();
			deployWorker.WorkerReportsProgress = true;
			deployWorker.WorkerSupportsCancellation = true;
			deployWorker.DoWork += DeployWorker_DoWork;
			deployWorker.RunWorkerAsync ();
		}

		void DeployWorker_DoWork (object sender, DoWorkEventArgs e)
		{
			// Create required path for deployment
			CreateDeployPath (ConstValues.DEFAULT_MATRIX_CONFIG_PATH);
			CreateDeployPath (ConstValues.DEFAULT_MATRIX_LOG_PATH);
			CreateDeployPath (ConstValues.DEFAULT_DEPLOY_PATH);
			if (DeployErrorFlag) {
				UpdateLog(textview2, String.Format(@"[ERROR] Deployment process stopped because of creating path failed."));
				return;
			}

			// Copy Agent application and rCuda library to client
			CopyFilesToClient (ConstValues.DEPLOY_AGENT_APP_SOURCE_PATH, ConstValues.DEFAULT_DEPLOY_PATH);
			CopyFilesToClient (ConstValues.DEPLOY_RCUDA_APP_SOURCE_PATH, ConstValues.DEFAULT_DEPLOY_PATH);
			if (DeployErrorFlag) {
				UpdateLog(textview2, String.Format(@"[ERROR] Deployment process stopped because of uploading application failed."));
				return;
			}




			// Generate agent configuration and upload to client server
			GenerateAgnetConfig ();
			if (DeployErrorFlag) {
				UpdateLog (textview2, "[ERROR] Deployment process ended unexpected since uploading server configuration to client server failed!");
				return;
			} else {
				UpdateLog (textview2, String.Format("[INFO] Agent configuration has been upload to {0}.", ConstValues.DEFAULT_AGENT_CONFIG_PATH));
			}


			//Register Client Agent.
			RegisterClientAgent();
			if (DeployErrorFlag) {
				UpdateLog (textview2, String.Format (@"[ERROR] Deployment process ended unexpected since register agent failed."));
				return;
			}


			// Generate rCuda Configuration
			GenerateRcudaConfig ();
			if (DeployErrorFlag) {
				UpdateLog (textview2, String.Format (@"[ERROR] Deployment process ended unexpected since runtime configuration generate failed."));
				return;
			}

			// Setup Auto startup script
			CopyFilesToClient(ConstValues.AGENT_DAEMON_STARTUP_SCRIPT, ConstValues.STARTUP_SCRIPT_PATH);
			if (DeployErrorFlag) {
				UpdateLog (textview2, String.Format (@"[ERROR] Deployment process ended unexpected since service startup script upload failed."));
				return;
			} else {
				SetupAgentAutoStartupScript ();
				if (DeployErrorFlag) {
					UpdateLog (textview2, String.Format (@"[ERROR] Matrix Agent startup script set fail."));
					return;
				} else {
					UpdateLog (textview2, String.Format (@"[INFO] Matrix Agent will be started as long as the system startup."));
				}
			}

			// Startup Agent Daemon and Check status.
			UpdateLog (textview2, "\n[INFO] Starting Remote Matrix Agent Daemon...");
			SystemCall startRemoteServer = new SystemCall();
			startRemoteServer.CommandText = @"sshpass";
			startRemoteServer.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} '{3}/Agent.exe &'", LoginPass, LoginUser, ClientIP, ConstValues.DEFAULT_DEPLOY_PATH);
			//startRemoteServer.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} /etc/init.d/matrix-agent restart", LoginPass, LoginUser, ClientIP);
			startRemoteServer.CommandExecute ();
			Thread.Sleep (8000);
			UpdateLog (textview2, "\n[INFO] Checking Remote Matrix Agent Daemon...");
			SystemCall checkRemoteServer = new SystemCall ();
			checkRemoteServer.CommandText = @"sshpass";
			checkRemoteServer.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} netstat -tln | grep 14729 ", LoginPass, LoginUser, ClientIP);
			checkRemoteServer.CommandExecute ();
			if (checkRemoteServer.StandOutput.Trim () != "") {
				string tcpListenResult = checkRemoteServer.StandOutput.Split (new [] {' '}, StringSplitOptions.RemoveEmptyEntries)[3];
				UpdateLog (textview2, "[INFO] Service has been linstened on the remote server " + tcpListenResult);
			} else {
				UpdateLog (textview2, String.Format("[ERROR] Service seems not started on the remote server. Please check the log under {0} for more information.", ConstValues.DEFAULT_MATRIX_LOG_PATH));
				DeployErrorFlag = true;
			}
			if (DeployErrorFlag) {
				UpdateLog (textview2, "[ERROR] Deployment process finished, but not succeed.");
			} else {
				UpdateLog (textview2, "[INFO] Deployment succeed.");
				button633.Sensitive = false;
			}

		}

		void CreateDeployPath (String dirPath){
			SystemCall createPath = new SystemCall ();
			createPath.CommandText = @"sshpass";
			createPath.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} mkdir -p {3}", LoginPass, LoginUser, ClientIP, dirPath);
			createPath.CommandExecute ();
			if (createPath.StandError.Trim() != "") {
				UpdateLog (textview2, String.Format (@"[ERROR] Create {0} failed on client server!", dirPath));
				DeployErrorFlag = true;
				UpdateLog (textview2, @"[DEBUG] " + createPath.StandError);
			} else {
				UpdateLog (textview2, String.Format (@"[INFO] Folder {0} has been created on client server!", dirPath));
			}
		}

		void CopyFilesToClient(String srcFile, String destFolder){
			SystemCall copyFile = new SystemCall ();
			copyFile.CommandText = @"sshpass";
			copyFile.Parameters = String.Format(@"-p {0} scp -r -o StrictHostKeyChecking=no {1} {2}@{3}:{4}", LoginPass, srcFile, LoginUser, ClientIP, destFolder);
			copyFile.CommandExecute ();
			if (copyFile.StandError.Trim () != "") {
				UpdateLog (textview2, String.Format (@"[ERROR] Copy file {0} to {1}:{2} failed!", srcFile, ClientIP ,destFolder));
				DeployErrorFlag = true;
				UpdateLog (textview2, @"[DEBUG] " + copyFile.StandError);
			} else {
				UpdateLog (textview2, String.Format (@"[INFO] File {0} has been copied to {1}:{2}!", srcFile, ClientIP, destFolder));
			}
		}

		void GenerateAgnetConfig(){
			Dictionary<String,String> agentInfo = new Dictionary<string, string> ();


			agentInfo.Add ("config_type", AgentType);
			agentInfo.Add ("serial_number", UUID);
			agentInfo.Add ("registered_server_ip", ServerIp);
			agentInfo.Add ("registered_server_port", TcpPort);
			agentInfo.Add ("agent_ip", ClientIP);
			agentInfo.Add ("agent_port", ClientTcpPort);
			agentInfo.Add ("cuda_path", ConstValues.DEFAULT_CUDA_FOLDER);
			agentInfo.Add ("nvidia_smi", ConstValues.DEFAULT_PATH_NVIDIA_SMI);
			var agentConfig = Configuration.GenerateAgentConfig (agentInfo);
			SystemCall writeClientConfig = new SystemCall ();
			writeClientConfig.CommandText = @"sshpass";
			bool firstLine = true;
			foreach (String configLine in agentConfig) {
				if (firstLine) {
					writeClientConfig.Parameters = String.Format (@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} 'echo ""{3}"" > {4}'", LoginPass, LoginUser, ClientIP, configLine, ConstValues.DEFAULT_AGENT_CONFIG_PATH);
					firstLine = false;
				} else {
					writeClientConfig.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} 'echo ""{3}"" >> {4}'", LoginPass, LoginUser, ClientIP, configLine, ConstValues.DEFAULT_AGENT_CONFIG_PATH);
				}
				writeClientConfig.CommandExecute ();
				DeployErrorFlag = writeClientConfig.StandError.Trim () != "" ? true : DeployErrorFlag;
			}


		}

		void GenerateRcudaConfig(){
			// If the agent type is no client agent, they must have the gpu resource to share. That's need the rCuda server configuration.
			if (AgentType != "ClientAgent") {
				//FIXME the integer 4 below is not a const, you need get the gpus count first and replace with realtime value.
				List<String> rCudaServerConfig = Configuration.GenerateRcudaServerRuntimeConfig (UUID, 4);

				object content = rCudaServerConfig;
				Type contentType = rCudaServerConfig.GetType ();
				TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
				Protocol uploadrCudaServerConfig = new Protocol (client, ConstValues.PROTOCOL_FN_SAVE_RCUDA_SERVER_CONFIG, contentType, content);
				uploadrCudaServerConfig.Start ();
				if ((String)uploadrCudaServerConfig.ResultObject != "True") {
					DeployErrorFlag = true;
				}
			}
		}

		void SetupAgentAutoStartupScript(){
			SystemCall updateDefaultInit = new SystemCall ();
			updateDefaultInit.CommandText = @"sshpass";
			updateDefaultInit.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} chmod a+x /etc/init.d/matrix-agent", LoginPass, LoginUser, ClientIP);
			updateDefaultInit.CommandExecute ();
			updateDefaultInit.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} update-rc.d -f matrix-agent remove", LoginPass, LoginUser, ClientIP);
			updateDefaultInit.CommandExecute ();
			updateDefaultInit.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} update-rc.d matrix-agent defaults", LoginPass, LoginUser, ClientIP);
			updateDefaultInit.CommandExecute ();
			if (updateDefaultInit.StandOutput.Trim () != "") {
			} else {
				DeployErrorFlag = true;
			}
		}


		void RegisterClientAgent(){
			Dictionary<String, String> agentInfo = new Dictionary<string, string> ();
			agentInfo.Add ("agent_type", AgentType);
			agentInfo.Add ("agent_id", UUID);
			agentInfo.Add ("registered_server_ip", ServerIp);
			agentInfo.Add ("registered_server_port", TcpPort);
			agentInfo.Add ("agent_ip", ClientIP);
			agentInfo.Add ("agent_port", ClientTcpPort);
			agentInfo.Add ("login_user", LoginUser);
			agentInfo.Add ("login_pass", LoginPass);
			agentInfo.Add ("agent_status", "Online");

			object content = agentInfo;
			Type contentType = agentInfo.GetType ();
			TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
			Protocol registAgent = new Protocol (client, ConstValues.PROTOCOL_FN_REGISTER_AGENT, contentType, content);
			registAgent.Start ();
			if ((String)registAgent.ResultObject != "True") {
				DeployErrorFlag = true;
			}
		}

	}
}

