using Microsoft.Win32;
using TrelloToolsLogicInterfaces;

namespace TrelloToolsLogic
{
    public class RegistryKeyWrapper : IRegistryKey
    {
        private RegistryKey registryKey;

        public RegistryKeyWrapper(RegistryKey registryKey)
        {
            this.registryKey = registryKey;
        }

        public void Dispose()
        {
            registryKey.Dispose();
        }

        public string[] GetSubKeyNames()
        {
            return registryKey.GetSubKeyNames();
        }

        public object GetValue(string name)
        {
            return registryKey.GetValue(name);
        }

        public IRegistryKey OpenSubKey(string key)
        {
            var subKey = registryKey.OpenSubKey(key);
            return new RegistryKeyWrapper(subKey);
        }
    }
}
