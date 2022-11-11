using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StudentManager.Logic.Wrappers.Implementations;
using StudentManager.Service.Models.Receive;
using StudentManager.Service.Models.Subjects;

namespace StudentManager.Service.Controllers;

[Route("teacher")]
[ApiController]
public class TeachersController : ExtendedMappingController
{
    private readonly HttpClient _client;

    private readonly SubjectsTableWrapper _subjects;
    private readonly TeachersTableWrapper _teachers;
    private readonly PracticeSubgroupsTableWrapper _subgroups;
    private readonly GradesEditorWrapper _gradesEditor;

    public TeachersController(HttpClient client, IMapper mapper,
            SubjectsTableWrapper subjects,
            TeachersTableWrapper teachers,
            PracticeSubgroupsTableWrapper subgroups,
            StudentsTableWrapper students,
            GradesEditorWrapper gradesEditor
    )
        : base(mapper)
    {
        _client = client;
        _client.BaseAddress = new UriBuilder("http", "wiki_service", 80, "wiki").Uri;

        _subjects = subjects;
        _teachers = teachers;
        _subgroups = subgroups;
        _gradesEditor = gradesEditor;
    }

    [HttpGet("{telegramId}/subjects")]
    public async Task<ActionResult<SubjectInfoDto[]>> GetSubjectsOfTeacher(string telegramId)
    {
        var user = await _teachers.ReadByTelegramId(telegramId);
        if (user.IsFailed)
            return CreateFailResult(user.Errors, HttpStatusCode.NotFound);

        var subjects = await _subjects.ReadByTeacherId(user.Value.Id);
        if(subjects.IsFailed)
            return CreateFailResult(subjects.Errors, HttpStatusCode.NotFound);

        var subgroups = await _subgroups.ReadByTeacherId(user.Value.Id);
        if(subgroups.IsFailed)
            return CreateFailResult(subgroups.Errors, HttpStatusCode.NotFound);

        var subjectIds = subjects.Value.Select(x => x.Id).Concat(subgroups.Value.Select(x => x.IdSubject)).Distinct();
        if(subjects.IsFailed)
            return CreateFailResult(subjects.Errors, HttpStatusCode.NotFound);

        var infos = await _subjects.ReadByIds(subjectIds);
        return infos.IsSuccess
            ? Mapper.Map<SubjectInfoDto[]>(infos.Value)
            : CreateFailResult(infos.Errors, HttpStatusCode.NotFound);
    }

    [HttpPost("{telegramId}/subjects/total")]
    public async Task<ActionResult<SubjectInfoDto[]>> GetTotalGradesSubjectsOfTeacher(string telegramId,
        [FromBody] SpreadsheetCreateDto dto)
    {
        var user = await _teachers.ReadByTelegramId(telegramId);
        if (user.IsFailed)
            return CreateFailResult(user.Errors, HttpStatusCode.NotFound);

        var result = await _gradesEditor.WriteToSpreadsheet(dto.LinkToSpreadsheet, await _gradesEditor.ReadAll());
        return CreateResponseByResult(result);
    }

    [HttpPost("subject/{subjectId}/section/{sectionId}/attach/link")]
    public async Task<ActionResult> SendLinkToCSCWiki(string subjectId, string sectionId, 
        [FromBody]TeacherAttachLinkDto info)
    {
        var subjects = await _subjects.ReadById(subjectId);
        if(subjects.IsFailed)
            return CreateFailResult(subjects.Errors, HttpStatusCode.NotFound);

        var link = new Uri(subjects.Value.LinkToCSC).LocalPath;
        var titleLink = link[(link.LastIndexOf('/') + 1)..];
        var result = new { section = sectionId, link = info.Link, tag = info.TagName, title = titleLink };
        try
        {
            var response = await _client.PostAsync("append-link",
                new StringContent(JsonSerializer.Serialize(result), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            Log.Information("Added link to CSC wiki: {Item}", response.ToString());
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Invalid post file of name {Filename} and section {SectionId}", info.TagName, sectionId);
            return new ObjectResult("CSC_TIMEOUT") { StatusCode = (int)HttpStatusCode.BadGateway };
        }
        catch (Exception e)
        {
            Log.Error(e, "Invalid post file of name {Filename} and section {SectionId}", info.TagName, sectionId);
            return BadRequest("REQUEST_INVALID");
        }

        return Ok();
    }
}