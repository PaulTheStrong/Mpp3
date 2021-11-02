using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.SignatureCreator
{
    public class FieldSignatureExtractor
    {
        public static string CreateTypeSignature(FieldInfo fieldInfo)
        {
            string accessorModifiers = ExtractAccessorModifiers(fieldInfo);
            string typeModifiers = ExtractTypeModifier(fieldInfo);
            string typeName = ExtractTypeName(fieldInfo);
            var result = string.Join(" ", 
                accessorModifiers, 
                typeModifiers,
                typeName, 
                fieldInfo.Name);
            return result;
        }
        private static string ExtractAccessorModifiers(FieldInfo filedInfo)
        {
            if (filedInfo.IsPublic)
                return "public";
            if (filedInfo.IsPrivate)
                return "private";
            if (filedInfo.IsFamily)
                return "protected";
            if (filedInfo.IsAssembly)
                return "internal";
            if (filedInfo.IsFamilyOrAssembly)
                return "protected internal";
            return "";
        }

        private static string ExtractTypeModifier(FieldInfo fieldInfo)
        {
            return fieldInfo.IsStatic ? "static" : "";
        }

        private static string ExtractTypeName(FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType.IsGenericType)
            {
                return ExtractGenericTypeName(fieldInfo.FieldType);
            }
            return fieldInfo.FieldType.Name;
        }

        private static string ExtractGenericTypeName(Type parameter)
        {
            var stringBuilder = new StringBuilder(Regex.Replace(parameter.Name, "`[0-9]+$", ""));
            stringBuilder.Append("<");
            if (parameter.IsGenericType)
            {
                stringBuilder.Append(ExtractGenericArgumentsTypes(parameter.GenericTypeArguments));
            }
            stringBuilder.Append(">");
            return stringBuilder.ToString();
        }


        private static string ExtractGenericArgumentsTypes(IEnumerable<Type> arguments)
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