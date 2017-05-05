using System;
using Gtk;
using MatrixResourceManager;

public partial class MainWindow: Gtk.Window
{
	public String ServerIP = string.Empty;
	public String TcpPort = string.Empty;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	protected void OnExitActionActivated (object sender, EventArgs e)
	{
		Application.Quit ();
	}

	void UnderConstructionDialog(){
		DialogUnderDevelopment underConstruction = new DialogUnderDevelopment ();
		underConstruction.ShowAll ();
	}

	protected void OnConnectActionActivated (object sender, EventArgs e)
	{
		DialogConnectToServer connServer = new DialogConnectToServer ();
		connServer.Hidden += (object obj, EventArgs eDialog) => {
			if (connServer.TcpPort!=String.Empty || connServer.ServerIP!=string.Empty ) {
				this.TcpPort = connServer.TcpPort;
				this.ServerIP = connServer.ServerIP;
				GuiSettingAfterConnectedToServer();
				OnClientViewActionActivated(ClientViewAction,new EventArgs());
			}
			connServer.Destroy();
		};
		connServer.ShowAll ();
	}

	void GuiSettingAfterConnectedToServer(){
		ConnectAction.Sensitive = false;
		DisconnectAction.Sensitive = true;
		ClientViewAction.Sensitive = true;
		ResourceAssignAction.Sensitive = true;
		GPUMetricMonitorAction.Sensitive = true;
		ServerAction.Sensitive = false;
		ClientAction.Sensitive = true;
		DebugLogAction.Sensitive = true;
		BackupRestoreAction.Sensitive = true;
		statusbar1.Push(0, String.Format("Connected Matrix Server on {0}:{1}",ServerIP, TcpPort));
	}

	void GuiSettingAfterDisconnectedFromServer(){
		ConnectAction.Sensitive = true;
		DisconnectAction.Sensitive = false;
		ClientViewAction.Sensitive = false;
		ResourceAssignAction.Sensitive = false;
		GPUMetricMonitorAction.Sensitive = false;
		ServerAction.Sensitive = true;
		ClientAction.Sensitive = false;
		DebugLogAction.Sensitive = false;
		BackupRestoreAction.Sensitive = false;
		statusbar1.Push(0, String.Format("Disconnected from Matrix Server {0}:{1}",ServerIP, TcpPort));
	}


	protected void OnClientViewActionActivated (object sender, EventArgs e)
	{
		GtkAlignment.Remove (GtkAlignment.Child);
		WidgetAgentsView clientView = new WidgetAgentsView (ServerIP, TcpPort);
		GtkAlignment.Add (clientView);
		GtkAlignment.ShowAll ();
	}



	protected void OnResourceAssignActionActivated (object sender, EventArgs e)
	{
		GtkAlignment.Remove (GtkAlignment.Child);
		WidgetAssignmentView assignView = new WidgetAssignmentView (ServerIP, TcpPort);
		GtkAlignment.Add (assignView);
		GtkAlignment.ShowAll ();
	}


	protected void OnServerActionActivated (object sender, EventArgs e)
	{
		GtkAlignment.Remove (GtkAlignment.Child);
		WidgetServerDeployment deployServer = new WidgetServerDeployment ();
		GtkAlignment.Add (deployServer);
		GtkAlignment.ShowAll ();
	}



	protected void OnClientActionActivated (object sender, EventArgs e)
	{
		GtkAlignment.Remove (GtkAlignment.Child);
		WidgetClientDeployment deployClient = new WidgetClientDeployment ();
		deployClient.ServerIp = this.ServerIP;
		deployClient.TcpPort = this.TcpPort;
		GtkAlignment.Add (deployClient);
		GtkAlignment.ShowAll ();
	}



	protected void OnGPUMetricMonitorActionActivated (object sender, EventArgs e)
	{
//		GtkAlignment.Remove (GtkAlignment.Child);
//		WidgetMonitor monitor = new WidgetMonitor ();
//		monitor.ServerIp = this.ServerIP;
//		monitor.TcpPort = this.TcpPort;
//		GtkAlignment.Add (monitor);
//		GtkAlignment.ShowAll ();	
		UnderConstructionDialog();
	}

	protected void OnDisconnectActionActivated (object sender, EventArgs e)
	{
		GuiSettingAfterDisconnectedFromServer ();
		ServerIP = string.Empty;
		TcpPort = string.Empty;
		MessageDialog msg = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Disconnected from Matrix Server");
		msg.Response += (o, args) => {
			if(args.ResponseId == ResponseType.Ok){
				msg.Destroy();
			}
		};
		msg.ShowAll ();
	}

	protected void OnAboutActionActivated (object sender, EventArgs e)
	{
		DialogAbout aboutBox = new DialogAbout ();
		aboutBox.WindowPosition = WindowPosition.CenterOnParent;
		aboutBox.Response += (o, args) => {
			if(args.ResponseId == ResponseType.Ok){
				aboutBox.Destroy();
			}
		};
		aboutBox.ShowAll ();
	}

	protected void OnLicenseActionActivated (object sender, EventArgs e)
	{
		DialogLicenseInfo license = new DialogLicenseInfo ();
		license.ShowAll ();
	}

}
