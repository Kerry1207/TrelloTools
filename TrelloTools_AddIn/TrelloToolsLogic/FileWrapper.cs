using System;
using System.Collections.Generic;
using System.IO;
using TrelloToolsLogicInterfaces;

namespace TrelloToolsLogic
{
    public class FileWrapper : IFileWrapper
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public FileStream Create(string path)
        {
            return File.Create(path);
        }

        public void WriteAllText(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
