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
        public string senderIdentification;
        public string receiverIdentification;
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
            senderIdentification = SIdentificationTextBox.Text;
            receiverIdentification = RIdentificationTextBox.Text;
            outagesPerBranch = (int)outagesCountUpDown.Value;
            inputOK = true;
            this.Close();
        }

        private void SIdentificationTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void InputPrompt_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !inputOK;
        }
    }
}
