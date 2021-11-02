namespace AssEmbly
{
    public class SomeClass
    {
        private class InnerClass
        {
            private int i;
            public float k { get; private set; }
        }
        
        static private class  StaticInnerClass
        {
            private static int i = 0;
            public static float k { get; private set; }
        }

        public int A { get; set; }

        public double B { get; private set; }

        public SomeClass(int a, int b)
        {
            this.A = a;
            this.B = b;
        }

        public int aPlusB()
        {
            return (int) (A + B);
        }

        public int aaaa(int x, int y, int z)
        {
            return x * y * z;
        }

        public bool equ(int asd, double qwe)
        {
            return (double) asd == qwe;
        }
    }
}