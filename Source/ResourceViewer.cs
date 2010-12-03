namespace Resourcer
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    internal class ResourcerViewer : ContainerControl
    {
        private ResourceItem item;

        public ResourcerViewer()
        {
            // this.TabStop = false;
            this.BackColor = Color.FromArgb(250, 250, 225);  
        }

        public ResourceItem Item
        {
            get
            {
                return this.item;
            }

            set
            {
                this.Controls.Clear();

                this.item = value;

                if ((this.item != null) && (this.item.ResourceValue != null))
                {
                    if (this.item.ResourceValue is string)
                    {
                        StringViewer viewer = new StringViewer();
                        this.Controls.Add(viewer);
                        viewer.Item = this.item;
                    }
                    
                    if (this.item.ResourceValue is byte[])
                    {
                        ByteArrayViewer viewer = new ByteArrayViewer();
                        this.Controls.Add(viewer);
                        viewer.Item = this.item;
                    }

                    if (this.item.ResourceValue is Cursor)
                    {
                        ImageViewer viewer = new ImageViewer();
                        this.Controls.Add(viewer);
                        viewer.Item = this.item;
                    }

                    if (this.item.ResourceValue is Icon)
                    {
                        ImageViewer viewer = new ImageViewer();
                        this.Controls.Add(viewer);
                        viewer.Item = this.item;
                    }

                    if (this.item.ResourceValue is Image)
                    {
                        ImageViewer viewer = new ImageViewer();
                        this.Controls.Add(viewer);
                        viewer.Item = this.item;
                    }
                }
            }
        }

        public void Cut()
        {
            StringViewer stringViewer = this.Controls[0] as StringViewer;
            if (stringViewer != null)
            {
                stringViewer.Cut();
            }
        }

        public void Copy()
        {
            StringViewer stringViewer = this.Controls[0] as StringViewer;
            if (stringViewer != null)
            {
                stringViewer.Copy();
            }
        }

        public void Paste()
        {
            StringViewer stringViewer = this.Controls[0] as StringViewer;
            if (stringViewer != null)
            {
                stringViewer.Paste();
            }
        }

        private class StringViewer: TextBox
        {
            private ResourceItem item;
            private bool isEnabled;

            public StringViewer()
            {
                this.Dock = DockStyle.Fill;
                this.Multiline = true;
                this.ScrollBars = ScrollBars.Both;
                this.WordWrap = false;
                this.Font = new Font("Arial", 10f);
            }

            public ResourceItem Item
            {
                set
                {
                    this.item = value;

                    this.isEnabled = false;
                    this.Text = (string) this.item.ResourceValue;
                    this.isEnabled = true;
                }

                get
                {
                    return this.item;
                }
            }

            protected override void OnTextChanged(EventArgs e)
            {
                if (this.isEnabled)
                {
                    this.item.ResourceValue = this.Text;
                }
                
                base.OnTextChanged(e);
            }

        }
        
        private class ByteArrayViewer: TextBox
        {
            private ResourceItem item;

            public ByteArrayViewer()
            {
                this.Dock = DockStyle.Fill;
                this.Multiline = true;
                this.ReadOnly = true;
                this.WordWrap = false;
                this.Font = new Font("Courier New", 7.5f);
                this.ScrollBars = ScrollBars.Both;
            }

            public ResourceItem Item
            {
                set
                {
                    this.item = value;
                    byte[] bytes = (byte[]) this.item.ResourceValue;
                    
                    StringWriter writer = new StringWriter();
    
                    ASCIIEncoding decoder = new ASCIIEncoding();
                    int position = 0;
            
                    while (position < bytes.Length)
                    {
                        writer.Write(position.ToString("X6", CultureInfo.InvariantCulture));
                        writer.Write(": ");
                        
                        for (int i = position; i < position + 16; i++)
                        {
                            writer.Write((i < bytes.Length) ? bytes[i].ToString("X2", CultureInfo.InvariantCulture) : "  ");
                            writer.Write(" ");
                        }
            
                        writer.Write(" ");
            
                        for (int i = position; i < position + 16; i++)
                        {
                            if (i < bytes.Length)
                            {
                                char[] chars = decoder.GetChars(new byte[] { bytes[i] } );
                                writer.Write(char.IsControl(chars[0]) ? '.' : chars[0]);
                            }
                        }
            
                        writer.WriteLine();

                        position += 16;
                    }
    
                    this.Text = writer.ToString();
                }

                get
                {
                    return this.item;   
                }   
            }
        }

        private class ImageViewer: ScrollableControl
        {
            private PictureBox pictureBox;
            private ResourceItem item;

            public ImageViewer()
            {
                this.Dock = DockStyle.Fill;
                this.AutoScroll = true;

                this.pictureBox = new PictureBox();
                this.pictureBox.BorderStyle = BorderStyle.FixedSingle;
                this.pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            }

            public ResourceItem Item
            {
                get
                {
                    return this.item;
                }

                set
                {
                    this.item = value;

                    this.Controls.Clear();

                    if (this.item.ResourceValue is Cursor)
                    {
                        Cursor cursor = this.item.ResourceValue as Cursor;
                        Bitmap image = new Bitmap(cursor.Size.Width, cursor.Size.Height);
                        Graphics graphics = Graphics.FromImage(image);
                        graphics.FillRectangle(new SolidBrush(Color.DarkCyan), 0, 0, image.Width, image.Height);
                        cursor.Draw(graphics, new Rectangle(0, 0, image.Width, image.Height));
                        this.SetPictureBox(image);
                    }

                    if (this.item.ResourceValue is Icon)
                    {
                        Icon icon = this.item.ResourceValue as Icon;
                        this.SetPictureBox(icon.ToBitmap());
                    }

                    if (this.item.ResourceValue is Image)
                    {
                        Image image = this.item.ResourceValue as Image;
                        this.SetPictureBox(image);
                    }
                }
            }

            private void SetPictureBox(Image image)
            {
                this.pictureBox.Image = image;

                Size autoScrollMargin = new Size(0, 0);
                int dx = (this.Size.Width - this.pictureBox.Size.Width);
                int dy = (this.Size.Height - this.pictureBox.Size.Height);

                if (dx < 80)
                {
                    dx = 80;
                    autoScrollMargin.Width = 40;
                }

                if (dy < 80)
                {
                    dy = 80;
                    autoScrollMargin.Height = 40;
                }

                this.AutoScrollMargin = autoScrollMargin;
                this.pictureBox.Location = new Point(dx / 2, dy / 2);
                this.Controls.Add(this.pictureBox);
            }
        }
    }
}