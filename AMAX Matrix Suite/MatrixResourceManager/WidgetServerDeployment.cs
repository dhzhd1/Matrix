using System;
using Gtk;
using System.ComponentModel;
using MatrixLibrary;
using MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Threading;



namespace MatrixResourceManager
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class WidgetServerDeployment : Gtk.Bin
	{
		public string serverIp;
		public string serverHostname;
		public string loginUser;
		public string loginPass;
		public string dbHost;
		public string dbUser;
		public string dbPass;
		public string dbName;
		public string dbPort;
		public string UUID;

		public bool PrecheckErrorFlag = false;
		public bool DeployErrorFlag = false;

		public WidgetServerDeployment ()
		{
			this.Build ();
			button10.Sensitive = false;
			#if DEBUG
			entry1.Text = "157.22.244.236";
			entry2.Text = "amaxhost";
			entry3.Text = "root";
			entry4.Text = "amax1565";
			entry5.Text = "157.22.244.236";
			entry6.Text = "root";
			entry7.Text = "P@ssw0rd";
			#endif
		}

		protected void OnButton11Clicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}

		void UpdateLog(TextView textview, String msg){
			Gdk.Threads.AddIdle (0, () => {
				textview.Buffer.Text += msg + "\n";
				return false;
			});
		}
			
		protected void OnButton9Clicked (object sender, EventArgs e)
		{
			// Deployment Pre-Check Method
			serverIp = entry1.Text;
			serverHostname = entry2.Text;
			loginUser = entry3.Text;
			loginPass = entry4.Text;
			dbHost = entry5.Text;
			dbUser = entry6.Text;
			dbPass = entry7.Text;
			dbName = entry8.Text;
			dbPort = entry9.Text;
			textview1.Buffer.Text = "";
			PrecheckErrorFlag = false;

			BackgroundWorker preCheckWorker = new BackgroundWorker ();
			preCheckWorker.WorkerReportsProgress = true;
			preCheckWorker.WorkerSupportsCancellation = true;
			preCheckWorker.DoWork += PreCheckWorker_DoWork;
			preCheckWorker.RunWorkerCompleted += PreCheckWorker_RunWorkerCompleted;
			preCheckWorker.RunWorkerAsync ();
		}

		void PreCheckWorker_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
		{
			// Maybe some actions will be added.
		}

		void PreCheckWorker_DoWork (object sender, DoWorkEventArgs e)
		{
			UpdateLog (textview1, "[INFO] Start to Check target server @" + DateTime.Now.ToString());
			//Check target server
			CheckLoginCredential ();
			GenerateGUID ();
			DeployFolderCheck (ConstValues.DEFAULT_DEPLOY_PATH);
			DeployFolderCheck (ConstValues.DEFAULT_MATRIX_LOG_PATH);
			DeployFolderCheck (ConstValues.DEFAULT_MATRIX_CONFIG_PATH);

			//Check database
			UpdateLog (textview1, "\n[INFO] Start to Check database server @" + DateTime.Now.ToString());
			DatabaseCheck ();

			if (PrecheckErrorFlag) {
				UpdateLog (textview1, "\n[INFO] Deploy Pre-Chech didn't pass. Please fix the issue and run the pre-check again.");
			} else {
				UpdateLog (textview1, "\n[INFO] Deploy Pre-Chech passed. Matrix could be deployed now.");
				button10.Sensitive = true;
			}
		}

		void CheckLoginCredential (){
			// This function will login the system get uptime to approve the system is working
			SystemCall getUptime = new SystemCall();
			getUptime.CommandText = @"sshpass";
			getUptime.Parameters = String.Format(@" -p {0} ssh -o StrictHostKeyChecking=no {1}@{2} uptime ",  loginPass, loginUser, serverIp);
			getUptime.CommandExecute ();
			if (getUptime.StandError != "") {
				MainClass.log.Error (getUptime.StandError);
				UpdateLog (textview1, "[ERROR] Check Login Credential Failed");
				PrecheckErrorFlag = true;
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
			checkFolder.Parameters = String.Format (@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} ls -d {3}", loginPass, loginUser, serverIp, folderPath);
			checkFolder.CommandExecute ();
			if (checkFolder.StandOutput.Trim () == "") {
				UpdateLog (textview1, String.Format (@"[INFO] Folder {0} doesn't exist. It will be created during the deployment stage.", folderPath));
			} else {
				UpdateLog (textview1, String.Format(@"[WARN] The folder {0} existed. If you decide continue to deploy on this server, the previouse deployment will be wipped.", folderPath));
			}
		}

		void DatabaseCheck(){
			MySqlConnectionStringBuilder connstrbuilder = new MySqlConnectionStringBuilder ();
			connstrbuilder.Server = dbHost;
			connstrbuilder.UserID = dbUser;
			connstrbuilder.Password = dbPass;
			connstrbuilder.Port = uint.Parse(dbPort);
			try {
				using (MySqlConnection conn = new MySqlConnection (connstrbuilder.GetConnectionString (true))){
					conn.Open ();
					conn.Close();
					UpdateLog (textview1, "[INFO] Database Login Testing Validated.");
				}

				try {
					connstrbuilder.Database = dbName;
					using (MySqlConnection conn = new MySqlConnection(connstrbuilder.GetConnectionString(true))){
						conn.Open();
						conn.Close();
						UpdateLog (textview1, String.Format(@"[WARN] Database {0} existing. If you continue to deploy, previous deployment will be wipped.",dbName));
					}
				} catch (Exception) {
					UpdateLog (textview1, String.Format(@"[INFO] Database {0} was not existing, it will be created during the deployment process.", dbName));
				}

			} catch (Exception ex) {
				PrecheckErrorFlag = true;
				UpdateLog (textview1, "[ERROR] Database Login Testing Failed.");
				UpdateLog (textview1, "[DEBUG] " + ex.Message);
				MainClass.log.Debug (ex.StackTrace);
			}
		}

		protected void OnButton10Clicked (object sender, EventArgs e)
		{
			textview2.Buffer.Text = "";
			DeployErrorFlag = false;
			BackgroundWorker deployWorker = new BackgroundWorker ();
			deployWorker.WorkerReportsProgress = true;
			deployWorker.WorkerSupportsCancellation = true;
			deployWorker.DoWork += DeployWorker_DoWork;
			deployWorker.RunWorkerCompleted += DeployWorker_RunWorkerCompleted;
			deployWorker.RunWorkerAsync ();
		}

		void DeployWorker_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
		{
			
		}

		void DeployWorker_DoWork (object sender, DoWorkEventArgs e)
		{
			
			UpdateLog (textview2, "[INFO] Starting Matrix Server Deployment @" + DateTime.Now.ToString ());

			// Create related Folder
			CreateDeployPath (ConstValues.DEFAULT_DEPLOY_PATH);
			CreateDeployPath (ConstValues.DEFAULT_MATRIX_LOG_PATH);
			CreateDeployPath (ConstValues.DEFAULT_MATRIX_CONFIG_PATH);
			if (DeployErrorFlag) {
				UpdateLog (textview2, "[ERROR] Deployment process ended unexpected since prerequired folder Creating failed!");
				return;
			}

			// Copy related file to server folder
			// Server Application file; rCUDA file
			// 1. Copy Server Application to target server
			CopyFilesToServer(ConstValues.DEPLOY_SERVER_APP_SOURCE_PATH, ConstValues.DEFAULT_DEPLOY_PATH);
			// 2. Copy rCuda with Libary 
			CopyFilesToServer(ConstValues.DEPLOY_RCUDA_APP_SOURCE_PATH, ConstValues.DEFAULT_DEPLOY_PATH);
			if (DeployErrorFlag) {
				UpdateLog (textview2, "[ERROR] Deployment process ended unexpected since uploading appliaction to target server failed!");
				return;
			}

			// Initialize Mysql Database
			string dbInitScript = File.ReadAllText (ConstValues.DEPLOY_SQL_INIT_SCRIPT);
			Database dbInstance = new Database (dbHost, dbUser, dbPass, dbName, dbPort);
			dbInstance.Connect ();
			if (dbInstance.InitializeDatabase (dbInitScript)) {
				UpdateLog (textview2, "[INFO] Database has been initialized.");
			} else {
				UpdateLog (textview2, "[ERROR] Deployment process ended unexpected since database initializing failed!");
				return;
			}
			//write server information / Register this server into `information` 
			Dictionary<String,String> serverInfo = new Dictionary<string, string>();
			serverInfo.Add ("service_id", UUID);
			serverInfo.Add ("service_ip", serverIp);
			serverInfo.Add ("service_hostname", serverHostname);
			serverInfo.Add ("db_version", ConstValues.APP_REQUIRED_DB_VERSION);
			if (dbInstance.RegistServer (serverInfo)) {
				UpdateLog (textview2, String.Format(@"[INFO] The Server (UUID:{0}) has been registed into database.",UUID));
			} else {
				UpdateLog (textview2, String.Format(@"[ERROR] Deployment process ended unexpected since Server (UUID:{0} cannot be registed into database.", UUID));
				return;
			}

			//Create systemd service in the targe system to make the Server.exe could be started automatically as long as the system start/reboot.
			// Copy the service startup script to destination.
			CopyFilesToServer(ConstValues.SERVER_DAEMON_STARTUP_SCRIPT, ConstValues.STARTUP_SCRIPT_PATH);
			if (DeployErrorFlag) {
				UpdateLog (textview2, String.Format (@"[ERROR] Deployment process ended unexpected since service startup script upload failed."));
				return;
			} else {
				SystemCall updateDefaultInit = new SystemCall ();
				updateDefaultInit.CommandText = @"sshpass";
				updateDefaultInit.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} chmod a+x /etc/init.d/matrix-server", loginPass, loginUser, serverIp);
				updateDefaultInit.CommandExecute ();
				updateDefaultInit.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} update-rc.d -f matrix-server remove", loginPass, loginUser, serverIp);
				updateDefaultInit.CommandExecute ();
				updateDefaultInit.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} update-rc.d matrix-server defaults", loginPass, loginUser, serverIp);
				updateDefaultInit.CommandExecute ();
				if (updateDefaultInit.StandOutput.Trim () != "") {
					UpdateLog(textview2, String.Format(@"[INFO] Matrix Server will be started as long as the system startup."));
				}
			}


			// Generate Server Startup Configuration and upload to server
			Dictionary<String,String> configInfo = new Dictionary<string, string>();
			configInfo.Add ("serial_number", UUID);
			configInfo.Add ("database_host", dbHost);
			configInfo.Add ("database_user", dbUser);
			configInfo.Add ("database_pass", dbPass);
			configInfo.Add ("database_name", dbName);
			configInfo.Add ("database_port", dbPort);
			configInfo.Add ("service_ip", serverIp);
			configInfo.Add ("service_port", "14928");  // Int the future release will allow customer to change port
			List<String> serverConfig = Configuration.GenerateServerConfig(configInfo);
			SystemCall writeServerConfig = new SystemCall ();
			writeServerConfig.CommandText = @"sshpass";
			bool firstLine = true;
			foreach (String configLine in serverConfig) {
				if (firstLine) {
					writeServerConfig.Parameters = String.Format (@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} 'echo ""{3}"" > {4}'", loginPass, loginUser, serverIp, configLine, ConstValues.DEFAULT_SERVER_CONFIG_PATH);
					firstLine = false;
				} else {
					writeServerConfig.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} 'echo ""{3}"" >> {4}'", loginPass, loginUser, serverIp, configLine, ConstValues.DEFAULT_SERVER_CONFIG_PATH);
				}
				writeServerConfig.CommandExecute ();
				DeployErrorFlag = writeServerConfig.StandError.Trim () != "" ? true : DeployErrorFlag;
			}
			if (DeployErrorFlag) {
				UpdateLog (textview2, "[ERROR] Deployment process ended unexpected since uploading server configuration to target server failed!");
				return;
			} else {
				UpdateLog (textview2, String.Format("[INFO] Server configuration has been upload to {0}.", ConstValues.DEFAULT_SERVER_CONFIG_PATH));
			}

			// Start the service remotely and check if it running.
			UpdateLog (textview2, "\n[INFO] Starting Remote Matrix Server...");
			SystemCall startRemoteServer = new SystemCall();
			startRemoteServer.CommandText = @"sshpass";
			startRemoteServer.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} '{3}/Server.exe &'", loginPass, loginUser, serverIp, ConstValues.DEFAULT_DEPLOY_PATH);
			//startRemoteServer.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} /etc/init.d/matrix-server restart", loginPass, loginUser, serverIp);
			startRemoteServer.CommandExecute ();
			Thread.Sleep (8000);
			UpdateLog (textview2, "\n[INFO] Checking Remote Matrix Server...");
			SystemCall checkRemoteServer = new SystemCall ();
			checkRemoteServer.CommandText = @"sshpass";
			checkRemoteServer.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} netstat -tln | grep 14928 ", loginPass, loginUser, serverIp);
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
				button10.Sensitive = false;
			}
		}

		void CreateDeployPath (String dirPath){
			SystemCall createPath = new SystemCall ();
			createPath.CommandText = @"sshpass";
			createPath.Parameters = String.Format(@"-p {0} ssh -o StrictHostKeyChecking=no {1}@{2} mkdir -p {3}", loginPass, loginUser, serverIp, dirPath);
			createPath.CommandExecute ();
			if (createPath.StandError.Trim() != "") {
				UpdateLog (textview2, String.Format (@"[ERROR] Create {0} failed on target server!", dirPath));
				DeployErrorFlag = true;
				UpdateLog (textview2, @"[DEBUG] " + createPath.StandError);
			} else {
				UpdateLog (textview2, String.Format (@"[INFO] Folder {0} has been created on target server!", dirPath));
			}
		}

		void CopyFilesToServer(String srcFile, String destFolder){
			SystemCall copyFile = new SystemCall ();
			copyFile.CommandText = @"sshpass";
			copyFile.Parameters = String.Format(@"-p {0} scp -r -o StrictHostKeyChecking=no {1} {2}@{3}:{4}", loginPass, srcFile, loginUser, serverIp, destFolder);
			copyFile.CommandExecute ();
			if (copyFile.StandError.Trim () != "") {
				UpdateLog (textview2, String.Format (@"[ERROR] Copy file {0} to {1}:{2} failed!", srcFile, serverIp ,destFolder));
				DeployErrorFlag = true;
				UpdateLog (textview2, @"[DEBUG] " + copyFile.StandError);
			} else {
				UpdateLog (textview2, String.Format (@"[INFO] File {0} has been copied to {1}:{2}!", srcFile, serverIp, destFolder));
			}
		}
	}
}

