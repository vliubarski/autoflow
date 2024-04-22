using AutoFlow.DAL;
using AutoFlow.Models;
using AutoFlow.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;

namespace AutoFlowTests
{
    public class ContactServiceTests
    {
        IContactService _contactService;
        Mock<IDbSimulator> _dbSimulatorMoq;

        [SetUp]
        public void Setup()
        {
            _dbSimulatorMoq = new Mock<IDbSimulator>();
            _contactService = new ContactService(_dbSimulatorMoq.Object);
        }

        [Test]
        public void When_AddContact_Called_Then_dbSimulator_Called_With_Correct_Values()
        {
            // Arrange
            string firstName = "firstName", lastName = "lastName", email = "email", phoneNumber = "1234567890", address = "test address";
            _dbSimulatorMoq.Setup(x => x.AddContact(It.IsAny<ContactModelDb>())).Returns(new ContactModelDb());

            // Act
            var result = _contactService.AddContact(firstName, lastName, email, phoneNumber, address);

            // Assert
            Assert.IsTrue(result);

            _dbSimulatorMoq.Verify(x => x.AddContact(It.Is<ContactModelDb>(
                         c => c.FirstName == "firstName" &&
                              c.LastName == "lastName" &&
                              c.PhoneNumber == "1234567890" &&
                              c.EmailAddress == "email" &&
                              c.Address == "test address")), Times.Once);
        }

        [Test]
        public void When_EditContact_Called_And_Db_Returnes_Null_Then__Returned_False()
        {
            // Arrange
            var newContact = new ContactModelUi
            {
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "9876543210",
                EmailAddress = "first@mail.com",
                Address = "test address"
            };

            // Act
            var result = _contactService.EditContacts(newContact);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void When_EditContact_Called_Then_Db_Adds_Same_Values()
        {
            // Arrange
            var newContact = new ContactModelUi
            {
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "9876543210",
                EmailAddress = "first@mail.com",
                Address = "test address"
            };
            _dbSimulatorMoq.Setup(x => x.GetContactById(It.IsAny<Guid>())).Returns(new ContactModelDb());

            // Act
            var result = _contactService.EditContacts(newContact);

            // Assert
            Assert.IsTrue(result);

            _dbSimulatorMoq.Verify(x => x.EditContact(It.Is<ContactModelDb>(
                         c => c.FirstName == "first" &&
                              c.LastName == "last" &&
                              c.PhoneNumber == "9876543210" &&
                              c.EmailAddress == "first@mail.com" &&
                              c.Address == "test address")), Times.Once);
        }

        [Test]
        public void When_DeleteContact_Called_Then_Db_Deletes_Same_Values()
        {
            // Arrange
            var id = Guid.NewGuid();
            var contact = new ContactModelDb
            {
                Id = id,
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "9876543210",
                EmailAddress = "first@mail.com",
                Address = "test address"
            };
            _dbSimulatorMoq.Setup(x => x.GetContactById(It.Is<Guid>(x => x == id))).Returns(contact);

            // Act
            var result = _contactService.DeleteContactById(id);

            // Assert
            _dbSimulatorMoq.Verify(x => x.DeleteContact(It.Is<ContactModelDb>(
                         c => c.FirstName == "first" &&
                              c.LastName == "last" &&
                              c.PhoneNumber == "9876543210" &&
                              c.EmailAddress == "first@mail.com" &&
                              c.Address == "test address")), Times.Once);
        }

        [Test]
        public void When_ListContact_Called_Then_Values_From_Db_Returned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var contact = new ContactModelDb
            {
                Id = id,
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "9876543210",
                EmailAddress = "first@mail.com",
                Address = "test address"
            };
            _dbSimulatorMoq.Setup(x => x.ListContacts()).Returns([contact]);

            // Act
            var result = _contactService.ListContacts();

            // Assert
            _dbSimulatorMoq.Verify(x => x.ListContacts(), Times.Once);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("first", result.ElementAt(0).FirstName);
            Assert.AreEqual("last", result.ElementAt(0).LastName);
            Assert.AreEqual("9876543210", result.ElementAt(0).PhoneNumber);
            Assert.AreEqual("first@mail.com", result.ElementAt(0).EmailAddress);
            Assert.AreEqual("test address", result.ElementAt(0).Address);
        }
    }
}