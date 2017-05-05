using System;
using Gtk;
using System.Net.Sockets;
using System.Net;
using MatrixLibrary;


namespace MatrixResourceManager
{
	public partial class DialogConnectToServer : Gtk.Dialog
	{
		public String ServerIP = string.Empty;
		public String TcpPort = string.Empty;

		public DialogConnectToServer ()
		{
			this.Build ();
			#if DEBUG
			entry10.Text = "157.22.244.236";
			#endif
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}


		protected void OnButton65Clicked (object sender, EventArgs e)
		{
			//			IPAddress serverIP = null;
			//			IPAddress.TryParse (entry10.Text.Trim (), out serverIP);
			//			if (serverIP == null) {
			//				MessageDialog msg = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Please provide a correct format IP address and try again");
			//				msg.Response += (object o, ResponseArgs args) => {
			//					if(args.ResponseId == ResponseType.Close){
			//						msg.Destroy();
			//					}
			//				};
			//				msg.ShowAll ();
			//				return;
			//			}
			//
			int tcpPort = -1;
			int.TryParse (entry11.Text.Trim (), out tcpPort);
			if (tcpPort < 1 || tcpPort > 65535) {
				MessageDialog msg = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Please provide a valid TCP Port number which your server listen on");
				msg.Response += (object o, ResponseArgs args) => {
					if(args.ResponseId == ResponseType.Close){
						msg.Destroy();
					}
				};
				msg.ShowAll ();
				return;
			}
			try {
				TcpClient client = new TcpClient(entry10.Text.Trim(), tcpPort);
				object content = "Hello";
				Type contentType = content.GetType();
				Protocol guiHandShake = new Protocol(client, ConstValues.PROTOCOL_FN_GUI_HANDSHAKE_WITH_SERVER, contentType, content);
				guiHandShake.Start();
				if((String)guiHandShake.ResultObject == "Live"){
					MessageDialog msg = new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent, MessageType.Info,ButtonsType.Ok, "Connected to Server");
					msg.Response += (object o, ResponseArgs args) => {
						if(args.ResponseId == ResponseType.Ok){
							this.ServerIP = entry10.Text.Trim();
							this.TcpPort = entry11.Text.Trim();
							this.HideAll();
						}
					};
					msg.ShowAll();
				}else{
					MessageDialog msg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close,"Cannot connect to the server. Please check the IP address, TCP port and make sure the server is running.");
					msg.Response+= (object o, ResponseArgs args) => {
						if (args.ResponseId == ResponseType.Close){
							msg.Destroy();
						}
					};
				}
			} catch (Exception ex) {
				MainClass.log.Error (ex.Message);
				MainClass.log.Debug (ex.StackTrace);
			}
		}
	}
}

