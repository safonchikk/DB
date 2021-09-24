using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB
{
    public partial class Form1 : Form
    {
        string ConnectionString;
        SqlConnection sqlconn;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "universityDataSet3.Grades". При необходимости она может быть перемещена или удалена.
            this.gradesTableAdapter.Fill(this.universityDataSet3.Grades);
            groupsTableAdapter.Fill(universityDataSet.Groups);
            studentsTableAdapter.Fill(universityDataSet.Students);
            subjectsTableAdapter.Fill(universityDataSet.Subjects);
            teachersTableAdapter.Fill(universityDataSet.Teachers); 
            groups_TeachersTableAdapter.Fill(universityDataSet.Groups_Teachers);
            gradesTableAdapter.Fill(universityDataSet.Grades);
            groupComboBox.SelectedIndex = -1;
            ConnectionString = "Data Source=DESKTOP-1GV8TVG;Initial Catalog=university;Integrated Security=True";
            sqlconn = new SqlConnection(ConnectionString);
            for (int i = 0; i < universityDataSet.Groups.Rows.Count; ++i)
            {
                string val = universityDataSet.Groups.Rows[i].ItemArray[1].ToString();
                if (!comboBox2.Items.Contains(val))
                {
                    comboBox2.Items.Add(val);
                }
                val = universityDataSet.Groups.Rows[i].ItemArray[2].ToString();
                if (!comboBox3.Items.Contains(val))
                {
                    comboBox3.Items.Add(val);
                }
            }
            comboBox2.Sorted = true;
            comboBox3.Sorted = true;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ef = new Student();
            ef.ShowDialog();
            studentsTableAdapter.Fill(this.universityDataSet.Students);
            universityDataSet.AcceptChanges();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.SelectedRows[0];
            var ef = new Student(
            Convert.ToInt32(row.Cells[0].Value),
            row.Cells[1].Value.ToString(),
            row.Cells[2].Value.ToString(),
            row.Cells[3].Value.ToString(),
            row.Cells[4].Value.ToString(),
            Convert.ToDateTime(row.Cells[5].Value),
            row.Cells[6].Value.ToString(),
            row.Cells[7].Value.ToString()
            );
            ef.ShowDialog();
            studentsTableAdapter.Fill(universityDataSet.Students);
            universityDataSet.AcceptChanges();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = (dataGridView1.SelectedRows.Count > 1) ? 
                dataGridView1.SelectedRows.Count + " записів таблиці \"Студенти\"" :
                dataGridView1.SelectedRows[0].Cells[1].Value + " " + dataGridView1.SelectedRows[0].Cells[2].Value;
            DialogResult mb = MessageBox.Show("Точно хочете видалити " + s + "?", "", MessageBoxButtons.OKCancel);
            if (mb == DialogResult.Cancel)
                return;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                studentsTableAdapter.DeleteQuery(Convert.ToInt32(row.Cells[0].Value));
            studentsTableAdapter.Fill(universityDataSet.Students);
            universityDataSet.AcceptChanges();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Selected = false;
                if (row.Cells[1].Value.ToString().ToLower().IndexOf(surTextBox.Text.ToLower()) != -1 &&
                    row.Cells[2].Value.ToString().ToLower().IndexOf(nameTextBox.Text.ToLower()) != -1 &&
                    row.Cells[3].Value.ToString().ToLower().IndexOf(patrTextBox.Text.ToLower()) != -1)
                    row.Selected = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "select * from students where";
            bool f = false;
            if (groupComboBox.SelectedIndex != -1)
            {
                query += $" group_name = '{groupComboBox.SelectedValue}'";
                f = true;
            }
            if (genderComboBox.SelectedIndex != -1)
            {
                if (f)
                    query += " and";
                query += $" gender = '{genderComboBox.SelectedItem.ToString()[0]}'";
                f = true;
            }
            if (payComboBox.SelectedIndex != -1)
            {
                if (f)
                    query += " and";
                query += $" payment = '{payComboBox.SelectedItem}'";
                f = true;
            }
            if (!f)
                query = "select * from students";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query + ";", sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            sqlconn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupComboBox.SelectedIndex = -1;
            genderComboBox.SelectedIndex = -1;
            payComboBox.SelectedIndex = -1;
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter("select * from students;", sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            sqlconn.Close();
        }

        private void deleteGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = (dataGridView2.SelectedRows.Count > 1) ?
                dataGridView2.SelectedRows.Count + " записів таблиці \"Групи\"" :
                dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            DialogResult mb = MessageBox.Show("Точно хочете видалити " + s + "?", "", MessageBoxButtons.OKCancel);
            if (mb == DialogResult.Cancel)
                return;
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                groupsTableAdapter.DeleteQuery(row.Cells[0].Value.ToString());
            groupsTableAdapter.Fill(universityDataSet.Groups);
            universityDataSet.AcceptChanges();
        }

        private void editGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView2.SelectedRows[0];
            var ef = new Group(
            row.Cells[0].Value.ToString(),
            Convert.ToInt32(row.Cells[1].Value),
            Convert.ToInt32(row.Cells[2].Value));
            ef.ShowDialog();
            groupsTableAdapter.Fill(universityDataSet.Groups);
            universityDataSet.AcceptChanges();
        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ef = new Group();
            ef.ShowDialog();
            groupsTableAdapter.Fill(this.universityDataSet.Groups);
            universityDataSet.AcceptChanges();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string query = "select * from groups where";
            bool f = false;
            if (textBox2.Text != "")
            {
                query += $" speciality = '{textBox2.Text}'";
                f = true;
            }
            if (textBox3.Text != "")
            {
                if (f)
                    query += " and";
                query += $" study_year >= {textBox3.Text}";
                f = true;
            }
            if (textBox4.Text != "")
            {
                if (f)
                    query += " and";
                query += $" study_year <= {textBox4.Text}";
                f = true;
            }
            if (!f)
                query = "select * from groups";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query + ";", sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView2.DataSource = dt;
            sqlconn.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox3.Text = textBox4.Text = "";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter("select * from groups;", sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView2.DataSource = dt;
            sqlconn.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                row.Selected = false;
                if (row.Cells[0].Value.ToString().ToLower().IndexOf(textBox1.Text.ToLower()) != -1)
                    row.Selected = true;
            }
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var ef = new Teacher();
            ef.ShowDialog();
            teachersTableAdapter.Fill(universityDataSet.Teachers);
            universityDataSet.AcceptChanges();
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView3.SelectedRows[0];
            var ef = new Teacher(
            Convert.ToInt32(row.Cells[0].Value),
            row.Cells[1].Value.ToString(),
            row.Cells[2].Value.ToString(),
            row.Cells[3].Value.ToString(),
            row.Cells[4].Value.ToString()
            );
            ef.ShowDialog();
            teachersTableAdapter.Fill(universityDataSet.Teachers);
            universityDataSet.AcceptChanges();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string s = (dataGridView3.SelectedRows.Count > 1) ?
                dataGridView3.SelectedRows.Count + " записів таблиці \"Викладачі\"" :
                dataGridView3.SelectedRows[0].Cells[1].Value + " " + dataGridView3.SelectedRows[0].Cells[2].Value;
            DialogResult mb = MessageBox.Show("Точно хочете видалити " + s + "?", "", MessageBoxButtons.OKCancel);
            if (mb == DialogResult.Cancel)
                return;
            foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                teachersTableAdapter.DeleteQuery(Convert.ToInt32(row.Cells[0].Value));
            teachersTableAdapter.Fill(universityDataSet.Teachers);
            universityDataSet.AcceptChanges();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            StatForm a = new StatForm();
            a.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            StatForm a = new StatForm(dataGridView2.SelectedRows[0].Cells[0].Value.ToString());
            a.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            StatForm a = new StatForm(1);
            a.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            StatForm a = new StatForm(dataGridView2.SelectedRows[0].Cells[0].Value.ToString(), 1);
            a.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string query = "select * from subjects where";
            bool f = false;
            if (textBox9.Text != "")
            {
                query += $" credits >= '{textBox9.Text}'";
                f = true;
            }
            if (textBox8.Text != "")
            {
                if (f)
                    query += " and";
                query += $" credits <= {textBox8.Text}";
                f = true;
            }
            if (textBox6.Text != "")
            {
                if (f)
                    query += " and";
                query += $" acad_hours >= {textBox6.Text}";
                f = true;
            }
            if (textBox5.Text != "")
            {
                if (f)
                    query += " and";
                query += $" acad_hours <= {textBox5.Text}";
                f = true;
            }
            if (!f)
                query = "select * from subjects";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query + ";", sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView4.DataSource = dt;
            sqlconn.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox6.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            string query = "select * from subjects;";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView4.DataSource = dt;
            sqlconn.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView4.Rows)
            {
                row.Selected = false;
                if (row.Cells[0].Value.ToString().ToLower().IndexOf(textBox7.Text.ToLower()) != -1)
                    row.Selected = true;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            string query = "select * from teachers;";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView3.DataSource = dt;
            sqlconn.Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string query = "select * from teachers where";
            if (comboBox1.SelectedIndex != -1)
                query += $" position = '{comboBox1.SelectedItem.ToString()}'";
            else
                query = "select * from teachers";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query + ";", sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView3.DataSource = dt;
            sqlconn.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                row.Selected = false;
                if (row.Cells[1].Value.ToString().ToLower().IndexOf(textBox10.Text.ToLower()) != -1 &&
                    row.Cells[2].Value.ToString().ToLower().IndexOf(textBox11.Text.ToLower()) != -1 &&
                    row.Cells[3].Value.ToString().ToLower().IndexOf(textBox12.Text.ToLower()) != -1)
                    row.Selected = true;
            }
        }

        private void додатиToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var ef = new Subject();
            ef.ShowDialog();
            subjectsTableAdapter.Fill(universityDataSet.Subjects);
            universityDataSet.AcceptChanges();
        }

        private void редагуватиToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView4.SelectedRows[0];
            var ef = new Subject(
            row.Cells[0].Value.ToString(),
            Convert.ToInt32(row.Cells[1].Value.ToString()),
            Convert.ToInt32(row.Cells[2].Value.ToString())
            );
            ef.ShowDialog();
            subjectsTableAdapter.Fill(universityDataSet.Subjects);
            universityDataSet.AcceptChanges();
        }

        private void видалитиToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string s = (dataGridView4.SelectedRows.Count > 1) ?
                dataGridView4.SelectedRows.Count + " записів таблиці \"Предмети\"" :
                dataGridView4.SelectedRows[0].Cells[0].Value.ToString();
            DialogResult mb = MessageBox.Show("Точно хочете видалити " + s + "?", "", MessageBoxButtons.OKCancel);
            if (mb == DialogResult.Cancel)
                return;
            foreach (DataGridViewRow row in dataGridView4.SelectedRows)
                subjectsTableAdapter.DeleteQuery(row.Cells[0].Value.ToString());
            subjectsTableAdapter.Fill(universityDataSet.Subjects);
            universityDataSet.AcceptChanges();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Готово!");
            string query = "select concat(s_surname, ' ', s_name, ' ', s_patr) as \"ПІБ\", " +
                "students.group_name as 'Група', count(mark) as 'Кількість незаліків' from grades left join students " +
                "on grades.student_id = students.student_id where mark < 60 " +
                "group by concat(s_surname, ' ', s_name, ' ', s_patr), students.group_name " +
                "having count(mark) > 2;";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            MessageBox.Show("Готово!");
            using (StreamWriter sw = new StreamWriter("список на відрахування.txt"))
            {
                sw.WriteLine(String.Format("{0, -35}|{1, -10}|{2, -2}", "ПІБ студента", "Група", "Кількість незаліків"));
                sw.WriteLine(new string('=', 65));
                foreach (DataRow row in dt.Rows)
                {
                    sw.WriteLine(String.Format("{0, -35}|{1, -10}|{2, -2}", row[0].ToString(), row[1].ToString(), row[2].ToString()));
                }
            }
            sqlconn.Close();
            MessageBox.Show("Готово!");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string query = "select concat(s_surname, ' ', s_name, ' ', s_patr), " +
                "mark from grades left join students " +
                "on grades.student_id = students.student_id where students.group_name = '{0}' and grades.subj_name = '{1}'";
            sqlconn.Open();
            DataTable dt = new DataTable();
            foreach (DataGridViewRow r in dataGridView6.SelectedRows) {
                SqlDataAdapter oda = new SqlDataAdapter(String.Format(query, r.Cells[0].Value.ToString(), r.Cells[1].Value.ToString()), sqlconn);
                oda.Fill(dt);
                using (StreamWriter sw = new StreamWriter(r.Cells[0].Value.ToString() + '_' + r.Cells[1].Value.ToString() + ".txt"))
                {
                    sw.WriteLine("Викладач: " + r.Cells[2].Value.ToString());
                    sw.WriteLine("Предмет: " + r.Cells[1].Value.ToString());
                    sw.WriteLine("Група: " + r.Cells[0].Value.ToString());
                    sw.WriteLine("Дата: " + DateTime.Now.ToString());
                    sw.WriteLine(new string('=', 40));
                    sw.WriteLine(String.Format("{0, -35}|{1, -4}", "ПІБ студента", "Оцінка"));
                    sw.WriteLine(new string('=', 40));
                    foreach (DataRow row in dt.Rows)
                    {
                        sw.WriteLine(String.Format("{0, -35}|{1, -4}", row[0].ToString(), row[1].ToString()));
                    }
                }
            }
            sqlconn.Close();
            MessageBox.Show("Готово!");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string query = "select concat(students.s_surname, ' ', students.s_name, ' ', students.s_patr), " +
                "students.group_name, sum(cast(grades.mark as float) * subjects.credits / 30) from grades left join students " +
                "on grades.student_id = students.student_id left join groups on students.group_name = groups.group_name " +
                "left join subjects on grades.subj_name = subjects.subj_name " +
                "where groups.study_year = " + comboBox3.SelectedItem +
                " and groups.speciality = " + comboBox2.SelectedItem +
                " and students.student_id not in (select student_id from grades where mark < 60)" +
                " and students.payment = 'Бюджет' group by concat(s_surname, ' ', s_name, ' ', s_patr), students.group_name " +
                "order by sum(cast(grades.mark as float) * subjects.credits / 30) desc;";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            SqlDataAdapter oda1 = new SqlDataAdapter("select count(students.student_id) " +
                "from students left join groups on students.group_name = groups.group_name " +
                "where groups.study_year = " + comboBox3.SelectedItem +
                " and groups.speciality = " + comboBox2.SelectedItem +
                " and students.payment = 'Бюджет'", sqlconn);
            DataTable dt1 = new DataTable();
            oda1.Fill(dt1);
            double budg_count = Convert.ToDouble(dt1.Rows[0].ItemArray[0].ToString());
            double st1;
            double st2;
            using (StreamReader sr = new StreamReader("квоти.txt"))
            {
                st1 = Convert.ToDouble(sr.ReadLine()); 
                st1 *= budg_count/100;
                st2 = Convert.ToDouble(sr.ReadLine());
                st2 *= st1/100;
            }
            oda1 = new SqlDataAdapter("select concat(students.s_surname, ' ', students.s_name, ' ', students.s_patr), " +
                "students.group_name, 'Не підраховується' from students " +
                "left join groups on students.group_name = groups.group_name " +
                "where groups.study_year = " + comboBox3.SelectedItem +
                " and groups.speciality = " + comboBox2.SelectedItem +
                " and (students.student_id in (select student_id from grades where mark < 60)" +
                " or students.payment = 'Контракт');", sqlconn);
            dt1 = new DataTable();
            oda1.Fill(dt1);
            int i = 1;
            using (StreamWriter sw = new StreamWriter(comboBox2.SelectedItem.ToString() + '_'  + comboBox3.SelectedItem.ToString() + "_курс_рейт.txt"))
            {
                sw.WriteLine(String.Format("{0, -35}|{1, -10}|{2, -20}|{3}", "ПІБ студента", "Група", "Рейтинговий бал", "Стипендія"));
                sw.WriteLine(new string('=', 80));
                foreach (DataRow row in dt.Rows)
                {
                    sw.Write(String.Format("{0, -35}|{1, -10}|{2, -20}|", row[0].ToString(), row[1].ToString(), row[2].ToString()));
                    if (i <= st2)
                        sw.WriteLine("Підвищена");
                    else if (i <= st1)
                        sw.WriteLine("Звичайна");
                    else
                        sw.WriteLine("Відсутня");
                    ++i;
                }
                foreach (DataRow row in dt1.Rows)
                {
                    sw.WriteLine(String.Format("{0, -35}|{1, -10}|{2, -20}|Відсутня", row[0].ToString(), row[1].ToString(), row[2].ToString()));
                }
                
            }
            sqlconn.Close();
            MessageBox.Show("Готово!");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string query = "SELECT subj_name, concat(s_surname, ' ', s_name, ' ', s_patr), " +
                "mark FROM dbo.Grades left join dbo.Students on Grades.student_id = Students.student_id where";
            bool f = false;
            if (textBox14.Text != "")
            {
                query += $" mark >= '{textBox14.Text}'";
                f = true;
            }
            if (textBox13.Text != "")
            {
                if (f)
                    query += " and";
                query += $" mark <= {textBox13.Text}";
                f = true;
            }
            if (!f)
                query = "SELECT subj_name, concat(s_surname, ' ', s_name, ' ', s_patr), " +
                    "mark FROM dbo.Grades left join dbo.Students on Grades.student_id = Students.student_id;";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query + ";", sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView5.DataSource = dt;
            sqlconn.Close();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            textBox13.Text = "";
            textBox14.Text = "";
            string query = "SELECT subj_name, concat(s_surname, ' ', s_name, ' ', s_patr), " +
                "mark FROM dbo.Grades left join dbo.Students on Grades.student_id = Students.student_id;";
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView4.DataSource = dt;
            sqlconn.Close();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView5.Rows)
            {
                row.Selected = false;
                if (row.Cells[0].Value.ToString().ToLower().IndexOf(textBox15.Text.ToLower()) != -1 &&
                    row.Cells[1].Value.ToString().ToLower().IndexOf(textBox16.Text.ToLower()) != -1)
                    row.Selected = true;
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView6.Rows)
            {
                row.Selected = false;
                if (row.Cells[0].Value.ToString().ToLower().IndexOf(textBox18.Text.ToLower()) != -1 &&
                    row.Cells[1].Value.ToString().ToLower().IndexOf(textBox17.Text.ToLower()) != -1 &&
                    row.Cells[2].Value.ToString().ToLower().IndexOf(textBox19.Text.ToLower()) != -1)
                    row.Selected = true;
            }
        }
    }
}
