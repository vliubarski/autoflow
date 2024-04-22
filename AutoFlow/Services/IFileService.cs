using AutoFlow.Models;

namespace AutoFlow.Services
{
    public interface IFileService
    {
        void ExportContactsToCsv(string filePath, List<ContactModelUi> contacts);
    }
}