using Smirnov_AP_kt_42_21.Interfaces.WorkloadInterfaces;
using Smirnov_AP_kt_42_21.Models;
using Smirnov_AP_kt_42_21.Filters;
using Microsoft.AspNetCore.Mvc;
using Smirnov_AP_kt_42_21.Database;
using Microsoft.EntityFrameworkCore;

namespace Smirnov_AP_kt_42_21.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkloadsController : ControllerBase
    {
        private readonly ILogger<WorkloadsController> _logger;
        private readonly IWorkloadService _workloadService;
        private readonly SmirnovDbContext _dbContext;
        public WorkloadsController(SmirnovDbContext dbContext, ILogger<WorkloadsController> logger, IWorkloadService workloadService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _workloadService = workloadService;
        }
        [HttpPost(Name = "Filter")]
        public async Task<IActionResult> GetWorkloadAsync(WorkloadFilter filter, CancellationToken cancellationToken = default)
        {
            var resp = await _workloadService.GetWorkloadAsync(filter, cancellationToken);
            return Ok(resp);
        }

        [HttpPost("AddProfessor")]
        public IActionResult CreateProfessor([FromBody] ProfessorFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var professor = new Professor();
            professor.Id = filter.Id;
            professor.FirstName = filter.LastName;
            professor.LastName = filter.FirstName;
            professor.MiddleName = filter.MiddleName;
            _dbContext.Professors.Add(professor);
            _dbContext.SaveChanges();
            return Ok(professor);
        }
        [HttpPost("AddEducationSubject")]
        public IActionResult CreateEducationSubject([FromBody] EducationSubjectFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var educationsubject = new EducationalSubject();
            educationsubject.Id = filter.Id;
            educationsubject.Name = filter.Name;
            _dbContext.EducationalSubjects.Add(educationsubject);
            _dbContext.SaveChanges();
            return Ok(educationsubject);
        }
        [HttpPost("AddWorkload")]
        public IActionResult CreateWorkload([FromBody] WorkloadEducationalSubjectFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var workload = new Workload();
            workload.Id = filter.Id;
            workload.ProfessorId = filter.professor_id;
            workload.EducationalSubjectId = filter.EducationalSubjectId;
            workload.NumberOfHours = filter.numberofhours;
            _dbContext.Workloads.Add(workload);
            _dbContext.SaveChanges();
            return Ok(workload);
        }
        [HttpDelete("DeleteProfessor")]
        public IActionResult DeleteProfessor(int id)
        {
            var existingProfessor = _dbContext.Professors.FirstOrDefault(g => g.Id == id);
            if (existingProfessor == null)
            {
                return NotFound("Такого профессора не существует.");
            }
            _dbContext.Professors.Remove(existingProfessor);
            _dbContext.SaveChanges();
            return Ok("Профессор удалён.");
        }
        [HttpDelete("DeleteEducationSubject")]
        public IActionResult DeleteEducationSubject(int id)
        {
            var existingEducationSubject = _dbContext.EducationalSubjects.FirstOrDefault(g => g.Id == id);
            if (existingEducationSubject == null)
            {
                return NotFound("Такой дисциплины не существует.");
            }
            _dbContext.EducationalSubjects.Remove(existingEducationSubject);
            _dbContext.SaveChanges();
            return Ok("Дисциплина удалена.");
        }
        [HttpDelete("DeleteWorkload")]
        public IActionResult DeleteWorkload(int id)
        {
            var existingWorkload = _dbContext.Workloads.FirstOrDefault(g => g.Id == id);
            if (existingWorkload == null)
            {
                return NotFound("Такой нагрузки не существует");
            }
            _dbContext.Workloads.Remove(existingWorkload);
            _dbContext.SaveChanges();
            return Ok("Нагрузка удалена.");
        }
        [HttpPut("EditProfessor")]
        public IActionResult UpdateProfessor(int id, [FromBody] ProfessorFilter updateProfessor)
        {
            var existingProfessor = _dbContext.Professors.FirstOrDefault(g => g.Id == id);
            if (existingProfessor == null)
            {
                return NotFound("Профессор не найден.");
            }
            existingProfessor.Id = updateProfessor.Id;
            existingProfessor.FirstName = updateProfessor.FirstName;
            existingProfessor.LastName = updateProfessor.LastName;
            existingProfessor.MiddleName = updateProfessor.MiddleName;
            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpPut("EditEducationSubject")]
        public IActionResult UpdateEducationSubject(int id, [FromBody] EducationSubjectFilter updateEducationSubject)
        {
            var existingEducationSubject = _dbContext.EducationalSubjects.FirstOrDefault(g => g.Id == id);
            if (existingEducationSubject == null)
            {
                return NotFound("Дисциплина не найдена.");
            }
            existingEducationSubject.Id = updateEducationSubject.Id;
            existingEducationSubject.Name = updateEducationSubject.Name;
            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpPut("EditWorkload")]
        public IActionResult UpdateWorkload(int id, [FromBody] WorkloadEducationalSubjectFilter updateWorkload)
        {
            var existingWorkload = _dbContext.Workloads.FirstOrDefault(g => g.Id == id);
            if (existingWorkload == null)
            {
                return NotFound("Нагрузка не найдена.");
            }
            existingWorkload.Id = updateWorkload.Id;
            existingWorkload.ProfessorId = updateWorkload.professor_id;
            existingWorkload.EducationalSubjectId = updateWorkload.Id;
            existingWorkload.NumberOfHours = updateWorkload.numberofhours;
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}