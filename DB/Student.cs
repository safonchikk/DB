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
    public partial class Student : Form
    {
        readonly bool edit;
        readonly int st_id;
        public Student()
        {
            InitializeComponent();
            payComboBox.SelectedIndex = payComboBox.FindString("Б");
            genderComboBox.SelectedIndex = genderComboBox.FindString("Ж");
            groupsTableAdapter.Fill(universityDataSet.Groups);
            edit = false;
            this.Text = "Додавання";
        }
        public Student(int id, string surname, string name, string patr, string group,
                       DateTime birth_date, string payment, string gender)
        {
            InitializeComponent();
            edit = true;
            st_id = id;
            surTextBox.Text = surname;
            nameTextBox.Text = name;
            patrTextBox.Text = patr;
            dateTimePicker1.Value = birth_date;
            payComboBox.SelectedIndex = payComboBox.FindString(payment);
            genderComboBox.SelectedIndex = genderComboBox.FindString(gender);
            groupsTableAdapter.Fill(universityDataSet.Groups);
            groupComboBox.SelectedIndex = groupComboBox.FindString(group);
            this.Text = "Редагування";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Student_Load(object sender, EventArgs e){ }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!edit) 
                studentsTableAdapter.InsertQuery(surTextBox.Text, nameTextBox.Text, patrTextBox.Text,
                    groupComboBox.SelectedValue.ToString(), dateTimePicker1.Value.ToString(),
                    payComboBox.SelectedItem.ToString(), genderComboBox.SelectedItem.ToString().Substring(0, 1)); 
            else
                studentsTableAdapter.UpdateQuery(surTextBox.Text, nameTextBox.Text, patrTextBox.Text,
                    groupComboBox.SelectedValue.ToString(), dateTimePicker1.Value.ToString(),
                    payComboBox.SelectedItem.ToString(), genderComboBox.SelectedItem.ToString().Substring(0, 1), st_id);
            Close();
        }
    }
}
