namespace CollegeApp.Models;

public class CollegeRepository
{
    public static List<Student> Students{get; set;} = new List<Student>(){ 
            new Student{
                Id = 1,
                StudentName = "John Doe",
                Email = "johndoe@example.com",
                Address = "123 Main St"
            },
            new Student{
                Id = 2,
                StudentName = "Jane Doe",
                Email = "janedoe@example.com",
                Address = "456 Main St"
            }
        };
}
