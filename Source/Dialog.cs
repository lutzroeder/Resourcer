namespace Resourcer
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class Dialog : Form
    {
        public Dialog()
        {
            this.Text = StringTable.GetString("ApplicationName");
            this.Icon = null;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Font = new Font("Tahoma", 8.25f);
            this.ControlBox = true;
            this.MaximizeBox = this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;      
        }


        public bool Run()
        {
            return (this.ShowDialog() == DialogResult.OK);
        }
    }
}
