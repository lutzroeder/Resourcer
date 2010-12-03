namespace Resourcer
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;

    internal sealed class CommandLine 
    {
        private IDictionary dictionary = new Hashtable();

        public CommandLine() 
        {
            string[] arguments = Environment.GetCommandLineArgs();
            for (int i = 1; i < arguments.Length; i++)
            {
                string argument = arguments[i];
                string name = string.Empty;
                string value = string.Empty;

                if ((argument[0] != '/') && (argument[0] != '-')) 
                {
                    value = argument;
                }
                else 
                {
                    int index = argument.IndexOf(':');
                    if (index == -1)
                    {
                        // "-option" without value
                        name = argument.Substring(1).ToLower(CultureInfo.InvariantCulture);

                        // Turn '-?' into '-help'
                        if (name == "?")
                            name = "help";
                    }
                    else 
                    {
                        // "-option:value"
                        name = argument.Substring(1, index - 1).ToLower(CultureInfo.InvariantCulture);
                        value = argument.Substring(index + 1);
                    }
                }

                // Ensure key exists and add value.
                ArrayList strings = (ArrayList) this.dictionary[name];
                if (strings == null)
                {
                    strings = new ArrayList();  
                    this.dictionary.Add(name, strings);
                }

                strings.Add(value);
            }
        }

        public string GetArgument(string name)
        {
            ArrayList list = (ArrayList) this.dictionary[name];
            if (list != null)
            {
                if (list.Count != 1)
                {
                    throw new InvalidOperationException();
                }

                return (string) list[0];
            }
            
            return null;
        }
        
        public string[] GetArguments(string name)
        {
            ArrayList list = (ArrayList) this.dictionary[name];
            if (list != null)
            {
                string[] array = new string[list.Count];
                list.CopyTo(array, 0);
                return array;
            }
            
            return null;
        }
    }
}