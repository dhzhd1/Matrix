using System;
using Gtk;
using System.Diagnostics;

namespace MatrixResourceManager
{
	public partial class DialogGpuAssign : Gtk.Dialog
	{
		String ipList = string.Empty;
		String speedList = string.Empty;
		public String selectedIp = string.Empty;
		public String selectedSpeed = string.Empty;
		public bool exclusiveFlag = false;

		public DialogGpuAssign (String ipList, String speedList)
		{
			this.Build ();
			this.ipList = ipList;
			this.speedList = speedList;
			InitOptionBox ();
		}

		void InitOptionBox(){
			String[] ip_list = ipList.Split (',');
			String[] speed = speedList.Split (',');
			for (int i = 0; i < ip_list.Length; i++) {
				RadioButton radioBt = new RadioButton ("IP Address: " + ip_list [i] + "\nInterface Speed: " + speed [i]);
				radioBt.DrawIndicator = false;
				radioBt.Clicked += RadioBt_Activated;;
				vbox2.PackStart (radioBt, true, true, 0);
				radioBt.Show ();
			}
		}

		void RadioBt_Activated (object sender, EventArgs e)
		{
			RadioButton rb = sender as RadioButton;
			selectedIp = rb.Label.Split ('\n') [0].Split (':') [1].Trim();
			selectedSpeed = rb.Label.Split ('\n') [1].Split (':') [1].Trim ();
		}
			
		protected void OnCheckbutton2Clicked (object sender, EventArgs e)
		{
			exclusiveFlag = checkbutton2.Active;
		}
	}
}

