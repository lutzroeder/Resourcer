namespace Resourcer.Forms
{
    using System;
    using System.ComponentModel;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CommandBarMenu : CommandBarItem
    {
        public event EventHandler DropDown;
        private CommandBarItemCollection items = new CommandBarItemCollection();

        public CommandBarMenu() : base("None")
        {
        }

        public CommandBarMenu(string text) : base(text)
        {
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CommandBarItemCollection Items
        {
            get { return this.items; }
        }

        protected virtual void OnDropDown(EventArgs e)
        {
            if (this.DropDown != null)
            {
                this.DropDown(this, e);
            }
        }

        internal void PerformDropDown(EventArgs e)
        {
            this.OnDropDown(e);
        }
    }
}
