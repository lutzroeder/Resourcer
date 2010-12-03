namespace Resourcer.Forms
{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CommandBarItem : Component
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private Image image = null;
        private string text = null;
        private bool isEnabled = true;
        private bool isVisible = true;
        private object tag;

        private CommandBarItem()
        {
        }

        public CommandBarItem(string text)
        {
            this.text = text;
        }

        public CommandBarItem(Image image)
        {
            this.image = image;
        }

        public CommandBarItem(Image image, string text)
        {
            this.text = text;
            this.image = image;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.image = null;
                this.text = null;
            }

            base.Dispose(disposing);
        }

        public virtual object Tag
        {
            set 
            { 
                this.tag = value; 
            }
            
            get 
            { 
                return this.tag; 
            }
        }

        public virtual bool Visible
        {
            set
            { 
                if (value != this.isVisible)
                {
                    this.isVisible = value; 
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsVisible")); 
                }
            }

            get 
            { 
                return this.isVisible; 
            }
        }

        public virtual bool Enabled
        {
            set
            { 
                if (this.isEnabled != value)
                { 
                  this.isEnabled = value; 
                  this.OnPropertyChanged(new PropertyChangedEventArgs("IsEnabled")); 
                }
            }
    
            get 
            { 
                return this.isEnabled; 
            }
        }

        public Image Image
        {
            set
            { 
                if (value != this.image)
                {
                    this.image = value; 
                    this.OnPropertyChanged(new PropertyChangedEventArgs("Image")); 
                }
            }
        
            get 
            { 
                return this.image; 
            }
        }
    
        public string Text
        {
            set
            { 
                if (value != this.text)
                { 
                    this.text = value; 
                    this.OnPropertyChanged(new PropertyChangedEventArgs("Text"));
                }
            }

            get { return this.text; }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }
    }
}
