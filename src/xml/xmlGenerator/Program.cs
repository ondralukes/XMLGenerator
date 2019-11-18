using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Item> items = LoadDataFromCSV("data.csv");

            XmlDocument doc = new XmlDocument();

            XmlElement rootNode = doc.CreateElement("FlowBasedContingency");
            rootNode.SetAttribute("xmnls", "flowbased");
            rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootNode.SetAttribute("xsi:schemaLocation","flowbasedcontingency-1.xsd");
            doc.AppendChild(rootNode);

            XmlElement outages = doc.CreateElement("outages");
            rootNode.AppendChild(outages);
            foreach (var item in items)
            {
                XmlElement outage = doc.CreateElement("outage");
                XmlElement branch = doc.CreateElement("branch");
                branch.SetAttribute("eic", item.TsoOrigin);
                branch.SetAttribute("elementName", item.ElementName);
                branch.SetAttribute("from", item.From);
                branch.SetAttribute("to", item.To);
                
                
                outage.AppendChild(branch);
                outages.AppendChild(outage);
            }
            StreamWriter outputStream = new StreamWriter("out.xml");
            doc.Save(outputStream);
            Console.ReadLine();
        }
        static List<Item> LoadDataFromCSV(string filename)
        {
            StreamReader streamReader = new StreamReader(filename);

            //Skip header
            streamReader.ReadLine();
            List<Item> items = new List<Item>();
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                string[] strValues = line.Split(';');
                Item item = new Item();
                item.From = strValues[0];
                item.To = strValues[1];
                item.ElementName = strValues[2];
                item.TsoOrigin = strValues[3];
                Console.WriteLine($"From {item.From} To {item.To} ElementName {item.ElementName} TsoOrigin {item.TsoOrigin}");
                items.Add(item);
            }
            return items;
        }
        class Item
        {
            public string From;
            public string To;
            public string ElementName;
            public string TsoOrigin;

        }
    }
}
