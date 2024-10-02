namespace Smirnov_AP_kt_42_21.Models
{
    public class Workload
    {
        public int Id { get; set; }
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }
        public int EducationalSubjectId { get; set; }
        public EducationalSubject EducationalSubject { get; set; }
        public int NumberOfHours { get; set; }
    }
}
