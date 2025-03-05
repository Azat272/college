using MySql.Data.MySqlClient;

public class Database
{
    private MySqlConnection connection = new MySqlConnection("server=127.0.0.1;database=college;user=root;port=3306;password=galimov231;");


    public void OpenConnection()
    {
        if (connection.State == System.Data.ConnectionState.Closed)
            connection.Open();
    }

    public void CloseConnection()
    {
        if (connection.State == System.Data.ConnectionState.Open)
            connection.Close();
    }

    public MySqlConnection GetConnection()
    {
        return connection;
    }

    public void AddStudent(string firstname, string lastname, int groupId)
    {
        OpenConnection();
        string query = "INSERT INTO students (firstname, lastname, group_id) VALUES (@firstname, @lastname, @groupId)";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@firstname", firstname);
        cmd.Parameters.AddWithValue("@lastname", lastname);
        cmd.Parameters.AddWithValue("@groupId", groupId);
        cmd.ExecuteNonQuery();
        CloseConnection();
    }

    public List<Student> GetStudents()
    {
        List<Student> students = new List<Student>();
        OpenConnection();

        string query = @"
        SELECT students.id, students.firstname, students.lastname, students.group_id, 
               IFNULL(study_groups.name, 'Без группы') AS group_name
        FROM students
        LEFT JOIN study_groups ON students.group_id = study_groups.id";

        MySqlCommand cmd = new MySqlCommand(query, connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = reader.GetInt32("id"),
                FirstName = reader.GetString("firstname"),
                LastName = reader.GetString("lastname"),
                GroupId = reader.IsDBNull(reader.GetOrdinal("group_id")) ? 0 : reader.GetInt32("group_id"),
                GroupName = reader.IsDBNull(reader.GetOrdinal("group_name")) ? "Без группы" : reader.GetString("group_name")
            });
        }

        reader.Close();
        CloseConnection();
        return students;
    }

    public void DeleteStudent(int id)
    {
        OpenConnection();
        string query = "DELETE FROM students WHERE id = @id";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        CloseConnection();
    }
    public void AddTeacher(string firstname, string lastname)
    {
        OpenConnection();
        string query = "INSERT INTO teachers (firstname, lastname) VALUES (@firstname, @lastname)";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@firstname", firstname);
        cmd.Parameters.AddWithValue("@lastname", lastname);
        cmd.ExecuteNonQuery();
        CloseConnection();
    }

    public List<Teacher> GetTeachers()
    {
        List<Teacher> teachers = new List<Teacher>();
        OpenConnection();

        string query = @"
        SELECT t.id, t.firstname, t.lastname, 
               IFNULL(d.name, 'Без дисциплины') AS discipline_name
        FROM teachers t
        LEFT JOIN teacher_discipline td ON t.id = td.teacher_id
        LEFT JOIN disciplines d ON td.discipline_id = d.id";

        MySqlCommand cmd = new MySqlCommand(query, connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            teachers.Add(new Teacher
            {
                Id = reader.GetInt32("id"),
                FirstName = reader.GetString("firstname"),
                LastName = reader.GetString("lastname"),
                DisciplineName = reader.IsDBNull(reader.GetOrdinal("discipline_name")) ? "Без дисциплины" : reader.GetString("discipline_name")
            });
        }

        reader.Close();
        CloseConnection();
        return teachers;
    }


    public void DeleteTeacher(int teacherId)
    {
        OpenConnection();

   
        string deleteRelationsQuery = "DELETE FROM teacher_discipline WHERE teacher_id = @teacherId";
        MySqlCommand deleteRelationsCmd = new MySqlCommand(deleteRelationsQuery, connection);
        deleteRelationsCmd.Parameters.AddWithValue("@teacherId", teacherId);
        deleteRelationsCmd.ExecuteNonQuery();
        string deleteTeacherQuery = "DELETE FROM teachers WHERE id = @teacherId";
        MySqlCommand deleteTeacherCmd = new MySqlCommand(deleteTeacherQuery, connection);
        deleteTeacherCmd.Parameters.AddWithValue("@teacherId", teacherId);
        deleteTeacherCmd.ExecuteNonQuery();

        CloseConnection();
    }

    public void AddDiscipline(string name)
    {
        OpenConnection();
        string query = "INSERT INTO disciplines (name) VALUES (@name)";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.ExecuteNonQuery();
        CloseConnection();
    }

    public List<Discipline> GetDisciplines()
    {
        List<Discipline> disciplines = new List<Discipline>();
        OpenConnection();
        string query = "SELECT * FROM disciplines";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            disciplines.Add(new Discipline
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name")
            });
        }
        reader.Close();
        CloseConnection();
        return disciplines;
    }
    public int GetLastInsertedId()
    {
        OpenConnection();
        string query = "SELECT LAST_INSERT_ID()";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        int lastId = Convert.ToInt32(cmd.ExecuteScalar());
        CloseConnection();
        return lastId;
    }
    public Discipline GetDisciplineByTeacherId(int teacherId)
    {
        OpenConnection();
        string query = "SELECT d.id, d.name FROM disciplines d " +
                       "JOIN teacher_discipline td ON d.id = td.discipline_id " +
                       "WHERE td.teacher_id = @teacherId";

        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@teacherId", teacherId);
        MySqlDataReader reader = cmd.ExecuteReader();

        Discipline discipline = null;
        if (reader.Read())
        {
            discipline = new Discipline
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name")
            };
        }

        reader.Close();
        CloseConnection();
        return discipline;
    }



    public void DeleteDiscipline(int id)
    {
        OpenConnection();
        string query = "DELETE FROM disciplines WHERE id = @id";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        CloseConnection();
    }
    public void AddGroup(string name)
    {
        OpenConnection();
        string query = "INSERT INTO study_groups (name) VALUES (@name)";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.ExecuteNonQuery();
        CloseConnection();
    }

    public List<StudyGroup> GetGroups()
    {
        List<StudyGroup> groups = new List<StudyGroup>();
        OpenConnection();

        string query = "SELECT id, name FROM study_groups";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            groups.Add(new StudyGroup
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name")
            });
        }

        reader.Close();
        CloseConnection();
        return groups;
    }
    public List<Student> GetStudentsByGroup(int groupId)
    {
        OpenConnection();
        List<Student> students = new List<Student>();
        string query = "SELECT * FROM students WHERE group_id = @groupId";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@groupId", groupId);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = reader.GetInt32("id"),
                FirstName = reader.GetString("firstname"),
                LastName = reader.GetString("lastname"),
                GroupId = reader.GetInt32("group_id")
            });
        }
        reader.Close();
        CloseConnection();
        return students;
    }
    public List<Teacher> GetTeachersByDiscipline(int disciplineId)
    {
        OpenConnection();
        List<Teacher> teachers = new List<Teacher>();

        string query = @"
        SELECT t.id, t.firstname, t.lastname 
        FROM teachers t
        JOIN teacher_discipline td ON t.id = td.teacher_id
        WHERE td.discipline_id = @disciplineId";

        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@disciplineId", disciplineId);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            teachers.Add(new Teacher
            {
                Id = reader.GetInt32("id"),
                FirstName = reader.GetString("firstname"),
                LastName = reader.GetString("lastname")
            });
        }

        reader.Close();
        CloseConnection();
        return teachers;
    }



    public void DeleteGroup(int id)
    {
        OpenConnection();
        string query = "DELETE FROM study_groups WHERE id = @id";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        CloseConnection();
    }
    public void AssignDisciplineToTeacher(int teacherId, int disciplineId)
    {
        OpenConnection();
        string query = "INSERT INTO teacher_discipline (teacher_id, discipline_id) VALUES (@teacherId, @disciplineId)";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@teacherId", teacherId);
        cmd.Parameters.AddWithValue("@disciplineId", disciplineId);
        cmd.ExecuteNonQuery();
        CloseConnection();
    }


}

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; }
}

public class Teacher
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisciplineName { get; set; } 
}



public class Discipline
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class StudyGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
}
