using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlGenerator
{
    class XMLGenerator
    {
        Random rnd;

        List<Outage> outages;
        List<CriticalBranches> criticalBranches;
        Settings settings;
        TextWriter outputStream;
        bool overwriteAll = false;
        bool dontOverwriteAll = false;
        bool consoleOnly = false;
        string outputPath = "";

        public delegate OverwritePromptResult OverwritePromptDelegate(string filename);
        public OverwritePromptDelegate overwritePrompt;

        public delegate ValidationResultPromptResult ValidationResultPromptDelegate(List<SuperXmlSchemaException> _xmlExceptions, bool allowContinue);
        public ValidationResultPromptDelegate validationResultPrompt;

        public XMLGenerator(TextWriter outStream, string outPath)
        {
            rnd = new Random();
            outputStream = outStream;
            outputStream.WriteLine("Start");

            //Get absolute output path
            outputPath = Path.Combine(Directory.GetCurrentDirectory(), outPath);

            //Copy schemas
            string[] schemas = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xsd");
            outStream.WriteLine($"Copying {schemas.Length} XSD files to target directory");
            foreach (var schema in schemas)
            {
                try
                {
                    File.Copy(schema, Path.Combine(outputPath, Path.GetFileName(schema)), true);
                } catch (Exception e)
                {
                    outputStream.WriteLine($"Failed to copy {schema} to target directory");
                }
            }
            
        }

        public void SetSettings(Settings s)
        {
            settings = s;
            outputStream.WriteLine("Loaded settings");
        }

        public void DontAsk()
        {
            overwriteAll = true;
            consoleOnly = true;
        }

        public bool Generate()
        {
            List<string> tsoOrigins = new List<string>();

            foreach (var item in criticalBranches)
            {
                if (!tsoOrigins.Contains(item.TsoOrigin)) tsoOrigins.Add(item.TsoOrigin);
            }


            /////////////////////////////////////////
            /////// ContingencyDictionary.xml ///////
            /////////////////////////////////////////
            #region ContingencyDictionary.xml
            outputStream.WriteLine("Generating ContingencyDictionary.xml...");

            XmlDocument doc = new XmlDocument();
            XmlElement rootNode = doc.CreateElement("FlowBasedContingency");
            rootNode.SetAttribute("DtdRelease", "4");
            rootNode.SetAttribute("DtdVersion", "0");
            rootNode.SetAttribute("xmlns", "flowbased");
            rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            XmlAttribute attr = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            attr.Value = "flowbasedcontingency-01.xsd";
            rootNode.SetAttributeNode(attr);
            rootNode.Attributes[rootNode.Attributes.Count - 1].Prefix = "xsi";
            doc.AppendChild(rootNode);

            WriteHeader(rootNode, "ContingencyTimeInterval");

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

            //Save and validate
            outputStream.WriteLine("Saving...");
            if (SaveXML(doc, Path.Combine(outputPath, "ContingencyDictionary.xml")))
            {
                Validator validator = new Validator(Path.Combine(outputPath, "ContingencyDictionary.xml"), Path.Combine(outputPath, "flowbasedcontingency-01.xsd"), outputStream, validationResultPrompt);
                if (!validator.Validate(consoleOnly)) return false;
            }
            outputStream.WriteLine();

            #endregion

            //////////////////////////////////////////////
            /////// IndividualCriticalBranches.xml ///////
            //////////////////////////////////////////////
            #region IndividualCriticalBranches.xml
            //Generate IndividualCriticalBranches.xml for each tsoOrigin
            foreach (var tsoOrigin in tsoOrigins)
            {

                outputStream.WriteLine($"Generating IndividualCriticalBranches.xml for tsoOrigin {tsoOrigin}...");

                doc = new XmlDocument();

                rootNode = doc.CreateElement("FlowBasedConstraintDocument");
                rootNode.SetAttribute("DtdRelease", "4");
                rootNode.SetAttribute("DtdVersion", "0");
                rootNode.SetAttribute("xmlns", "flowbased");
                rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

                attr = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                attr.Value = "flowbasedconstraintdocument-17.xsd";

                rootNode.SetAttributeNode(attr);

                doc.AppendChild(rootNode);

                WriteHeader(rootNode, "ConstraintTimeInterval");

                XmlElement criticalBranchesXml = doc.CreateElement("criticalBranches");
                rootNode.AppendChild(criticalBranchesXml);

                foreach (var criticalBranch in criticalBranches)
                {
                    int outagesCount = rnd.Next(settings.outagesPerBranch) + 1;

                    //Use only criticalBranches with same tsoOrigin
                    if (criticalBranch.TsoOrigin != tsoOrigin) continue;

                    for (int i = 0; i < outagesCount; i++)
                    {
                        Outage outage = null;
                        //Find i-th possible outage
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
                            outputStream.WriteLine("No possible outage!");
                            return false;
                        }

                        XmlElement criticalBranchElement = doc.CreateElement("criticalBranch");
                        criticalBranchElement.SetAttribute("id", "testId");

                        XmlElement timeIntervalXml = doc.CreateElement("timeInterval");
                        timeIntervalXml.SetAttribute("v", settings.constraintTimeInterval);
                        criticalBranchElement.AppendChild(timeIntervalXml);

                        XmlElement branchElement = doc.CreateElement("branch");
                        branchElement.SetAttribute("from", criticalBranch.From);
                        branchElement.SetAttribute("to", criticalBranch.To);
                        branchElement.SetAttribute("order", criticalBranch.Order);
                        branchElement.SetAttribute("name", "testName");
                        branchElement.SetAttribute("eic", "testEic");

                        criticalBranchElement.AppendChild(branchElement);

                        //Append custom values
                        foreach (var item in criticalBranch.details)
                        {
                            XmlElement detailXml = doc.CreateElement(item.Key);
                            detailXml.InnerText = item.Value;
                            criticalBranchElement.AppendChild(detailXml);
                        }

                        XmlElement tsoOriginElement = doc.CreateElement("tsoOrigin");
                        tsoOriginElement.InnerText = tsoOrigin;
                        criticalBranchElement.AppendChild(tsoOriginElement);

                        XmlElement outageElement = doc.CreateElement("outage");
                        outageElement.SetAttribute("id", outage.id);
                        criticalBranchElement.AppendChild(outageElement);

                        criticalBranchesXml.AppendChild(criticalBranchElement);
                    }
                }
                outputStream.WriteLine("Saving...");

                //Save and validate
                if (SaveXML(doc, Path.Combine(outputPath,$"IndividualCriticalBranches_{tsoOrigin}.xml")))
                {
                    Validator validator = new Validator(Path.Combine(outputPath, $"IndividualCriticalBranches_{tsoOrigin}.xml"), Path.Combine(outputPath, "flowbasedconstraintdocument-17.xsd"), outputStream, validationResultPrompt);
                    if (!validator.Validate(consoleOnly)) return false;
                }
                outputStream.WriteLine();
            }
            #endregion
            return true;
        }

        void WriteHeader(XmlElement rootNode, string cTimeIntervalName)
        {
            XmlDocument doc = rootNode.OwnerDocument;

            XmlElement dIdentElement = doc.CreateElement("DocumentIdentification");
            dIdentElement.SetAttribute("v", "1");
            rootNode.AppendChild(dIdentElement);

            XmlElement dVerElement = doc.CreateElement("DocumentVersion");
            dVerElement.SetAttribute("v", "1");
            rootNode.AppendChild(dVerElement);

            XmlElement dTypeElement = doc.CreateElement("DocumentType");
            dTypeElement.SetAttribute("v", "B06");
            rootNode.AppendChild(dTypeElement);

            XmlElement pTypeElement = doc.CreateElement("ProcessType");
            pTypeElement.SetAttribute("v", "A06");
            rootNode.AppendChild(pTypeElement);

            XmlElement sIdentElement = doc.CreateElement("SenderIdentification");
            sIdentElement.SetAttribute("codingScheme", "A01");
            sIdentElement.SetAttribute("v", settings.senderIdentification);
            rootNode.AppendChild(sIdentElement);

            XmlElement sRoleElement = doc.CreateElement("SenderRole");
            sRoleElement.SetAttribute("v", "A36");
            rootNode.AppendChild(sRoleElement);

            XmlElement rIdentElement = doc.CreateElement("ReceiverIdentification");
            rIdentElement.SetAttribute("codingScheme", "A01");
            rIdentElement.SetAttribute("v", settings.receiverIdentification);
            rootNode.AppendChild(rIdentElement);

            XmlElement rRoleElement = doc.CreateElement("ReceiverRole");
            rRoleElement.SetAttribute("v", "A36");
            rootNode.AppendChild(rRoleElement);

            XmlElement cDateElement = doc.CreateElement("CreationDateTime");
            cDateElement.SetAttribute("v", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            rootNode.AppendChild(cDateElement);

            XmlElement cTimeIntElement = doc.CreateElement(cTimeIntervalName);
            cTimeIntElement.SetAttribute("v", settings.constraintTimeInterval);
            rootNode.AppendChild(cTimeIntElement);

            XmlElement domainElement = doc.CreateElement("Domain");
            domainElement.SetAttribute("v", "10YDOM-REGION-1V");
            domainElement.SetAttribute("codingScheme", "A01");
            rootNode.AppendChild(domainElement);
        }

        bool SaveXML(XmlDocument doc, string filename)
        {

            if (File.Exists(filename))
            {

                if (!overwriteAll && !dontOverwriteAll)
                {
                    outputStream.Write($"{filename} already exists. Overwrite? : ");

                    while (true)
                    {
                        OverwritePromptResult result = new OverwritePromptResult();
                        if (overwritePrompt != null)
                        {
                            result = overwritePrompt(filename);
                        }
                        
                        if (result.overwrite)
                        {
                            if (result.forAll) overwriteAll = true;
                            outputStream.WriteLine("Overwriting");
                            break;
                        }
                        else if (!result.overwrite)
                        {
                            if (result.forAll) dontOverwriteAll = true;
                            outputStream.WriteLine("File not saved");
                            return false;

                        }
                    }
                }
                else
                {
                    if (overwriteAll)
                    {
                        outputStream.Write($"{filename} already exists. ");
                        outputStream.WriteLine("Overwriting.");
                    }
                    else if (dontOverwriteAll)
                    {
                        outputStream.Write($"{filename} already exists. ");
                        outputStream.WriteLine("Not saved.");
                        return false;
                    }
                }
            }

            while (true)
            {
                try
                {
                    StreamWriter outStream = new StreamWriter(filename);
                    doc.Save(outStream);
                    outStream.Close();

                    outputStream.WriteLine("Saved.");
                    return true;
                }
                catch (Exception e)
                {
                    outputStream.WriteLine($"Saving {filename} failed ({e.Message})");
                    if (consoleOnly) return false;
                    outputStream.Write("Retry? [y/n]:");
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

        public bool LoadOutagesFromCSV(string filename)
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader(filename);

                //Skip header
                streamReader.ReadLine();
                List<Outage> items = new List<Outage>();
                while (!streamReader.EndOfStream)
                {
                    //Read and split line
                    string line = streamReader.ReadLine();
                    string[] strValues = line.Split(';');

                    //Create Outlet object
                    Outage item = new Outage();

                    item.From = strValues[0];
                    item.To = strValues[1];
                    item.ElementName = strValues[2];
                    item.TsoOrigin = strValues[3];
                    item.id = $"OU-{rnd.Next(100)}-{rnd.Next(10000)}";

                    items.Add(item);
                }
                outputStream.WriteLine($"Loaded {items.Count} outages");
                outages = items;
            } catch (Exception e)
            {
                outputStream.WriteLine($"Failed to load outages: {e.Message}");
                if (streamReader != null) streamReader.Close();
                return false;
            }
            if (streamReader != null) streamReader.Close();
            return true;
        }

        public bool LoadCriticalBranchesFromCSV(string filename)
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader(filename);

                //Read column headers
                string[] keys = streamReader.ReadLine().Split(';');
                List<CriticalBranches> items = new List<CriticalBranches>();

                while (!streamReader.EndOfStream)
                {
                    //Read line
                    string line = streamReader.ReadLine();
                    string[] strValues = line.Split(';');

                    //Put data to object
                    CriticalBranches item = new CriticalBranches();
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (strValues.Length <= i) break;
                        switch (keys[i])
                        {
                            case "from":
                                item.From = strValues[i];
                                break;
                            case "to":
                                item.To = strValues[i];
                                break;
                            case "order":
                                item.Order = strValues[i];
                                break;
                            case "tsoOrigin":
                                item.TsoOrigin = strValues[i];
                                break;
                            default:
                                if (keys[i] != "")
                                {
                                    item.details.Add(new KeyValuePair<string, string>(keys[i], strValues[i]));
                                }

                                break;
                        }
                    }

                    items.Add(item);
                }
                outputStream.WriteLine($"Loaded {items.Count} critical branches");
                criticalBranches = items;
            }
            catch (Exception e)
            {
                outputStream.WriteLine($"Failed to load critical branches: {e.Message}");
                if(streamReader != null) streamReader.Close();
                return false;
            }
            if (streamReader != null) streamReader.Close();
            return true;
        }
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
        public List<KeyValuePair<string, string>> details = new List<KeyValuePair<string, string>>();
    }
    public class Settings
    {
        public string constraintTimeInterval { get; set; }
        public string senderIdentification { get; set; }
        public string receiverIdentification { get; set; }
        public int outagesPerBranch { get; set; }
    }
    class OverwritePromptResult
    {
        public bool overwrite = true;
        public bool forAll = true;
    }
    class ValidationResultPromptResult
    {
        public bool stop = false;
        public bool ignoreWarnings = true;
    }
}
