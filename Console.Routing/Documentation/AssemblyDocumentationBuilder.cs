using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ConsoleRouting
{

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
                {
                    documentation.Add(item);
                }
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

        public IEnumerable<MethodDoc> ReadMethods(XDocument xdoc)
        {
            var mnodes = xdoc.XPathSelectElements("doc/members/member").ToList();

            foreach (var mnode in mnodes)
            {
                var doc = new MethodDoc();

                doc.Key = mnode?.Attribute("name")?.Value;
                doc.Summary = DocumentationHelper.ConsiseTrim(mnode?.Element("summary")?.Value);

                var pnodes = mnode?.Elements("param");
                
                foreach (var pnode in pnodes)
                {
                    var name = pnode.Attribute("name")?.Value;
                    var summary = pnode?.Value?.Trim();
                    doc.Params.Add(name, summary);
                }
                yield return doc;
            }
        }

    }
}
