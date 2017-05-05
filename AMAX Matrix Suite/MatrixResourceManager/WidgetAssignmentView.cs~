using System;
using Gtk;
using System.Collections.Generic;
using System.Net.Sockets;
using MatrixLibrary;
using System.Diagnostics;
using System.Xml;
using System.Runtime.Remoting;
using System.IO.Ports;
using System.Runtime.InteropServices;
using Glade;
using Pango;

namespace MatrixResourceManager
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class WidgetAssignmentView : Gtk.Bin
	{
		List<String> gpuListColumnName = new List<String>() {"Resource IP","Link Speed", "GPU ID", "GPU UUID","GPU Model", "Assignment Type"};
		List<String> gpuDetailInfoColumnName = new List<String>() {"GPU Parameter", "Value"};
		List<String> gpuAssignListColumnName = new List<String>() {"Resource IP", "GPU ID", "GPU UUID", "GPU Model", "Link Speed", "Assignment Type"};
		//List<Dictionary<String,String>> CurrentGpuAssignList = new List<Dictionary<string, string>>();
		List<Dictionary<String,String>> AssignmentChange = new List<Dictionary<string, string>>();
		TreeStore gpuAssignView;
		TreeStore gpuInfoView;
		String selectedInterface = string.Empty;
		String selectedSpeed = string.Empty;
		bool exclusiveFlag = false;
		public String ServerIp = string.Empty;
		public String TcpPort = string.Empty;
		bool AssignmentInProcess = false;

		public WidgetAssignmentView (String serverIp, String tcpPort)
		{
			this.Build ();
			ServerIp = serverIp;
			TcpPort = tcpPort;
			LoadAllGpuList ();
			InitialAgentList ();
		}

		void LoadAllGpuList(){
			foreach (var column in treeview4.Columns) {
				treeview4.RemoveColumn (column);
			}
			for (int i = 0; i< gpuListColumnName.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = gpuListColumnName[i];
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview4.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);
			}

			TreeStore gpuListView = new TreeStore (typeof(String), typeof(String), typeof(String), typeof(String), typeof(String), typeof(String));
			TreeIter iterAvaliable = gpuListView.AppendValues ("Avaliable GPUs");
			TreeIter iterUnabaliable = gpuListView.AppendValues ("Exclusive Assigned GPUs");
			TreeIter iterOffline = gpuListView.AppendValues ("Offline GPUs");
			TcpClient client = new TcpClient (ServerIp, Int32.Parse(TcpPort));
			object content = "all-gpus";
			Type contentType = content.GetType ();
			Protocol getGpuList = new Protocol (client, ConstValues.PROTOCOL_FN_GET_GPU_VIEW_LIST, contentType, content);
			getGpuList.Start ();
			client.Close ();
			var gpuList = getGpuList.ResultObject as List<Dictionary<String,String>>;
			if (gpuList != null) {
				foreach (var gpuItem in gpuList) {
					if (gpuItem ["agent_status"] != "Online" || gpuItem ["ip_list"].Trim () == "") {
						gpuListView.AppendValues (iterOffline, gpuItem ["ip_list"], gpuItem ["link_speed"], gpuItem ["gpu_id"], gpuItem ["gpu_uuid"], gpuItem ["gpu_model"], gpuItem ["assign_type"]);
					}else if (gpuItem ["assign_type"] == "Exclusive" && gpuItem ["assign_list"].Trim () != "") {
						gpuListView.AppendValues (iterUnabaliable, gpuItem ["ip_list"], gpuItem ["link_speed"], gpuItem ["gpu_id"], gpuItem ["gpu_uuid"], gpuItem ["gpu_model"], gpuItem ["assign_type"]);
					} else {
						gpuListView.AppendValues (iterAvaliable, gpuItem ["ip_list"], gpuItem ["link_speed"], gpuItem ["gpu_id"], gpuItem ["gpu_uuid"], gpuItem ["gpu_model"], gpuItem ["assign_type"]);
					}
				}
			}
			treeview4.Model = gpuListView;
			treeview4.ExpandAll ();
		}

		void InitialAgentList(){
			TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
			object content = "all-agents";
			Type contentType = content.GetType ();
			Protocol getRcudaClientList = new Protocol (client, ConstValues.PROTOCOL_FN_GET_AGENT_LIST, contentType, content);
			getRcudaClientList.Start ();
			var clientList = getRcudaClientList.ResultObject as List<Dictionary<String,String>>;
			if (clientList != null) {
				foreach (var agent in clientList) {
					combobox2.AppendText(agent["agent_ip"] + ":" + agent["agent_id"]);
				}
			}
		}


		protected void OnButton48Clicked (object sender, EventArgs e)
		{
			try {
				if (combobox2.ActiveText == null) {
					MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "No Client Selected!");
					msg.Response += (o, args) => {
						if (args.ResponseId == ResponseType.Ok) {
							msg.Destroy ();
						}
					};
					msg.ShowAll ();
				} else {
					AssignmentInProcess = true;
					TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
					object content = combobox2.ActiveText.Split (':') [1];
					Type contentType = content.GetType ();
					Protocol getGpuListByClientId = new Protocol (client, ConstValues.PROTOCOL_FN_GET_AGENT_ASSIGNMENT_LIST, contentType, content);
					getGpuListByClientId.Start ();
					var gpuList = getGpuListByClientId.ResultObject as List<Dictionary<String,String>>;
					//CurrentGpuAssignList = gpuList;
					gpuAssignView = new TreeStore (typeof(String), typeof(String), typeof(String), typeof(String), typeof(String), typeof(String));
					if (gpuList != null) {
						foreach (var column in treeview6.Columns) {
							treeview6.RemoveColumn (column);
						}
						for (int i = 0; i < gpuAssignListColumnName.Count; i++) {
							Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
							treeviewColumn.Title = gpuAssignListColumnName [i];
							Gtk.CellRendererText cellRenderText = new CellRendererText ();
							treeviewColumn.PackStart (cellRenderText, true);
							treeview6.AppendColumn (treeviewColumn);
							treeviewColumn.AddAttribute (cellRenderText, "text", i);
						}
				
						foreach (var gpu in gpuList) {
							gpuAssignView.AppendValues (gpu ["resource_ip"], gpu ["gpu_id"], gpu ["gpu_uuid"], gpu ["gpu_model"], Int32.Parse (gpu ["link_speed"]) / 1000000 + " Gbps", gpu ["assign_type"]);
						}
						button43.Sensitive = true;
						button44.Sensitive = true;
						button45.Sensitive = true;
						button46.Sensitive = true;
						combobox2.Sensitive = false;
						button48.Sensitive= false;
					}
					treeview6.Model = gpuAssignView;
					treeview6.ExpandAll ();
				}
			} catch (Exception ex) {
				MainClass.log.Error (ex.Message);
				MainClass.log.Debug (ex.StackTrace);
			}
		}


		protected void OnButton46Clicked (object sender, EventArgs e)
		{
			foreach (var column in treeview6.Columns) {
				treeview6.RemoveColumn (column);
			}
			combobox2.Active = -1;
			//CurrentGpuAssignList = new List<Dictionary<string, string>> ();
			button43.Sensitive = false;
			button44.Sensitive = false;
			button45.Sensitive = false;
			button46.Sensitive = false;
			combobox2.Sensitive = true;
			gpuAssignView = null;
			button48.Sensitive = true;
			AssignmentInProcess = false;


		}
			

		protected void OnButton43Clicked (object sender, EventArgs e)
		{
			// 1. Check is there any gpu item selected in the treeview
			// 2. Check if the selected the iter node
			// 3. Check if the unavaliable gpu card selected.
			// 4. add one more line into 
			TreeSelection selection = treeview4.Selection;
			TreeIter iter;
			TreeModel model;
			TreeIter parentIter;


			if (selection.GetSelected (out model, out iter) &&
			    model.IterParent (out parentIter, iter) &&
			    model.GetValue (parentIter, 0).ToString () == "Avaliable GPUs") {
				DialogGpuAssign diagGpuAssign = new DialogGpuAssign (model.GetValue (iter, 0).ToString (), model.GetValue (iter, 1).ToString ());
				diagGpuAssign.Modal = true;
				diagGpuAssign.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Cancel) {
						diagGpuAssign.Destroy ();
					} else if (args.ResponseId == ResponseType.Ok) {
						selectedInterface = diagGpuAssign.selectedIp;
						selectedSpeed = diagGpuAssign.selectedSpeed;
						exclusiveFlag = diagGpuAssign.exclusiveFlag;
						UpdateAssignView (model, iter);
						diagGpuAssign.Destroy ();
					}
				};
				diagGpuAssign.ShowAll ();
			} else {
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "No GPU card selected");
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Ok) {
						msg.Destroy ();
					}
				};
				msg.ShowAll ();
			}
		}

		void UpdateAssignView(TreeModel model, TreeIter iter){
			if (selectedInterface != string.Empty && selectedInterface != "") {
				// if exclusive flag is on, please check if the card has been assigned
				String clientIp = combobox2.ActiveText.Split (':') [0];
				String clientId = combobox2.ActiveText.Split (':') [1];
				if (exclusiveFlag) {
					TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
					object content = model.GetValue (iter, 3).ToString ();  //GPU UUID
					Type contentType = content.GetType ();
					Protocol verifyExclusiveStatus = new Protocol (client, ConstValues.PROTOCOL_FN_VERIFY_GPU_EXCLUSIVE, contentType, content);
					verifyExclusiveStatus.Start ();
					if ((String)verifyExclusiveStatus.ResultObject == "True") {
						// allow to setup the selected gpu card to exclusive mode
						// a. save related information to a temploary table 
						Dictionary<String,String> assignEntry = new Dictionary<string, string> ();
						assignEntry.Add ("action", "add");
						assignEntry.Add ("resource_ip", selectedInterface);
						assignEntry.Add ("gpu_uuid", model.GetValue (iter, 3).ToString ());
						assignEntry.Add ("assign_type", exclusiveFlag ? "Exclusive" : "Share");
						assignEntry.Add ("agent_id", clientId);
						AssignmentChange.Add (assignEntry);
						// b. a record will append into current treeview
						gpuAssignView.AppendValues (selectedInterface, model.GetValue (iter, 2).ToString (), model.GetValue (iter, 3).ToString (), model.GetValue (iter, 4).ToString (), selectedSpeed, exclusiveFlag ? "Exclusive" : "Share");
						treeview6.Model = gpuAssignView;
					} else {
						// not allow to to set this gpu card to exclusive mode. exit function, and give a customer a message box.
						MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "This GPU has been assigned to other clinet.\nIt can't be assigned as EXCLUSIVE mode.\nPlease remove other assignment and try again.");
						msg.Response += (o, args) => {
							if (args.ResponseId == ResponseType.Ok) {
								msg.Destroy ();
							}
						};
						msg.ShowAll ();
					}
				} else {
					Dictionary<String,String> assignEntry = new Dictionary<string, string> ();
					assignEntry.Add ("action", "add");
					assignEntry.Add ("resource_ip", selectedInterface);
					assignEntry.Add ("gpu_uuid", model.GetValue (iter, 3).ToString ());
					assignEntry.Add ("assign_type", exclusiveFlag ? "Exclusive" : "Share");
					assignEntry.Add ("agent_id", clientId);
					AssignmentChange.Add (assignEntry);
					// b. a record will append into current treeview
					gpuAssignView.AppendValues (selectedInterface, model.GetValue (iter, 2).ToString (), model.GetValue (iter, 3).ToString (), model.GetValue (iter, 4).ToString (), selectedSpeed, exclusiveFlag ? "Exclusive" : "Share");
					treeview6.Model = gpuAssignView;
				}
			} else {
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "No GPU Source Interface Chosen");
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Ok) {
						msg.Destroy ();
					}
				};
				msg.ShowAll ();
			}
			selectedSpeed = string.Empty;
			selectedInterface = string.Empty;
			exclusiveFlag = false;
		}



		protected void OnButton44Clicked (object sender, EventArgs e)
		{
			TreeModel model;
			TreeIter iter;
			TreeSelection selection = treeview6.Selection;
			String clientIp = combobox2.ActiveText.Split (':') [0];
			String clientId = combobox2.ActiveText.Split (':') [1];

			if(selection.GetSelected(out model, out iter)){
				Dictionary<String,String> assignEntry = new Dictionary<string, string> ();
				assignEntry.Add ("action", "remove");
				assignEntry.Add ("resource_ip", model.GetValue (iter, 0).ToString ());
				assignEntry.Add ("gpu_uuid", model.GetValue (iter, 2).ToString ());
				assignEntry.Add ("assign_type", model.GetValue (iter, 3).ToString ());
				assignEntry.Add ("agent_id", clientId);
				AssignmentChange.Add (assignEntry);

				gpuAssignView.Remove (ref iter);
				treeview6.Model = gpuAssignView;
			}else{
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "No Assignment Entry has been selected");
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Ok) {
						msg.Destroy ();
					}
				};
				msg.ShowAll ();
			}
		}

		protected void OnButton45Clicked (object sender, EventArgs e)
		{
			TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
			object content = AssignmentChange;
			Type contentType = AssignmentChange.GetType ();
			Protocol submitAssignmentChanges = new Protocol (client, ConstValues.PROTOCOL_FN_SAVE_ASSIGNMENT, contentType, content);
			submitAssignmentChanges.Start ();
			if ((String)submitAssignmentChanges.ResultObject == "True") {
				List<Dictionary<String,String>> assignmentList = new List<Dictionary<string, string>> ();
				TreeIter iter;
				TreeModel model = treeview6.Model;
				string agent_id = combobox2.ActiveText.Split (':') [1];
				if (model.GetIterFirst (out iter)) {
					do {
						Dictionary<String, String> gpuAssign = new Dictionary<string, string> ();
						gpuAssign.Add ("agent_id", agent_id);
						gpuAssign.Add ("resource_ip", model.GetValue (iter, 0).ToString ());
						gpuAssign.Add ("gpu_id", model.GetValue (iter, 1).ToString ());
						assignmentList.Add(gpuAssign);
					} while(model.IterNext (ref iter));
				}
				List<String> rCudaClientRuntimeConfig = Configuration.GenerateRcudaClientRuntimeConfig (assignmentList);
				TcpClient client2 = new TcpClient (ServerIp, Int32.Parse (TcpPort));
				object contentConfig = rCudaClientRuntimeConfig;
				Type contentTypeConfig = rCudaClientRuntimeConfig.GetType ();
				Protocol uploadRuntimeClientConfig = new Protocol (client2, ConstValues.PROTOCOL_FN_SAVE_RCUDA_CLIENT_CONFIG, contentTypeConfig, contentConfig);
				uploadRuntimeClientConfig.Start ();
				if ((String)uploadRuntimeClientConfig.ResultObject == "True") {
					MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Resource assignment and Runtime Config has been saved");
					msg.Response += (o, args) => {
						if (args.ResponseId == ResponseType.Ok) {
							combobox2.Sensitive = true;
							button48.Sensitive = true;
							button43.Sensitive = false;
							button44.Sensitive = false;
							button45.Sensitive = false;
							button46.Sensitive = false;
							gpuAssignView = null;
							AssignmentInProcess = false;
							combobox2.Active = -1;
							foreach (var column in treeview6.Columns) {
								treeview6.RemoveColumn (column);
							}
							LoadAllGpuList();
							msg.Destroy ();
						}
					};
					msg.Show ();
				} else {
					MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Save Runtime Config Failed");
					msg.Response += (o, args) => {
						if (args.ResponseId == ResponseType.Ok) {
							msg.Destroy ();
						}
					};
					msg.Show ();
				}
			} else {
				MessageDialog msg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Updating Resource Assignment Failed");
				msg.Response += (o, args) => {
					if (args.ResponseId == ResponseType.Ok) {
						msg.Destroy ();
					}
				};
				msg.Show ();
			}
		}

		protected void OnTreeview4RowActivated (object o, RowActivatedArgs args)
		{
			TreeSelection selectedRow = treeview4.Selection;
			TreeModel model;
			TreeIter iter;
			TreeIter parentIter;
			Dictionary<String, String> gpuInfo = new Dictionary<string, string> ();
			if (selectedRow.GetSelected (out model, out iter) &&
			    model.IterParent (out parentIter, iter)) {
				ShowGpuDetailInfo (model.GetValue(iter,3).ToString());
				if (!AssignmentInProcess) {
					gpuInfo.Add ("resource_ip", model.GetValue (iter, 0).ToString());
					gpuInfo.Add ("gpu_id", model.GetValue (iter, 2).ToString());
					gpuInfo.Add ("gpu_uuid", model.GetValue (iter, 3).ToString());
					gpuInfo.Add ("gpu_model", model.GetValue (iter, 4).ToString());
					gpuInfo.Add	("assign_type", model.GetValue (iter, 5).ToString());
				
					DialogModifyGpuAssign modifyAssignByGpu = new DialogModifyGpuAssign (gpuInfo, ServerIp, TcpPort);
					modifyAssignByGpu.Hidden += (object sender, EventArgs e) => {
						LoadAllGpuList();
						modifyAssignByGpu.Destroy();
					};
					modifyAssignByGpu.ShowAll();
				}
			}
		}


		void ShowGpuDetailInfo(String gpuUUID){
			foreach (var column in treeview5.Columns) {
				treeview5.RemoveColumn (column);
			}
			for (int i = 0; i< gpuDetailInfoColumnName.Count;i++)
			{
				Gtk.TreeViewColumn treeviewColumn = new TreeViewColumn ();
				treeviewColumn.Title = gpuDetailInfoColumnName[i];
				treeviewColumn.Sizing = TreeViewColumnSizing.Autosize;
				Gtk.CellRendererText cellRenderText = new CellRendererText ();
				treeviewColumn.PackStart (cellRenderText, true);
				treeview5.AppendColumn (treeviewColumn);
				treeviewColumn.AddAttribute (cellRenderText, "text", i);
			}


			XmlDocument gpuXmlInfo = new XmlDocument ();
			TcpClient client = new TcpClient (ServerIp, Int32.Parse (TcpPort));
			object content = gpuUUID;
			Type contentType = gpuUUID.GetType ();
			Protocol getGpuXml = new Protocol (client, ConstValues.PROTOCOL_FN_GET_GPU_XML, contentType, content);
			getGpuXml.Start ();
			var xmlString = getGpuXml.ResultObject as String;
			client.Close ();
			gpuInfoView = new TreeStore (typeof(String), typeof(String));
			ParseXml (xmlString);
			treeview5.Model = gpuInfoView;
			treeview5.ExpandAll ();

		}

		void ParseXml(String xmlString){
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.LoadXml (xmlString);
			XmlNode node = xmlDoc.FirstChild;
			TreeIter iter = gpuInfoView.AppendValues (node.Name + " : " + node.Attributes ["id"].Value);
			XmlParseLoop (node, iter);
		}

		void XmlParseLoop(XmlNode node, TreeIter iter){
			XmlNode localNode = node.FirstChild;
			TreeIter localIter = iter;
			try {
				do {
					if (localNode.HasChildNodes && localNode.FirstChild.Name == "#text") {
						gpuInfoView.AppendValues (localIter, localNode.Name, localNode.InnerText);
						Debug.WriteLine (localNode.Name + ":" + localNode.InnerText);
					} else {
						TreeIter innerIter = gpuInfoView.AppendValues (localIter, localNode.Name);
						Debug.WriteLine (localNode.Name);
						XmlParseLoop (localNode, innerIter);
					}
				} while((localNode = localNode.NextSibling) != null);
			} catch (Exception ex) {
				Debug.WriteLine (localNode.Name + ":" + localNode.Value);
				Debug.WriteLine (ex.Message);
				Debug.WriteLine (ex.StackTrace);
			}
		
		}

	}

}

