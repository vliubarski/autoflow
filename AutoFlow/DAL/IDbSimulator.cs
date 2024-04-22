using AutoFlow.Models;

namespace AutoFlow.DAL;

public interface IDbSimulator
{
    ContactModelDb AddContact(ContactModelDb contactDb);
    bool DeleteContact(ContactModelDb contact);
    ContactModelDb? GetContactById(Guid id);
    IEnumerable<ContactModelDb> ListContacts();
    bool EditContact(ContactModelDb contactDb);
}