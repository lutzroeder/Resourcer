namespace Resourcer
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ResourceItem : ListViewItem
    {
        private ResourceBrowser resourceBrowser;

        public ResourceBrowser ResourceBrowser
        {
            get
            {
                return this.resourceBrowser;    
            }
            
            set
            {
                this.resourceBrowser = value;
            }   
        }

        public string ResourceName
        {
            get
            {
                return this.Text;
            }

            set
            {
                this.Text = value;
                
                this.ResourceBrowser.IsDirty = true;
            }
        }

        public object ResourceValue
        {
            get
            {
                return this.Tag;
            }

            set
            {
                this.Tag = value;

                if (this.SubItems.Count > 1)
                {
                    this.SubItems.RemoveAt(2);
                    this.SubItems.RemoveAt(1);
                }

                if (value != null)
                {
                    Type type = value.GetType();

                    this.SubItems.Add(value.ToString());
                    this.SubItems.Add(type.AssemblyQualifiedName);

                    switch (type.FullName)
                    {
                        case "System.String":
                            this.ImageIndex = 1;
                            break;

                        case "System.Drawing.Image":    
                        case "System.Drawing.Bitmap":
                            this.ImageIndex = 2;
                            break;

                        case "System.Drawing.Icon":
                            this.ImageIndex = 3;
                            break;
                        
                        case "System.Windows.Forms.Cursor":
                            this.ImageIndex = 4;
                            break;

                        case "System.Drawing.Font":
                            this.ImageIndex = 5;
                            break;

                        case "System.Byte[]":
                            this.ImageIndex = 6;
                            break;

                        case "System.Boolean":
                        case "System.SByte":
                        case "System.Byte":
                        case "System.Int32":
                        case "System.UInt32":
                        case "System.Int16":
                        case "System.UInt16":
                        case "System.Int64":
                        case "System.UInt64":
                            this.ImageIndex = 7;
                            break;

                        default:
                            this.ImageIndex = 8;
                            break;
                    }

                }
                else
                {
                    this.SubItems.Add("(null)");
                    this.SubItems.Add("-");
                    this.ImageIndex = 0;
                }
                
                this.ResourceBrowser.IsDirty = true;
            }
        }
    }
}