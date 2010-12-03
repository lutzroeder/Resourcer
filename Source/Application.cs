namespace Resourcer
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;
    using Resourcer.Forms;

    internal class Application
    {
        [STAThread()]
        public static void Main(string[] arguments)
        {
            // Register file extensions
            try
            {
                Registry.ClassesRoot.CreateSubKey(".resources").SetValue(null, "resourcesfile");
                Registry.ClassesRoot.CreateSubKey("resourcesfile\\shell\\Browse with .NET Resourcer\\command").SetValue(null, "\"" + typeof(Application).Module.FullyQualifiedName + "\" \"%L\"");
                Registry.ClassesRoot.CreateSubKey(".resx").SetValue(null, "resxfile");
                Registry.ClassesRoot.CreateSubKey("resxfile\\shell\\Browse with .NET Resourcer\\command").SetValue(null, "\"" + typeof(Application).Module.FullyQualifiedName + "\" \"%L\"");
            }
            catch (UnauthorizedAccessException)
            {
            }

            new Application().Run();
        }

        private ApplicationWindow applicationWindow;
        private CommandBarButton cutItem;
        private CommandBarButton copyItem;
        private CommandBarButton pasteItem;
        private CommandBarButton deleteItem;
    
        public Application()
        {   
            this.applicationWindow = new ApplicationWindow();
            this.applicationWindow.Load +=new EventHandler(this.ApplicationWindow_Load);
            this.applicationWindow.Closing += new CancelEventHandler(this.ApplicationWindow_Closing);
            this.applicationWindow.ResourceBrowser.PropertyChanged += new PropertyChangedEventHandler(this.ResourceBrowser_PropertyChanged);
            this.applicationWindow.ResourceBrowser.SelectedIndexChanged += new EventHandler(this.ResourceBrowser_SelectedIndexChanged);

            this.cutItem = new CommandBarButton(CommandBarResource.Cut, "Cu&t", new EventHandler(this.Cut_Click), Keys.Control | Keys.X);
            this.copyItem = new CommandBarButton(CommandBarResource.Copy, "&Copy", new EventHandler(this.Copy_Click), Keys.Control | Keys.C);
            this.pasteItem = new CommandBarButton(CommandBarResource.Paste, "&Paste", new EventHandler(this.Paste_Click), Keys.Control | Keys.V);
            this.deleteItem = new CommandBarButton(CommandBarResource.Delete, "&Delete", new EventHandler(this.Delete_Click));

            // MenuBar
            CommandBar menuBar = this.applicationWindow.MenuBar;

            CommandBarMenu fileMenu = menuBar.Items.AddMenu("&File");
            fileMenu.Items.AddButton(CommandBarResource.New, "&New", new EventHandler(this.New_Click), Keys.Control | Keys.N);
            fileMenu.Items.AddButton(CommandBarResource.Open, "&Open...", new EventHandler(this.Open_Click), Keys.Control | Keys.O);
            fileMenu.Items.AddButton(CommandBarResource.Save, "&Save", new EventHandler(this.Save_Click), Keys.Control | Keys.S);
            fileMenu.Items.AddButton("Save &As...", new EventHandler(this.SaveAs_Click));
            fileMenu.Items.AddSeparator();
            fileMenu.Items.AddButton("E&xit", new EventHandler(this.Exit_Click));

            CommandBarMenu editMenu = menuBar.Items.AddMenu("&Edit");
            editMenu.Items.Add(this.cutItem);
            editMenu.Items.Add(this.copyItem);
            editMenu.Items.Add(this.pasteItem);
            editMenu.Items.Add(this.deleteItem);
            editMenu.Items.AddSeparator();
            editMenu.Items.AddButton(CommandBarResource.Edit, "Insert &Text...", new EventHandler(this.InsertText_Click), Keys.Control | Keys.T);
            editMenu.Items.AddButton(CommandBarResource.Parent, "Insert &Files...", new EventHandler(this.InsertFiles_Click), Keys.Control | Keys.F);
            
            CommandBarMenu helpMenu = menuBar.Items.AddMenu("&Help");
            helpMenu.Items.AddButton("&About .NET Resourcer...", new EventHandler(this.About_Click));

            // ToolBar
            CommandBar toolBar = this.applicationWindow.ToolBar;
            toolBar.Items.AddButton(CommandBarResource.New, "New", new EventHandler(this.New_Click), Keys.Control | Keys.N);
            toolBar.Items.AddButton(CommandBarResource.Open, "Open...", new EventHandler(this.Open_Click), Keys.Control | Keys.O);
            toolBar.Items.AddButton(CommandBarResource.Save, "Save", new EventHandler(this.Save_Click), Keys.Control | Keys.S);
            toolBar.Items.AddSeparator();
            toolBar.Items.Add(this.cutItem);
            toolBar.Items.Add(this.copyItem);
            toolBar.Items.Add(this.pasteItem);
            toolBar.Items.Add(this.deleteItem);
            toolBar.Items.AddSeparator();
            toolBar.Items.AddButton(CommandBarResource.Edit, "Insert &Text...", new EventHandler(this.InsertText_Click), Keys.Control | Keys.T);
            toolBar.Items.AddButton(CommandBarResource.Parent, "Insert &Files...", new EventHandler(this.InsertFiles_Click), Keys.Control | Keys.F);

            System.Windows.Forms.Application.Idle += new EventHandler(this.Application_Idle);
        }
        
        public void Run()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(this.applicationWindow);   
        }
        
        private void Application_Idle(object sender, EventArgs e)
        {
            this.copyItem.Enabled = this.applicationWindow.ResourceBrowser.CanCopy;
            this.cutItem.Enabled = this.applicationWindow.ResourceBrowser.CanCut;
            this.pasteItem.Enabled = this.applicationWindow.ResourceBrowser.CanPaste;
            this.deleteItem.Enabled = this.applicationWindow.ResourceBrowser.CanDelete;
        }
        
        private void New_Click(object sender, EventArgs e)
        {
            this.applicationWindow.ResourceBrowser.New();   
            this.ResourceBrowser_SelectedIndexChanged(this, EventArgs.Empty);
        }
        
        private void Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All Resource files (*.resources, *.resx, *.txt)|*.resources;*.resx;*.txt|Resource files (*.resources)|*.resources|ResX files (*.resX)|*.resX|Text resource files (*.txt)|*.txt|All files|*.*";
            dialog.Title = "Open";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.applicationWindow.ResourceBrowser.Load(dialog.FileName);
            }
        }
        
        private void SaveAs_Click(object sender, EventArgs e)
        {
            this.applicationWindow.ResourceBrowser.SaveAs();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.applicationWindow.ResourceBrowser.Save();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.applicationWindow.Close();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (this.applicationWindow.ResourceBrowser.Focused)
            {
                this.applicationWindow.ResourceBrowser.Cut();
            }
            else
            {
                this.applicationWindow.ResourcerViewer.Cut();
            }
        }
        
        private void Copy_Click(object sender, EventArgs e)
        {
            if (this.applicationWindow.ResourceBrowser.Focused)
            {
                this.applicationWindow.ResourceBrowser.Copy();
            }
            else
            {
                this.applicationWindow.ResourcerViewer.Copy();
            }
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (this.applicationWindow.ResourceBrowser.Focused)
            {
                this.applicationWindow.ResourceBrowser.Paste();
            }
            else
            {
                this.applicationWindow.ResourcerViewer.Paste();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (this.applicationWindow.ResourceBrowser.Focused)
            {
                this.applicationWindow.ResourceBrowser.Delete();
            }
        }

        private void InsertText_Click(object sender, EventArgs e)
        {
            this.applicationWindow.ResourceBrowser.InsertText();
        }

        private void InsertFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All Files (*.*)|*.*";
            dialog.Title = "Insert Files";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in dialog.FileNames)
                {
                    this.applicationWindow.ResourceBrowser.InsertFile(fileName);
                }
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            AboutDialog dialog = new AboutDialog();
            dialog.ShowDialog(this.applicationWindow);
        }

        private void ResourceBrowser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            StringWriter writer = new StringWriter();

            if ((this.applicationWindow.ResourceBrowser.FileName != null) && (this.applicationWindow.ResourceBrowser.FileName.Length != 0))
            {
                writer.Write(this.applicationWindow.ResourceBrowser.FileName + " ");
            }

            this.applicationWindow.FileName = writer.ToString();
        }

        private void ResourceBrowser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.applicationWindow.ResourceBrowser.SelectedItems.Count == 1)
            {
                this.applicationWindow.ResourcerViewer.Item = (ResourceItem)this.applicationWindow.ResourceBrowser.SelectedItems[0];
            }
            else
            {
                this.applicationWindow.ResourcerViewer.Item = null;
            }
        }

        private void ApplicationWindow_Load(object sender, EventArgs e)
        {
            this.applicationWindow.ResourceBrowser.New();

            CommandLine commandLine = new CommandLine();
            string[] fileNames = commandLine.GetArguments(string.Empty);

            if ((fileNames != null) && (fileNames.Length == 1))
            {
                this.applicationWindow.ResourceBrowser.Load(fileNames[0]);
            }

            this.ResourceBrowser_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void ApplicationWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !this.applicationWindow.ResourceBrowser.Close();
        }
    }
}
