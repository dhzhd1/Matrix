using System;
using Gtk;
using System.Net.Sockets;
using MatrixLibrary;
using System.Threading;
using GLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Xsl.Runtime;
using System.Net;
using MySql.Data.Types;

namespace MatrixResourceManager
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class WidgetAgentsView : Gtk.Bin
	{
		string ServerIP = string.Empty;
		String TcpPort = String.Empty;
		List<String> agentListColumn = new List<string>() {"Agent IP", "Agent ID", "Agent Type", "Service Port", "Status", "Memo"};
		List<String> gpuInfoColumn = new List<string> () {"GPU ID", "GPU Model", "GPU Assign Status", "GPU UUID","Memo" };
		List<String> fabricInfoColumn = new List<string> () { "NIC Name", "IP Address","Mac Address", "Link Speed", "Status", "Memo" };
		List<String> assignmentColumn = new List<string> () {"Resource IP", "GPU ID", "GPU Model", "GPU UUID", "Link Speed", "Assign Status" };
		List<String> runtimeConfigColumn = new List<string> () {"Command"};



		public WidgetAgentsView (String serverIp, String tcpPort)
		{
			this.Build ();
			ServerIP = serverIp;
			TcpPort = tcpPort;
			LoadAgentInfo ();
		}

		void LoadAgentInfo(){
			foreach (var column in treeview1.Columns) {
				treeview1.RemoveColumn (column);
			}
			for (int i = 0; i< agentListColumn.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = agentListColumn[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview1.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);

			}

			TcpClient client = new TcpClient (ServerIP, Int32.Parse(TcpPort));
			String agentID = "all-agents";
			object content = agentID;
			Type contentType = agentID.GetType ();
			Protocol getAgentList = new Protocol (client, ConstValues.PROTOCOL_FN_GET_AGENT_LIST, contentType, content);
			getAgentList.Start ();
			var agentList = getAgentList.ResultObject as List<Dictionary<String,String>>;
			if (agentList != null) {
				// implement the agent list on agent list tree view.
				Gtk.TreeStore agentListView = new TreeStore (typeof(String), typeof(String), typeof(String), typeof(String), typeof(String), typeof(String));
				foreach (var agentItem in agentList) {
					agentListView.AppendValues (agentItem["agent_ip"], agentItem["agent_id"], agentItem["agent_type"], agentItem["agent_port"], agentItem["agent_status"], agentItem["memo"]);
				}
				treeview1.Model = agentListView;
				treeview1.ExpandAll ();
			}

		}


		protected void OnTreeview1RowActivated (object o, RowActivatedArgs args)
		{
			TreeModel model = treeview1.Model;
			TreeIter iter;
			model.GetIter (out iter, args.Path);
			String agentId = model.GetValue (iter, 1).ToString();
			LoadGpuInformationView (agentId);
			LoadFabricInformationView (agentId);
			LoadAssignmentView (agentId);
			LoadSrvRuntimeConfig (agentId);
			LoadClientRuntimeConfig (agentId);
		}

		void LoadGpuInformationView(String agentId){
			foreach (var column in treeview2.Columns) {
				treeview2.RemoveColumn (column);
			}

			for (int i = 0; i< gpuInfoColumn.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = gpuInfoColumn[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview2.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);

			}

			TcpClient client = new TcpClient (ServerIP, Int32.Parse (TcpPort));
			object content = agentId;
			Type contentType = agentId.GetType ();
			Protocol getGpuInfo = new Protocol (client, ConstValues.PROTOCOL_FN_GET_GPU_LIST, contentType, content);
			getGpuInfo.Start ();
			var gpuList = getGpuInfo.ResultObject as List<Dictionary<String,String>>;
			if (gpuList != null) {
				Gtk.TreeStore gpuListView = new TreeStore (typeof(String), typeof(String), typeof(String), typeof(String), typeof(String));
				foreach (var gpuItem in gpuList) {
					gpuListView.AppendValues (gpuItem["gpu_id"], gpuItem["gpu_model"], gpuItem["assign_type"], gpuItem["gpu_uuid"],gpuItem["memo"]);
				}
				treeview2.Model = gpuListView;
				treeview2.ExpandAll ();
			}

		}

		void LoadFabricInformationView(String agentId){
			foreach (var column in treeview4.Columns) {
				treeview4.RemoveColumn (column);
			}

			for (int i = 0; i< fabricInfoColumn.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = fabricInfoColumn[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview4.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);

			}

			TcpClient client = new TcpClient (ServerIP, Int32.Parse (TcpPort));
			object content = agentId;
			Type contentType = agentId.GetType ();
			Protocol getFabricInfo = new Protocol (client, ConstValues.PROTOCOL_FN_GET_FABRIC_LIST, contentType, content);
			getFabricInfo.Start ();
			var fabricList = getFabricInfo.ResultObject as List<Dictionary<String,String>>;
			if (fabricList != null) {
				Gtk.TreeStore fabricListView = new TreeStore (typeof(String), typeof(String), typeof(String), typeof(String), typeof(String), typeof(String));
				foreach (var fabricItem in fabricList) {
					fabricListView.AppendValues (fabricItem["nic_name"], fabricItem["ip_addr"], fabricItem["mac_addr"], (Int32.Parse(fabricItem["link_speed"])/1000000).ToString() + " Gbps",fabricItem["op_status"], fabricItem["memo"]);
				}
				treeview4.Model = fabricListView;
				treeview4.ExpandAll ();
			}
		}

		void LoadAssignmentView(String agentId){
			// `resourceinfo`.`assign_list` will use a comma separaet list and stored in the database;
			//  SQL sample : "SELECT `system_id`, `gpu_model`, `gpu_assigned_status` ,`gpu_uuid` FROM " + tbAssign + " WHERE FIND_IN_SET('"+ systemId +"',gpu_assigned_to_list)";
			foreach (var column in treeview5.Columns) {
				treeview5.RemoveColumn (column);
			}

			for (int i = 0; i< assignmentColumn.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = assignmentColumn[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview5.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);

			}

			TcpClient client = new TcpClient (ServerIP, Int32.Parse (TcpPort));
			object content = agentId;
			Type contentType = agentId.GetType ();
			Protocol getAssignedGpu = new Protocol (client, ConstValues.PROTOCOL_FN_GET_AGENT_ASSIGNMENT_LIST, contentType, content);
			getAssignedGpu.Start ();
			var assignedGpuList = getAssignedGpu.ResultObject as List<Dictionary<String,String>>;
			if (assignedGpuList != null) {
				Gtk.TreeStore assignedGpuView = new TreeStore (typeof(String), typeof(String), typeof(String), typeof(String), typeof(String), typeof(String));
				foreach (var assignedGPU in assignedGpuList) {
					assignedGpuView.AppendValues (assignedGPU["resource_ip"], assignedGPU["gpu_id"], assignedGPU["gpu_model"], assignedGPU["gpu_uuid"],(Int32.Parse(assignedGPU["link_speed"])/1000000).ToString() + " Gbps", assignedGPU["assign_type"]);
				}
				treeview5.Model = assignedGpuView;
				treeview5.ExpandAll ();
			}
		
		}

		void LoadSrvRuntimeConfig(String agentId){
			foreach (var column in treeview3.Columns) {
				treeview3.RemoveColumn (column);
			}

			for (int i = 0; i< runtimeConfigColumn.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = runtimeConfigColumn[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview3.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);

			}

			TcpClient client = new TcpClient (ServerIP, Int32.Parse (TcpPort));
			object content = agentId;
			Type contentType = agentId.GetType ();
			Protocol getRcudaSrvConfig = new Protocol (client, ConstValues.PROTOCOL_FN_GET_RCUDA_SRV_CONFIG, contentType, content);
			getRcudaSrvConfig.Start ();
			var configList = getRcudaSrvConfig.ResultObject as List<String>;
			if (configList != null) {
				Gtk.TreeStore configListView = new TreeStore (typeof(String));
				foreach (var configCmd in configList) {
					configListView.AppendValues (configCmd);
				}
				treeview3.Model = configListView;
				treeview3.ExpandAll ();
			}
		}

		void LoadClientRuntimeConfig(String agentId){
			foreach (var column in treeview6.Columns) {
				treeview6.RemoveColumn (column);
			}

			for (int i = 0; i< runtimeConfigColumn.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = runtimeConfigColumn[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview6.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);

			}

			TcpClient client = new TcpClient (ServerIP, Int32.Parse (TcpPort));
			object content = agentId;
			Type contentType = agentId.GetType ();
			Protocol getRcudaClientConfig = new Protocol (client, ConstValues.PROTOCOL_FN_GET_RCUDA_CLIENT_CONFIG, contentType, content);
			getRcudaClientConfig.Start ();
			var configList = getRcudaClientConfig.ResultObject as List<String>;
			if (configList != null) {
				Gtk.TreeStore configListView = new TreeStore (typeof(String));
				foreach (var configCmd in configList) {
					configListView.AppendValues (configCmd);
				}
				treeview6.Model = configListView;
				treeview6.ExpandAll ();
			}
		}

		protected void OnButton41Clicked (object sender, EventArgs e)
		{
			// Download config to local
			FileChooserDialog folderChoose = new FileChooserDialog("Choose configuration download path", null, FileChooserAction.SelectFolder);
			folderChoose.AddButton ("Cancel", (int)ResponseType.Cancel);
			folderChoose.AddButton ("Select", (int)ResponseType.Ok);
			if (folderChoose.Run () == (int) ResponseType.Ok) {
				//TODO
				List<String> config = GetConfigFromTreeview(treeview3);
				#if DEBUG
				foreach (var line in config) {
					Debug.WriteLine(line);
				}
				#endif
				File.WriteAllLines (folderChoose.Filename + "/load_server_runtime_config", config);
				config = GetConfigFromTreeview (treeview6);
				#if DEBUG
				foreach (var line in config) {
					Debug.WriteLine(line);
				}
				#endif
				File.WriteAllLines (folderChoose.Filename + "/load_client_runtime_config", config);
			}
			folderChoose.Destroy ();
		}

		List<String> GetConfigFromTreeview(TreeView tv){
			TreeIter iter;
			List<String> config = new List<string> ();
			try {
				TreeModel model = tv.Model;
				model.GetIterFirst (out iter);
				do {
					config.Add (model.GetValue (iter, 0).ToString());
				} while (model.IterNext (ref iter));
				return config;
			} catch (Exception ex) {
				MainClass.log.Error ("Load config from treeview failed");
				MainClass.log.Debug (ex.Message);
				MainClass.log.Debug (ex.StackTrace);
				return config = new List<string> ();
			}
		}

		protected void OnButton42Clicked (object sender, EventArgs e)
		{
			String agentId, agentIp;
			Int32 agentPort = 14729;
			TreeIter iter;
			TreeModel model;
			TreeSelection selection = treeview1.Selection;
			if (!selection.GetSelected (out model, out iter)) {
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "No Agent Selected");
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Ok) {
						msg.Destroy ();
					}
				};
				msg.ShowAll ();
			} else {
				agentIp = model.GetValue (iter, 0).ToString();
				agentId = model.GetValue (iter, 1).ToString ();

				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.YesNo, String.Format (@"Do you want to restart the agent of {0} at {1} now?", agentId, agentIp));
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.No){
						msg.Destroy();
					}else{
						try {
							msg.Destroy();
							TcpClient client = new TcpClient (agentIp, agentPort);
							object content = agentId;
							Type contentType = agentId.GetType();
							Protocol restartAgent = new Protocol(client, ConstValues.PROTOCOL_FN_RESTART_AGENT, contentType, content);
							restartAgent.Start();
							client.Close();
						} catch (Exception ex) {
							MessageDialog msgConnFail = new MessageDialog(null, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Failed connect to agent!\nError:" + ex.Message);
							msgConnFail.Response+= (obj, msgargs) => {
								if(msgargs.ResponseId == ResponseType.Close){
									msgConnFail.Destroy();
								}
							};
							msgConnFail.ShowAll();
						}
					}
				};
				msg.ShowAll ();
			}
		}

		protected void OnButton39Clicked (object sender, EventArgs e)
		{
			Alignment parentAlignment = (Alignment)this.Parent;
			parentAlignment.Remove (parentAlignment.Child);
			WidgetClientDeployment clientDeploy = new WidgetClientDeployment ();
			clientDeploy.ServerIp = this.ServerIP;
			clientDeploy.TcpPort = this.TcpPort;
			parentAlignment.Add (clientDeploy);
			parentAlignment.ShowAll ();
			this.Destroy ();
		}

		protected void OnButton40Clicked (object sender, EventArgs e)
		{
			//TODO  -- Done this function, TODO for serveraction, clientaction and database code page.
			// 1. Query all of the GPU resource of this agent
			// 2. If any of these gpu resource has been assigned.
			// 3. a) if no, remove record of agentinfo, fabricinfo, resourceinfo by agentid
			//    b) if yes, promot a message to let customer know need to remove all of the assignment before delete the agent.

			TreeIter iter;
			TreeModel model;
			TreeSelection selection = treeview1.Selection;
			if (!selection.GetSelected (out model, out iter)) {
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "No Agent Selected");
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Ok) {
						msg.Destroy ();
					}
				};
				msg.ShowAll ();
			} else {
				TcpClient client = new TcpClient (ServerIP, Int32.Parse (TcpPort));
				object content = model.GetValue (iter, 1).ToString();
				Type contentType = content.GetType ();
				Protocol checkResShareStatus = new Protocol (client, ConstValues.PROTOCOL_FN_CHECK_RESOURCE_IF_ASSIGNED, contentType, content);
				checkResShareStatus.Start ();
				client.Close ();
				if ((String)checkResShareStatus.ResultObject == "True") {
					//has some gpu resource already assigned to other clients
					MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Warning, ButtonsType.Ok, "Some of GPU resource have been assigned already.\nPlease revoke these assignation before remove this agent.");
					msg.Response += (o, args) => {
						if (args.ResponseId == ResponseType.Ok) {
							msg.Destroy ();
						}
					};
					msg.ShowAll ();
				} else {
					// since there is no assignation existed, this agent could be remove now.
					TcpClient client2 = new TcpClient(ServerIP, Int32.Parse(TcpPort));
					object content2 = model.GetValue(iter, 1).ToString();
					Type contentType2 = content2.GetType ();
					Protocol removeAgent = new Protocol (client2, ConstValues.PROTOCOL_FN_REMOVE_AGENT, contentType2, content2);
					removeAgent.Start ();
					client2.Close ();
					if ((String)removeAgent.ReceivedContent == "True") {
						MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Agent has been removed");
						msg.Response += (o, args) => {
							if (args.ResponseId == ResponseType.Ok) {
								msg.Destroy ();
							}
						};
						msg.ShowAll ();
					} else {
						MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Removing Agent failed. \nTry again later.");
						msg.Response += (o, args) => {
							if (args.ResponseId == ResponseType.Ok) {
								msg.Destroy ();
							}
						};
						msg.ShowAll ();
					}
				}
			}
		}
	}
}

