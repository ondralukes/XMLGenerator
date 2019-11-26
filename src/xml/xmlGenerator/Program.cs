using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlGenerator
{
    class Program
    {
        static Random rnd;
        static bool overwriteAll = false;
        static bool dontOverwriteAll = false;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            rnd = new Random();
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
                }
                else
                {
                    Console.WriteLine("Failed");
                }
            }

            string cTimeInterval = prompt.constraintTimeInterval;
            string sIdentification = prompt.senderIndentification;
            string rIdentification = prompt.receiverIndentification;
            int outagesPerBranch = prompt.outagesPerBranch;

            List<Outage> outages = LoadOutagesFromCSV("outages.csv");
            List<CriticalBranches> criticalBranches = LoadCriticalBranchesFromCSV("criticalBranches.csv");
            List<string> tsoOrigins = new List<string>();
            foreach (var item in criticalBranches)
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
            rootNode.SetAttribute("xsi:schemaLocation", "flowbasedcontingency-01.xsd");
            doc.AppendChild(rootNode);

            WriteHeader(rootNode, cTimeInterval, sIdentification, rIdentification);

            XmlElement outagesElement = doc.CreateElement("outages");
            rootNode.AppendChild(outagesElement);

            foreach (var item in outages)
            {
                XmlElement outage = doc.CreateElement("outage");
                outage.SetAttribute("id", item.id);
                XmlElement branch = doc.CreateElement("branch");
                branch.SetAttribute("eic", item.TsoOrigin);
                branch.SetAttribute("elementName", item.ElementName);
                branch.SetAttribute("from", item.From);
                branch.SetAttribute("to", item.To);

                outage.AppendChild(branch);
                outagesElement.AppendChild(outage);
            }
            Console.WriteLine("Saving...");
            if(SaveXML(doc, "ContingencyDictionary.xml"))
            {
                Console.WriteLine("Validating...");
                Validator validator = new Validator("ContingencyDictionary.xml");
                if (!validator.Validate()) return;
            }

            #endregion

            //////////////////////////////////////////////
            /////// IndividualCriticalBranches.xml ///////
            //////////////////////////////////////////////
            #region IndividualCriticalBranches.xml
            Console.WriteLine("Generating IndividualCriticalBranches.xml...");
            foreach (var tsoOrigin in tsoOrigins)
            {

                Console.WriteLine($"Generating IndividualCriticalBranches.xml for tsoOrigin {tsoOrigin}...");



                doc = new XmlDocument();

                rootNode = doc.CreateElement("FlowBasedConstraintDocument");
                rootNode.SetAttribute("xmnls", "flowbased");
                rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                rootNode.SetAttribute("xsi:noNamespaceSchemaLocation", "flowbasedconstraintdocument-17.xsd");

                doc.AppendChild(rootNode);

                WriteHeader(rootNode, cTimeInterval, sIdentification, rIdentification);

                XmlElement criticalBranchesXml = doc.CreateElement("criticalBranches");
                rootNode.AppendChild(criticalBranchesXml);


                foreach (var criticalBranch in criticalBranches)
                {
                    int outagesCount = rnd.Next(outagesPerBranch) + 1;
                    if (criticalBranch.TsoOrigin != tsoOrigin) continue;
                    for (int i = 0; i < outagesCount; i++)
                    {
                        Outage outage = null;

                        int skippedOutages = 0;
                        foreach (var item in outages)
                        {
                            if (item.TsoOrigin != criticalBranch.TsoOrigin) continue;
                            if (item.From == criticalBranch.From) continue;
                            if (item.To == criticalBranch.To) continue;
                            if (skippedOutages != i)
                            {
                                skippedOutages++;
                            }
                            else
                            {
                                outage = item;
                                break;
                            }
                        }
                        if (outage == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No possible outage!");
                            Console.WriteLine("Press Enter to exit");
                            Console.ReadLine();
                            return;
                        }
                        XmlElement criticalBranchElement = doc.CreateElement("criticalBranch");

                        XmlElement timeIntervalXml = doc.CreateElement("timeInterval");
                        timeIntervalXml.SetAttribute("v", cTimeInterval);
                        criticalBranchElement.AppendChild(timeIntervalXml);

                        XmlElement branchElement = doc.CreateElement("branch");
                        branchElement.SetAttribute("from", criticalBranch.From);
                        branchElement.SetAttribute("to", criticalBranch.To);
                        branchElement.SetAttribute("order", criticalBranch.Order);
                        criticalBranchElement.AppendChild(branchElement);

                        XmlElement tsoOriginElement = doc.CreateElement("tsoOrigin");
                        tsoOriginElement.InnerText = tsoOrigin;
                        criticalBranchElement.AppendChild(tsoOriginElement);

                        XmlElement outageElement = doc.CreateElement("outage");
                        outageElement.SetAttribute("id", outage.id);
                        criticalBranchElement.AppendChild(outageElement);

                        criticalBranchesXml.AppendChild(criticalBranchElement);
                    }
                }
                Console.WriteLine("Saving...");
                if (SaveXML(doc, $"IndividualCriticalBranches_{tsoOrigin}.xml")){
                    Console.WriteLine("Validating...");
                    Validator validator = new Validator($"IndividualCriticalBranches_{tsoOrigin}.xml");
                    if(!validator.Validate())return;
                }
            }
            #endregion
            Console.WriteLine("Press O to open location or any other key to close");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.O)
                {
                    string argument = Directory.GetCurrentDirectory();
                    Process.Start("explorer.exe", argument);
                }
                else
                {
                    break;
                }
            }

        }
        
        static void WriteHeader(XmlElement rootNode, string cTimeInterval, string sIdentification, string rIdentification)
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

                if (!overwriteAll && !dontOverwriteAll)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{filename} already exists. Overwrite? : ");

                    while (true)
                    {
                        OverwriteDialog overwriteDialog = new OverwriteDialog(filename);
                        overwriteDialog.ShowDialog();
                        Console.ResetColor();
                        if (!overwriteDialog.inputOk) continue;
                        if (overwriteDialog.overwrite)
                        {
                            if (overwriteDialog.forAll) overwriteAll = true;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Overwriting");
                            Console.ResetColor();
                            break;
                        }
                        else if (!overwriteDialog.overwrite)
                        {
                            if (overwriteDialog.forAll) dontOverwriteAll = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("File not saved");
                            Console.ResetColor();
                            return false;

                        }
                    }
                }
                else
                {
                    if (overwriteAll)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"{filename} already exists. ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Overwriting.");
                        Console.ResetColor();
                    }
                    else if (dontOverwriteAll)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"{filename} already exists. ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Not saved.");
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
        static List<Outage> LoadOutagesFromCSV(string filename)
        {
            StreamReader streamReader = new StreamReader(filename);

            //Skip header
            streamReader.ReadLine();
            List<Outage> items = new List<Outage>();
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                string[] strValues = line.Split(';');
                Outage item = new Outage();
                item.From = strValues[0];
                item.To = strValues[1];
                item.ElementName = strValues[2];
                item.TsoOrigin = strValues[3];
                item.id = $"OU-{rnd.Next(100)}-{rnd.Next(10000)}";
                items.Add(item);
            }
            Console.WriteLine($"Loaded {items.Count} outages");
            return items;
        }
        static List<CriticalBranches> LoadCriticalBranchesFromCSV(string filename)
        {
            StreamReader streamReader = new StreamReader(filename);

            //Skip header
            streamReader.ReadLine();
            List<CriticalBranches> items = new List<CriticalBranches>();
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                string[] strValues = line.Split(';');
                CriticalBranches item = new CriticalBranches();
                item.From = strValues[0];
                item.To = strValues[1];
                item.Order = strValues[2];
                item.TsoOrigin = strValues[3];
                items.Add(item);
            }
            Console.WriteLine($"Loaded {items.Count} critical branches");
            return items;
        }
        class Outage
        {
            public string From;
            public string To;
            public string ElementName;
            public string TsoOrigin;
            public string id;

        }
        class CriticalBranches
        {
            public string From;
            public string To;
            public string Order;
            public string TsoOrigin;
        }
    }
}
