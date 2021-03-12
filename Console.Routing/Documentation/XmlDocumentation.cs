using System.Collections.Generic;
using System.Reflection;

namespace ConsoleRouting
{
    public class XmlDocumentation : Dictionary<string, MethodDoc>
    {
        public void Add(MethodDoc doc)
        {
            this.Add(doc.Key, doc);
        }

        public MethodDoc this[MethodInfo method] => Get(method);

        public MethodDoc Get(MethodInfo method)
        {
            var key = DocumentationHelper.GetKey(method);
            return TryGetValue(key, out var doc) ? doc : null;
        }

     

    }
}
