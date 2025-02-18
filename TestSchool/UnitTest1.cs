namespace TestSchool
{
    using SchoolProject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SqlServer;
    using System.Data.SqlClient;
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        private SchoolDatabase school;
        string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        [Test]
        public void TestDatabaseAndTables()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='Students';", conn);
                var result = cmd.ExecuteScalar();
                Assert.AreEqual("Students", result);
            }
        }
        public void TestInsertData()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Students;", conn);
                var students = cmd.ExecuteScalar();
                Assert.AreEqual(9, students);
                cmd = new SqlCommand("SELECT COUNT(*) FROM Teachers;", conn);
                var teachers = cmd.ExecuteScalar();
                Assert.AreEqual(3, teachers);
                cmd = new SqlCommand("SELECT COUNT(*) FROM Subjects;", conn);
                var subjects = cmd.ExecuteScalar();
                Assert.AreEqual(7, subjects);
                cmd = new SqlCommand("SELECT COUNT(*) FROM Classes;", conn);
                var classes = cmd.ExecuteScalar();
                Assert.AreEqual(4, classes);
                cmd = new SqlCommand("SELECT COUNT(*) FROM Classrooms;", conn);
                var classrooms = cmd.ExecuteScalar();
                Assert.AreEqual(4, classrooms);
                cmd = new SqlCommand("SELECT COUNT(*) FROM Parents;", conn);
                var parents = cmd.ExecuteScalar();
                Assert.AreEqual(5, parents);
                cmd = new SqlCommand("SELECT COUNT(*) FROM Student_Subjects;", conn);
                var student_Subjects = cmd.ExecuteScalar();
                Assert.AreEqual(3, student_Subjects);
                cmd = new SqlCommand("SELECT COUNT(*) FROM Teacher_Subjects;", conn);
                var teacher_Subjects = cmd.ExecuteScalar();
                Assert.AreEqual(3, teacher_Subjects);
            }
        }
        [Test]
        public void TestSelectQueryWhenNumIs1()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                school.SelectQuery(1);
                SqlCommand cmd = new SqlCommand("SELECT s.full_name FROM Students s JOIN Classes c ON s.class_id = c.id WHERE c.class_number = 11 AND c.class_letter = 'Б';", conn);
                var roomCount = cmd.ExecuteScalar();
                Assert.AreEqual(2, roomCount);
            }
        }
        [Test]
        public void TestSelectQueryWhenNumIs2()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                school.SelectQuery(2);
                SqlCommand cmd = new SqlCommand("SELECT t.full_name, s.name AS subject FROM Teachers t JOIN Teacher_Subjects ts ON t.id = ts.teacher_id JOIN Subjects s ON ts.subject_id = s.id ORDER BY s.name;", conn);
                var roomCount = cmd.ExecuteScalar();
                Assert.AreEqual(3, roomCount);
            }
        }
        [Test]
        public void TestSelectQueryWhenNumIs3()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                school.SelectQuery(3);
                SqlCommand cmd = new SqlCommand("SELECT c.class_number, c.class_letter, t.full_name FROM Classes c JOIN Teachers t ON c.class_teacher_id = t.id;", conn);
                var roomCount = cmd.ExecuteScalar();
                Assert.AreEqual(4, roomCount);
            }
        }
        [Test]
        public void TestSelectQueryWhenNumIs4()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                school.SelectQuery(4);
                SqlCommand cmd = new SqlCommand("SELECT s.name, COUNT(*) AS 'broi uchiteli' FROM Subjects s JOIN Teacher_Subjects ts ON s.id = ts.subject_id GROUP BY s.name;", conn);
                var roomCount = cmd.ExecuteScalar();
                Assert.AreEqual(5, roomCount);
            }
        }
        [Test]
        public void TestSelectQueryWhenNumIs5()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                school.SelectQuery(5);
                SqlCommand cmd = new SqlCommand("SELECT id, capacity FROM Classrooms WHERE capacity > 26 ORDER BY floor;", conn);
                var roomCount = cmd.ExecuteScalar();
                Assert.AreEqual(6, roomCount);
            }
        }
        [Test]
        public void TestSelectQueryWhenNumIs6()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                school.SelectQuery(6);
                SqlCommand cmd = new SqlCommand("SELECT s.full_name, c.class_number, c.class_letter FROM Students s JOIN Classes c ON s.class_id = c.id ORDER BY c.class_number, c.class_letter;", conn);
                var roomCount = cmd.ExecuteScalar();
                Assert.AreEqual(6, roomCount);
            }
        }
    }
}
