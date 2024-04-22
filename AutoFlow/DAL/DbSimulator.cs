using AutoFlow.Models;
using Microsoft.Extensions.Logging;

namespace AutoFlow.DAL;

/// <summary>
/// In the real world DAL will also have 
/// Concurrency Handling
/// Transaction Support
/// Async data access process
/// which are omitted here
/// </summary>


public class DbSimulator : IDbSimulator
{
    List<ContactModelDb> _contacts = new();
    private readonly ILogger<DbSimulator> _logger;

    public DbSimulator(ILogger<DbSimulator> logger)
    {
        _logger = logger;
        _contacts.AddRange(
            [
                new() {Id = Guid.NewGuid(), FirstName = "Mark", LastName = "Huse",Address = "address 1" ,EmailAddress = "email1@mail.com", PhoneNumber = "01234567890"},
                new() {Id = Guid.NewGuid(), FirstName = "Hue", LastName = "Blue", Address = "address 2" ,EmailAddress = "email2@mail.com", PhoneNumber = "01234567891"}
            ]);
    }

    public bool EditContact(ContactModelDb contactDb)
    {
        try
        {
            var contactToUpdate = _contacts.FirstOrDefault(c => c.Id == contactDb.Id) ?? throw new Exception("no contact found");

            contactToUpdate.FirstName = contactDb.FirstName;
            contactToUpdate.LastName = contactDb.LastName;
            contactToUpdate.PhoneNumber = contactDb.PhoneNumber;
            contactToUpdate.EmailAddress = contactDb.EmailAddress;
            contactToUpdate.Address = contactDb.Address;
            return true;
        }
        catch(Exception e)
        {
            _logger.LogError($"Can't edit contact with id {contactDb.Id}, error: {e.Message}");
            return false;
        }
    }

    public ContactModelDb AddContact(ContactModelDb contactDb)
    {
        try
        {
            if (contactDb == null)
            {
                _logger.LogError("Contact cannot be null.");
                return null!;
            }

            contactDb.Id = Guid.NewGuid();
            _contacts.Add(contactDb);
            return contactDb;

        }
        catch (Exception e)
        {
            _logger.LogError($"Can't add contact, error: {e.Message}");
            return null!;
        }
    }

    public ContactModelDb? GetContactById(Guid id)
    {
        try
        {
            return _contacts.FirstOrDefault(x => x.Id == id);
        }
        catch (Exception e)
        {
            _logger.LogError($"Can't get contact with id {id}, error: {e.Message}");
            return null;
        }
    }


    public IEnumerable<ContactModelDb> ListContacts()
    {
        return _contacts ?? new();
    }

    public bool DeleteContact(ContactModelDb contact)
    {
        try
        {
            return _contacts.Remove(contact);
        }
        catch (Exception e)
        {
            _logger.LogError($"Can't delete record with id {contact.Id}, error: {e.Message}");
            return false;
        }
    }
}
