using System.Collections.Generic;

namespace Library
{
    public class Namespace
    {
        public string Name { get; private set; }
        public List<ClassInfo> NamespaceClasses { get; private set; }

        public Namespace(string name, List<ClassInfo> namespaceClasses)
        {
            Name = name;
            NamespaceClasses = namespaceClasses;
        }
        
        public Namespace(string name)
        {
            this.Name = name;
            NamespaceClasses = new List<ClassInfo>();
        }
    }
}