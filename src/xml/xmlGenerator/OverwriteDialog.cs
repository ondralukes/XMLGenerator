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
    public partial class OverwriteDialog : Form
    {
        public string filename;
        public bool overwrite;
        public bool forAll;
        public bool inputOk = false;
        public OverwriteDialog(string _filename)
        {
            filename = _filename;
            InitializeComponent();
            mainLabel.Text = $"{filename} already exists\nOverwrite?";
        }

        private void YesBtn_Click(object sender, EventArgs e)
        {
            overwrite = true;
            forAll = false;
            inputOk = true; 
            Close();
        }

        private void YesForAllBtn_Click(object sender, EventArgs e)
        {
            overwrite = true;
            forAll = true;
            inputOk = true;
            Close();
        }

        private void NoBtn_Click(object sender, EventArgs e)
        {
            overwrite = false;
            forAll = false;
            inputOk = true;
            Close();
        }

        private void NoForAllBtn_Click(object sender, EventArgs e)
        {
            overwrite = false;
            forAll = true;
            inputOk = true;
            Close();
        }
    }
}
