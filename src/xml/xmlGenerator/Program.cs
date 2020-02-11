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

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            if (!Directory.Exists("output")) Directory.CreateDirectory("output");

            StringWriter testOutput = new StringWriter();
            XMLGenerator generator = new XMLGenerator(Console.Out,"output");

            //Prompt user for data
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

            Settings s = new Settings();

            s.constraintTimeInterval = prompt.constraintTimeInterval;
            s.senderIdentification = prompt.senderIdentification;
            s.receiverIdentification = prompt.receiverIdentification;
            s.outagesPerBranch = prompt.outagesPerBranch;

            generator.SetSettings(s);

            bool loaded = true;
            loaded = loaded && generator.LoadOutagesFromCSV("outages.csv");
            loaded = loaded && generator.LoadCriticalBranchesFromCSV("criticalBranches.csv");
            if (!loaded)
            {
                Console.ReadLine();
                return;
            }

            generator.overwritePrompt = new XMLGenerator.OverwritePromptDelegate(OverwritePrompt);
            generator.validationResultPrompt = new XMLGenerator.ValidationResultPromptDelegate(ValidationResultPrompt);

            bool success = generator.Generate();

            if (success)
            {
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
        }
        static OverwritePromptResult OverwritePrompt(string filename)
        {
            OverwriteDialog dialog;
            while (true)
            {
                dialog = new OverwriteDialog(filename);
                dialog.ShowDialog();
                if (dialog.inputOk) break;
            }
            OverwritePromptResult res = new OverwritePromptResult();
            res.overwrite = dialog.overwrite;
            res.forAll = dialog.forAll;
            return res;
        }
        static ValidationResultPromptResult ValidationResultPrompt(List<SuperXmlSchemaException> xmlExceptions, bool allowContinue)
        {
            ValidationResultForm dialog;
            dialog = new ValidationResultForm(xmlExceptions, allowContinue);
            dialog.ShowDialog();

            ValidationResultPromptResult res = new ValidationResultPromptResult();
            res.stop = dialog.stop;
            res.ignoreWarnings = dialog.ignoreWarnings;
            return res;
        }
    }
}
