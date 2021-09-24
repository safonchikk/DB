using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB
{
    public partial class StatForm : Form
    {
        const string ConnectionString = @"Data Source=DESKTOP-1GV8TVG;Initial Catalog=university;Integrated Security=True;"
        + "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        string querystr;
        public StatForm()
        {
            InitializeComponent();
            querystr = "select speciality as 'Спеціальність', study_year as 'Курс', count(student_id) as 'Кількість студентів' from " +
            "students left join groups on students.group_name = groups.group_name " +
            "group by speciality, study_year;";
        }

        public StatForm(string group)
        {
            InitializeComponent();
            querystr = "select students.group_name as 'Група', grades.subj_name as 'Предмет', avg(grades.mark) as 'Середня оцінка' from " +
            "grades left join students on students.student_id = grades.student_id where students.group_name = '" + group +
            "' group by students.group_name, grades.subj_name;";
        }

        public StatForm(int i)
        {
            InitializeComponent();
            querystr = "select st.group_name as 'Група', (cast(count(st.student_id) as float)/qq.c*100) as 'Відсоток бюджетників' " +
                "from students as st left join (select students.group_name, count(students.student_id) as c from students group by students.group_name) " +
                "as qq on st.group_name = qq.group_name where st.payment = 'Бюджет' group by st.group_name, qq.c;";
        }

        public StatForm(string group, int i)
        {
            InitializeComponent();
            querystr = "select concat(s_surname, ' ', s_name, ' ', s_patr) as \"ПІБ\", students.group_name as 'Група', avg(grades.mark) as 'Середня оцінка' from " +
            "grades left join students on students.student_id = grades.student_id where students.group_name = '" + group +
            "' group by students.group_name, concat(s_surname, ' ', s_name, ' ', s_patr);";
        }

        private void StatForm_Load(object sender, EventArgs e)
        {
            SqlConnection sqlconn = new SqlConnection(ConnectionString);
            sqlconn.Open();
            SqlDataAdapter oda = new SqlDataAdapter(querystr, sqlconn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            sqlconn.Close();
        }
    }
}
