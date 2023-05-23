using System;

namespace TrelloToolsLogicInterfaces
{
    public interface IRegistryKey : IDisposable
    {
        string[] GetSubKeyNames();

        IRegistryKey OpenSubKey(string key);

        object GetValue(string name);
    }
}
