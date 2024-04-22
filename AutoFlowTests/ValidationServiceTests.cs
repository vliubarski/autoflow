using AutoFlow.Services;

namespace AutoFlowTests
{
    public class ValidationServiceTests
    {
        ValidationService _validationService = new ValidationService();

        [TestCase("", false, "Field can't be empty")]
        [TestCase(null, false, "Field can't be empty")]
        [TestCase("123", true, "")]
        public void IsNotEmptyString_Works_As_Expected( string? testString, bool validated, string error)
        {
            // Act
            var result = _validationService.IsNotEmptyString(testString!);

            // Assert
            Assert.AreEqual(validated, result.Success);
            Assert.AreEqual(error, result.ErrorMsg);
        }

        [TestCase("", false, "Email is in incorrect format or empty")]
        [TestCase(null, false, "Email is in incorrect format or empty")]
        [TestCase("123", false, "Email is in incorrect format or empty")]
        [TestCase("123@sdf", false, "Email is in incorrect format or empty")]
        [TestCase("123@sdf.", false, "Email is in incorrect format or empty")]
        [TestCase("123@sdf.v", false, "Email is in incorrect format or empty")]
        [TestCase("123@sdf.sdff", false, "Email is in incorrect format or empty")]
        [TestCase("123@sdf.sdf", true, "")]
        public void IsValidEmailAddress_Works_As_Expected(string? testEmail, bool validated, string error)
        {
            // Act
            var result = _validationService.IsValidEmailAddress(testEmail!);

            // Assert
            Assert.AreEqual(validated, result.Success);
            Assert.AreEqual(error, result.ErrorMsg);
        }

        [TestCase("", false, "Phone number is in incorrect format or empty")]
        [TestCase(null, false, "Phone number is in incorrect format or empty")]
        [TestCase("12345678901", false, "Phone number is in incorrect format or empty")]
        [TestCase("0234567890", false, "Phone number is in incorrect format or empty")]
        [TestCase("01234567890", true, "")]
        [TestCase("01707333666", true, "")]
        [TestCase("+441707333666", true, "")]
        public void IsValidPhoneNumber_Works_As_Expected(string? testEmail, bool validated, string error)
        {
            // Act
            var result = _validationService.IsValidPhoneNumber(testEmail!);

            // Assert
            Assert.AreEqual(validated, result.Success);
            Assert.AreEqual(error, result.ErrorMsg);
        }
    }
}