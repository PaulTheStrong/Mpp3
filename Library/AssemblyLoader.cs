using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Library.SignatureCreator;

namespace Library
{
    public class AssemblyLoader
    {
        public List<Namespace> GetAssemblyInfo(string path)
        {
            var assembly = Assembly.LoadFrom(path);
            var namespaces = new Dictionary<string, Namespace>();
            
            foreach (var type in assembly.GetTypes())
            {
                var namespaceName = type.Namespace;
                var namespaceInfo = new Namespace(namespaceName);
                if (!namespaces.ContainsKey(namespaceName))
                {
                    namespaces.Add(namespaceName, namespaceInfo);
                }

                var namespaceClassInfo = CreateNamespaceClassInfo(type);
                var namespaceClasses = namespaces[namespaceName].NamespaceClasses;
                namespaceClasses.Add(namespaceClassInfo);
                // if (type.IsDefined(typeof(ExtensionAttribute), false))
                // {
                //     assemblyInfo = GetExtensionNamespaces(type, assemblyInfo);
                // }
            }

            return namespaces.Values.ToList();
        }

        // private Dictionary<string, DataContainer> GetExtensionNamespaces(Type type, Dictionary<string, DataContainer> assemblyInfo)
        // {
        //     foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        //     {
        //         if (!type.IsDefined(typeof(ExtensionAttribute), false) ||
        //             !method.IsDefined(typeof(ExtensionAttribute), false))
        //         {
        //             continue;
        //         }
        //         var firstParameterType = method.GetParameters()[0].ParameterType;
        //         var typeNamespaceName = firstParameterType.Namespace;
        //         var firstParameterTypeSignature = ClassSignatureExtractor.CreateClassSignature(firstParameterType);
        //         var dataContainer = new DataContainer(typeNamespaceName, firstParameterTypeSignature);
        //         if (!assemblyInfo.ContainsKey(typeNamespaceName))
        //         {
        //             assemblyInfo.Add(typeNamespaceName, dataContainer);
        //         }
        //         DataContainer @class = new DataContainer(firstParameterTypeSignature, firstParameterTypeSignature);
        //         string methodSignature = MethodSignatureExtractor.CreateMethodSignature(method) + " - extension method";
        //         var classSignature = ClassSignatureExtractor.CreateClassSignature(type);
        //         @class.Members.Add(new MemberInformation(methodSignature, classSignature));
        //         dataContainer.Members.Add(@class);
        //     } 
        //     return assemblyInfo;
        // }

        private ClassInfo CreateNamespaceClassInfo(Type type)
        {
            string classSignature = ClassSignatureExtractor.CreateClassSignature(type);
            var classInfo = new ClassInfo(classSignature);
            var fields = GetTypeFields(type);
            var properties = GetProperties(type);
            var methods = GetMethods(type);
            foreach (var nestedType in type.GetNestedTypes())
            {
                Console.WriteLine(nestedType);
            }
            classInfo.Members.AddRange(fields);
            classInfo.Members.AddRange(properties);
            classInfo.Members.AddRange(methods);
            return classInfo;
        }

        private List<MemberInformation> GetMethods(Type type)
        {
            var allMethods = new List<MemberInformation>();
            var constructors = GetConstructors(type);
            allMethods.AddRange(constructors);
            var typeMethods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (var method in typeMethods)
            {
                if (type.IsDefined(typeof(ExtensionAttribute), false) &&
                    method.IsDefined(typeof(ExtensionAttribute), false))
                {
                    continue;
                }
                var methodSignature = MethodSignatureExtractor.CreateMethodSignature(method);
                allMethods.Add(new MemberInformation(methodSignature));
            }
            return allMethods;
        }

        private List<MemberInformation> GetTypeFields(Type type)
        {
            return type
                .GetFields()
                .Select(field => new MemberInformation(
                    FieldSignatureExtractor.CreateTypeSignature(field)
                ))
                .ToList();
        }

        private List<MemberInformation> GetProperties(Type type)
        {
            return type
                .GetProperties()
                .Select(property => new MemberInformation(
                    PropertySignatureExtractor.CreatePropertySignature(property))
                )
                .ToList();
        }
        
        private List<MemberInformation> GetConstructors(Type type)
        {
            return type
                .GetConstructors()
                .Select(constructor => new MemberInformation(
                    ConstructorSignatureExtractor.CreateConstructorSignature(constructor))
                    )
                .ToList();
        }

        public static void Main(string[] args)
        {
            var assembly = Assembly.Load("E:/Univer/5sem/spp/Assembly Browser/Assembly Browser/AssEmbly/bin/Debug/net5.0/AssEmbly.dll");
            foreach (Type oType in assembly.GetTypes()) {
                Console.WriteLine(oType.Name);
            }
        }
    }
}