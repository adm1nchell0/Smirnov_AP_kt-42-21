using Smirnov_AP_kt_42_21.Database;
using Smirnov_AP_kt_42_21.Filters;
using Smirnov_AP_kt_42_21.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Smirnov_AP_kt_42_21.Interfaces.WorkloadInterfaces
{
    public interface IWorkloadService
    {
        public Task<Workload[]> GetWorkloadsByProfessorNameAsync(string firstName, string lastName, string middleName, CancellationToken cancellationToken);
    }
    public class WorkloadService : IWorkloadService
    {
        //private readonly SmirnovDbContext _dbContext;
        private SmirnovDbContext _dbContext;
        public WorkloadService(SmirnovDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Workload[]> GetWorkloadsByProfessorNameAsync(string firstName, string lastName, string middleName, CancellationToken cancellationToken = default)
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
        }
    }
}