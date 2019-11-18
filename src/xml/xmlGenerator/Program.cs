using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;
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
            List<string> tsoOrigins = new List<string>();
            foreach (var item in items)
            {
                if (!tsoOrigins.Contains(item.TsoOrigin)) tsoOrigins.Add(item.TsoOrigin);
            }
            /////////////////////////////////////////
            /////// ContingencyDictionary.xml ///////
            /////////////////////////////////////////
            #region ContingencyDictionary.xml
            Console.WriteLine("Generating ContingencyDictionary.xml...");
            XmlDocument doc = new XmlDocument();

            XmlElement rootNode = doc.CreateElement("FlowBasedContingency");
            rootNode.SetAttribute("xmnls", "flowbased");
            rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootNode.SetAttribute("xsi:schemaLocation", "flowbasedcontingency-1.xsd");
            doc.AppendChild(rootNode);

            WriteHeader(rootNode, cTimeInterval, sIdentification, rIdentification);

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
            Console.WriteLine("Saving...");
            if (SaveXML(doc, "ContingencyDictionary.xml"))
            {
                Console.WriteLine("Press O to open, E to open location or any other key to close");
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.O)
                    {
                        Process.Start("out.xml");
                    }
                    else if (key.Key == ConsoleKey.E)
                    {
                        string argument = "/select, \"" + new FileInfo("ContingencyDictionary.xml").FullName + "\"";
                        Process.Start("explorer.exe", argument);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            #endregion

            //////////////////////////////////////////////
            /////// IndividualCriticalBranches.xml ///////
            //////////////////////////////////////////////
            //#region IndividualCriticalBranches.xml
            //Console.WriteLine("Generating IndividualCriticalBranches.xml...");
            //foreach (var tsoOrigin in tsoOrigins)
            //{
            //    Console.WriteLine($"Generating IndividualCriticalBranches.xml for tsoOrigin {tsoOrigin}...");
            //    doc = new XmlDocument();

            //    rootNode = doc.CreateElement("FlowBasedContingency");
            //    rootNode.SetAttribute("xmnls", "flowbased");
            //    rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //    rootNode.SetAttribute("xsi:schemaLocation", "flowbasedcontingency-1.xsd");

            //    doc.AppendChild(rootNode);

            //    WriteHeader(rootNode, cTimeInterval, sIdentification, rIdentification);

            //    Console.WriteLine("Saving...");
            //    SaveXML(doc, $"IndividualCriticalBranches_{tsoOrigin}.xml");
            //}
//#endregion
            Console.ReadLine();
        }
        static void WriteHeader(XmlElement rootNode,string cTimeInterval, string sIdentification, string rIdentification)
        {
            XmlDocument doc = rootNode.OwnerDocument;
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
        }
        static bool SaveXML(XmlDocument doc, string filename)
        {
            
            if (File.Exists(filename))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{filename} already exists. Overwrite? [y/n]: ");
                Console.ResetColor();
                while (true)
                {
                    int cursorLeftPos = Console.CursorLeft;
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.SetCursorPosition(cursorLeftPos, Console.CursorTop);
                    if(key.Key == ConsoleKey.Y)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Overwriting");
                        Console.ResetColor();
                        break;
                    } else if(key.Key == ConsoleKey.N)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("File not saved");
                        Console.ResetColor();
                        return false; 
                        
                    }
                }
            }
            while (true)
            {
                try
                {
                    StreamWriter outputStream = new StreamWriter(filename);
                    doc.Save(outputStream);
                    outputStream.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Saving {filename} failed ({e.Message})");
                    Console.Write("Retry? [y/n]:");
                    Console.ResetColor();
                    while (true)
                    {
                        int cursorLeftPos = Console.CursorLeft;
                        ConsoleKeyInfo key = Console.ReadKey();
                        Console.SetCursorPosition(cursorLeftPos, Console.CursorTop);
                        if (key.Key == ConsoleKey.Y)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Retrying");
                            Console.ResetColor();
                            break;
                        }
                        else if (key.Key == ConsoleKey.N)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("File not saved");
                            Console.ResetColor();
                            return false;

                        }
                    }
                }
            }
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
