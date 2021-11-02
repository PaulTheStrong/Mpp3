using System.Collections.Generic;

namespace Library
{
    
    public class ClassInfo
    {
        public string ClassSignature { get; }
        public List<MemberInformation> Members { get; }

        public ClassInfo(string classSignature, List<MemberInformation> members)
        {
            this.ClassSignature = ClassSignature;
            this.Members = members;
        }

        public ClassInfo(string classSignature)
        {
            this.ClassSignature = classSignature;
            this.Members = new List<MemberInformation>();
        }
    }
}