using Smirnov_AP_kt_42_21.Models;

namespace SmirnovKr_42_21.Tests
{
    public class ProfessorTests
    {
        [Fact]
        public void IsValidFirstName_Иван_Test()
        {
            var testProfessor = new Professor
            {
                LastName = "Петров",
                FirstName = "Иван",
                MiddleName = "Михайлович"
            };
            var resultLastName = testProfessor.isValidLastName();
            var resultFirstName = testProfessor.isValidFirstName();
            var resultMiddleName = testProfessor.isValidMiddleName();
            Assert.True(resultLastName);
            Assert.True(resultFirstName);
            Assert.True(resultMiddleName);
        }
    }
}