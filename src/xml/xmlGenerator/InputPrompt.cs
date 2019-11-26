using System;
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
    public partial class InputPrompt : Form
    {
        public string constraintTimeInterval;
        public string senderIndentification;
        public string receiverIndentification;
        public int outagesPerBranch = 0;
        public bool inputOK = false;
        public InputPrompt()
        {
            InitializeComponent();
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            constraintTimeInterval = cTimeIntTextBox.Text;
            senderIndentification = SIndentificationTextBox.Text;
            receiverIndentification = RIndentificationTextBox.Text;
            outagesPerBranch = (int)outagesCountUpDown.Value;
            inputOK = true;
            this.Close();
        }

        private void SIndentificationTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
