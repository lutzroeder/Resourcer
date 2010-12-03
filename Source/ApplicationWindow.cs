namespace Resourcer
{
    using System.Drawing;
    using System.Windows.Forms;
    using Resourcer.Forms;

    internal class ApplicationWindow : Form
    {
        private CommandBarManager commandBarManager;
        private CommandBar toolBar;
        private CommandBar menuBar;

        private ResourceBrowser resourceBrowser;
        private Splitter verticalSplitter;
        private ResourcerViewer resourceViewer;
        private StatusBar statusBar;

        public ApplicationWindow()
        {
            this.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("Resourcer.Application.ico"));
            this.Font = new Font("Tahoma", 8.25f);
            this.Text = (this.GetType().Assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false)[0] as System.Reflection.AssemblyTitleAttribute).Title;
            this.Size = new Size(480, 600);
            this.MinimumSize = new Size (240, 300);

            this.resourceBrowser = new ResourceBrowser();
            this.resourceBrowser.Dock = DockStyle.Fill;
            this.Controls.Add(this.resourceBrowser);
            
            this.verticalSplitter = new Splitter ();
            this.verticalSplitter.Dock = DockStyle.Bottom;
            this.verticalSplitter.BorderStyle = BorderStyle.None;
            this.Controls.Add(this.verticalSplitter);
            
            this.resourceViewer = new ResourcerViewer();
            this.resourceViewer.Dock = DockStyle.Bottom;
            this.resourceViewer.Height = 100;
            this.Controls.Add(this.resourceViewer);

            this.statusBar = new StatusBar();
            this.Controls.Add(this.statusBar);

            this.commandBarManager = new CommandBarManager();
            this.menuBar = new CommandBar(this.commandBarManager, CommandBarStyle.Menu);
            this.commandBarManager.CommandBars.Add(this.menuBar);
            this.toolBar = new CommandBar(this.commandBarManager, CommandBarStyle.ToolBar);
            this.commandBarManager.CommandBars.Add(this.toolBar);
            this.Controls.Add(this.commandBarManager);
        }

        public CommandBar MenuBar
        {
            get
            {
                return this.menuBar;    
            }   
        }

        public CommandBar ToolBar
        {
            get
            {
                return this.toolBar;    
            }   
        }
        
        public ResourceBrowser ResourceBrowser
        {
            get
            {
                return this.resourceBrowser;    
            }   
        }

        public ResourcerViewer ResourcerViewer
        {
            get
            {
                return this.resourceViewer;
            }
        }

        public string FileName
        {
            get
            {
                return this.statusBar.Text;
            }

            set
            {
                this.statusBar.Text = value;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.commandBarManager.PreProcessMessage(ref msg))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}