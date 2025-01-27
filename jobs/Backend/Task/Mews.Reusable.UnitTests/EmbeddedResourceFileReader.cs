using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mews.Reusable.UnitTests
{
    public static class EmbeddedResourceFileReader
    {
        public static string ReadFileContent(Assembly assembly, string fileName)
        {
            string foundResourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));
            using Stream? stream = assembly.GetManifestResourceStream(foundResourceName);
            if (stream is null)
            {
                throw new FileNotFoundException("Resource file Not Found!", fileName);
            }
            using var reader = new StreamReader(stream!);
            return reader.ReadToEnd();
        }
    }
}
