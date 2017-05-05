using System;
using Gtk;
using GLib;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using MatrixLibrary;
using System.Threading;

namespace MatrixResourceManager
{
	public partial class DialogModifyGpuAssign : Gtk.Dialog
	{
		
		public String ServerIp = string.Empty;
		public String TcpPort = string.Empty;
		List<String> agentListColumnName = new List<String>() {"Client IP", "Client ID", "Resource IP", "Link Speed"};
		Dictionary<String,String> gpuInfo = new Dictionary<string, string>();
		List<Dictionary<String,String>> clientListCurrent = new List<Dictionary<string, string>>();
		List<Dictionary<String,String>> clientChanges = new List<Dictionary<string, string>>();
		public TreeStore agentListView = null;

		public DialogModifyGpuAssign (Dictionary<String,String> gpuInfo, String serverip, String tcpport)
		{
			this.Build ();
			this.gpuInfo = gpuInfo;
			ServerIp = serverip;
			TcpPort = tcpport;
			InitGpuInformation ();
			InitAgentListView ();
		}
	
		void InitAgentListView(){
			foreach (var column in treeview1.Columns) {
				treeview1.RemoveColumn (column);
			}
			for (int i = 0; i< agentListColumnName.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = agentListColumnName[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview1.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);
			}
			TreeStore agentListView = new TreeStore (typeof(String), typeof(String), typeof(String), typeof(String));
			TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
			object content = gpuInfo["gpu_uuid"];
			Type contentType = content.GetType ();
			Protocol getAssignedClientList = new Protocol (client, ConstValues.PROTOCOL_FN_GET_AGENT_LIST_BY_GPU, contentType, content);
			getAssignedClientList.Start ();
			client.Close ();
			var assignedClientList = getAssignedClientList.ResultObject as List<Dictionary<String, String>>;
			clientListCurrent = assignedClientList;
			if (assignedClientList != null) {
				foreach (var clientInfo in assignedClientList) {
					agentListView.AppendValues (clientInfo["agent_ip"], clientInfo["agent_id"], clientInfo["resource_ip"], clientInfo["link_speed"]);
				}
			}
			UpdateProvisionRate (assignedClientList.Count);
			treeview1.Model = agentListView;
			treeview1.ExpandAll ();
		}

		void InitGpuInformation(){
			label7.Text = gpuInfo ["gpu_uuid"];
			label10.Text = gpuInfo ["gpu_model"];
			label8.Text = gpuInfo ["resource_ip"];
			label11.Text = gpuInfo ["gpu_id"];
			label9.Text = gpuInfo ["assign_type"];
			label12.Text = "N/A";
		}

		void UpdateProvisionRate(int clientCount){
			label12.Text = clientCount * 100 + "%";
			if (clientCount > 1) {
				// Set the label color to Red
				label12.ModifyFg (StateType.Normal, new Gdk.Color (1, 0, 0));
			} else {
				label12.ModifyFg (StateType.Normal, new Gdk.Color (0, 0, 0));
			}
		}

		protected void OnButton165Clicked (object sender, EventArgs e)
		{
			foreach (var column in treeview1.Columns) {
				treeview1.RemoveColumn (column);
			}
			for (int i = 0; i< agentListColumnName.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = agentListColumnName[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview1.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);
			}

			agentListView = new TreeStore (typeof(String), typeof(String), typeof(String), typeof(String));
			if (clientListCurrent != null) {
				foreach (var clientInfo in clientListCurrent) {
					agentListView.AppendValues (clientInfo["agent_ip"], clientInfo["agent_id"], clientInfo["resource_ip"], clientInfo["link_speed"]);
				}
			}
			UpdateProvisionRate (clientListCurrent.Count);
			clientChanges = new List<Dictionary<string, string>> ();
			treeview1.Model = agentListView;
			treeview1.ExpandAll ();
		}
			

		protected void OnButton164Clicked (object sender, EventArgs e)
		{
			TreeSelection selection = treeview1.Selection;
			TreeIter iter;
			TreeModel model;
			if (selection.GetSelected (out model, out iter)) {
				Dictionary<String,String> modifyAction = new Dictionary<string, string> ();
				modifyAction.Add ("action", "remove");
				//modifyAction.Add ("client_ip", model.GetValue (iter, 0).ToString());
				modifyAction.Add ("agent_id", model.GetValue (iter, 1).ToString());
				modifyAction.Add ("resource_ip", model.GetValue (iter, 2).ToString());
				modifyAction.Add("gpu_uuid", gpuInfo["gpu_uuid"]);
				modifyAction.Add ("assign_type", gpuInfo ["assign_type"]);
				clientChanges.Add (modifyAction);
				agentListView = (TreeStore)model;
				agentListView.Remove (ref iter);
				UpdateProvisionRate(CountNodes (agentListView));
			} else {
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "No Client Selected");
				msg.Response+= (o, args) => {
					if(args.ResponseId == ResponseType.Ok){
						msg.Destroy();
					}
				};
				msg.Show ();
			}

			treeview1.Model = agentListView;
			treeview1.ExpandAll ();
		}

		int CountNodes(TreeStore treeStore){
			int count = 0;
			TreeIter iter;
			if(treeStore.GetIterFirst (out iter)){
				do {
					count ++;
				} while (treeStore.IterNext(ref iter));
			}
			return count;
		}

		void SaveModification(){
			// Update the client changes, GUI button lable has been changed to "Save"
			TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
			object content = clientChanges;
			Type contentType = clientChanges.GetType ();
			Protocol updateChanges = new Protocol (client, ConstValues.PROTOCOL_FN_SAVE_ASSIGNMENT, contentType, content);
			updateChanges.Start ();
			client.Close ();
			var result = (String)updateChanges.ResultObject;
			if (result != "True") {
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Save Client assignment changes failed");
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Ok) {
						msg.Destroy ();
					}
				};
				msg.ShowAll ();
			} else {
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Client assignment changes Saved");
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Ok) {
						clientChanges = new List<Dictionary<string, string>>();
						InitAgentListView();
						msg.Destroy ();
					}
				};
				msg.ShowAll ();
			}
		}

		protected void OnButton136Clicked (object sender, EventArgs e)
		{
			SaveModification ();
		}



		protected void OnButton137Clicked (object sender, EventArgs e)
		{
			this.HideAll ();
		}
	}
}