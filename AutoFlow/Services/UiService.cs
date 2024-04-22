using AutoFlow.Extentions;
using AutoFlow.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using ValidationResult = AutoFlow.Models.ValidationResult;

namespace AutoFlow.Services;

public class UiService : IUiService
{
    Menu userInput = Menu.Undefined;
    readonly IContactService _contactService;
    readonly IValidationService _validationService;
    private readonly IFileService _fileService;
    readonly ILogger<UiService> _logger;
    List<ContactModelUi> _listedContacts = new();
    SortedBy _sortedBy = SortedBy.FirstName;

    public UiService(IContactService contactService, IValidationService validationService,
        IFileService fileService, ILogger<UiService> logger)
    {
        _contactService = contactService;
        _validationService = validationService;
        _fileService = fileService;
        _logger = logger;
    }

    public void Run()
    {
        GetContacts();
        while ((userInput = PrintMenu()) != Menu.Quit)
        {
            switch (userInput)
            {
                case Menu.ListContact: GetContacts(); ShowContacts(_listedContacts); break;
                case Menu.AddContact: AddContacts(); break;
                case Menu.EditContact: EditContact(); break;
                case Menu.DeleteContact: DeleteContact(); break;
                case Menu.SearchByName: SearchByName(); break;
                case Menu.SortByField: SortByField(); break;
                case Menu.ExportToCsvFile: ExportToCsvFile(); break;
                default:
                    Console.WriteLine("Please enter one of listed options."); break;
            }
        }
    }

    private void ExportToCsvFile()
    {
        var filePath = GetValidatedField("Enter the file path to save the CSV file", _validationService.IsValidFilePath);

        //Console.WriteLine("Enter the file path to save the CSV file:");
        //string filePath = Console.ReadLine().Trim();

        //if (string.IsNullOrWhiteSpace(filePath))
        //{
        //    Console.WriteLine("Invalid file path. Export aborted.");
        //    return;
        //}

        // Convert contacts to CSV format
        //string csvContent = ConvertContactsToCsv(filePath, _listedContacts);
        _fileService.ExportContactsToCsv(filePath, _listedContacts);
        //try
        //{
        //    // Write CSV content to file
        //    File.WriteAllText(filePath, csvContent, Encoding.UTF8);
        //    Console.WriteLine($"Contacts exported successfully to: {filePath}");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Error exporting contacts: {ex.Message}");
        //}
    }

    void SortByField()
    {
        Console.WriteLine("Please enter a field to sort by:");
        Console.WriteLine("FirstName    - 1");
        Console.WriteLine("LastName     - 2");
        Console.WriteLine("EmailAddress - 3");
        Console.WriteLine("PhoneNumber  - 4");
        Console.WriteLine("Address      - 5");

        var field = Console.ReadLine();
        if (!int.TryParse(field, out var value))
        {
            _logger.LogError("Incorrect field number");
        }

        _sortedBy = (SortedBy)value;
        ShowContacts(_listedContacts);
    }

    void ShowContacts(List<ContactModelUi> contacts)
    {
        contacts = contacts.OrderBy(GetPropertySelector(_sortedBy)).ToList();
        int index = 1;

        Console.WriteLine($"{"No",-4} {"First Name",-15} {"Last Name",-15} {"Email Address",-30} {"Phone Number",-15} {"Address",-30}");

        Console.WriteLine(new string('-', 105));

        foreach (var contact in contacts)
        {
            Console.WriteLine($"{index++,-4} {contact.FirstName,-15} {contact.LastName,-15} {contact.EmailAddress,-30} {contact.PhoneNumber,-15} {contact.Address,-30}");
        }
    }

    Func<ContactModelUi, object> GetPropertySelector(SortedBy sortedBy)
    {
        var propertySelectors = new Dictionary<SortedBy, Func<ContactModelUi, object>>
        {
            { SortedBy.FirstName, c => c.FirstName },
            { SortedBy.LastName, c => c.LastName },
            { SortedBy.Address, c => c.Address },
            { SortedBy.EmailAddress, c => c.EmailAddress },
            { SortedBy.PhoneNumber, c => c.PhoneNumber }
        };

        if (propertySelectors.TryGetValue(sortedBy, out var selector))
        {
            return selector;
        }
        _logger.LogError("Invalid sorting criteria.");
        return propertySelectors[SortedBy.FirstName];
    }

    void SearchByName()
    {
        Console.WriteLine("Serching by first and/or last name...");
        Console.WriteLine("Please enter a whole or part of a first name or press Enter to skip:");
        var firstName = Console.ReadLine() ?? "";

        Console.WriteLine("Please enter a whole or part of a last name or press Enter to skip:");
        var lastName = Console.ReadLine() ?? "";

        var contacts = _listedContacts.Where(x => x.FirstName.Contains(firstName, StringComparison.CurrentCultureIgnoreCase)
            && x.LastName.Contains(lastName, StringComparison.CurrentCultureIgnoreCase)).ToList();
        ShowContacts(contacts);
    }

    void DeleteContact()
    {
        var id = GetGuidForRecord("Please enter a number of contact to be deleted");
        if (id == Guid.Empty) { return; }
        if (_contactService.DeleteContactById(id))
        {
            Console.WriteLine("Contact deleted");
            return;
        }
        Console.WriteLine("Can't delete this contact");
    }

    Guid GetGuidForRecord(string msg)
    {
        Console.WriteLine(msg);
        var numberStr = Console.ReadLine();
        if (!int.TryParse(numberStr, out int index))
        {
            _logger.LogError("Can't parse intered number");
            return Guid.Empty;
        }
        if (index > _listedContacts.Count || index < 1)
        {
            _logger.LogError("No record with specified number");
            return Guid.Empty;
        }
        return _listedContacts.OrderBy(GetPropertySelector(_sortedBy)).ElementAt(index - 1).Id;
    }

    string GetValidatedField(string message, Func<string, ValidationResult> validator, bool skipIfEmpty = false)
    {
        string field;
        ValidationResult validationResult;
        do
        {
            Console.WriteLine(message);
            field = Console.ReadLine()!;

            if (skipIfEmpty && string.IsNullOrWhiteSpace(field))
            {
                return null!;
            }

            validationResult = validator(field);
            if (!validationResult.Success)
            {
                Console.WriteLine(validationResult.ErrorMsg);
            }

        } while (!validationResult.Success);
        return field!;
    }

    void AddContacts()
    {
        var firstName = GetValidatedField("Please enter first name", _validationService.IsNotEmptyString);
        var lastName = GetValidatedField("Please enter last name", _validationService.IsNotEmptyString);
        var email = GetValidatedField("Please enter email", _validationService.IsValidEmailAddress);
        var phoneNumber = GetValidatedField("Please enter phone number", _validationService.IsValidPhoneNumber);
        var address = GetValidatedField("Please enter address", _validationService.IsNotEmptyString);

        if (_contactService.AddContact(firstName, lastName, email, phoneNumber, address))
        {
            Console.WriteLine("Contact added");
            return;
        }
        Console.WriteLine("Failed to add this Contact");
    }

    void EditContact()
    {
        var id = GetGuidForRecord("Please enter a number of contact to be edited");
        if (id == Guid.Empty) { return; }

        var contact = _listedContacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
            return;

        var firstName = GetValidatedField("Please enter New First Name or press Enter to skip", _validationService.IsNotEmptyString, true) ?? contact.FirstName;
        var lastName = GetValidatedField("Please enter New Last Name or press Enter to skip", _validationService.IsNotEmptyString, true) ?? contact.LastName;
        var email = GetValidatedField("Please enter New Email Address or press Enter to skip", _validationService.IsValidEmailAddress, true) ?? contact.EmailAddress;
        var phoneNumber = GetValidatedField("Please enter New Phone Number or press Enter to skip", _validationService.IsValidPhoneNumber, true) ?? contact.PhoneNumber;
        var address = GetValidatedField("Please enter New Address or press Enter to skip", _validationService.IsNotEmptyString, true) ?? contact.Address;

        contact.FirstName = firstName;
        contact.LastName = lastName;
        contact.EmailAddress = email;
        contact.PhoneNumber = phoneNumber;
        contact.Address = address;

        _contactService.EditContacts(contact);
    }

    void GetContacts()
    {
        _listedContacts = _contactService.ListContacts().ToList();
    }

    Menu PrintMenu()
    {
        Console.WriteLine();

        Console.WriteLine("1 - List contacts");
        Console.WriteLine("2 - Add contacts");
        Console.WriteLine("3 - Delete contacts");
        Console.WriteLine("4 - Edit contacts");
        Console.WriteLine("5 - Search By Name");
        Console.WriteLine("6 - Sort By Field");
        Console.WriteLine("7 - Export to Csv file");
        Console.WriteLine("8 - Quit");

        var userInput = Console.ReadLine();

        var parsedMenu = EnumExtensions.ParseMenu(userInput);
        return parsedMenu;
    }
}
