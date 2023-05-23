using Microsoft.Win32;
using Moq;
using System.Drawing;
using TrelloToolsBean;
using TrelloToolsLogic;
using TrelloToolsLogicInterfaces;

namespace TrelloToolsTest
{
    public class Test_Utilities
    {
        [SetUp] 
        public void Setup() 
        { 
        }

        [Test]
        [Description("Test when configuration file exists into user storage")]
        public void Test_CheckConfFileExists_Success()
        {
            string credentialsLine = "key: TRELLO_KEY \r\n token: TRELLO_TOKEN";

            Mock<IFileWrapper> fileWrapperMock = new Mock<IFileWrapper>();
            fileWrapperMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            fileWrapperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(credentialsLine);
            Utilities utilities = new Utilities(fileWrapperMock.Object, null);
            Assert.That(utilities.CheckConfFileExists(), Is.EqualTo(true));
        }

        [Test]
        [Description("Test when configuration file doesn't exist into user storage")]
        public void Test_CheckConfFileExists_FileNotFound()
        {
            Mock<IFileWrapper> fileWrapperMock = new Mock<IFileWrapper>();
            fileWrapperMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            Utilities utilities = new Utilities(fileWrapperMock.Object, null);
            Assert.That(utilities.CheckConfFileExists(), Is.EqualTo(false));
        }

        [Test]
        [Description("Test to retrieve credentials from configuration file correctly")]
        public void Test_RetrieveCredentials_Success()
        {
            string credentialsLine = "key: TRELLO_KEY \r\n token: TRELLO_TOKEN";
            Dictionary<string, string> expectedCredentials = new Dictionary<string, string>() { { "key", "TRELLO_KEY" }, { "token", "TRELLO_TOKEN" } };

            Mock<IFileWrapper> fileWrapperMock = new Mock<IFileWrapper>();
            fileWrapperMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            fileWrapperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(credentialsLine);
            Utilities utilities = new Utilities(fileWrapperMock.Object, null);
            Assert.That(utilities.RetrieveCredentials(), Is.EqualTo(expectedCredentials));
        }

        [Test]
        [Description("Test for retrieve credentials, but configuration file doesn't exist")]
        public void Test_RetrieveCredentials_FileNotFound()
        {
            Mock<IFileWrapper> fileWrapperMock = new Mock<IFileWrapper>();
            fileWrapperMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            Utilities utilities = new Utilities(fileWrapperMock.Object, null);
            Assert.Throws<System.Exception>(() => utilities.RetrieveCredentials());
        }

        [Test]
        [Description("Test for check if user have Outlook with dark theme")]
        public void Test_IsDarkTheme_Success()
        {
            string[] outlookVersion = new string[] { "15.0", "16.0", "test" };

            Mock<IRegistryKey> registryKeyMock = new Mock<IRegistryKey>();
            registryKeyMock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(registryKeyMock.Object);
            registryKeyMock.Setup(x => x.GetSubKeyNames()).Returns(outlookVersion);
            // registryKeyMock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(registryKeyMock.Object);
            registryKeyMock.Setup(x => x.GetValue(It.IsAny<string>())).Returns(4);
            Utilities utilities = new Utilities(null, registryKeyMock.Object);
            Assert.That(utilities.IsDarkTheme(), Is.EqualTo(true));
        }
        
        [Test]
        [Description("Test when there aren't subkey concerns 'Microsoft Office' registry key")]
        public void Test_IsDarkTheme_SubKeyNotFound()
        {
            string[] outlookVersion = new string[] { "test", "test" };

            Mock<IRegistryKey> registryKeyMock = new Mock<IRegistryKey>();
            registryKeyMock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(registryKeyMock.Object);
            registryKeyMock.Setup(x => x.GetSubKeyNames()).Returns(outlookVersion);
            // registryKeyMock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(registryKeyMock.Object);
            registryKeyMock.Setup(x => x.GetValue(It.IsAny<string>())).Returns(4);
            Utilities utilities = new Utilities(null, registryKeyMock.Object);
            Assert.Throws<CustomException>(() => utilities.IsDarkTheme());
        }

        [Test]
        [Description("Test when Microsoft Office application didn't install into computer")]
        public void Test_IsDarkTheme_RegistryKeyNotFound()
        {
            Mock<IRegistryKey> registryKeyMock = new Mock<IRegistryKey>();
            registryKeyMock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(registryKeyMock.Object);
            registryKeyMock.Setup(x => x.GetSubKeyNames()).Throws(new NullReferenceException());
            Utilities utilities = new Utilities(null, registryKeyMock.Object);
            Assert.Throws<CustomException>(() => utilities.IsDarkTheme());
        }

        [Test]
        [Description("Test for normalize a string")]
        public void Test_NormalizeString_Success()
        {
            string subjectMail = "OMG!?! It's my first test for this case. Hope are doing well :)";
            string expectNormalizedString = "OMG Its my first test for this case Hope are doing well ";

            Utilities utilities = new Utilities(null, null);
            Assert.That(utilities.NormalizeString(subjectMail), Is.EqualTo(expectNormalizedString));
        }

        [Test]
        [Description("Test for concatenate credentials correctly")]
        public void Test_ConcatenateCredentialsWithUrl_Success()
        {
            string uri = "https://google.com";
            string key = "YOUR_KEY";
            string token = "YOUR_TOKEN";
            string expectedFullUri = String.Concat(uri, "?", "key=", key, "&", "token=", token);

            Utilities utilities = new Utilities(null, null);
            Assert.That(utilities.ConcatenateCredentialsWithUrl(uri, key, token), Is.EqualTo(expectedFullUri));
        }
    }
}
