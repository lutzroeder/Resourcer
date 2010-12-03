namespace Resourcer.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CommandBarCheckBox : CommandBarButtonBase
    {
        private bool isChecked = false;

        public CommandBarCheckBox() : base("None")
        {
        }

        public CommandBarCheckBox(string text) : base(text)
        {
        }
    
        public CommandBarCheckBox(Image image, string text) : base(image, text)
        {   
        }

        public bool IsChecked
        {
            set
            { 
                if (value != this.isChecked)
                { 
                    this.isChecked = value; 
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsChecked")); 
                }
            }

            get { return this.isChecked; }
        }

        protected override void OnClick(EventArgs e)
        {
            this.IsChecked = !this.IsChecked;
            base.OnClick(e);
        }

        public override string ToString()
        {
            return "CheckBox(" + this.Text + "," + this.IsChecked + ")";
        }
    }
}
