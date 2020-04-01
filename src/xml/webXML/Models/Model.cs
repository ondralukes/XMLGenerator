using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xmlGenerator;


namespace webXML.Models
{
    public class Model
    {
        public Settings Settings { get; set; }
        public string Output { get; set; }
        public string OutagesCSV { get; set; }
        public string CriticalBranchesCSV { get; set; }
        public string ResultSummary { get; set; }
        public bool IncludeXSD { get; set; }
        private string _OutputFile;
        public string OutputFile {
            get {
                if (!File.Exists(_OutputFile))  return null;
                return _OutputFile;
            }
            set
            {
                _OutputFile = value;
            }
        }
        private Random rnd;
        public Model()
        {
            Settings = new Settings();
            rnd = new Random();
        }

        public void Generate()
        {
            
            StringWriter outStream = new StringWriter();
            string tempDirectory = $"temp/{rnd.Next()}-{rnd.Next()}";
            Directory.CreateDirectory(tempDirectory);
            XMLGenerator generator = new XMLGenerator(outStream,tempDirectory);

            bool loaded = true;
            loaded = loaded && generator.LoadOutagesFromCSV(OutagesCSV);
            loaded = loaded && generator.LoadCriticalBranchesFromCSV(CriticalBranchesCSV);

            if (loaded)
            {
                generator.DontAsk();
                generator.SetSettings(Settings);
                if (generator.Generate())
                {
                    if (!IncludeXSD)
                    {
                        //Remove XSD files
                        string[] xsdFiles = Directory.GetFiles(tempDirectory, "*.xsd");
                        foreach (var file in xsdFiles)
                        {
                            File.Delete(file);
                        }
                    }
                    OutputFile = $"{tempDirectory}.zip";
                    ZipFile.CreateFromDirectory(tempDirectory, _OutputFile);
                }
            }
            Directory.Delete(tempDirectory, true);
            ResultSummary = generator.Summary;
            if (File.Exists(OutagesCSV)) File.Delete(OutagesCSV);
            if (File.Exists(CriticalBranchesCSV)) File.Delete(CriticalBranchesCSV);

            Output = outStream.ToString();
    }
    }
}
