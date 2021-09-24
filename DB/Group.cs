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
    public partial class Group : Form
    {
        readonly bool edit;
        readonly string orig_group;
        public Group()
        {
            InitializeComponent();
            edit = false;
        }
        public Group(string group_name, int spec, int year)
        {
            InitializeComponent();
            orig_group = group_name;
            edit = true;
            textBox1.Text = group_name;
            textBox2.Text = spec.ToString();
            textBox3.Text = year.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Group_Load(object sender, EventArgs e) {}

        private void button1_Click(object sender, EventArgs e)
        {
            if (!edit)
                groupsTableAdapter1.InsertQuery(textBox1.Text, 
                    Convert.ToInt32(textBox2.Text), Convert.ToByte(textBox3.Text));
            else
                groupsTableAdapter1.UpdateQuery(textBox1.Text, Convert.ToInt32(textBox2.Text), 
                   Convert.ToByte(textBox3.Text), orig_group);
            Close();
        }
    }
}
