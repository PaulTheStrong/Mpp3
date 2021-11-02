using System;

namespace Library.SignatureCreator
{
    public class ClassSignatureExtractor
    {
        public static string CreateClassSignature(Type type)
        {
            // string typeNamespace = type.Namespace;
            string typeName = type.Name;
            string typeAccessor = ExtractTypeAccessor(type);
            string typeModifier = ExtractTypeModifier(type);
            string classType = ExtractClassType(type);

            return typeAccessor + " " + typeModifier + " " + classType + " " + typeName;
        }
        private static string ExtractClassType(Type type)
        {
            if (type.IsClass)
                return "class ";
            if (type.IsEnum)
                return "enum ";
            if (type.IsInterface)
                return "interface ";
            if (type.IsGenericType)
                return "generic ";
            if (type.IsValueType && !type.IsPrimitive)
                return "struct ";
            return "";
        }
        private static string ExtractTypeModifier(Type type)
        {
            if (type.IsAbstract && type.IsSealed)
                return "static";
            if (type.IsAbstract)
                return "abstract";
            if (type.IsSealed)
                return "sealed";
            return "";
        }
        private static string ExtractTypeAccessor(Type type)
        {
            if (type.IsNestedPublic || type.IsPublic)
                return "public";
            if (type.IsNestedPrivate)
                return "private";
            if (type.IsNestedFamily)
                return "protected";
            if (type.IsNestedAssembly)
                return "internal";
            if (type.IsNestedFamORAssem)
                return "protected internal";
            if (type.IsNestedFamANDAssem)
                return "private protected ";
            if (type.IsNotPublic)
                return "private";
            return "";
        }
    }
}