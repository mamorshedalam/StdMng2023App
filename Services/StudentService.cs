using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using StdMng2023App.Models;

namespace StdMng2023App.Services
{
    public class StudentService
    {
        private readonly string connStr = "Server=localhost;Database=StdMng2023V2;User Id=sa;Password=MAC945434;Encrypt=False;";
        
        // GENERATE NEW STUDENT ID
        public string GenerateNewStudentID()
        {
            const int startingID = 200215100;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Get the highest existing numeric student ID
                var cmd = new SqlCommand(
                    "SELECT MAX(CAST(stuID AS BIGINT)) FROM t_Borrower WHERE ISNUMERIC(stuID) = 1", conn);

                var result = cmd.ExecuteScalar();

                long maxId = startingID - 1;
                if (result != DBNull.Value && result != null)
                {
                    long.TryParse(result.ToString(), out maxId);
                }

                // Generate the next ID
                long nextId = maxId + 1;
                return nextId.ToString("D9");
            }
        }

        // LOAD STUDENT DATA
        public List<Student> GetStudentSummaries()
        {
            var students = new List<Student>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT stuID, name, sex, depno, depname, grade, class, tel, addr FROM t_Borrower", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        stuID = reader.GetString(0),
                        name = reader.GetString(1),
                        sex = reader.GetString(2),
                        depno = reader.GetString(3),
                        depname = reader.GetString(4),
                        grade = reader.GetString(5),
                        @class = reader.GetString(6),
                        tel = reader.GetString(7),
                        addr = reader.GetString(8)
                    });
                }
            }

            return students;
        }
        // ADD STUDENT
        public void AddStudent(Student student)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO t_Borrower (stuID, name, sex, depno, depname, grade, class, tel, addr) " +
                    "VALUES (@stuID, @name, @sex, @depno, @depname, @grade, @class, @tel, @addr)",
                    conn
                );

                cmd.Parameters.AddWithValue("@stuID", student.stuID);
                cmd.Parameters.AddWithValue("@name", student.name);
                cmd.Parameters.AddWithValue("@sex", student.sex);
                cmd.Parameters.AddWithValue("@depno", student.depno);
                cmd.Parameters.AddWithValue("@depname", student.depname);
                cmd.Parameters.AddWithValue("@grade", student.grade);
                cmd.Parameters.AddWithValue("@class", student.@class);
                cmd.Parameters.AddWithValue("@tel", student.tel);
                cmd.Parameters.AddWithValue("@addr", student.addr);

                cmd.ExecuteNonQuery();
            }
        }

        // UPDATE STUDENT
        public void UpdateStudent(string originalStudentID, Student updatedStudent)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE t_Borrower SET " +
                    "stuID = @newStuID, " +
                    "name = @name, " +
                    "sex = @sex, " +
                    "depno = @depno, " +
                    "depname = @depname, " +
                    "grade = @grade, " +
                    "class = @class, " +
                    "tel = @tel, " +
                    "addr = @addr " +
                    "WHERE stuID = @originalStuID",
                    conn
                );
                
                cmd.Parameters.AddWithValue("@newStuID", updatedStudent.stuID);
                cmd.Parameters.AddWithValue("@name", updatedStudent.name);
                cmd.Parameters.AddWithValue("@sex", updatedStudent.sex);
                cmd.Parameters.AddWithValue("@depno", updatedStudent.depno);
                cmd.Parameters.AddWithValue("@depname", updatedStudent.depname);
                cmd.Parameters.AddWithValue("@grade", updatedStudent.grade);
                cmd.Parameters.AddWithValue("@class", updatedStudent.@class);
                cmd.Parameters.AddWithValue("@tel", updatedStudent.tel);
                cmd.Parameters.AddWithValue("@addr", updatedStudent.addr);
                cmd.Parameters.AddWithValue("@originalStuID", originalStudentID);
                
                cmd.ExecuteNonQuery();
            }
        }
        // DELETE STUDENT
        public void DeleteStudentById(string studentID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM t_Borrower WHERE stuID = @stuID", conn);
                cmd.Parameters.AddWithValue("@stuID", studentID);
                cmd.ExecuteNonQuery();
            }
        }
    }

}
