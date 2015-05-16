using ikvm.runtime;
using java.lang;

namespace Grobid.NET
{
    namespace Grobid
    {
        public class GrobidClassLoader : ClassLoader
        {
            public GrobidClassLoader(ClassLoader parent)
                : base(new AppDomainAssemblyClassLoader(typeof(GrobidClassLoader).Assembly))
            {
            }
        }
    }
}
