using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.SignatureCreator
{
    public class ConstructorSignatureExtractor
    {
         public static string CreateConstructorSignature(ConstructorInfo constructorInfo)
        {
            string accessModifier = ExtractAccessModifier(constructorInfo);
            string constructorName = ExtractConstructorName(constructorInfo);
            string arguments = ExtractArguments(constructorInfo);
            var result = string.Join(" ", 
                accessModifier,
                constructorName,
                arguments);
            return result;
        }

        private static string ExtractConstructorName(ConstructorInfo constrInfo)
        {
            return constrInfo.DeclaringType.Name;
        }
        private static string ExtractAccessModifier(ConstructorInfo constrInfo)
        {
            if (constrInfo.IsPublic)
                return "public";
            if (constrInfo.IsPrivate)
                return "private";
            if (constrInfo.IsFamily)
                return "protected";
            if (constrInfo.IsAssembly)
                return "internal";
            if (constrInfo.IsFamilyOrAssembly)
                return "protected internal";
            return "";
        }
        private static string ExtractArguments(ConstructorInfo constrInfo)
        {
            var stringBuilder = new StringBuilder("(");
            foreach (var parameter in constrInfo.GetParameters())
            {
                string parameterType;
                if (parameter.ParameterType.IsGenericType)
                {
                    parameterType = ExtractGenericType(parameter.ParameterType);
                }
                else parameterType = parameter.ParameterType.ToString();
                stringBuilder.Append(parameterType).Append(" ").Append(parameter.Name).Append(",");
            }
            if (stringBuilder.Length > 1)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        private static string ExtractGenericType(Type parameter)
        {
            var stringBuilder = new StringBuilder(Regex.Replace(parameter.Name, "`[0-9]+$", ""));
            stringBuilder.Append("<");
            if (parameter.IsGenericType)
            {
                stringBuilder.Append(ExtractGenericArgumentType(parameter.GenericTypeArguments));
            }
            stringBuilder.Append(">");
            return stringBuilder.ToString();
        }


        private static string ExtractGenericArgumentType(IEnumerable<Type> arguments)
        {
            var stringBuilder = new StringBuilder();
            foreach (var argument in arguments)
            {
                if (argument.IsGenericType)
                {
                    stringBuilder.Append(ExtractGenericType(argument));
                }
                else stringBuilder.Append(argument);

                stringBuilder.Append(", ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }
    }
}