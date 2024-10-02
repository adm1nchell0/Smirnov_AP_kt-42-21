using Smirnov_AP_kt_42_21.Database;
using Smirnov_AP_kt_42_21.Filters;
using Smirnov_AP_kt_42_21.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Smirnov_AP_kt_42_21.Interfaces.WorkloadInterfaces
{
    public interface IWorkloadService
    {
        public Task<Workload[]> GetWorkloadAsync(WorkloadFilter filter, CancellationToken cancellationToken);
    }
    public class WorkloadService : IWorkloadService
    {
        //private readonly SmirnovDbContext _dbContext;
        private SmirnovDbContext _dbContext;
        public WorkloadService(SmirnovDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Workload[]> GetWorkloadAsync(WorkloadFilter filter, CancellationToken cancellationToken = default)
        {
            //Заменять w.ProfessorId и filter.professor_id на необходимые
            var workloads = _dbContext.Set<Workload>().Where(w => w.ProfessorId == filter.professor_id).ToArrayAsync(cancellationToken);
            return workloads;
        }
    }
}