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
    public partial class Teacher : Form
    {
        readonly bool edit;
        readonly int t_id;
        public Teacher()
        {
            InitializeComponent();
            posComboBox.SelectedIndex = posComboBox.FindString("А");
            edit = false;
        }
        public Teacher(int id, string surname, string name, string patr, string position)
        {
            InitializeComponent();
            edit = true;
            t_id = id;
            surTextBox.Text = surname;
            nameTextBox.Text = name;
            patrTextBox.Text = patr;
            posComboBox.SelectedIndex = posComboBox.FindString(position);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!edit)
                teachersTableAdapter1.InsertQuery(surTextBox.Text, nameTextBox.Text, 
                    patrTextBox.Text, posComboBox.SelectedItem.ToString());
            else
                teachersTableAdapter1.UpdateQuery(surTextBox.Text, nameTextBox.Text,
                    patrTextBox.Text, posComboBox.SelectedItem.ToString(), t_id);
            Close();
        }
    }
}
