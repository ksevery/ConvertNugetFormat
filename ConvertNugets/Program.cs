using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ConvertNugets
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = ConvertFromPackageConfig("/Users/ksevery/VSTS/kaufland.vkl-app-xamarin/Vapp/Vapp.UnitTests/packages.config");

            Console.WriteLine(result);
        }

        static XDocument ConvertFromPackageConfig(string path)
        {
            var configXml = File.ReadAllText(path);

            var model = new
            {
                dependencies = new List<KeyValuePair<string, string>>()
            };

            var xml = XDocument.Load(path);

            var result = new XDocument();
            var rootNode = new XElement("root");
            result.Add(rootNode);
            foreach (var dependency in xml.Element("packages").Elements("package"))
            {
                var newNode = new XElement("PackageReference");
                newNode.SetAttributeValue("Include", dependency.Attribute("id").Value);
                newNode.SetAttributeValue("Version", dependency.Attribute("version").Value);
                rootNode.Add(newNode);
            }

            return result;
        }

        static XDocument ConvertFromProjectJson(string path)
        {
            var configJson = File.ReadAllText(path);

            var model = new
            {
                dependencies = new Dictionary<string, string>()
            };

            var json = JsonConvert.DeserializeAnonymousType(configJson, model);

            var result = new XDocument();
            var rootNode = new XElement("root");
            result.Add(rootNode);
            foreach (var dependency in json.dependencies)
            {
                var newNode = new XElement("PackageReference");
                newNode.SetAttributeValue("Include", dependency.Key);
                newNode.SetAttributeValue("Version", dependency.Value);
                rootNode.Add(newNode);
            }

            return result;
        }
    }
}
