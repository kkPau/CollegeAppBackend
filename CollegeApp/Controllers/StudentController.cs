using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;

    public StudentController(ILogger<StudentController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("All", Name = "GetAllStudents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<StudentDTO>> GetStudents(){
        _logger.LogInformation("Get students method started");

        // var students = new List<StudentDTO>();
        // foreach(var student in CollegeRepository.Students){
        //     students.Add(new StudentDTO{
        //         StudentName = student.StudentName,
        //         Email = student.Email,
        //         Address = student.Address
        //     });
        // }

        //============== OR ================

        // Use LINQ to filter and map the data
        var students = CollegeRepository.Students.Select(student => new StudentDTO{
            StudentName = student.StudentName,
            Email = student.Email,
            Address = student.Address
        });

        // OK - 200 - Success
        return Ok(students);
    }

    [HttpGet]
    [Route("{id:int}", Name = "GetStudentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDTO> GetStudentById(int id){
        // Bad Request - 400 - Invalid request
        if(id <= 0) {
            _logger.LogWarning("Bad Request");
            return BadRequest("Invalid Student ID");
        }
        
        var student = CollegeRepository.Students.Where(student => student.Id == id).FirstOrDefault();

        // Not Found - 404 - Student not found
        if(student is null) {
            _logger.LogWarning($"Student with ID {id} not found");
            return NotFound($"Student with ID {id} not found");
        }

        var studentDTO = new StudentDTO(){
            StudentName = student.StudentName,
            Email = student.Email,
            Address = student.Address
        };

        // OK - 200 - Success
        return Ok(studentDTO);
    }

    [HttpGet("{name:alpha}", Name = "GetStudentByName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDTO> GetStudentByName(string name){
        // Bad Request - 400 - Invalid request
        if(string.IsNullOrEmpty(name)) return BadRequest("Invalid Student ID");
        
        var student = CollegeRepository.Students.Where(student => student.StudentName == name).FirstOrDefault();

        // Not Found - 404 - Student not found
        if(student is null) return NotFound($"Student with name {name} not found");

        var studentDTO = new StudentDTO(){
            StudentName = student.StudentName,
            Email = student.Email,
            Address = student.Address
        };

        // OK - 200 - Success
        return Ok(studentDTO);
    }

    [HttpPost]
    [Route("Create", Name = "CreateStudent")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model){
        if(model == null) return BadRequest();

        int newId = CollegeRepository.Students.Count() + 1;

        Student student = new Student{
            Id = newId,
            StudentName = model.StudentName,
            Email = model.Email,
            Address = model.Address
        };

        CollegeRepository.Students.Add(student);

        var studentDTO = new StudentDTO{
            StudentName = student.StudentName,
            Email = student.Email,
            Address = student.Address
        };

        // Created - 201 - Created at https://localhost:5215/api/Student/3
        return CreatedAtRoute("GetStudentById", new {id = student.Id}, studentDTO);
    }

    [HttpPut]
    [Route("Update/{id:int}", Name = "UpdateStudentById")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDTO> UpdateStudent(int id, [FromBody] StudentDTO model){
        if(model == null || id <= 0) return BadRequest();

        var existingStudent = CollegeRepository.Students.Where(student => student.Id == id).FirstOrDefault();

        if (existingStudent is null) return NotFound();

        existingStudent.StudentName = model.StudentName;
        existingStudent.Email = model.Email;
        existingStudent.Address = model.Address;

        // No Content - 204
        return NoContent();
    }

    [HttpPatch]
    [Route("UpdatePartial/{id:int}", Name = "UpdateStudentPartialById")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDTO> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument){
        if(patchDocument == null || id <= 0) return BadRequest();

        var existingStudent = CollegeRepository.Students.Where(student => student.Id == id).FirstOrDefault();

        if (existingStudent is null) return NotFound();

        var studentDTO = new StudentDTO(){
            StudentName = existingStudent.StudentName,
            Email = existingStudent.Email,
            Address = existingStudent.Address
        };

        patchDocument.ApplyTo(studentDTO, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        existingStudent.StudentName = studentDTO.StudentName;
        existingStudent.Email = studentDTO.Email;
        existingStudent.Address = studentDTO.Address;

        // No Content - 204
        return NoContent();
    }

    [HttpDelete]
    [Route("{id}", Name = "DeleteStudentById")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeleteStudent(int id){
        // Bad Request - 400 - Invalid request
        if(id <= 0) return BadRequest("Invalid Student ID");
        
        var student = CollegeRepository.Students.Where(student => student.Id == id).FirstOrDefault();

        // Not Found - 404 - Student not found
        if(student is null) return NotFound($"Student with ID {id} not found");

        CollegeRepository.Students.Remove(student);

        // OK - 200 - Success
        return NoContent();
        
    }
}
