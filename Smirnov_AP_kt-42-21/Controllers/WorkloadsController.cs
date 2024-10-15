using Smirnov_AP_kt_42_21.Interfaces.WorkloadInterfaces;
using Smirnov_AP_kt_42_21.Models;
using Smirnov_AP_kt_42_21.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        [HttpPost("GetWorkloadByProfessor")]
        public async Task<IActionResult> GetWorkloadAsync(string lastName, string firstName, string middleName)
        {
            var professor = await _dbContext.Set<Professor>()
        .FirstOrDefaultAsync(p =>
            p.LastName.Contains(lastName) ||
            p.FirstName.Contains(firstName) ||
            p.MiddleName.Contains(middleName));
            if (professor == null)
            {
                return NotFound("Профессор с таким именем не найден.");
            }
            // Получаем нагрузки для найденного профессора
            var workloads = await _dbContext.Workloads
                .Where(w => w.ProfessorId == professor.Id)
                .Select(w => new
                {
                    DisciplineName = w.EducationalSubject.Name,
                    NumberOfHours = w.NumberOfHours
                })
                .ToListAsync();
            return Ok(new { Professor = professor, Workloads = workloads });
        }

        [HttpPost("GetWorkloadByEducationSubject")]
        public async Task<IActionResult> GetWorcloadByEducationSubject(string educationSubject)
        {
            var edSub = await _dbContext.Set<EducationalSubject>()
                .FirstOrDefaultAsync(e =>
                e.Name.Contains(educationSubject));
            if (edSub == null)
            {
                return NotFound("Нагрузки с такой дисциплиной нет.");
            }
            var workloads = await _dbContext.Workloads
            .Include(w => w.Professor) // Загружаем информацию о профессоре
            .Include(w => w.EducationalSubject) // Загружаем информацию о дисциплине
            .Where(w => w.EducationalSubject.Name.Contains(educationSubject))
            .Select(w => new
            {
                ProfessorName = $"{w.Professor.FirstName} {w.Professor.LastName}",
                DisciplineName = w.EducationalSubject.Name,
                NumberOfHours = w.NumberOfHours
            })
            .ToListAsync();
            return Ok(new { EducationSubject = edSub, Workloads = workloads });
        }
        /*[HttpPost("GetWorkloadByEducationSubjectForId")]
        public async Task<IActionResult> GetWorcloadByEducationSubjectForId(int educationSubjectId)
        {
            var edSub = await _dbContext.Set<EducationalSubject>()
         .FirstOrDefaultAsync(e => e.Id == educationSubjectId);
            if (edSub == null)
            {
                return NotFound("Нагрузки с такой дисциплиной нет.");
            }
            var workloads = await _dbContext.Workloads
            .Include(w => w.Professor) // Загружаем информацию о профессоре
            .Include(w => w.EducationalSubject) // Загружаем информацию о дисциплине
            .Where(w => w.EducationalSubject.Id == educationSubjectId)
            .Select(w => new
            {
                ProfessorName = $"{w.Professor.FirstName} {w.Professor.LastName}",
                DisciplineName = w.EducationalSubject.Name,
                NumberOfHours = w.NumberOfHours
            })
            .ToListAsync();
            return Ok(new { EducationSubject = edSub, Workloads = workloads });
        }*/

        [HttpPost("AddProfessor")]
        public IActionResult CreateProfessor([FromBody] ProfessorFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var professor = new Professor();
            professor.LastName = filter.LastName;
            professor.FirstName = filter.FirstName;
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
            existingProfessor.LastName = updateProfessor.LastName;
            existingProfessor.FirstName = updateProfessor.FirstName;
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
            existingWorkload.ProfessorId = updateWorkload.professor_id;
            existingWorkload.EducationalSubjectId = updateWorkload.EducationalSubjectId;
            existingWorkload.NumberOfHours = updateWorkload.numberofhours;
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}