using Microsoft.EntityFrameworkCore;
using Smirnov_AP_kt_42_21.Database;
using Smirnov_AP_kt_42_21.Filters;
using Smirnov_AP_kt_42_21.Interfaces.WorkloadInterfaces;
using Smirnov_AP_kt_42_21.Models;

namespace SmirnovKr_42_21.Tests
{
    public class WorkloadForProfessorTests
    {
        public readonly DbContextOptions<SmirnovDbContext> _dbContextOptions;
        public WorkloadForProfessorTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SmirnovDbContext>()
            .UseInMemoryDatabase(databaseName: "project_practice")
            .Options;
        }
        [Fact]
        public async Task AddWorkloadsAsync_workloadObject_Test()
        {
            // Arrange
            var ctx = new SmirnovDbContext(_dbContextOptions);
            var workloadService = new WorkloadService(ctx);
            var professors = new List<Professor>
            {
                new Professor
                {
                    LastName = "Матлаш",
                    FirstName = "Илья",
                    MiddleName = "Федорович"
                },
                new Professor
                {
                    LastName = "Смирнов",
                    FirstName = "Александр",
                    MiddleName = "Павлович"
                },
            };
            await ctx.Set<Professor>().AddRangeAsync(professors);
            var educationalsubjects = new List<EducationalSubject>
            {
                new EducationalSubject
                {
                    Name = "Программирование"
                },
                new EducationalSubject
                {
                    Name = "Алгебра и геометрия"
                },
            };
            await ctx.Set<EducationalSubject>().AddRangeAsync(educationalsubjects);
            var workloads = new List<Workload>
            {
                new Workload
                {
                    ProfessorId = 1,
                    EducationalSubjectId = 1,
                    NumberOfHours = 10
                },
                new Workload
                {
                    ProfessorId = 1,
                    EducationalSubjectId = 2,
                    NumberOfHours = 20
                },
                new Workload
                {
                    ProfessorId = 2,
                    EducationalSubjectId = 2,
                    NumberOfHours = 30
                }
            };
            await ctx.Set<Workload>().AddRangeAsync(workloads);
            await ctx.SaveChangesAsync();

            var filterByProfessorName = new WorkloadFilterProfessor
            {
                LastName = "Матлаш"
            };

            var workloadResultByProfessorName = await workloadService.GetWorkloadsByProfessorNameAsync(filterByProfessorName, CancellationToken.None);
            // Assert
            Assert.Equal(2, workloadResultByProfessorName.Length);


            var filterByEducationSubject = new WorkloadFilterEdSub
            {
                Name = "Программирование"
            };

            var workloadResultByEducationSubject = await workloadService.GetWorkloadByEducationSubject(filterByEducationSubject, CancellationToken.None);

            Assert.Equal(1, workloadResultByEducationSubject.Length);


            var filterByNumberOfHours = new NumberOfHourseFilter
            {
                minHours = 9,
                maxHours = 21
            };

            var workloadResultByNumberOfHours = await workloadService.GetWorkloadByEducationSubjectForNumOfHourse(filterByNumberOfHours, CancellationToken.None);

            Assert.Equal(2, workloadResultByNumberOfHours.Length);
        }
    }
}
