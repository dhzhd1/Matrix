using System;

namespace MatrixResourceManager
{
	public partial class DialogUnderDevelopment : Gtk.Dialog
	{
		public DialogUnderDevelopment ()
		{
			this.Build ();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}
	}
}

