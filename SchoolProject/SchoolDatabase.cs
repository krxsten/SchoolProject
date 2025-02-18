using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace SchoolProject
{
    public class SchoolDatabase
    {
        public string ConnectionString;

        public SchoolDatabase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void CreateDatabaseAndTables()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string studentsTable = @"CREATE TABLE IF NOT EXISTS Students(
                                        id INT PRIMARY KEY,
                                        student_code NVARCHAR(MAX) NOT NULL,
                                        full_name NVARCHAR(MAX) NOT NULL,
                                        gender NVARCHAR(MAX) NOT NULL,
                                        date_of_birth DATE,
                                        email NVARCHAR(MAX) NOT NULL, 
                                        phone NVARCHAR(MAX) NOT NULL,
                                        class_id INT
                                        is_active BOOLEAN)";
                string teachersTable = @"CREATE TABLE IF NOT EXISTS Teachers
                                        (id INT PRIMARY KEY,
                                        teacher_code NVARCHAR(MAX) NOT NULL,
                                        full_name NVARCHAR(MAX) NOT NULL);";

                string subjectsTable = @"CREATE TABLE IF NOT EXISTS Subjects
                                        (id INT PRIMARY KEY,
                                        full_name NVARCHAR(MAX) NOT NULL
                                        gender NVARCHAR(MAX) NOT NULL,
                                        date_of_birth DATE,
                                        email NVARCHAR(MAX) NOT NULL, 
                                        phone NVARCHAR(MAX) NOT NULL,
                                        working_days INT);";

                string classesTable = @"CREATE TABLE IF NOT EXISTS Classes(
                                        id INT PRIMARY KEY,
                                        class_number INT NOT NULL,
                                        class_letter NVARCHAR(MAX) NOT NULL,
                                        class_teacher_id INT,
                                        FOREIGN KEY (class_teacher_id) REFERENCES Teachers(id));";

                string classroomsTable = @"CREATE TABLE IF NOT EXISTS Classrooms(
                                        id INT PRIMARY KEY,
                                        floor INT NOT NULL,
                                        capacity INT NOT NULL,
                                        description NVARCHAR(MAX) NOT NULL);";

                string parentsTable = @"CREATE TABLE IF NOT EXISTS Parents(
                                        id INT PRIMARY KEY,
                                        parent_code NVARCHAR(MAX) NOT NULL,
                                        full_name NVARCHAR(MAX) NOT NULL,
                                        email NVARCHAR(MAX) NOT NULL.
                                        phone NVARCHAR(MAX) NOT NULL);";

                string studentSubjectsTable = @"CREATE TABLE IF NOT EXISTS Student_Subjects(
                                        student_id INT,
                                        subject_id INT,
                                        PRIMARY KEY (student_id, subject_id),
                                        FOREIGN KEY (student_id) REFERENCES Students(id),
                                        FOREIGN KEY (subject_id) REFERENCES Subjects(id));";

                string teacherSubjectsTable = @"CREATE TABLE IF NOT EXISTS Teacher_Subjects(
                                        teacher_id INT,
                                        subject_id INT,
                                        PRIMARY KEY (teacher_id, subject_id),
                                        FOREIGN KEY (teacher_id) REFERENCES Teachers(id),
                                        FOREIGN KEY (subject_id) REFERENCES Subjects(id));";

            }
        }

        public void InsertData()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string insertTeachers = @"INSERT INTO Teachers (id, teacher_code, full_name) VALUES (1, 'T001', 'Maria Georgieva');";

                string insertSubjects = @"INSERT INTO Subjects (id, full_name, gender, date_of_birth, email, phone, working_days) VALUES (1, 'Mathematics', 'N/A', NULL, 'math@example.com', NULL, 5);";

                string insertClasses = @"INSERT INTO Classes(id, class_number, class_letter, class_teacher_id) VALUES(1, 7, 'A', 1); ";

                string insertClassrooms = @"INSERT INTO Classrooms (id, floor, capacity, description) VALUES (1, 2, 30, 'Mathematics Room');";

                string insertParents = @"INSERT INTO Parents (id, parent_code, full_name, email, phone) VALUES (1, 'P001', 'Petar Petrov', 'petar.petrov@example.com', '+359888654321');";

                string insertStudentSubjects = @"INSERT INTO Student_Subjects (student_id, subject_id) VALUES (1, 1);";

                string insertStudents = @"INSERT INTO Students (id, student_code, full_name, gender, date_of_birth, email, phone, class_id, is_active) VALUES (1, 'S001', 'Ivan Petrov', 'Male', '2008-05-10', 'ivan.petrov@example.com', '+359888123456', 1, TRUE);";

                string insertTeacherSubjects = @"INSERT INTO Teacher_Subjects (teacher_id, subject_id) VALUES (1, 1);";

            }
        }
        public void SelectQuery(int num)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                if (num == 1)
                {
                    string query = @"SELECT s.full_name FROM Students s JOIN Classes c ON s.class_id = c.id WHERE c.class_number = 11 AND c.class_letter = 'Б';";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["full_name"]}");
                        }
                    }
                }
                else if (num == 2)
                {
                    string query = @"SELECT t.full_name, s.name AS subject FROM Teachers t JOIN Teacher_Subjects ts ON t.id = ts.teacher_id JOIN Subjects s ON ts.subject_id = s.id ORDER BY s.name;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["full_name"]} {reader["subject"]}");
                        }
                    }
                }
                else if (num == 3)
                {
                    string query = @"SELECT c.class_number, c.class_letter, t.full_name FROM Classes c JOIN Teachers t ON c.class_teacher_id = t.id;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["class_number"]} {reader["class_letter"]} {reader["full_name"]}");
                        }
                    }
                }
                else if (num == 4)
                {
                    string query = @"SELECT s.name, COUNT(*) AS 'broi uchiteli' FROM Subjects s JOIN Teacher_Subjects ts ON s.id = ts.subject_id GROUP BY s.name;";
                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["name"]} {reader["broi uchiteli'"]}");
                        }
                    }
                }
                else if (num == 5)
                {
                    string query = @"SELECT id, capacity FROM Classrooms WHERE capacity > 26 ORDER BY floor;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["id"]} {reader["capacity"]}");
                        }
                    }
                }
                else if (num == 6)
                {
                    string query = @"SELECT s.full_name, c.class_number, c.class_letter FROM Students s JOIN Classes c ON s.class_id = c.id ORDER BY c.class_number, c.class_letter;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["full_name"]}  {reader["class_number"]} {reader["class_letter"]}");
                        }
                    }
                }
                else if (num == 7)
                {
                    int classNumber = int.Parse(Console.ReadLine());
                    string classLetter = Console.ReadLine();
                    string query = @"SELECT s.full_name FROM Students s JOIN Classes c ON s.class_id = c.id WHERE c.class_number = @classNumber AND c.class_letter = @classLetter;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@classNumber", classNumber);
                        cmd.Parameters.AddWithValue("@classLetter", classLetter);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["full_name"]}");
                            }
                        }
                    }
                }
                else if (num == 8)
                {
                    string birthDate = Console.ReadLine();
                    string query = @"SELECT full_name FROM Students WHERE birth_date = @birthDate;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@birthDate", birthDate);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["full_name"]}");
                            }
                        }
                    }
                }
                else if (num == 9)
                {
                    string firstName = Console.ReadLine();
                    string lastName = Console.ReadLine();
                    string query = @"SELECT COUNT(*) AS 'broi' FROM Students s JOIN Student_Subjects ss ON s.id = ss.student_id WHERE s.first_name = @firstName AND s.last_name = @lastName;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Console.WriteLine($"{reader["broi"]}");
                            }
                        }
                    }
                }
                else if (num == 10)
                {
                    string fullName = Console.ReadLine();
                    string query = @"SELECT t.full_name, s.name AS subject FROM Teachers t JOIN Teacher_Subjects ts ON t.id = ts.teacher_id JOIN Subjects s ON ts.subject_id = s.id JOIN Student_Subjects ss ON s.id = ss.subject_id
                JOIN Students st ON ss.student_id = st.id WHERE st.full_name = @fullName;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", fullName);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["full_name"]} {reader["subject"]}");
                            }
                        }
                    }
                }
                else if (num == 11)
                {
                    string email = Console.ReadLine();
                    string query = @"SELECT c.class_number, c.class_letter FROM Students s JOIN Classes c ON s.class_id = c.id WHERE s.parent_email = @email;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["class_number"]} {reader["class_letter"]}");
                            }
                        }
                    }
                }
            }
        }
    }
}

