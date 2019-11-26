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
            OK, Warning, Error
        }
        string filename;
        public Validator(string _filename)
        {
            filename = _filename;
        }
        private int validationWarnings = 0;
        private int validationErrors = 0;
        private List<SuperXmlSchemaException> xmlExceptions = new List<SuperXmlSchemaException>();
        public bool Validate()
        {
            Validator.ValidationResult validationResult = ValidateCore();
            switch (validationResult)
            {
                case Validator.ValidationResult.OK:
                    Console.WriteLine("Validation OK");
                    break;
                case Validator.ValidationResult.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Validation finished with {validationWarnings} warnings. Continue? ");
                    ValidationResultForm validationResultForm = new ValidationResultForm(xmlExceptions,true);
                    validationResultForm.ShowDialog();
                    if (validationResultForm.stop)
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
                    return !validationResultForm.stop;
                    break;
                case Validator.ValidationResult.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Validation failed!");
                    validationResultForm = new ValidationResultForm(xmlExceptions, false);
                    validationResultForm.ShowDialog();
                    return false;
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
            readerSettings.ValidationEventHandler += new ValidationEventHandler(onValidation);
            readerSettings.ValidationType = ValidationType.Schema;
            XmlReader xmlReader = XmlReader.Create("ContingencyDictionary.xml", readerSettings);
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
