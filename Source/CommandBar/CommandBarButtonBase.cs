namespace Resourcer.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    
    public abstract class CommandBarButtonBase : CommandBarControl
    {
        private Keys shortcut = Keys.None;

        protected CommandBarButtonBase(string text) : base(text)
        {
        }

        protected CommandBarButtonBase(Image image) : base(image)
        {
        }

        protected CommandBarButtonBase(Image image, string text) : base(image, text)
        {
        }

        public Keys Shortcut
        {
            set
            { 
                if (value != this.shortcut)
                { 
                    this.shortcut = value; 
                    this.OnPropertyChanged(new PropertyChangedEventArgs("Shortcut")); 
                }
            }

            get { return this.shortcut; }
          }
    }
}
