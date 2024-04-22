using AutoFlow.DAL;
using AutoFlow.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;

namespace AutoFlowTests
{
    public class DbSimulatorTests
    {
        private Mock<ILogger<DbSimulator>> _loggerMock;
        IDbSimulator _dbSimulator;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<DbSimulator>>();
            _dbSimulator = new DbSimulator(_loggerMock.Object);
        }
        [Test]
        public void When_No_Contact_To_Edit_Found_Then_Exception_Thrown()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var contact = new ContactModelDb
            {
                Id = testId,
            };

            // Act
            bool result = _dbSimulator.EditContact(contact);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void When_New_Contact_Add_Called_Then_It_Added_To_Db_Correctly()
        {
            var newContact = new ContactModelDb
            {
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "9876543210",
                EmailAddress = "first@mail.com",
                Address = "test address"
            };
            var newId = _dbSimulator.AddContact(newContact).Id;

            // Act
            var dbContact = _dbSimulator.GetContactById(newId);

            // Assert
            Assert.NotNull(dbContact);

            Assert.AreEqual(newContact.FirstName, dbContact.FirstName);
            Assert.AreEqual(newContact.LastName, dbContact.LastName);
            Assert.AreEqual(newContact.PhoneNumber, dbContact.PhoneNumber);
            Assert.AreEqual(newContact.EmailAddress, dbContact.EmailAddress);
            Assert.AreEqual(newContact.Address, dbContact.Address);
        }

        [Test]
        public void When_Contact_To_Edit_Found_Then_DB_Updated_Correctly()
        {
            var newContact = new ContactModelDb
            {
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "9876543210",
                EmailAddress = "first@mail.com",
                Address = "456 Elm St"
            };
            var newId = _dbSimulator.AddContact(newContact).Id;
            var contact = new ContactModelDb
            {
                Id = newId,
                FirstName = "first2",
                LastName = "last",
                PhoneNumber = "9876543212",
                EmailAddress = "first2@example.com",
                Address = "test address"
            };
            // Act
            bool result = _dbSimulator.EditContact(contact);
            _dbSimulator.EditContact(contact);

            // Assert
            Assert.IsTrue(result);
            var editedContact = _dbSimulator.GetContactById(newId);

            Assert.AreEqual(contact.FirstName, editedContact.FirstName);
            Assert.AreEqual(contact.LastName, editedContact.LastName);
            Assert.AreEqual(contact.PhoneNumber, editedContact.PhoneNumber);
            Assert.AreEqual(contact.EmailAddress, editedContact.EmailAddress);
            Assert.AreEqual(contact.Address, editedContact.Address);
        }

        [Test]
        public void When_Contact_Deleted_Then_DB_Updated_Correctly()
        {
            var newContact = new ContactModelDb
            {
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "9876543210",
                EmailAddress = "first@mail.com",
                Address = "test address"
            };
            var addedContact = _dbSimulator.AddContact(newContact);

            // Act
            bool deleted = _dbSimulator.DeleteContact(addedContact);
            bool deletedTwice = _dbSimulator.DeleteContact(addedContact);

            // Assert
            Assert.IsTrue(deleted);
            Assert.IsFalse(deletedTwice);
        }
    }
}