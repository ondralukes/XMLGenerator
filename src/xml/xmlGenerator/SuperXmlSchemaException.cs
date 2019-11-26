using System;
using System.Xml.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlGenerator
{
    public class SuperXmlSchemaException
    {
        public XmlSchemaException exception;
        public XmlSeverityType severity;
        public SuperXmlSchemaException(XmlSchemaException _ex, XmlSeverityType _severity)
        {
            exception = _ex;
            severity = _severity;
        }
    }
}
