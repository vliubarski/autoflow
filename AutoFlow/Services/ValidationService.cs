using AutoFlow.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace AutoFlow.Services;

public class ValidationService : IValidationService
{
    string UkPhoneNumberPattern = @"^((\+44)|(0)) ?\d{4} ?\d{6}$";
    string EmailPattern = @"^[\w -\.]+@([\w-]+\.)+[\w-]{2,3}$";

    string StringValidationError = "Field can't be empty";
    string EmailError = "Email is in incorrect format or empty";
    string PhoneNumberError = "Phone number is in incorrect format or empty";
    string FilePathError = "Filel path is incorrect";

    public ValidationResult IsNotEmptyString(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new ValidationResult { Success = false, ErrorMsg = StringValidationError };
        }
        return new ValidationResult { Success = true };
    }

    public ValidationResult IsValidEmailAddress(string emailAddress)
    {
        return ValidateAgainstPattern(emailAddress, EmailPattern, EmailError);
    }

    public ValidationResult IsValidFilePath(string path)
    {
        try
        {
            string fullPath = Path.GetFullPath(path);
            bool isRooted = Path.IsPathRooted(path);
            bool isValid = !string.IsNullOrEmpty(fullPath) &&
                           isRooted &&
                           fullPath.StartsWith(Path.GetPathRoot(fullPath));

            return isValid ? new ValidationResult { Success = true } : throw new Exception("Invalid file path");
        }
        catch (Exception)
        {
            return new ValidationResult { Success = false, ErrorMsg = FilePathError };
        }
    }

    public ValidationResult IsValidPhoneNumber(string phoneNumber)
    {
        return ValidateAgainstPattern(phoneNumber, UkPhoneNumberPattern, PhoneNumberError);
    }

    ValidationResult ValidateAgainstPattern(string field, string regexValue, string errorMEssage)
    {
        Regex regex = new(regexValue);

        if (field != null && regex.IsMatch(field))
        {
            return new ValidationResult { Success = true };
        }
        return new ValidationResult { Success = false, ErrorMsg = errorMEssage };
    }
}