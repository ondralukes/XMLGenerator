using System;
using System.IO;
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
        public static bool ignoreWarnings = false;
        private int validationWarnings = 0;
        private int validationErrors = 0;
        private List<SuperXmlSchemaException> xmlExceptions = new List<SuperXmlSchemaException>();
        private TextWriter outputStream;
        private XMLGenerator.ValidationResultPromptDelegate validationResultPrompt;
        public Validator(string _filename,string _xsdLocation, TextWriter outStream, XMLGenerator.ValidationResultPromptDelegate prompt)
        {
            filename = _filename;
            xsdLocation = _xsdLocation;
            outputStream = outStream;
            validationResultPrompt = prompt;
        }
        
        public bool Validate(bool consoleOnly)
        {
            Validator.ValidationResult validationResult = ValidateCore();
            switch (validationResult)
            {
                case Validator.ValidationResult.OK:
                    outputStream.WriteLine("Validation OK");
                    break;
                case Validator.ValidationResult.Warning:
                    outputStream.Write($"Validation finished with {validationWarnings} warnings. Continue? ");
                    bool stop = false;
                    if (!ignoreWarnings && !consoleOnly)
                    {
                        ValidationResultPromptResult res = new ValidationResultPromptResult();
                        if(validationResultPrompt != null) res = validationResultPrompt(xmlExceptions, true);
                        stop = res.stop;
                        ignoreWarnings = res.ignoreWarnings;
                    } else if (consoleOnly)
                    {
                        ExceptionDump();
                    }
                    if (stop)
                    {
                        outputStream.WriteLine("No.");
                    } else
                    {
                        outputStream.WriteLine("Yes.");
                    }
                    return !stop;
                case Validator.ValidationResult.Error:
                    outputStream.WriteLine("Validation finished with errors!");
                    if (!consoleOnly)
                    {
                        if (validationResultPrompt != null) validationResultPrompt(xmlExceptions, false);
                    } else
                    {
                        ExceptionDump();
                    }
                    return false;
                case ValidationResult.Failed:
                    outputStream.WriteLine("Validation failed!");
                    return false;
            }
            return true;
        }
        public ValidationResult ValidateCore()
        {
            outputStream.WriteLine($"[Validator] Validating {XMLGenerator.ToRelativePath(filename)}");
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.AllowXmlAttributes;
            readerSettings.ValidationEventHandler += new ValidationEventHandler(onValidation);
            readerSettings.ValidationType = ValidationType.Schema;
            readerSettings.Schemas.XmlResolver = new XmlUrlResolver();
            XmlReader xmlReader = null;
            FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read);
            try
            {

                readerSettings.Schemas.Add("flowbased", xsdLocation);

                xmlReader = XmlReader.Create(fs, readerSettings);
            } catch (Exception e)
            {
                outputStream.WriteLine($"[Validator] Validation failed: {e.Message}");
                if(xmlReader != null) xmlReader.Close();
                fs.Close();
                return ValidationResult.Failed;
            }
            
            while (xmlReader.Read()) ;
            xmlReader.Close();
            fs.Close();
            if (validationWarnings == 0 && validationErrors == 0) return ValidationResult.OK;
            if (validationErrors == 0) return ValidationResult.Warning;
            return ValidationResult.Error;
        }

        private void ExceptionDump()
        {
            outputStream.WriteLine();
            outputStream.WriteLine($"======{validationWarnings} warnings==={validationErrors} errors======");
            foreach (var exception in xmlExceptions)
            {
                outputStream.WriteLine(
                    $"==={(exception.severity==XmlSeverityType.Error?"Error":"Warning")}" +
                    $" at line {exception.exception.LineNumber}: {exception.exception.Message}"
                    );
            }
            outputStream.WriteLine();
        }
        private void onValidation(object sender, ValidationEventArgs e)
        {
            xmlExceptions.Add(new SuperXmlSchemaException(e.Exception,e.Severity));
            if (e.Severity == XmlSeverityType.Warning)
            {
                //outputStream.WriteLine($"[Validator] Warning: {e.Message} at line {e.Exception.LineNumber}");
                validationWarnings++;
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                //outputStream.WriteLine($"[Validator] Error: {e.Message} at line {e.Exception.LineNumber}");
                validationErrors++;
            }
        }
        protected class XmlXsdResolver : XmlUrlResolver
        {
            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                return base.GetEntity(absoluteUri, role, ofObjectToReturn);
            }
        }
    }
}
