using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Logic.Wrappers.Implementations;
using StudentManager.Service.Models.Subjects;
using StudentManager.Service.Models.Users;

namespace StudentManager.Service.Controllers;

[Route("student")]
[ApiController]
public class StudentController : ExtendedMappingController
{
    private readonly StudentsTableWrapper _students;
    private readonly SubjectsTableWrapper _subjects;

    public StudentController(IMapper mapper,
        StudentsTableWrapper students,
        SubjectsTableWrapper subjects)
        : base(mapper)
    {
        _students = students;
        _subjects = subjects;
    }

    [HttpGet("{telegramId}")]
    public async Task<ActionResult<StudentDto>> GetUserByTelegramId(string telegramId)
    {
        var user = await _students.ReadByTelegramId(telegramId);
        if (user.IsFailed)
            return NotFound();

        return Mapper.Map<StudentDto>(user.Value);
    }

    [HttpGet("{telegramId}/subjects")]
    public async Task<ActionResult<SubjectInfoDto[]>> GetSubjectsNamesOfUserByTelegramId(string telegramId)
    {
        var user = await _students.ReadByTelegramId(telegramId);
        if (user.IsFailed)
            return NotFound();

        var subjects = await _subjects.ReadByGroupId(user.Value.IdGroup);
        if (subjects.IsFailed)
            return NotFound();

        return Mapper.Map<SubjectInfoDto[]>(subjects.Value);
    }
}