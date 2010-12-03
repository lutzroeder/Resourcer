namespace Resourcer.Forms
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;

    public class CommandBarTypeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if ((destinationType == typeof(InstanceDescriptor)) || (destinationType == typeof(string)))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                ConstructorInfo constructorInfo = typeof(CommandBar).GetConstructor(Type.EmptyTypes);
                return new InstanceDescriptor(constructorInfo, null, false);
            }

            if (destinationType == typeof(string))
            {
                CommandBar commandBar = (CommandBar) value;
                return (commandBar != null) ? commandBar.Style.ToString() : string.Empty;
            }


            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
