using AutoFlow.Models;

namespace AutoFlow.Services;

public interface IValidationService
{
    ValidationResult IsValidEmailAddress(string emailAddress);
    ValidationResult IsValidPhoneNumber(string phoneNumber);
    ValidationResult IsNotEmptyString(string name);
    ValidationResult IsValidFilePath(string path);
}