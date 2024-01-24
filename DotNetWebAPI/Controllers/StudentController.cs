using DotNetWebAPI.Dtos;
using DotNetWebAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace DotNetWebAPI.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private static int _id = 0;
        private static List<Student> _students = new List<Student>();

        [HttpPost()]
        public void AddStudent([FromBody] StudentAddDto input)
        {
            _students.Add(new Student
            {
                Id = _id++,
                Code = input.StudentCode,
                Name = input.StudentName
            });
        }

        [HttpGet()]
        public List<StudentDto> GetAllStudents()
        {
            List<StudentDto> result = new List<StudentDto>();
            _students.ForEach((student) =>
            {
                result.Add(new StudentDto
                {
                    Code = student.Code,
                    Name = student.Name
                });
            });
            return result;
        }

        [HttpGet("{id:min(0)}")]
        public StudentDto GetStudentById([FromRoute] int id)
        {
            Student student = _students.FirstOrDefault((student) => id == student.Id);

            return new StudentDto()
            {
                Code = student.Code,
                Name = student.Name
            };
        }

        [HttpGet("filter")]
        public List<StudentDto> Filter([FromQuery] StudentFilterDto input)
        {
            List<StudentDto> result = new List<StudentDto>();
            List<Student> studentsFiltered = new List<Student>();
            if (!string.IsNullOrEmpty(input.StudentCode) && !string.IsNullOrEmpty(input.StudentName))
            {
                studentsFiltered = _students.Where((student) => input.StudentCode == student.Code && input.StudentName == student.Name).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(input.StudentCode))
                {
                    studentsFiltered = _students.Where((student) => input.StudentCode == student.Code).ToList();
                }
                else
                {
                    studentsFiltered = _students.Where((student) => input.StudentName == student.Name).ToList();
                }
            }
            studentsFiltered.ForEach((student) =>
            {
                result.Add(new StudentDto
                {
                    Code = student.Code,
                    Name = student.Name
                });
            });
            return result;
        }

        [HttpPut("{id:min(0)}")]
        public void EditById([FromRoute] int id, [FromBody] StudentDto student)
        {
            Student _student = _students.FirstOrDefault((student) => id == student.Id);
            _student.Code = student.Code;
            _student.Name = student.Name;
        }

        [HttpDelete("{id:min(0)}")]
        public void DeleteById([FromRoute] int id)
        {
            Student _student = _students.FirstOrDefault((student) => id == student.Id);
            _students.Remove(_student);
        }
    }
}
