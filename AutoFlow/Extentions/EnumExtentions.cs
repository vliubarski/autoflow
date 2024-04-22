using AutoFlow.Models;
using System.Reflection;

namespace AutoFlow.Extentions
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());

            if (fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) is StringValueAttribute[] attrs && attrs.Length > 0)
            {
                return attrs[0].Value;
            }

            return value.ToString();
        }

        public static Menu ParseMenu(string value)
        {
            foreach (Menu enumValue in Enum.GetValues(typeof(Menu)))
            {
                var memberInfo = typeof(Menu).GetField(enumValue.ToString());
                var attribute = memberInfo.GetCustomAttribute<StringValueAttribute>();

                if (attribute != null && attribute.Value == value)
                {
                    return enumValue;
                }
            }

            // If no matching enum value is found, return a default value (e.g., Menu.Undefined)
            return Menu.Undefined;
        }
    }


}
