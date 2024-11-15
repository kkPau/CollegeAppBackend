namespace CollegeApp.Data.Repository;

public interface IStudentRepository
{
    Task<List<Student>> GetAllStudents();

    Task<Student> GetStudentById(int id, bool useNoTracking = false);

    Task<Student> GetStudentByStudentName(string name);

    Task<int> CreateStudent(Student student);

    Task<int> UpdateStudent(Student student);
    
    Task<bool> DeleteStudent(Student student);
}
