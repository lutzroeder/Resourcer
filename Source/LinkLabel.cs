namespace Resourcer
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    internal class LinkLabel : Label
    {
        public LinkLabel()
        {
            this.Cursor = Cursors.Hand;
            this.Font = new Font("Tahoma", 8.25f, FontStyle.Underline);
            this.ForeColor = Color.DarkGreen;
        }  
        
        protected override void OnMouseEnter(EventArgs e)
        {
            this.ForeColor = Color.Green;
            base.OnMouseEnter(e);
        }
        
        protected override void OnMouseLeave(EventArgs e)
        {
            this.ForeColor = Color.DarkGreen;
            base.OnMouseLeave(e);
        }
    
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
            {
                Process.Start(Text);
            }

            base.OnMouseDown(e);
        }
    
        public override string Text
        {
            get { return base.Text; }

            set 
            { 
                base.Text = value; 
                this.Size = new Size(this.PreferredWidth, this.PreferredHeight); 
            }
        }
    }
}
