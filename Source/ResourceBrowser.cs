namespace Resourcer
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;

    internal class ResourceBrowser : ListView
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string fileName;
        private bool isDirty;
        
        public ResourceBrowser()
        {
            this.AllowDrop = true;
            this.View = View.Details;
            this.MultiSelect = true;
            this.AllowColumnReorder = true;
            this.FullRowSelect = true;
            this.HeaderStyle = ColumnHeaderStyle.Clickable;
            this.LabelEdit = true;
            this.HideSelection = false;
            this.TabIndex = 1;

            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Name";
            nameHeader.Width = 240;
            this.Columns.Add(nameHeader);

            ColumnHeader valueHeader = new ColumnHeader();
            valueHeader.Text = "Value";
            valueHeader.Width = 300;
            this.Columns.Add(valueHeader);

            ColumnHeader typeHeader = new ColumnHeader();
            typeHeader.Text = "Type";
            typeHeader.Width = 500;
            this.Columns.Add(typeHeader);

            this.SmallImageList = new ImageList();
            this.SmallImageList.Images.AddStrip(new Bitmap(this.GetType().Assembly.GetManifestResourceStream("Resourcer.Browser.png")));
            this.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;
            this.SmallImageList.TransparentColor = Color.FromArgb(255, 0, 128, 0);
        }
        
        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.fileName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        public bool IsDirty
        {
            get
            {
                return this.isDirty;    
            }
            
            set
            {
                this.isDirty = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("IsDirty"));
            }
        }

        public void New()
        {
            if (this.Close())
            {
                this.Items.Clear();
                this.FileName = string.Empty;
                this.IsDirty = false;
            }
        }

        public bool Close()
        {
            if (this.IsDirty)
            {
                DialogResult result = MessageBox.Show("Do you want to save the changes?", StringTable.GetString("ApplicationName"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (result)
                {
                    case DialogResult.Yes:  
                        if (!this.Save())
                        {
                            return false;
                        }
                        break;
                    
                    case DialogResult.Cancel:
                        return false;

                    case DialogResult.No:
                        return true;
                }
            }
            
            return true;
        }

        public bool SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save As";
            dialog.Filter = "All Resource files (*.resources, *.resx, *.txt)|*.resources;*.resx;*.txt|Resource files (*.resources)|*.resources|ResX files (*.resX)|*.resX|Text resource files (*.txt)|*.txt|All files|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return this.Save(dialog.FileName);
            }
            
            return false;
        }

        public bool Save()
        {
            string fileName = this.FileName;
            if ((fileName == null) || (fileName.Length == 0))
            {
                return this.SaveAs();   
            }

            return this.Save(fileName);
        }

        public void Load(string fileName)
        {
            this.New();
            
            this.FileName = fileName;
            string fileExtension = Path.GetExtension(fileName).ToLower();

            try
            {

            switch (fileExtension)
            {
                case ".xml":
                case ".resx":
                    using (ResXResourceReader reader = new ResXResourceReader(fileName))
                    {
                        this.BeginUpdate();

                        Hashtable resources = new Hashtable();
                        IDictionaryEnumerator enumerator = reader.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            string name = (string) enumerator.Key;
                            object value = enumerator.Value;
                            if (!resources.Contains(name))
                            {
                                this.AddResource(name, value);
                                resources.Add(name, value);
                            }
                        }   

                        this.EndUpdate();
                    }
                    break;
                    
                case ".resources":
                    using (FileStream stream = File.OpenRead(fileName))
                    {
                        this.BeginUpdate();
                        
                        using (ResourceReader reader = new ResourceReader(stream))
                        {
                            Hashtable resources = new Hashtable();
                            IDictionaryEnumerator enumerator = reader.GetEnumerator();

                            while (enumerator.MoveNext())
                            {
                                string name = (string)enumerator.Key;
                                object value = enumerator.Value;

                                if (!resources.Contains(name))
                                {
                                    this.AddResource(name, value);
                                    resources.Add(name, value);
                                }
                            }
                        }
                        
                        this.EndUpdate();
                    }
                    break;

                case ".txt":
                    using (StreamReader reader = File.OpenText(fileName))
                    {
                        this.BeginUpdate();

                        Hashtable resources = new Hashtable();
                        while (reader.Peek() != -1)
                        {
                            string line = reader.ReadLine();
                            line = line.TrimStart();
                            if (!line.StartsWith(";"))
                            {
                                int index = line.IndexOf("=");
                                if (index != -1)
                                {
                                    string name = line.Substring(0, index);
                                    string value = line.Substring(index + 1);
                                    name = name.Trim();
                                    if (!resources.Contains(name))
                                    {
                                        this.AddResource(name, value);
                                        resources.Add(name, value);
                                    }
                                }
                            }
                        }

                        this.EndUpdate();
                    }
                    break;


                default:
                    MessageBox.Show(this, "Unknown resource file format.", StringTable.GetString("ApplicationName"));
                    break;
            }
            
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());  
            }
        
            
            this.IsDirty = false;
        }

        public bool Save(string fileName)
        {
            this.FileName = fileName;

            string fileExtension = Path.GetExtension(fileName).ToLower();

            switch (fileExtension)
            {
                case ".xml" :
                case ".resx" :
                    try
                    {
                        using (ResXResourceWriter writer = new ResXResourceWriter(fileName))
                        {
                            foreach (ResourceItem item in this.Items)
                            {
                                writer.AddResource(item.ResourceName, item.ResourceValue);
                            }
                            writer.Generate();
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(this, exception.Message, StringTable.GetString("ApplicationName"));
                        return false;
                    }
                    break;

                case ".resources":
                    try
                    {
                        using (ResourceWriter writer = new ResourceWriter(fileName))
                        {
                            foreach (ResourceItem item in this.Items)
                            {
                                writer.AddResource(item.ResourceName, item.ResourceValue);
                            }
                            writer.Generate();
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(this, exception.Message, StringTable.GetString("ApplicationName"));
                        return false;
                    }
                    break;

                case ".txt":
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(fileName))
                        {
                            foreach (ResourceItem item in this.Items)
                            {
                                if ((item.ResourceValue != null) && (item.ResourceValue is String))
                                {
                                    writer.Write(item.ResourceName);
                                    writer.Write("=");
                                    writer.Write(item.ResourceValue);
                                    writer.WriteLine();
                                }
                                else
                                {
                                    MessageBox.Show(this, "Resource \'" + item.ResourceName + "\' cannot be saved as a text resource.", StringTable.GetString("ApplicationName"));
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(this, exception.Message, StringTable.GetString("ApplicationName"));
                        return false;
                    }
                    break;

                default:
                    MessageBox.Show(this, "Unknown resource file format.", StringTable.GetString("ApplicationName"));
                    return false;
            }
            
            this.IsDirty = false;
            return true;
        }

        public void Copy()
        {
            if (this.SelectedItems.Count > 0)
            {
                Hashtable dictionary = new Hashtable();
                foreach (ResourceItem item in this.SelectedItems)
                {
                    try
                    {
                        ICloneable value = (ICloneable)item.ResourceValue;
                        dictionary.Add(item.ResourceName, value.Clone());
                    }
                    catch (Exception)
                    {
                    }
                }

                Clipboard.SetDataObject(dictionary);
            }
        }

        public bool CanCopy
        {
            get
            {
                return (this.SelectedItems.Count > 0);
            }
        }

        public void Cut()
        {
            if (this.SelectedItems.Count > 0)
            {
                IDictionary dictionary = new Hashtable();

                foreach (ResourceItem item in this.SelectedItems)
                {
                    try
                    {
                        dictionary.Add(item.ResourceName, item.ResourceValue);
                        this.RemoveResource(item.ResourceName);
                    }
                    catch (Exception)
                    {
                    }
                }

                Clipboard.SetDataObject(dictionary);
            }
        }

        public bool CanCut
        {
            get
            {
                return (this.SelectedItems.Count > 0);
            }
        }

        public void Paste()
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(typeof(Hashtable).FullName))
            {
                Hashtable dictionary = (Hashtable)dataObject.GetData(typeof(Hashtable));

                foreach (DictionaryEntry n in dictionary)
                {
                    if (!this.ContainsName((string)n.Key))
                    {
                        ICloneable cloneable = (ICloneable)n.Value;
                        object value = cloneable.Clone();
                        ResourceItem item = this.AddResource((string)n.Key, value);
                        item.Selected = true;
                    }
                    else
                    {
                        MessageBox.Show(this, "Resource \'" + n.Key + "\' already exists.", StringTable.GetString("ApplicationName"));
                    }
                }
            }
        }

        public bool CanPaste
        {
            get
            {
                IDataObject dataObject = Clipboard.GetDataObject();
                return dataObject.GetDataPresent(typeof(Hashtable).FullName);
            }
        }

        public void Delete()
        {
            foreach (ResourceItem item in this.SelectedItems)
            {
                if (item.Text != null)
                {
                    this.RemoveResource(item.ResourceName);
                }
            }
        }

        public bool CanDelete
        {
            get
            {
                return (this.SelectedItems.Count > 0);
            }
        }

        public void InsertText()
        {
            ResourceItem item = this.AddResource("[name]", "[value]");
            this.SelectedItems.Clear();
            item.Selected = true;
            item.BeginEdit();
        }

        public void InsertFile(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();

            object value = null;
            switch (fileExtension)
            {
                case "cur":
                    value = new Cursor(fileName);
                    break;

                case ".ico":
                    value = new Icon(fileName);
                    break;

                case ".bmp":
                case ".emf":
                case ".exif":
                case ".gif":
                case ".jpeg":
                case ".jpg":
                case ".png":
                case ".tif":
                case ".tiff":
                case ".wmf":
                    value = new Bitmap(fileName);
                    break;

                case ".txt":
                case ".xml":
                case ".xsl":
                case ".xsd":
                case ".css":
                case ".htm":
                case ".html":
                case ".asp":
                case ".aspx":
                    using (StreamReader reader = File.OpenText(fileName))
                    {
                        value = reader.ReadToEnd();
                    }
                    break;


                default:
                    using (FileStream stream = File.OpenRead(fileName))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        value = buffer;
                    }
                    break;
            }

            if (value != null)
            {
                ResourceItem item = this.AddResource(Path.GetFileName(fileName), value);
                this.SelectedItems.Clear();
                item.Selected = true;
            }
        }

        private bool ContainsName(ResourceItem item)
        {
            foreach (ResourceItem currentItem in this.Items)
            {
                if ((item.ResourceName == currentItem.ResourceName) && (item != currentItem))
                {
                    return true;
                }
            }

            return false;
        }


        private bool ContainsName(string name)
        {
            ListViewItemCollection items = this.Items;
            for (int i = 0; i < items.Count; i++)
            {
                ResourceItem item = (ResourceItem) items[i];
                if (name == item.ResourceName)
                {
                    return true;
                }
            }

            return false;
        }

        private ResourceItem AddResource(string name, object value)
        {
            ResourceItem item = new ResourceItem();
            this.Items.Add(item);
            item.ResourceBrowser = this;
            item.ResourceName = name;
            item.ResourceValue = value;
            return item;
        }

        private void RemoveResource(string name)
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                ResourceItem item = (ResourceItem) this.Items[i];
                if (name == item.ResourceName)
                {
                    this.Items.RemoveAt(i);
                }
            }
        }

        protected override void OnItemActivate(EventArgs e)
        {
            if (this.SelectedItems.Count == 1)
            {
                this.SelectedItems[0].BeginEdit();
            }

            base.OnItemActivate(e);
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
            {
                if (this.SelectedItems.Count == 1)
                {
                    this.SelectedItems[0].BeginEdit();
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnAfterLabelEdit(LabelEditEventArgs e)
        {
            base.OnAfterLabelEdit(e);

            ResourceItem item = (ResourceItem) this.Items[e.Item];
            
            bool hasChanged = true;
            
            if ((e.Label != null) && (this.ContainsName(e.Label)))
            {
                MessageBox.Show(this, "Resource \'" + e.Label + "\' already exists.", StringTable.GetString("ApplicationName"));
                item.BeginEdit();
                hasChanged = false;
            }
            
            if ((e.Label == null) && (this.ContainsName(item)))
            {
                MessageBox.Show(this, "Resource \'" + item.ResourceName + "\' already exists.", StringTable.GetString("ApplicationName"));
                item.BeginEdit();
                hasChanged = false;
            }
            
            if (hasChanged)
            {
                this.IsDirty = true;    
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileDrop"))
            {
                foreach (string fileName in (string[])e.Data.GetData("FileDrop"))
                {
                    this.Load(fileName);
                }
            }

            base.OnDragDrop(e);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            base.OnDragEnter(e);
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);
            this.ListViewItemSorter = new Comparer(e.Column);
            this.Sorting = SortOrder.Ascending;
        }

        private class Comparer: IComparer
        {
            private int columnIndex = 0;

            public Comparer(int columnIndex)
            {
                this.columnIndex = columnIndex;
            }

            public int Compare(object a, object b)
            {
                string text1 = (a as ListViewItem).SubItems[this.columnIndex].Text;
                string text2 = (b as ListViewItem).SubItems[this.columnIndex].Text;
                return text1.CompareTo(text2);
            }
        }
    }
}