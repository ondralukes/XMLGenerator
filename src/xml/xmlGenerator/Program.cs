using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlGenerator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            InputPrompt prompt;
            while (true)
            {
                Console.Write("Getting user input...");
                prompt = new InputPrompt();
                prompt.ShowDialog();
                if (prompt.inputOK)
                {
                    Console.WriteLine("OK");
                    break;
                } else
                {
                    Console.WriteLine("Failed");
                }
            }

            string cTimeInterval = prompt.constraintTimeInterval;
            string sIdentification = prompt.senderIndentification;
            string rIdentification = prompt.receiverIndentification;

            List<Item> items = LoadDataFromCSV("data.csv");

            XmlDocument doc = new XmlDocument();

            XmlElement rootNode = doc.CreateElement("FlowBasedContingency");
            rootNode.SetAttribute("xmnls", "flowbased");
            rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootNode.SetAttribute("xsi:schemaLocation","flowbasedcontingency-1.xsd");
            doc.AppendChild(rootNode);

            XmlElement sIdentElement = doc.CreateElement("ServerIdentification");
            sIdentElement.SetAttribute("codingScheme", "A01");
            sIdentElement.SetAttribute("v", sIdentification);
            rootNode.AppendChild(sIdentElement);

            XmlElement rIdentElement = doc.CreateElement("ReceiverIdentification");
            rIdentElement.SetAttribute("codingScheme", "A01");
            rIdentElement.SetAttribute("v", rIdentification);
            rootNode.AppendChild(rIdentElement);

            XmlElement cTimeIntElement = doc.CreateElement("ConstraintTimeInterval");
            cTimeIntElement.SetAttribute("v", cTimeInterval);
            rootNode.AppendChild(cTimeIntElement);


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
            Console.Write("Saving...");
            StreamWriter outputStream = new StreamWriter("out.xml");
            doc.Save(outputStream);
            Console.WriteLine($" Size: {new FileInfo("out.xml").Length} bytes");
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
               
                items.Add(item);
            }
            Console.WriteLine($"Loaded {items.Count} items");
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
