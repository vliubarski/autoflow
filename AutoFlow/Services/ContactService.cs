using AutoFlow.DAL;
using AutoFlow.Mapping;
using AutoFlow.Models;
using AutoMapper;

namespace AutoFlow.Services;

public class ContactService : IContactService
{
    readonly IMapper _mapper;
    private readonly IDbSimulator _dbSimulator;

    public ContactService(IDbSimulator dbSimulator)
    {
        _dbSimulator = dbSimulator;
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    public bool AddContact(string firstName, string lastName, string email, string phoneNumber, string address)
    {
        ContactModelUi contactModelUi = new ContactModelUi
        {
            FirstName =firstName,
            LastName = lastName,
            EmailAddress =email,
            PhoneNumber = phoneNumber,
            Address = address
        };
        var contactModelDb = _mapper.Map<ContactModelDb>(contactModelUi);

        return _dbSimulator.AddContact(contactModelDb) != null;
    }

    public bool DeleteContactById(Guid id)
    {
        var contactModelDb = _dbSimulator.GetContactById(id);
        if (contactModelDb == null)
        {
            return false;
        }

        return _dbSimulator.DeleteContact(contactModelDb);
    }

    public IEnumerable<ContactModelUi> ListContacts()
    {
        var contactsDb = _dbSimulator.ListContacts();
        var contactsUi = _mapper.Map<IEnumerable<ContactModelUi>>(contactsDb);
        return contactsUi;
    }

    public bool EditContacts(ContactModelUi contactModelUi)
    {
        var contactModelDb = _dbSimulator.GetContactById(contactModelUi.Id);
        if (contactModelDb == null)
        {
            return false;
        }

        var contactModelDbNew = _mapper.Map<ContactModelDb>(contactModelUi);
        _dbSimulator.EditContact(contactModelDbNew);
        return true;
    }
}
