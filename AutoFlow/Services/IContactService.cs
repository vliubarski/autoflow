using AutoFlow.Models;

namespace AutoFlow.Services
{
    public interface IContactService
    {
        bool AddContact(string firstName, string lastName, string email, string phoneNumber, string address);
        bool DeleteContactById(Guid id);
        bool EditContacts(ContactModelUi contactModelUi);
        IEnumerable<ContactModelUi> ListContacts();
    }
}