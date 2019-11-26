using System;
using System.Xml.Schema;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xmlGenerator
{
    public partial class ValidationResultForm : Form
    {
        private bool inputOK = false;
        public bool stop = false;
        private List<SuperXmlSchemaException> xmlExceptions;
        public ValidationResultForm(List<SuperXmlSchemaException> _xmlExceptions,bool allowContinue)
        {
            xmlExceptions = _xmlExceptions;
            
            InitializeComponent();
            ColumnHeader header = new ColumnHeader();
            header.Text = "";
            header.Name = "col1";
            
            exceptionList.Columns.Add(header);
            exceptionList.Items.Clear();
            continueBtn.Enabled = allowContinue;
            foreach (var exception in xmlExceptions)
            {
                ListViewItem item = new ListViewItem($"At line {exception.exception.LineNumber}: {exception.exception.Message}");


                if (exception.severity == XmlSeverityType.Error)
                {
                    item.BackColor = Color.Red;
                }
                else
                {
                    item.BackColor = Color.Yellow;
                }
                exceptionList.Items.Add(item);
            }
            header.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            exceptionList.Refresh();
        }

        private void ValidationResultForm_Load(object sender, EventArgs e)
        {
            
        }

        private void ValidationResultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel =!inputOK;
        }

        private void ExceptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (exceptionList.SelectedItems.Count == 0) return;
            SuperXmlSchemaException exception = xmlExceptions[ exceptionList.Items.IndexOf(exceptionList.SelectedItems[0])];
            exceptionText.Text = $"At line {exception.exception.LineNumber}, position {exception.exception.LinePosition}\r\n{exception.exception.SourceUri}\r\n{exception.exception.Message}";
        }

        private void ContinueBtn_Click(object sender, EventArgs e)
        {
            stop = false;
            inputOK = true;
            Close();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            stop = true;
            inputOK = true;
            Close();
        }
    }
}
