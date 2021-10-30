using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ConsoleRouting
{

    public class DocumentationBuilder
    {
        Documentation documentation = new();

        public DocumentationBuilder Add(Assembly assembly)
        {
            var xdoc = ReadXmlDocumentation(assembly);
            if (xdoc is not null)
            {
                var items = ReadMembersDocumentation(xdoc);
                foreach(var item in items)
                {
                    this.documentation.Add(item);
                }
            }
            return this;
        }

        public Documentation Build()
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

        public IEnumerable<MemberDoc> ReadMembersDocumentation(XDocument xdoc)
        {
            var membernodes = xdoc.XPathSelectElements("doc/members/member").ToList();

            foreach (var membernode in membernodes)
            {
                var doc = new MemberDoc();

                doc.Key = membernode?.Attribute("name")?.Value;
                doc.Text = DocKeys.ConsiseTrim(membernode?.Element("summary")?.Value);

                var paramnodes = membernode?.Elements("param");
                
                foreach (var paramnode in paramnodes)
                {
                    var name = paramnode.Attribute("name")?.Value;
                    var summary = paramnode?.Value?.Trim();
                    doc.Params.Add(name, summary);
                }
                yield return doc;
            }
        }

    }
}
