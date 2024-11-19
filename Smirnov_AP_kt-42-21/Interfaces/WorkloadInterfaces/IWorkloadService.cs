using Smirnov_AP_kt_42_21.Database;
using Smirnov_AP_kt_42_21.Filters;
using Smirnov_AP_kt_42_21.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Smirnov_AP_kt_42_21.Interfaces.WorkloadInterfaces
{
    public interface IWorkloadService
    {
        public Task<Workload> AddWorkloadAsync(Workload workload, CancellationToken cancellationToken);
        public Task<Workload[]> GetWorkloadByEducationSubjectForNumOfHourse(NumberOfHourseFilter filter, CancellationToken cancellationToken = default);
        public Task<Workload[]> GetWorkloadsByProfessorNameAsync(WorkloadFilterProfessor filter, CancellationToken cancellationToken);
        //public Task<Workload[]> GetWorkloadsByProfessorNameAsync(string firstName, string lastName, string middleName, CancellationToken cancellationToken);
    }
    public class WorkloadService : IWorkloadService
    {
        //private readonly SmirnovDbContext _dbContext;
        private SmirnovDbContext _dbContext;
        public WorkloadService(SmirnovDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /*public async Task<Workload[]> GetWorkloadsByProfessorNameAsync(string firstName, string lastName, string middleName, CancellationToken cancellationToken = default)
        {
            //Заменять w.ProfessorId и filter.professor_id на необходимые
            var professor = await _dbContext.Set<Professor>()
                .FirstOrDefaultAsync(p =>
                    p.LastName.Contains(lastName) ||
                    p.FirstName.Contains(firstName) ||
                    p.MiddleName.Contains(middleName), cancellationToken);
            if (professor == null)
            {
                return Array.Empty<Workload>(); // Или выбросьте исключение
            }
            return await _dbContext.Workloads
                .Where(w => w.ProfessorId == professor.Id)
                .ToArrayAsync(cancellationToken);
        }*/
        public Task<Workload[]> GetWorkloadsByProfessorNameAsync(WorkloadFilterProfessor filter, CancellationToken cancellationToken = default)
        {
            var workload = _dbContext.Set<Workload>()
                .Where(p =>
                    (p.Professor.FirstName == filter.FirstName) ||
                    (p.Professor.LastName == filter.LastName) ||
                    (p.Professor.MiddleName == filter.MiddleName)).ToArrayAsync(cancellationToken);

            return workload;
        }

        public async Task<Workload> AddWorkloadAsync(Workload workload, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(workload);

            await _dbContext.SaveChangesAsync();

            return workload;
        }

        public Task<Workload[]> GetWorkloadByEducationSubject(WorkloadFilterEdSub filter, CancellationToken cancellationToken = default)
        {
            var workload = _dbContext.Set<Workload>()
                .Where(e =>
                (e.EducationalSubject.Name == filter.Name)).ToArrayAsync(cancellationToken);

            return workload;
        }

        public Task<Workload[]> GetWorkloadByEducationSubjectForNumOfHourse(NumberOfHourseFilter filter, CancellationToken cancellationToken = default)
        {
            var workload = _dbContext.Set<Workload>()
            .Where(w => (w.NumberOfHours >= filter.minHours) && (w.NumberOfHours <= filter.maxHours))
            .ToArrayAsync(cancellationToken);

            return workload;
        }
    }
}