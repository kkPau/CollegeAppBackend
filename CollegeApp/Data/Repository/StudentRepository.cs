using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository;

public class StudentRepository : IStudentRepository
{
    private readonly CollegeDbContext _dbContext;
    
    public StudentRepository(CollegeDbContext dbContext){
        _dbContext = dbContext;
    }

    public async Task<int> CreateStudent(Student student)
    {
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();
        return student.Id;
    }

    public async Task<bool> DeleteStudent(Student student)
    {
        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<List<Student>> GetAllStudents()
    {
        return await _dbContext.Students.ToListAsync();
    }

    public async Task<Student> GetStudentById(int id, bool useNoTracking = false)
    {
        if (useNoTracking)
            return await _dbContext.Students.AsNoTracking().Where(student => student.Id == id).FirstOrDefaultAsync();
        else
        return await _dbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Student> GetStudentByStudentName(string name)
    {
        return await _dbContext.Students.Where(student => student.StudentName.Contains(name)).FirstOrDefaultAsync();
    }

    public async Task<int> UpdateStudent(Student student)
    {
        _dbContext.Students.Update(student);
        
        await _dbContext.SaveChangesAsync();

         return student.Id;
    }
}
