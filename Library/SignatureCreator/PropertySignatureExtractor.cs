using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.SignatureCreator
{
    public class PropertySignatureExtractor
    {
        public static string CreatePropertySignature(PropertyInfo propertyInfo)
        {
            MethodInfo? getMethod = propertyInfo.GetGetMethod(true);
            string typeAccessorModifiers = ExtractTypeAccessorModifiers(getMethod);
            string typeName = ExtractTypeName(propertyInfo);
            string propertyName = propertyInfo.Name;
            string propertyAccessors = ExtractPropertyAccessors(propertyInfo);
            var result = string.Join(" ", 
                typeAccessorModifiers,
                typeName, 
                propertyName, 
                propertyAccessors);
            return result;
        }
        private static string ExtractPropertyAccessors(PropertyInfo propertyInfo)
        {
            const string begin = "{", end = "}", separator = "; ";
            var accessors = propertyInfo.GetAccessors(true);
            var stringBuilder = new StringBuilder(begin).Append(" ");
            foreach (var accessor in accessors)
            {
                if (accessor.IsSpecialName)
                {
                    stringBuilder.Append(ExtractTypeAccessorModifiers(accessor)).Append(" ").Append(accessor.Name).Append(separator);
                }
            }
            stringBuilder.Append(end);
            return stringBuilder.ToString();
        }

        private static string ExtractTypeAccessorModifiers(System.Reflection.MethodInfo methodInfo)
        {
            if (methodInfo.IsPublic)
                return "public";
            if (methodInfo.IsPrivate)
                return "private";
            if (methodInfo.IsFamily)
                return "protected";
            if (methodInfo.IsAssembly)
                return "internal";
            if (methodInfo.IsFamilyOrAssembly)
                return "protected internal";

            return "";
        }

        private static string ExtractTypeName(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsGenericType ? ExtractGenericTypeName(propertyInfo.PropertyType) : propertyInfo.PropertyType.Name;
        }

        private static string ExtractGenericTypeName(Type parameter)
        {
            var stringBuilder = new StringBuilder(Regex.Replace(parameter.Name, "`[0-9]+$", ""));
            stringBuilder.Append("<");
            if (parameter.IsGenericType)
            {
                stringBuilder.Append(ExtractGenericArgumentsTypeName(parameter.GenericTypeArguments));
            }
            stringBuilder.Append(">");
            return stringBuilder.ToString();
        }


        private static string ExtractGenericArgumentsTypeName(IEnumerable<Type> arguments)
        {
            var stringBuilder = new StringBuilder();
            foreach (var argument in arguments)
            {
                if (argument.IsGenericType)
                {
                    stringBuilder.Append(ExtractGenericTypeName(argument));
                }
                else stringBuilder.Append(argument);

                stringBuilder.Append(", ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }
    }
}