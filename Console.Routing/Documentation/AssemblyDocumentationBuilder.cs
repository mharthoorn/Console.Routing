using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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
            var key = GetKey(method);
            return TryGetValue(key, out var doc) ? doc : null;
        }
        public static string GetKey(MethodInfo method)
        {
            var builder = new StringBuilder();
            builder.Append("M:");
            builder.Append(method.DeclaringType.FullName);
            builder.Append(".");
            builder.Append(method.Name);
            builder.Append("(");
            bool first = true;
            foreach (var par in method.GetParameters())
            {
                if (!first) builder.Append(",");
                builder.Append(par.ParameterType.FullName);
            }
            builder.Append(")");

            return builder.ToString();
        }

    }

    public class MethodDoc
    {
        public string Key;
        public string Summary;
     
        public Dictionary<string, string> Params = new();
    }

   

    public class AssemblyDocumentationBuilder
    {
        XmlDocumentation documentation = new();

        public AssemblyDocumentationBuilder Add(Assembly assembly)
        {
            var xdoc = ReadXmlDocumentation(assembly);
            if (xdoc is not null)
            {
                var items = ReadMethods(xdoc);
                foreach(var item in items) 
                    documentation.Add(item);
            }
            return this;
        }

        public XmlDocumentation Build()
        {
            return documentation;
        }

        private static string GetDirectoryPath(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static XDocument ReadXmlDocumentation(Assembly assembly)
        {
            string directoryPath = GetDirectoryPath(assembly);
            string xmlFilePath = Path.Combine(directoryPath, assembly.GetName().Name + ".xml");
            if (File.Exists(xmlFilePath))
            {
                return ReadXmlDocumentation(File.ReadAllText(xmlFilePath));
            }
            else return null;
        }

        public static XDocument ReadXmlDocumentation(string content)
        { 
            using var reader = new StringReader(content);
            var document = XDocument.Load(reader);
            return document;
        }

        public string GetKey(MethodInfo method)
        {
            var builder = new StringBuilder();
            builder.Append("M:");
            builder.Append(method.DeclaringType.FullName);
            builder.Append(".");
            builder.Append(method.Name);
            builder.Append("(");
            bool first = true;
            foreach (var par in method.GetParameters())
            {
                if (!first) builder.Append(",");
                builder.Append(par.ParameterType.FullName);
            }
            builder.Append(")");

            return builder.ToString();
        }
     

        public IEnumerable<MethodDoc> ReadMethods(XDocument xdoc)
        {
            //var methods = new List<MethodDoc>();

            //var docnode = xdoc.Element("doc");
            var mnodes = xdoc.XPathSelectElements("doc/members/member").ToList().ToList().ToList();
            //var memnode = docnode.Element("members");
            //var mnodes = memnode.Elements("member");

            foreach (var mnode in mnodes)
            {
                var method = new MethodDoc();

                method.Key = mnode?.Attribute("name")?.Value;
                method.Summary = mnode?.Element("summary")?.Value;

                var pnodes = mnode?.Elements("param");
                
                foreach (var pnode in pnodes)
                {
                    var name = pnode.Attribute("name")?.Value;
                    var summary = pnode?.Value;
                    method.Params.Add(name, summary);
                }
                yield return method;
            }
            //return methods;
        }

        //public MethodDoc GetMethodDocumentation(MethodInfo method)
        //{
        //    var mnode = GetMethodNode(method);
        //    if (mnode is null) return null;

        //    var doc = new MethodDoc();
        //    doc.Summary = mnode?.Element("summary").Value;

        //    var pnodes = mnode.Elements("param");
        //    foreach (var param in method.GetParameters())
        //    {
        //        var pnode = pnodes.Where(e => e.Attribute("name")?.Value == param.Name).FirstOrDefault();
        //        if (pnode is not null)
        //        doc.Params[param.Name] = pnode?.Value;
        //    }

        //    return doc;
        //}

    }

    public static class AssemblyDocumentationBuilderExtensions
    {
        public static AssemblyDocumentationBuilder Add(this AssemblyDocumentationBuilder builder, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies) builder.Add(assembly);
            return builder;
        }
    }
}
