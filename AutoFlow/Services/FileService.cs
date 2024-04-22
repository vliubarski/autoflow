using AutoFlow.DAL;
using AutoFlow.Models;
using Microsoft.Extensions.Logging;

namespace AutoFlow.Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(IDbSimulator dbSimulator, ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public void ExportContactsToCsv(string filePath, List<ContactModelUi> contacts)
    {
        try
        {
            DateTime now = DateTime.Now;
            string formattedDateTime = now.ToString("yyyyMMddHHmmss");
            var fileName = DateTime.UtcNow;
            filePath += $"/{formattedDateTime}-contacts.csv";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("FirstName,LastName,EmailAddress,PhoneNumber,Address");

                foreach (var contact in contacts)
                {
                    string firstName = $"{contact.FirstName}";
                    string lastName = $"{contact.LastName}";
                    string emailAddress = $"{contact.EmailAddress}";
                    string phoneNumber = $"{contact.PhoneNumber}";
                    string address = $"{contact.Address}";

                    writer.WriteLine($"{firstName},{lastName},{emailAddress},{phoneNumber},{address}");
                }
            }

            Console.WriteLine($"Contacts exported successfully to: {filePath}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error exporting contacts: {ex.Message}");
        }
    }
}
