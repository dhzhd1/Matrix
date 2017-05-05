using System;
using Gtk;
using Gdk;
using Cairo;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;



namespace MatrixResourceManager
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class WidgetMonitor : Gtk.Bin
	{
		Context axisContext;			// Global Context store the graphic context for Axis, 
										// so the these information doesn't need to be re-generate during the data update
		Context legendContext;
		int refreshInterval = 5000;  	// This number is millsecond
		Thread startMonitor; 
		List<Cairo.Color> chartLineColors = new List<Cairo.Color> () { new Cairo.Color(1,0,0),new Cairo.Color(1,0.6,0),new Cairo.Color(0,0,1),new Cairo.Color(1, 0,0.5),new Cairo.Color(1,0,1), new Cairo.Color(0,1,1), new Cairo.Color(0.5, 0, 0.5), new Cairo.Color(0.5,0.5,0.5)};

		public String ServerIp;
		public String TcpPort;

		public WidgetMonitor ()
		{
			this.Build ();
			// Metric ID Define: 0: GPU Util, 1: Mem Util, 2: Power Draw, 3: Temperature


		}

		protected void OnDrawingarea2ExposeEvent (object o, ExposeEventArgs args)
		{
			DrawingArea legendDrawArea = (DrawingArea)o;
			legendContext = CairoHelper.Create (legendDrawArea.GdkWindow);
			List<int> gpuIds = new List<int> ();
			#if DEBUG
			 gpuIds = new List<int> () { 0, 1, 2, 3 };

			#endif

			int legendLabelStartX = 20;
			int legendLabelStartY = 20;
			int legendTextStartX = 20;
			int legendTextStartY = 50;
			int legendLabelLength = 50;
			int labelDistance = 40;
			//int labelToFontDistance = 15;

			foreach (int gpuId in gpuIds) {
				legendContext.SetSourceColor(chartLineColors[gpuId]);
				legendContext.Antialias = Antialias.Subpixel;
				legendContext.LineWidth = 20;
				legendContext.MoveTo (legendLabelStartX + gpuId * (legendLabelLength + labelDistance), legendLabelStartY);
				legendContext.LineTo (legendLabelStartX + gpuId * (legendLabelLength + labelDistance) + legendLabelLength, legendLabelStartY);
				legendContext.Stroke ();

				legendContext.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Bold);
				legendContext.SetFontSize(16);
				String notation = "GPU " + gpuId.ToString ();
				for (int i = 0; i < notation.Length; i++) {
					string subStr = notation.Substring (i, 1);
					TextExtents te = legendContext.TextExtents (subStr);
					legendContext.MoveTo (legendTextStartX + te.Width * i + gpuId * (labelDistance + legendLabelLength), legendTextStartY + te.Height/2);
					legendContext.ShowText (subStr);
				}

			}
		}


		protected void OnButton29Clicked (object sender, EventArgs e)
		{
			InitLegend ();
			InitAxis ();
			startMonitor = new Thread (new ThreadStart (StartMonitorGpu));
			startMonitor.Start ();
			button29.Sensitive = false;
			combobox4.Sensitive = false;
			combobox5.Sensitive = false;
			treeview2.Sensitive = false;
			button74.Sensitive = true;
			button30.Sensitive = true;

		}

		protected void OnButton30Clicked (object sender, EventArgs e)
		{
			startMonitor.Abort ();
			button29.Sensitive = true;
			combobox4.Sensitive = true;
			combobox5.Sensitive = true;
			treeview2.Sensitive = true;
			button30.Sensitive = false;
			button74.Sensitive = false;

		}



		void StartMonitorGpu(){
			while (true) {
				// Infinity loop for the update the data metric until this thread has been killed.
				BackgroundWorker updateMetricData = new BackgroundWorker ();
				updateMetricData.DoWork += UpdateMetricData_DoWork;
				updateMetricData.RunWorkerCompleted += UpdateMetricData_RunWorkerCompleted;
				updateMetricData.RunWorkerAsync ();
			}

		}

		void UpdateMetricData_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
		{
			Thread.Sleep (refreshInterval);
		}

		void UpdateMetricData_DoWork (object sender, DoWorkEventArgs e)
		{
			// On this portion, retrieve data only, when the data is ready, using worker complete event to update front line chart

		}

		Context InitLegend(){
			return null;
		}

		Context InitAxis(){
			return null;
		}


		
	}
}

