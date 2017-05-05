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
using MySql.Data.MySqlClient;
using MySql;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Collections;


namespace MatrixLibrary
{
	public class Database
	{
		public MySqlConnection Instance { get; set; }
		public String DatabaseName { get; set; }
		public String Password { get; set; }
		public String UserId { get; set; }
		public String Host { get; set; }
		public String Port { get; set; }

		String connectString = string.Empty;

		public Database ()
		{

		}

		public Database(String dbHost, String userId, String dbPass, String dbName, String dbPort){
			DatabaseName = dbName;
			Password = dbPass;
			UserId = userId;
			Host = dbHost;
			Port = dbPort;
		}

		public Database(Dictionary<String,String> dbInfo){
			DatabaseName = dbInfo["database_name"];
			Password = dbInfo["database_pass"];
			UserId = dbInfo["database_user"];
			Host = dbInfo["database_host"];
			Port = dbInfo ["database_port"];
		}

		public MySqlConnection Connect(){
			try {
				Instance = new MySqlConnection (CreateDatabaseConnectionString ());
				return Instance;
			} catch (MySqlException ex) {
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				return null;
			}
		}

		String CreateDatabaseConnectionString(){
			MySqlConnectionStringBuilder strBuilder = new MySqlConnectionStringBuilder ();
			strBuilder.Database = DatabaseName;
			strBuilder.Server = Host;
			strBuilder.UserID = UserId;
			strBuilder.Password = Password;
			strBuilder.Pooling = false;
			strBuilder.Port = uint.Parse (Port);
			connectString = strBuilder.GetConnectionString(true);
			return connectString;
		}

		public bool IfAgentRegisted(String agentId){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			int count = 0;
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"select count(*) as `count` from `agentinfo` where `agent_id`='{0}'", agentId);
			using (MySqlDataReader reader = cmd.ExecuteReader ()) {
				reader.Read ();
				count = Int32.Parse (reader ["count"].ToString());
			}
			Instance.Close ();
			if (count == 1) {
				return true;
			} else
				return false;
		}

		public bool IfServerRegisted(String serviceId){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			string server_id = string.Empty;
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format (@"SELECT `key`, `value` FROM `information` WHERE `key`='service_id'");
			try {
				using (MySqlDataReader reader = cmd.ExecuteReader ()) {
					reader.Read ();
					server_id = reader ["value"].ToString ();
				}
				if (server_id == serviceId) {
					return true;
				} else {
					return false;
				}
			} catch (Exception ex) {
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				return false;
			}


		}

		public bool UpsertGpuInfo(List<Dictionary<String,String>> gpuInfo){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			int count = 0;
			foreach (var gpu in gpuInfo) {
				MySqlCommand cmd = Instance.CreateCommand ();
				cmd.CommandText = String.Format(@"INSERT INTO `resourceinfo` (`gpu_uuid`, `agent_id`, `gpu_id`, `gpu_model`, `gpu_xml_info`,`assign_type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}','Share') ON DUPLICATE KEY UPDATE `agent_id`='{5}', `gpu_id`='{6}', `gpu_model`='{7}', `gpu_xml_info`='{8}', `assign_type`='Share'", gpu["uuid"], gpu["agent_id"], Int32.Parse(gpu["gpuid"]), gpu["prodname"], gpu["xml"], gpu["agent_id"], Int32.Parse(gpu["gpuid"]), gpu["prodname"], gpu["xml"]);
				if (cmd.ExecuteNonQuery () == 1) {
					count++;
				}
			}

			if (count == gpuInfo.Count) {
				return true;
			} else {
				return false;
			}
		}

		public bool UpsertFabricInfo(List<Dictionary<String,String>> fabricInfo) {
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			int count = 0;
			foreach (var fabric in fabricInfo) {
				MySqlCommand cmd = Instance.CreateCommand ();
				cmd.CommandText = String.Format (@"INSERT INTO `fabricinfo` (`mac_addr`, `agent_id`, `ip_addr`, `nic_name`, `link_speed`, `op_status`, `gateway_ip`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}') ON DUPLICATE KEY UPDATE `agent_id`='{7}', `ip_addr`='{8}', `nic_name`='{9}', `link_speed`='{10}', `op_status`='{11}', `gateway_ip`='{12}'", fabric ["mac_addr"], fabric ["agent_id"], fabric ["ip_addr"], fabric ["nic_name"], fabric ["link_speed"], fabric ["op_status"], fabric ["gateway_ip"], fabric ["agent_id"], fabric ["ip_addr"], fabric ["nic_name"], fabric ["link_speed"], fabric ["op_status"], fabric ["gateway_ip"]);
				if (cmd.ExecuteNonQuery () == 1) {
					count++;
				}
			}
			if (count == fabricInfo.Count) {
				return true;
			} else {
				return false;
			}
		}


		public bool RegistServer(Dictionary<String, String> ServerInfo){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			int count = 0;
			MySqlCommand cmd = Instance.CreateCommand ();
			foreach (var item in ServerInfo) {
				cmd.CommandText = String.Format(@"INSERT INTO `information` (`key`, `value`) VALUES ('{0}', '{1}')", item.Key, item.Value);
				if (cmd.ExecuteNonQuery () == 1) {
					count++;
				}
			}
			if (count == ServerInfo.Count) {
				return true;
			} else {
				return false;
			}
		}

		public bool UpdateAgentStatus(String agentId, String status){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			var cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"UPDATE `agentinfo` SET `agent_status`='{0}' WHERE `agent_id`='{1}'",status, agentId);
			if (cmd.ExecuteNonQuery () == 1) {
				return true;
			} else {
				return false;
			}
		}

		public bool GetRcudaServerConfig(String agentId, out List<String> configStr){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			var cmd = Instance.CreateCommand ();
			List<String> config = new List<string> ();
			cmd.CommandText = String.Format(@"SELECT `rcuda_server_conf` FROM `runtimeconf` WHERE `agent_id`='{0}'", agentId);
			using (MySqlDataReader reader = cmd.ExecuteReader ()) {
				try {
					reader.Read();
					String[] resultArray = reader[0].ToString().Split('\n');
					foreach (var item in resultArray) {
						config.Add(item);
					}
					configStr = config;
					return true;
				} catch (Exception ex) {
					MatrixLibrary.log.Info ("No record found!");
					MatrixLibrary.log.Debug (ex.Message);
					MatrixLibrary.log.Debug (ex.StackTrace);
					configStr = new List<string> ();;
					return false;
				}
			}
		}

		public bool GetRcudaClientConfig(String agentId, out List<String> configStr){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			var cmd = Instance.CreateCommand ();
			List<String> config = new List<string> ();
			cmd.CommandText = String.Format(@"SELECT `rcuda_client_conf` FROM `runtimeconf` WHERE `agent_id`='{0}'", agentId);
			using (MySqlDataReader reader = cmd.ExecuteReader ()) {
				try {
					reader.Read();
					String[] resultArray = reader[0].ToString().Split('\n');
					foreach (var item in resultArray) {
						config.Add(item);
					}
					configStr = config;
					return true;
				} catch (Exception ex) {
					MatrixLibrary.log.Info("No record found!");
					MatrixLibrary.log.Debug (ex.Message);
					MatrixLibrary.log.Debug (ex.StackTrace);
					configStr = new List<String> ();
					return false;
				}
			}
		}

		public bool InitializeDatabase(String sqlScript){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			MySqlScript initDatabase = new MySqlScript (Instance, sqlScript);
			initDatabase.Error+= InitDatabase_Error;
			var result = initDatabase.Execute ();
			Debug.WriteLine ("script:" + result.ToString ());
			Instance.Close ();
			return true;

		}

		void InitDatabase_Error (object sender, MySqlScriptErrorEventArgs args)
		{
			MatrixLibrary.log.Error (args.Exception.Message);
			MatrixLibrary.log.Debug (args.Exception.StackTrace);
		}

		public bool SaveRcudaSrvConfig (String agentId, List<String> config){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"INSERT INTO `runtimeconf` (`agent_id`, `rcuda_server_conf`) VALUES ('{0}', '{1}') ON DUPLICATE KEY UPDATE `rcuda_server_conf`='{1}'", agentId, String.Join("\n",config));
			if (cmd.ExecuteNonQuery() == 1) {
				Instance.Close ();
				return true;
			} else {
				Instance.Close ();
				return false;
			}
		}


		public bool SaveRcudaClientConfig (String agentId, List<String> config){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"INSERT INTO `runtimeconf` (`agent_id`, `rcuda_client_conf`) VALUES ('{0}', '{1}') ON DUPLICATE KEY UPDATE `rcuda_client_conf`='{1}'", agentId, String.Join("\n",config));
			if (cmd.ExecuteNonQuery() == 1) {
				Instance.Close ();
				return true;
			} else {
				Instance.Close ();
				return false;
			}
		}


		public bool RegistAgent(Dictionary<String, String> agentInfo){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"INSERT INTO `agentinfo` (`agent_id`, `agent_type`, `login_user`, `login_pass`, `agent_ip`, `agent_port`, `agent_status`, `registered_server_ip`, `registered_server_port`) VALUES ('{0}','{1}', '{2}', '{3}','{4}','{5}','{6}','{7}','{8}') ON DUPLICATE KEY UPDATE `agent_type`='{9}', `login_user`='{10}', `login_pass`='{11}', `agent_ip`='{12}', `agent_port`='{13}', `agent_status`='{14}', `registered_server_ip`='{15}', `registered_server_port`='{16}'",agentInfo["agent_id"], agentInfo["agent_type"], agentInfo["login_user"], agentInfo["login_pass"], agentInfo["agent_ip"], Int32.Parse(agentInfo["agent_port"]), agentInfo["agent_status"], agentInfo["registered_server_ip"], Int32.Parse(agentInfo["registered_server_port"]), agentInfo["agent_type"], agentInfo["login_user"], agentInfo["login_pass"], agentInfo["agent_ip"], Int32.Parse(agentInfo["agent_port"]), agentInfo["agent_status"], agentInfo["registered_server_ip"], Int32.Parse(agentInfo["registered_server_port"]));
			if (cmd.ExecuteNonQuery () == 1) {
				Instance.Close ();
				return true;
			} else {
				Instance.Close ();
				return false;
			}
		}

		public List<Dictionary<String,String>> GetAgentList (){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			List<Dictionary<string,string>> result = new List<Dictionary<string, string>> ();
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = @"SELECT * FROM `agentinfo`";
			try {
				using (MySqlDataReader reader = cmd.ExecuteReader ()) {
					while(reader.Read()){
						Dictionary<String, String> rowRecord = new Dictionary<string, string>();
						rowRecord.Add("agent_id", reader["agent_id"].ToString());
						rowRecord.Add("agent_type", reader["agent_type"].ToString());
						rowRecord.Add("agent_ip", reader["agent_ip"].ToString());
						rowRecord.Add("agent_port", reader["agent_port"].ToString());
						rowRecord.Add("agent_status", reader["agent_status"].ToString());
						rowRecord.Add("memo", reader["memo"].ToString());
						result.Add(rowRecord);
					}
				}
				Instance.Close();
				return result;
			} catch (Exception ex) {
				MatrixLibrary.log.Info ("No agent found!");
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				Instance.Close ();
				return result = new List<Dictionary<String,String>> ();
			}
		}

		public List<Dictionary<String,String>> GetGpuResourceList(String agentId){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			List<Dictionary<String,String>> result = new List<Dictionary<string, string>> ();
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format (@"SELECT * FROM `resourceinfo` WHERE `agent_id`='{0}'", agentId);
			try {
				using (MySqlDataReader reader = cmd.ExecuteReader()){
					while (reader.Read()) {
						Dictionary<String,String> rowRecord = new Dictionary<string, string>();
						rowRecord.Add("gpu_uuid",reader["gpu_uuid"].ToString());
						rowRecord.Add("agent_id", reader["agent_id"].ToString());
						rowRecord.Add("gpu_id", reader["gpu_id"].ToString());
						rowRecord.Add("gpu_model", reader["gpu_model"].ToString());
						//rowRecord.Add("gpu_xml_info", reader["gpu_xml_info"].ToString());
						rowRecord.Add("assign_type", reader["assign_type"].ToString());
						rowRecord.Add("memo", reader["memo"].ToString());
						result.Add(rowRecord);
					}
				}
				Instance.Close();
				return result;
			} catch (Exception ex) {
				Instance.Close ();
				MatrixLibrary.log.Info ("No GPU card record found");
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				return result = new List<Dictionary<string, string>> ();
			}
		}


		public List<Dictionary<String, String>> GetFabricResourceList(String agentId){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			List<Dictionary<String,String>> result = new List<Dictionary<string, string>> ();
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"SELECT `mac_addr`, `agent_id`, `ip_addr`, `nic_name`, `link_speed`, `op_status`, `memo` FROM `fabricinfo` WHERE `agent_id`='{0}'",agentId);
			try {
				using (MySqlDataReader reader = cmd.ExecuteReader ()) {
					while (reader.Read ()) {
						Dictionary<String,String> rowRecord = new Dictionary<string, string> ();
						rowRecord.Add ("mac_addr", reader ["mac_addr"].ToString ());
						rowRecord.Add ("agent_id", reader ["agent_id"].ToString ());
						rowRecord.Add ("ip_addr", reader ["ip_addr"].ToString ());
						rowRecord.Add ("nic_name", reader ["nic_name"].ToString ());
						rowRecord.Add ("link_speed", reader ["link_speed"].ToString ());
						rowRecord.Add ("op_status", reader ["op_status"].ToString ());
						rowRecord.Add("memo", reader["memo"].ToString());
						result.Add (rowRecord);
					}
				}
				Instance.Close ();
				return result;
			} catch (Exception ex) {
				Instance.Close ();
				MatrixLibrary.log.Info ("No Fabric record found");
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				return result = new List<Dictionary<string, string>> ();
			}
		}


//		public 	List<Dictionary<String, String>> GetAssignedGpuByClient(String agentId){
//			if (Instance.State != ConnectionState.Open) {
//				Instance.Open ();
//			}
//			List<Dictionary<String,String>> result = new List<Dictionary<string, string>> ();
//			List<String> ipList = new List<string> ();
//			MySqlCommand cmd = Instance.CreateCommand ();
//			cmd.CommandText = String.Format(@"SELECT `ip_addr`  FROM `fabricinfo` WHERE `ip_addr` IS NOT NULL AND TRIM(`ip_addr`) <> '' AND `agent_id`='{0}'",agentId);
//			try {
//				using (MySqlDataReader reader = cmd.ExecuteReader ()) {
//					while (reader.Read ()) {
//						ipList.Add (reader ["ip_addr"].ToString ());
//					}
//				}
//			} catch (Exception ex) {
//				Instance.Close ();
//				MatrixLibrary.log.Info ("No Ip address found");
//				MatrixLibrary.log.Debug (ex.Message);
//				MatrixLibrary.log.Debug (ex.StackTrace);
//				return result;
//			}
//			if (ipList.Count > 0) {
//				foreach (var ipAddr in ipList) {
//					cmd.CommandText = String.Format(@"SELECT 
//							    `resourceinfo`.`gpu_id`,
//							    `resourceinfo`.`gpu_uuid`,
//							    `resourceinfo`.`gpu_model`,
//							    `resourceinfo`.`agent_id`,
//							    `resourceinfo`.`assign_status`,
//							    `fabricinfo`.`ip_addr`,
//							    `fabricinfo`.`link_speed`,
//							    `resourceinfo`.`assign_list`
//							FROM
//							    `resourceinfo`
//							        LEFT JOIN
//							    `agentinfo` ON `resourceinfo`.`agent_id` = `agentinfo`.`agent_id`
//							        LEFT JOIN
//							    `fabricinfo` ON `fabricinfo`.`ip_addr` = '{0}'
//							WHERE
//							    FIND_IN_SET('{0}',
//							            `resourceinfo`.`assign_list`)", ipAddr);
//					try {
//						using (MySqlDataReader reader = cmd.ExecuteReader ()) {
//							while (reader.Read ()) {
//								Dictionary<String,String> rowRecord = new Dictionary<string, string> ();
//								rowRecord.Add ("gpu_id", reader ["gpu_id"].ToString ());
//								rowRecord.Add ("gpu_uuid", reader ["gpu_uuid"].ToString ());
//								rowRecord.Add ("gpu_model", reader ["gpu_model"].ToString ());
//								rowRecord.Add ("agent_id", reader ["agent_id"].ToString ());
//								rowRecord.Add ("assign_status", reader ["assign_status"].ToString ());
//								rowRecord.Add ("ip_addr", reader ["ip_addr"].ToString ());
//								rowRecord.Add ("link_speed", reader ["link_speed"].ToString ());
//								rowRecord.Add ("assign_list", reader ["assign_list"].ToString ());
//								result.Add (rowRecord);
//							}
//						}
//					} catch (Exception ex) {
//						Instance.Close ();
//						MatrixLibrary.log.Info ("No Ip address found");
//						MatrixLibrary.log.Debug (ex.Message);
//						MatrixLibrary.log.Debug (ex.StackTrace);
//						return result;
//					}
//				}
//			}
//			Instance.Close ();
//			return result;
//		}

		public List<Dictionary<String,String>> GetAssignedGpuByClient(String agentId){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			List<Dictionary<String, String>> result = new List<Dictionary<string, string>> ();
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"SELECT 
								    `assignment`.`agent_id`,
								    `assignment`.`gpu_uuid`,
								    `assignment`.`resource_ip`,
								    `resourceinfo`.`assign_type`,
								    `agentinfo`.`agent_ip`,
								    `resourceinfo`.`gpu_id`,
								    `resourceinfo`.`gpu_model`,
								    `fabricinfo`.`link_speed`
								FROM
								    `assignment`
								        LEFT JOIN
								    `agentinfo` ON `agentinfo`.`agent_id` = `assignment`.`agent_id`
								        LEFT JOIN
								    `resourceinfo` ON `resourceinfo`.`gpu_uuid` = `assignment`.`gpu_uuid`
								        LEFT JOIN
								    `fabricinfo` ON `fabricinfo`.`ip_addr` = `assignment`.`resource_ip`
								WHERE `assignment`.`agent_id`='{0}'",agentId);
			try {
				using (MySqlDataReader reader = cmd.ExecuteReader ()) {
					while (reader.Read ()) {
						Dictionary<String,String> rowRecord = new Dictionary<string, string> ();
						rowRecord.Add ("agent_id", reader ["agent_id"].ToString ());
						rowRecord.Add ("gpu_uuid", reader ["gpu_uuid"].ToString ());
						rowRecord.Add ("resource_ip", reader ["resource_ip"].ToString ());
						rowRecord.Add ("assign_type", reader ["assign_type"].ToString ());
						rowRecord.Add ("agent_ip", reader ["agent_ip"].ToString ());
						rowRecord.Add ("gpu_id", reader ["gpu_id"].ToString ());
						rowRecord.Add ("gpu_model", reader ["gpu_model"].ToString ());
						rowRecord.Add ("link_speed", reader ["link_speed"].ToString ());
						result.Add (rowRecord);
					}
				}
			} catch (Exception ex) {
				Instance.Close ();
				MatrixLibrary.log.Info ("No GPU Infomation found");
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				return result;
			}
			Instance.Close ();
			return result;

		}


		public List<Dictionary<String,String>> GetGpuViewList(){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			List<Dictionary<String,String>> result = new List<Dictionary<string, string>> ();
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = @"SELECT 
								    `resourceinfo`.`gpu_uuid`,
								    `resourceinfo`.`agent_id`,
								    `resourceinfo`.`gpu_id`,
								    `resourceinfo`.`gpu_model`,
								    `agentinfo`.`agent_status`,
								    `a`.`ip_list`,
								    `a`.`link_speed`,
								    `resourceinfo`.`assign_type`,
								    `b`.`assign_list`
								FROM
								    `resourceinfo`
								        LEFT JOIN
								    `agentinfo` ON `agentinfo`.`agent_id` = `resourceinfo`.`agent_id`
								        LEFT JOIN
								    (SELECT 
								        `agent_id`,
								            GROUP_CONCAT(`ip_addr`) AS `ip_list`,
								            GROUP_CONCAT(ROUND(CAST(`link_speed` AS SIGNED) / 1000000), ' Gbps') AS `link_speed`
								    FROM
								        `fabricinfo`
								    WHERE
								        `ip_addr` IS NOT NULL
								            AND TRIM(`ip_addr`) <> ''
								            AND `op_status` = 'Up'
								    GROUP BY `agent_id`) AS `a` ON `resourceinfo`.`agent_id` = `a`.`agent_id`
								        LEFT JOIN
								    (SELECT 
								        `gpu_uuid`, GROUP_CONCAT(`agent_id`) AS `assign_list`
								    FROM
								        `assignment`
								    GROUP BY `assignment`.`gpu_uuid`) AS `b` ON `b`.`gpu_uuid` = `resourceinfo`.`gpu_uuid`";
			try {
				using (MySqlDataReader reader = cmd.ExecuteReader ()) {
					while (reader.Read ()) {
						Dictionary<String,String> rowRecord = new Dictionary<string, string> ();
						rowRecord.Add ("gpu_uuid", reader ["gpu_uuid"].ToString ());
						rowRecord.Add ("gpu_id", reader ["gpu_id"].ToString ());
						rowRecord.Add ("gpu_model", reader ["gpu_model"].ToString ());
						rowRecord.Add ("assign_type", reader ["assign_type"].ToString ());
						rowRecord.Add ("assign_list", reader ["assign_list"].ToString ());
						rowRecord.Add ("agent_id", reader ["agent_id"].ToString ());
						rowRecord.Add ("ip_list", reader ["ip_list"].ToString ());
						rowRecord.Add ("link_speed", reader ["link_speed"].ToString ());
						rowRecord.Add ("agent_status", reader ["agent_status"].ToString ());
						result.Add (rowRecord);
					}
				}
				Instance.Close();
				return result;
			} catch (Exception ex) {
				Instance.Close ();
				MatrixLibrary.log.Info ("No GPU record found");
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				return result = new List<Dictionary<string, string>> ();
			}
		}

		public bool CheckGpuExclusiveable(String gpuUUID){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			bool result = true;
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"SELECT 
								    GROUP_CONCAT(`agent_id`) AS `client_list`
								FROM
								    `assignment`
								WHERE
								    `gpu_uuid` = '{0}'
								GROUP BY `gpu_uuid`", gpuUUID);
			try {
				using (MySqlDataReader reader = cmd.ExecuteReader ()) {
					reader.Read ();
					if (reader ["client_list"].ToString ().Trim () != "") {
						result = false;
					}
				}
			} catch (Exception ex) {
				Instance.Close ();
				MatrixLibrary.log.Info ("no gpu found with this uuid");
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
			}
			Instance.Close ();
			return result;
		}


		public bool UpdateGpuAssignment(List<Dictionary<String,String>> assignmentChanges){
			//update the assignment changes 
			// use the transaction to update these changes. rolling back if any error happened.
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			MySqlTransaction sqlTr = Instance.BeginTransaction ();
			try {
				foreach (var assignChange in assignmentChanges) {
					MySqlCommand cmd = Instance.CreateCommand();
					cmd.Transaction = sqlTr;
					if (assignChange ["action"] == "add") {
						cmd.CommandText = String.Format (@"INSERT IGNORE INTO `assignment` (`agent_id`, `gpu_uuid`, `resource_ip`) VALUES ('{0}', '{1}', '{2}')", assignChange ["agent_id"], assignChange ["gpu_uuid"], assignChange ["resource_ip"]);
						cmd.ExecuteNonQuery ();
						MatrixLibrary.log.Debug(cmd.CommandText);
						cmd.CommandText = String.Format (@"UPDATE `resourceinfo` SET `assign_type`='{0}' WHERE `gpu_uuid`='{1}'", assignChange ["assign_type"], assignChange ["gpu_uuid"]);
						cmd.ExecuteNonQuery ();
						MatrixLibrary.log.Debug(cmd.CommandText);
					} else if (assignChange ["action"] == "remove") {
						cmd.CommandText = String.Format (@"DELETE FROM `assignment` WHERE `agent_id`='{0}' AND `gpu_uuid`='{1}' AND `resource_ip`='{2}'", assignChange ["agent_id"], assignChange ["gpu_uuid"], assignChange ["resource_ip"]);
						cmd.ExecuteNonQuery ();
						MatrixLibrary.log.Debug(cmd.CommandText);
						cmd.CommandText = String.Format (@"UPDATE `resourceinfo` SET `assign_type`='Share' WHERE `gpu_uuid`='{0}'", assignChange ["gpu_uuid"]);
						cmd.ExecuteNonQuery ();
						MatrixLibrary.log.Debug(cmd.CommandText);

					}
				}
				sqlTr.Commit();
				Instance.Close ();
				return true;
			} catch (Exception ex) {
				sqlTr.Rollback ();
				Instance.Close ();
				MatrixLibrary.log.Error ("Transaction of update assignment change failed");
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				Instance.Close ();
				return false;
			}

		}


		public List<Dictionary<String,String>> GetClientListByGpu(String gpuUUID){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			MatrixLibrary.log.Debug ("GPUUUID: " + gpuUUID);
			List<Dictionary<String,String>> result = new List<Dictionary<string, string>> ();
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"SELECT 
							    `assignment`.`agent_id`,
							    `assignment`.`gpu_uuid`,
							    `assignment`.`resource_ip`,
							    `agentinfo`.`agent_ip`,
							    CONCAT(ROUND(CAST(`fabricinfo`.`link_speed` AS SIGNED) / 1000000),' Gbps') AS `link_speed`
							FROM
							    `assignment`
							        LEFT JOIN
							    `agentinfo` ON `agentinfo`.`agent_id` = `assignment`.`agent_id`
							        LEFT JOIN
							    `fabricinfo` ON `assignment`.`resource_ip` = `fabricinfo`.`ip_addr`
							WHERE
							    `assignment`.`gpu_uuid` = '{0}'", gpuUUID);
			try {
				using (MySqlDataReader reader = cmd.ExecuteReader ()) {
					while (reader.Read ()) {
						Dictionary<String,String> rowRecord = new Dictionary<string, string> ();
						rowRecord.Add ("agent_id", reader ["agent_id"].ToString ());
						rowRecord.Add ("gpu_uuid", reader ["gpu_uuid"].ToString ());
						rowRecord.Add ("resource_ip", reader ["resource_ip"].ToString ());
						rowRecord.Add ("agent_ip", reader ["agent_ip"].ToString ());
						rowRecord.Add ("link_speed", reader ["link_speed"].ToString ());
						result.Add (rowRecord);
					}
				}
				Instance.Close();
				return result;
			} catch (Exception ex) {
				Instance.Close ();
				MatrixLibrary.log.Error (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				return result = new List<Dictionary<string, string>> ();
			}

		}


		public String GetGpuXmlInfo(String gpuUUID){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			String result = string.Empty;
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"SELECT `gpu_xml_info` FROM `resourceinfo` WHERE `gpu_uuid`='{0}'", gpuUUID);
			using (MySqlDataReader reader = cmd.ExecuteReader ()) {
				reader.Read ();
				result = reader ["gpu_xml_info"].ToString ();
			}
			Instance.Close ();
			return result;
		}

		public bool CheckGpuAssignStatus(String agentId){
			// This agentid is the resource agent's id
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			MySqlCommand cmd = Instance.CreateCommand ();
			cmd.CommandText = String.Format(@"SELECT 
								COUNT(`assignment`.`gpu_uuid`) AS `assigned_count`
								FROM
								`assignment`
								LEFT JOIN
								`resourceinfo` ON `resourceinfo`.`gpu_uuid` = `assignment`.`gpu_uuid`
								WHERE
								`resourceinfo`.`agent_id` = '{0}'", agentId);
			int assignCount=0;
			using (MySqlDataReader reader = cmd.ExecuteReader ()) {
				reader.Read ();
				assignCount = int.Parse (reader ["assigned_count"].ToString ());
			}
			Instance.Close ();
			if (assignCount > 0) {
				return true;
			} else {
				return false;
			}
						
		}

		public bool RemoveAgent(String agentId){
			if (Instance.State != ConnectionState.Open) {
				Instance.Open ();
			}
			MySqlTransaction sqlTr = Instance.BeginTransaction ();
			try {
				MySqlCommand cmd = Instance.CreateCommand ();
				cmd.Transaction = sqlTr;
				cmd.CommandText = String.Format("DELETE FROM `agentinfo` WHERE `agent_id` = '{0}'",agentId);
				MatrixLibrary.log.Debug(cmd.CommandText);
				cmd.ExecuteNonQuery();
				cmd.CommandText = String.Format("DELETE FROM `fabricinfo` WHERE `agent_id` = '{0}'", agentId);
				MatrixLibrary.log.Debug(cmd.CommandText);
				cmd.ExecuteNonQuery();
				cmd.CommandText = String.Format("DELETE FROM `resourceinfo` WHERE `agent_id` = '{0}'",agentId);
				MatrixLibrary.log.Debug(cmd.CommandText);
				cmd.ExecuteNonQuery();
				sqlTr.Commit();
				Instance.Close ();
				return true;
			} catch (Exception ex) {
				sqlTr.Rollback ();
				Instance.Close ();
				MatrixLibrary.log.Error ("Transaction of remove agent failed");
				MatrixLibrary.log.Debug (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
				Instance.Close ();
				return false;
			}

		}


		public Dictionary<int,Dictionary<double, int>> GetMetricData(Dictionary<int, string> gpuUUIDList, int metrixId, string endTime, int timefram){
			// TODO finish this
			if (Instance.State != ConnectionState.Open ) {
				Instance.Open ();
			}
			Dictionary<int, Dictionary<double,int>> result = new Dictionary<int, Dictionary<double, int>> ();
			string viewTableName;
			switch (metrixId) {
			case 0:
				viewTableName = "v_gpu_utilization";
				break;
			case 1:
				viewTableName = "v_gpu_memoryutilization";
				break;
			case 2:
				viewTableName = "v_gpu_powerdraw";
				break;
			case 3:
				viewTableName = "v_gpu_temperature";
				break;
			default:
				break;
			}

			return result;

		}

	}

}

