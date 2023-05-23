using System.IO;

namespace TrelloToolsLogicInterfaces
{
    public interface IFileWrapper
    {
        byte[] ReadAllBytes(string path);

        void Delete(string path);

        FileStream Create(string path);

        void WriteAllText(string path, string text);

        bool Exists(string path);

        string ReadAllText(string path);
    }
}
