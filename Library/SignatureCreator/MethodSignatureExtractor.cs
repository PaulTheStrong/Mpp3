using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.SignatureCreator
{
    public class MethodSignatureExtractor
    {
        public static string CreateMethodSignature(MethodInfo methodInfo)
        {
            string methodAccessorModifiers = GetTypeAccessorModifiers(methodInfo);
            string typeModifiers = GetTypeModifiers(methodInfo);
            string methodReturnTypeName = ExtractReturnTypeName(methodInfo);
            string methodArguments = ExtractMethodArguments(methodInfo);
            string methodName = methodInfo.Name;
            string result = string.Join(" ", 
                methodAccessorModifiers,
                typeModifiers,
                methodReturnTypeName, 
                methodName, 
                methodArguments);
            return result;
        }

        private static string GetTypeAccessorModifiers(MethodInfo methodInfo)
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

        private static string GetTypeModifiers(MethodInfo methodInfo)
        {
            if (methodInfo.IsAbstract)
                return "abstract";
            if (methodInfo.IsStatic)
                return "static";
            if (methodInfo.IsVirtual)
                return "virtual";
            if (methodInfo.GetBaseDefinition() != methodInfo)
                return "override";
            return "";
        }

        private static string ExtractReturnTypeName(MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType.IsGenericType)
            {
                return ExtractGenericReturnTypeName(methodInfo.ReturnType);
            }
            return methodInfo.ReturnType.Name;
        }

        private static string ExtractMethodArguments(MethodBase methodInfo)
        {
            var stringBuilder = new StringBuilder("(");
            foreach (var parameter in methodInfo.GetParameters())
            {
                string parameterType;
                if (parameter.ParameterType.IsGenericType)
                {
                    parameterType = ExtractGenericReturnTypeName(parameter.ParameterType);
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

        private static string ExtractGenericReturnTypeName(Type parameter)
        {
            var stringBuilder = new StringBuilder(Regex.Replace(parameter.Name, "`[0-9]+$", ""));
            stringBuilder.Append("<");
            if (parameter.IsGenericType)
            {
                stringBuilder.Append(GetGenericArgumentsType(parameter.GenericTypeArguments));
            }
            stringBuilder.Append(">");
            return stringBuilder.ToString();
        }


        private static string GetGenericArgumentsType(IEnumerable<Type> arguments)
        {
            var stringBuilder = new StringBuilder();
            foreach (var argument in arguments)
            {
                if (argument.IsGenericType)
                {
                    stringBuilder.Append(ExtractGenericReturnTypeName(argument));
                }
                else stringBuilder.Append(argument);
                stringBuilder.Append(", ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }
    }
}