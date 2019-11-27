using System;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlGenerator
{
    class Validator
    {
        public enum ValidationResult
        {
            OK, Warning, Error,Failed
        }
        string filename;
        string xsdLocation;
        public Validator(string _filename,string _xsdLocation)
        {
            filename = _filename;
            xsdLocation = _xsdLocation;
        }
        public static bool ignoreWarnings = false;
        private int validationWarnings = 0;
        private int validationErrors = 0;
        private List<SuperXmlSchemaException> xmlExceptions = new List<SuperXmlSchemaException>();
        public bool Validate()
        {
            Validator.ValidationResult validationResult = ValidateCore();
            ValidationResultForm validationResultForm;
            switch (validationResult)
            {
                case Validator.ValidationResult.OK:
                    Console.WriteLine("Validation OK");
                    break;
                case Validator.ValidationResult.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Validation finished with {validationWarnings} warnings. Continue? ");
                    bool stop=false;
                    if (!ignoreWarnings)
                    {
                        validationResultForm = new ValidationResultForm(xmlExceptions, true);
                        validationResultForm.ShowDialog();
                        stop = validationResultForm.stop;
                        ignoreWarnings = validationResultForm.ignoreWarnings;
                    }
                    if (stop)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No.");
                        Console.ResetColor();
                    } else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Yes.");
                        Console.ResetColor();
                    }
                    return !stop;
                    break;
                case Validator.ValidationResult.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Validation finished with errors!");
                    validationResultForm = new ValidationResultForm(xmlExceptions, false);
                    validationResultForm.ShowDialog();
                    return false;
                    break;
                case ValidationResult.Failed:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Validation failed!");
                    Console.WriteLine("Continuing without validation");
                    Console.ResetColor();
                    return true;
                    break;
            }
            return true;
        }
        public ValidationResult ValidateCore()
        {
            Console.WriteLine($"[Validator] Validating {filename}");
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.AllowXmlAttributes;
            readerSettings.ValidationEventHandler += new ValidationEventHandler(onValidation);
            readerSettings.ValidationType = ValidationType.Schema;
            XmlReader xmlReader;
            try
            {
                readerSettings.Schemas.Add("flowbased", xsdLocation);
            
            
                xmlReader = XmlReader.Create("ContingencyDictionary.xml", readerSettings);
            } catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Validator] Validation failed: {e.Message}");
                Console.ResetColor();
                return ValidationResult.Failed;
            }
            
            while (xmlReader.Read()) ;
            xmlReader.Close();
            if (validationWarnings == 0 && validationErrors == 0) return ValidationResult.OK;
            if (validationErrors == 0) return ValidationResult.Warning;
            return ValidationResult.Error;
        }
        private void onValidation(object sender, ValidationEventArgs e)
        {
            xmlExceptions.Add(new SuperXmlSchemaException(e.Exception,e.Severity));
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine($"[Validator] Warning: {e.Message} at line {e.Exception.LineNumber}");
                Console.ResetColor();
                validationWarnings++;
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine($"[Validator] Error: {e.Message} at line {e.Exception.LineNumber}");
                Console.ResetColor();
                validationErrors++;
            }
        }
    }
}
