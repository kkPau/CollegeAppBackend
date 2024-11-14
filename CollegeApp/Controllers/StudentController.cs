using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    private readonly CollegeDbContext _dbContext;
    private readonly IMapper _mapper;

    public StudentController(ILogger<StudentController> logger, CollegeDbContext dbContext, IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("All", Name = "GetAllStudents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents(){
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
        // var students =await _dbContext.Students.Select(student => new StudentDTO{
        //     StudentName = student.StudentName,
        //     Email = student.Email,
        //     Address = student.Address,
        //     DOB = student.DOB.ToShortDateString()
        // }).ToListAsync();

        var students = await _dbContext.Students.ToListAsync();

        // Map the Student model to StudentDTO using automapper
        var studentDTOData = _mapper.Map<List<StudentDTO>>(students);

        // OK - 200 - Success
        return Ok(studentDTOData);
    }

    [HttpGet]
    [Route("{id:int}", Name = "GetStudentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> GetStudentById(int id){
        // Bad Request - 400 - Invalid request
        if(id <= 0) {
            _logger.LogWarning("Bad Request");
            return BadRequest("Invalid Student ID");
        }
        
        var student = await _dbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();

        // Not Found - 404 - Student not found
        if(student is null) {
            _logger.LogWarning($"Student with ID {id} not found");
            return NotFound($"Student with ID {id} not found");
        }

        // var studentDTO = new StudentDTO(){
        //     StudentName = student.StudentName,
        //     Email = student.Email,
        //     Address = student.Address,
        //     DOB = student.DOB.ToShortDateString()
        // };

        // Map the Student model to StudentDTO using automapper
        var studentDTO = _mapper.Map<StudentDTO>(student);

        // OK - 200 - Success
        return Ok(studentDTO);
    }

    [HttpGet("{name:alpha}", Name = "GetStudentByName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> GetStudentByName(string name){
        // Bad Request - 400 - Invalid request
        if(string.IsNullOrEmpty(name)) return BadRequest("Invalid Student ID");
        
        var student = await _dbContext.Students.Where(student => student.StudentName.Contains(name)).FirstOrDefaultAsync();

        // Not Found - 404 - Student not found
        if(student is null) return NotFound($"Student with name {name} not found");

        // var studentDTO = new StudentDTO(){
        //     StudentName = student.StudentName,
        //     Email = student.Email,
        //     Address = student.Address,
        //     DOB = student.DOB.ToShortDateString()
        // };

        // Map the Student model to StudentDTO using automapper
        var studentDTO = _mapper.Map<StudentDTO>(student);

        // OK - 200 - Success
        return Ok(studentDTO);
    }

    [HttpPost]
    [Route("Create", Name = "CreateStudent")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO dto){
        if(dto == null) return BadRequest();

        // Student student = new Student{
        //     StudentName = model.StudentName,
        //     Email = model.Email,
        //     Address = model.Address,
        //     DOB = Convert.ToDateTime(model.DOB)
        // };

        // Map the StudentDTO to Student model using automapper
        Student student = _mapper.Map<Student>(dto);

        await _dbContext.Students.AddAsync(student);
        await _dbContext.SaveChangesAsync();

        var studentDTO = new StudentDTO{
            StudentName = student.StudentName,
            Email = student.Email,
            Address = student.Address,
            DOB = student.DOB.ToShortDateString()
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
    public async Task<ActionResult<StudentDTO>> UpdateStudent(int id, [FromBody] StudentDTO dto){
        if(dto == null || id <= 0) return BadRequest();

        var existingStudent = await _dbContext.Students.AsNoTracking().Where(student => student.Id == id).FirstOrDefaultAsync();

        if (existingStudent is null) return NotFound();

        // var newRecord = new Student(){
        //     Id = existingStudent.Id,
        //     StudentName = model.StudentName,
        //     Email = model.Email,
        //     Address = model.Address,
        //     DOB = Convert.ToDateTime(model.DOB)
        // };

        // Map the StudentDTO to Student model using automapper
        var newRecord = _mapper.Map<Student>(dto);

        _dbContext.Students.Update(newRecord);

        // existingStudent.StudentName = model.StudentName;
        // existingStudent.Email = model.Email;
        // existingStudent.Address = model.Address;
        // existingStudent.DOB = Convert.ToDateTime(model.DOB);

        await _dbContext.SaveChangesAsync();

        // No Content - 204
        return NoContent();
    }

    [HttpPatch]
    [Route("UpdatePartial/{id:int}", Name = "UpdateStudentPartialById")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDTO>> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument){
        if(patchDocument == null || id <= 0) return BadRequest();

        var existingStudent = await _dbContext.Students.AsNoTracking().Where(student => student.Id == id).FirstOrDefaultAsync();

        if (existingStudent is null) return NotFound();

        // var studentDTO = new StudentDTO(){
        //     StudentName = existingStudent.StudentName,
        //     Email = existingStudent.Email,
        //     Address = existingStudent.Address
        // };

        // Map the StudentDTO to Student model using automapper
        var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

        patchDocument.ApplyTo(studentDTO, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        // existingStudent.StudentName = studentDTO.StudentName;
        // existingStudent.Email = studentDTO.Email;
        // existingStudent.Address = studentDTO.Address;
        // existingStudent.DOB = Convert.ToDateTime(studentDTO.DOB);

        existingStudent = _mapper.Map<Student>(studentDTO);

        _dbContext.Students.Update(existingStudent);

        await _dbContext.SaveChangesAsync();

        // No Content - 204
        return NoContent();
    }

    [HttpDelete]
    [Route("{id}", Name = "DeleteStudentById")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> DeleteStudent(int id){
        // Bad Request - 400 - Invalid request
        if(id <= 0) return BadRequest("Invalid Student ID");
        
        var student = await _dbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();

        // Not Found - 404 - Student not found
        if(student is null) return NotFound($"Student with ID {id} not found");

        _dbContext.Students.Remove(student);

        await _dbContext.SaveChangesAsync();

        // OK - 200 - Success
        return NoContent();
        
    }
}
