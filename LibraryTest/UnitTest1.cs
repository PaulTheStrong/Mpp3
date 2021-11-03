using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Library;

namespace LibraryTest
{
    public class Tests
    {
        private AssemblyLoader _assemblyLoader;

        private List<Namespace> _namespaces;
        private Assembly _lib;

        [SetUp]
        public void Init()
        {
            _assemblyLoader = new AssemblyLoader();
            _namespaces = _assemblyLoader.GetAssemblyInfo("E:/Univer/5sem/spp/Assembly Browser/Assembly Browser/AssEmbly/bin/Debug/net5.0/AssEmbly.dll");
        }


        [Test]
        public void TestNamespacesCount()
        {
            var expectedCount = 1;
            Assert.AreEqual(expectedCount, _namespaces.Count);
        }

        [Test]
        public void TestIsNamespacePresentAndContainsAllClasses()
        {
            Namespace nameSpace = _namespaces.Find(ns => ns.Name == "AssEmbly");
            Assert.NotNull(nameSpace);
            int actual = nameSpace.NamespaceClasses.Count;
            int expected = 3;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestClassSignature()
        {
            Namespace nameSpace = _namespaces.Find(ns => ns.Name == "AssEmbly");
            Assert.NotNull(nameSpace);
            ClassInfo classInfo = nameSpace.NamespaceClasses[0];
            Assert.NotNull(classInfo);
            string actual = classInfo.ClassSignature.Trim();
            actual = Regex.Replace(actual, @"\s+", " ");
            string expected = "public class SomeClass";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestClassMemebersCount()
        {
            Namespace nameSpace = _namespaces.Find(ns => ns.Name == "AssEmbly");
            Assert.NotNull(nameSpace);
            ClassInfo classInfo = nameSpace.NamespaceClasses[0];
            Assert.NotNull(classInfo);
            Assert.AreEqual(10, classInfo.Members.Count);
        }

        [Test]
        public void TestClassMemeberSignature()
        {
            Namespace nameSpace = _namespaces.Find(ns => ns.Name == "AssEmbly");
            Assert.NotNull(nameSpace);
            ClassInfo classInfo = nameSpace.NamespaceClasses[0];
            Assert.NotNull(classInfo);
            string expected = "public SomeClass (System.Int32 a,System.Int32 b)";
            MemberInformation actual = classInfo.Members.Find(mi =>
                Regex.Replace(mi.MemberSignature.Trim(), @"\s+", " ")
                    .Equals(expected));
            Assert.NotNull(actual);
        }
    }
}