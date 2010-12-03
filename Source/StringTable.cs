namespace Resourcer
{
    using System;
    using System.IO;
    using System.Drawing;

    internal sealed class StringTable
    {
        private StringTable()
        {
        }

        public static string GetString(string name)
        {
            switch (name)
            {
                case "ApplicationName":
                    return ".NET Resourcer";

                case "Homepage":
                    return "http://www.lutzroeder.com";

                case "Ok":
                    return "OK";

                case "Cancel":
                    return "Cancel";
            }
            
            throw new NotSupportedException();  
        }
    }
}
