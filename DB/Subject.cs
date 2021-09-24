using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB
{
    public partial class Subject : Form
    {
        readonly bool edit;
        readonly string orig_subj;
        public Subject()
        {
            InitializeComponent();
            edit = false;
        }
        public Subject(string subj_name, int cred, int hours)
        {
            InitializeComponent();
            orig_subj = subj_name;
            edit = true;
            textBox1.Text = subj_name;
            textBox2.Text = cred.ToString();
            textBox3.Text = hours.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!edit)
                subjectsTableAdapter1.InsertQuery(textBox1.Text,
                    Convert.ToByte(textBox2.Text), Convert.ToByte(textBox3.Text));
            else
                subjectsTableAdapter1.UpdateQuery(textBox1.Text, Convert.ToByte(textBox2.Text),
                   Convert.ToByte(textBox3.Text), orig_subj);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
