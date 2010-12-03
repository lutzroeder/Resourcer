namespace Resourcer
{
    using System;
    using System.Drawing;
    using System.IO;
    
    internal sealed class CommandBarResource
    {
        private static Image[] images = null;

        private CommandBarResource()
        {
        }

        static CommandBarResource()
        {
            using (Stream stream = typeof(CommandBarResource).Assembly.GetManifestResourceStream("Resourcer.CommandBar.png"))
            {
                Bitmap bitmap = new Bitmap(stream);
                int count = (int)(bitmap.Width / bitmap.Height);

                images = new Image[count];

                Rectangle rectangle = new Rectangle(0, 0, bitmap.Height, bitmap.Height);
                for (int i = 0; i < count; i++)
                {
                    images[i] = bitmap.Clone(rectangle, bitmap.PixelFormat);
                    rectangle.X += bitmap.Height;
                }
            }
        }   
    
        public static Image New
        {
            get { return images[0]; }
        }
        
        public static Image Open
        {
            get { return images[1]; } 
        }
        
        public static Image Save
        { 
            get { return images[2]; } 
        }
        
        public static Image Cut
        {
            get { return images[3]; }
        }
        
        public static Image Copy
        { 
            get { return images[4]; } 
        }
        
        public static Image Paste
        { 
            get { return images[5]; } 
        }
        
        public static Image Delete          
        { 
            get { return images[6]; } 
        }
        
        public static Image Properties
        { 
            get { return images[7]; } 
        }
        
        public static Image Undo
        { 
            get { return images[8]; } 
        }
        
        public static Image Redo
        { 
            get { return images[9]; }
        }
        
        public static Image Preview
        { 
            get { return images[10]; } 
        }
        
        public static Image Print
        {
            get { return images[11]; }
        }
        
        public static Image Search
        {
            get { return images[12]; } 
        }
        
        public static Image ReSearch
        { 
            get { return images[13]; } 
        }
        
        public static Image Help
        { 
            get { return images[14]; } 
        }
        
        public static Image ZoomIn
        { 
            get { return images[15]; } 
        }
        
        public static Image ZoomOut
        {
            get { return images[16]; } 
        }
        
        public static Image Back
        {
            get { return images[17]; } 
        }
        
        public static Image Forward
        {
            get { return images[18]; } 
        }
        
        public static Image Favorites
        { 
            get { return images[19]; } 
        }
        
        public static Image AddToFavorites
        { 
            get { return images[20]; } 
        }
        
        public static Image Stop
        { 
            get { return images[21]; } 
        }
        
        public static Image Refresh
        { 
            get { return images[22]; } 
        }
        
        public static Image Home
        { 
            get { return images[23]; } 
        }
        
        public static Image Edit
        {   
            get { return images[24]; } 
        }
        
        public static Image Tools
        {
            get { return images[25]; }
        }
    
        public static Image Tiles
        {
            get { return images[26]; } 
        }
    
        public static Image Icons
        {
            get { return images[27]; } 
        }
    
        public static Image List
        {
            get { return images[28]; } 
        }
        
        public static Image Details
        {
            get { return images[29]; } 
        }
    
        public static Image Pane
        {
            get { return images[30]; } 
        }
    
        public static Image Culture
        {
            get { return images[31]; } 
        }
        
        public static Image Languages
        {
            get { return images[32]; }
        }

        public static Image History
        {   
            get { return images[33]; } 
        }

        public static Image Email
        {   
            get { return images[34]; } 
        }
    
        public static Image Parent
        {
            get { return images[35]; } 
        }
        
        public static Image FolderProperties
        {
            get { return images[36]; } 
        }
    }
}
