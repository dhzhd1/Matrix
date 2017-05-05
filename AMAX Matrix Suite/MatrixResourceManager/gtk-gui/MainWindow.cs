
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	
	private global::Gtk.Action MainAction;
	
	private global::Gtk.Action ConnectAction;
	
	private global::Gtk.Action DisconnectAction;
	
	private global::Gtk.Action ExitAction;
	
	private global::Gtk.Action ViewAction;
	
	private global::Gtk.RadioAction ClientViewAction;
	
	private global::Gtk.RadioAction ResourceAssignAction;
	
	private global::Gtk.RadioAction DashboardAction;
	
	private global::Gtk.Action MonitorAction;
	
	private global::Gtk.Action GPUMetricMonitorAction;
	
	private global::Gtk.Action HelpAction;
	
	private global::Gtk.Action DocumentationAction;
	
	private global::Gtk.Action UpdateAction;
	
	private global::Gtk.Action LicenseAction;
	
	private global::Gtk.Action ReportBugsAction;
	
	private global::Gtk.Action AboutAction;
	
	private global::Gtk.Action ToolsAction;
	
	private global::Gtk.Action DeploymentAction;
	
	private global::Gtk.Action ServerAction;
	
	private global::Gtk.Action ClientAction;
	
	private global::Gtk.Action DebugLogAction;
	
	private global::Gtk.Action BackupRestoreAction;
	
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.Image image1;
	
	private global::Gtk.MenuBar menubar1;
	
	private global::Gtk.Frame frame1;
	
	private global::Gtk.Alignment GtkAlignment;
	
	private global::Gtk.Statusbar statusbar1;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.MainAction = new global::Gtk.Action ("MainAction", global::Mono.Unix.Catalog.GetString ("Main"), null, null);
		this.MainAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Main");
		w1.Add (this.MainAction, null);
		this.ConnectAction = new global::Gtk.Action ("ConnectAction", global::Mono.Unix.Catalog.GetString ("Connect..."), null, null);
		this.ConnectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Connect...");
		w1.Add (this.ConnectAction, null);
		this.DisconnectAction = new global::Gtk.Action ("DisconnectAction", global::Mono.Unix.Catalog.GetString ("Disconnect"), null, null);
		this.DisconnectAction.Sensitive = false;
		this.DisconnectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Disconnect");
		w1.Add (this.DisconnectAction, null);
		this.ExitAction = new global::Gtk.Action ("ExitAction", global::Mono.Unix.Catalog.GetString ("Exit"), null, null);
		this.ExitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Exit");
		w1.Add (this.ExitAction, null);
		this.ViewAction = new global::Gtk.Action ("ViewAction", global::Mono.Unix.Catalog.GetString ("View"), null, null);
		this.ViewAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("View");
		w1.Add (this.ViewAction, null);
		this.ClientViewAction = new global::Gtk.RadioAction ("ClientViewAction", global::Mono.Unix.Catalog.GetString ("Client View"), null, null, 0);
		this.ClientViewAction.Group = new global::GLib.SList (global::System.IntPtr.Zero);
		this.ClientViewAction.Sensitive = false;
		this.ClientViewAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Client View");
		w1.Add (this.ClientViewAction, null);
		this.ResourceAssignAction = new global::Gtk.RadioAction ("ResourceAssignAction", global::Mono.Unix.Catalog.GetString ("Resource Assign"), null, null, 0);
		this.ResourceAssignAction.Group = this.ClientViewAction.Group;
		this.ResourceAssignAction.Sensitive = false;
		this.ResourceAssignAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Resource Assign");
		w1.Add (this.ResourceAssignAction, null);
		this.DashboardAction = new global::Gtk.RadioAction ("DashboardAction", global::Mono.Unix.Catalog.GetString ("Dashboard"), null, null, 0);
		this.DashboardAction.Group = this.ResourceAssignAction.Group;
		this.DashboardAction.Sensitive = false;
		this.DashboardAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Dashboard");
		this.DashboardAction.Visible = false;
		this.DashboardAction.VisibleHorizontal = false;
		this.DashboardAction.VisibleVertical = false;
		this.DashboardAction.VisibleOverflown = false;
		w1.Add (this.DashboardAction, null);
		this.MonitorAction = new global::Gtk.Action ("MonitorAction", global::Mono.Unix.Catalog.GetString ("Monitor"), null, null);
		this.MonitorAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Monitor");
		w1.Add (this.MonitorAction, null);
		this.GPUMetricMonitorAction = new global::Gtk.Action ("GPUMetricMonitorAction", global::Mono.Unix.Catalog.GetString ("GPU Metric Monitor"), null, null);
		this.GPUMetricMonitorAction.Sensitive = false;
		this.GPUMetricMonitorAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("GPU Metric Monitor");
		w1.Add (this.GPUMetricMonitorAction, null);
		this.HelpAction = new global::Gtk.Action ("HelpAction", global::Mono.Unix.Catalog.GetString ("Help"), null, null);
		this.HelpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Help");
		w1.Add (this.HelpAction, null);
		this.DocumentationAction = new global::Gtk.Action ("DocumentationAction", global::Mono.Unix.Catalog.GetString ("Documentation"), null, null);
		this.DocumentationAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Documentation");
		w1.Add (this.DocumentationAction, null);
		this.UpdateAction = new global::Gtk.Action ("UpdateAction", global::Mono.Unix.Catalog.GetString ("Update..."), null, null);
		this.UpdateAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Update...");
		w1.Add (this.UpdateAction, null);
		this.LicenseAction = new global::Gtk.Action ("LicenseAction", global::Mono.Unix.Catalog.GetString ("License..."), null, null);
		this.LicenseAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("License...");
		w1.Add (this.LicenseAction, null);
		this.ReportBugsAction = new global::Gtk.Action ("ReportBugsAction", global::Mono.Unix.Catalog.GetString ("Report Bugs..."), null, null);
		this.ReportBugsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Report Bugs...");
		w1.Add (this.ReportBugsAction, null);
		this.AboutAction = new global::Gtk.Action ("AboutAction", global::Mono.Unix.Catalog.GetString ("About"), null, null);
		this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("About");
		w1.Add (this.AboutAction, null);
		this.ToolsAction = new global::Gtk.Action ("ToolsAction", global::Mono.Unix.Catalog.GetString ("Tools"), null, null);
		this.ToolsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Tools");
		w1.Add (this.ToolsAction, null);
		this.DeploymentAction = new global::Gtk.Action ("DeploymentAction", global::Mono.Unix.Catalog.GetString ("Deployment"), null, null);
		this.DeploymentAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Deployment");
		w1.Add (this.DeploymentAction, null);
		this.ServerAction = new global::Gtk.Action ("ServerAction", global::Mono.Unix.Catalog.GetString ("Server ..."), null, null);
		this.ServerAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Server ...");
		w1.Add (this.ServerAction, null);
		this.ClientAction = new global::Gtk.Action ("ClientAction", global::Mono.Unix.Catalog.GetString ("Client ..."), null, null);
		this.ClientAction.Sensitive = false;
		this.ClientAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Client ...");
		w1.Add (this.ClientAction, null);
		this.DebugLogAction = new global::Gtk.Action ("DebugLogAction", global::Mono.Unix.Catalog.GetString ("Debug Log..."), null, null);
		this.DebugLogAction.Sensitive = false;
		this.DebugLogAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Debug Log...");
		w1.Add (this.DebugLogAction, null);
		this.BackupRestoreAction = new global::Gtk.Action ("BackupRestoreAction", global::Mono.Unix.Catalog.GetString ("Backup & Restore..."), null, null);
		this.BackupRestoreAction.Sensitive = false;
		this.BackupRestoreAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Backup & Restore...");
		w1.Add (this.BackupRestoreAction, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(3));
		this.BorderWidth = ((uint)(2));
		this.Gravity = ((global::Gdk.Gravity)(5));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.image1 = new global::Gtk.Image ();
		this.image1.Name = "image1";
		this.image1.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("MatrixResourceManager.logo.png");
		this.vbox1.Add (this.image1);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.image1]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='MainAction' action='MainAction'><menuitem name='ConnectAction' action='ConnectAction'/><menuitem name='DisconnectAction' action='DisconnectAction'/><menuitem name='ExitAction' action='ExitAction'/></menu><menu name='ViewAction' action='ViewAction'><menuitem name='ClientViewAction' action='ClientViewAction'/><menuitem name='ResourceAssignAction' action='ResourceAssignAction'/><menuitem name='DashboardAction' action='DashboardAction'/></menu><menu name='ToolsAction' action='ToolsAction'><menu name='DeploymentAction' action='DeploymentAction'><menuitem name='ServerAction' action='ServerAction'/><menuitem name='ClientAction' action='ClientAction'/></menu><menuitem name='DebugLogAction' action='DebugLogAction'/><menuitem name='BackupRestoreAction' action='BackupRestoreAction'/></menu><menu name='MonitorAction' action='MonitorAction'><menuitem name='GPUMetricMonitorAction' action='GPUMetricMonitorAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='DocumentationAction' action='DocumentationAction'/><menuitem name='UpdateAction' action='UpdateAction'/><menuitem name='LicenseAction' action='LicenseAction'/><menuitem name='ReportBugsAction' action='ReportBugsAction'/><menuitem name='AboutAction' action='AboutAction'/></menu></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
		this.menubar1.Name = "menubar1";
		this.vbox1.Add (this.menubar1);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubar1]));
		w3.Position = 1;
		w3.Expand = false;
		w3.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.frame1 = new global::Gtk.Frame ();
		this.frame1.Name = "frame1";
		this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame1.Gtk.Container+ContainerChild
		this.GtkAlignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
		this.GtkAlignment.Name = "GtkAlignment";
		this.GtkAlignment.LeftPadding = ((uint)(12));
		this.frame1.Add (this.GtkAlignment);
		this.vbox1.Add (this.frame1);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.frame1]));
		w5.Position = 2;
		// Container child vbox1.Gtk.Box+BoxChild
		this.statusbar1 = new global::Gtk.Statusbar ();
		this.statusbar1.Name = "statusbar1";
		this.statusbar1.Spacing = 6;
		this.vbox1.Add (this.statusbar1);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.statusbar1]));
		w6.Position = 3;
		w6.Expand = false;
		w6.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 1476;
		this.DefaultHeight = 797;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.ConnectAction.Activated += new global::System.EventHandler (this.OnConnectActionActivated);
		this.DisconnectAction.Activated += new global::System.EventHandler (this.OnDisconnectActionActivated);
		this.ExitAction.Activated += new global::System.EventHandler (this.OnExitActionActivated);
		this.ClientViewAction.Activated += new global::System.EventHandler (this.OnClientViewActionActivated);
		this.ResourceAssignAction.Activated += new global::System.EventHandler (this.OnResourceAssignActionActivated);
		this.GPUMetricMonitorAction.Activated += new global::System.EventHandler (this.OnGPUMetricMonitorActionActivated);
		this.LicenseAction.Activated += new global::System.EventHandler (this.OnLicenseActionActivated);
		this.AboutAction.Activated += new global::System.EventHandler (this.OnAboutActionActivated);
		this.ServerAction.Activated += new global::System.EventHandler (this.OnServerActionActivated);
		this.ClientAction.Activated += new global::System.EventHandler (this.OnClientActionActivated);
	}
}
