using Moq;
using TrelloToolsBean;
using TrelloToolsLogic;
using TrelloToolsLogicInterfaces;

namespace TrelloToolsTest
{
    public class Test_ConfigurationBL
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Description("Test when user inserted API credentials into add-in and it's created correctly the configuration file")]
        public void Test_CreateConfigurationFile_Success()
        {
            string key = "YOUR_TRELLO_KEY";
            string trelloToken = "YOUR_TRELLO_TOKEN";

            Mock<FileStream> fileStreamMock = new Mock<FileStream>("path", FileMode.Create);
            Mock<IFileWrapper> fileWrapperMock = new Mock<IFileWrapper>();
            fileWrapperMock.Setup(x => x.Create(It.IsAny<string>())).Returns(fileStreamMock.Object);
            fileWrapperMock.Setup(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));
            ConfigurationBL configurationBL= new ConfigurationBL(fileWrapperMock.Object);
            configurationBL.ConnectTrelloAccount(key, trelloToken);
            fileWrapperMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once());
            fileWrapperMock.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        [Description("Test when user inserted API credentials into add-in, but this file was opened also into another process")]
        public void Test_CreateConfigurationFile_ErrorFileInUse()
        {
            string key = "YOUR_TRELLO_KEY";
            string trelloToken = "YOUR_TRELLO_TOKEN";

            Mock<IFileWrapper> fileWrapperMock = new Mock<IFileWrapper>();
            fileWrapperMock.Setup(x => x.Create(It.IsAny<string>())).Throws(new IOException());
            ConfigurationBL configurationBL = new ConfigurationBL(fileWrapperMock.Object);
            Assert.Throws<CustomException>(() => configurationBL.ConnectTrelloAccount(key, trelloToken));
        }

        [Test]
        [Description("Test when user disconnected his API credentials from add-in, and consequently it's deleted the file from file system")]
        public void Test_DeleteConfigurationFile_Sucess()
        {
            Mock<IFileWrapper> fileWrapperMock = new Mock<IFileWrapper>();
            fileWrapperMock.Setup(x => x.Delete(It.IsAny<string>()));
            ConfigurationBL configurationBL = new ConfigurationBL(fileWrapperMock.Object);
            configurationBL.DisconnectTrelloAccount();
            fileWrapperMock.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
        }

        [Test]
        [Description("Test when user inserted API credentials into add-in, but this file was opened also into another process")]
        public void Test_DeleteConfigurationFile_ErrorFileInUse()
        {
            Mock<IFileWrapper> fileWrapperMock = new Mock<IFileWrapper>();
            fileWrapperMock.Setup(x => x.Delete(It.IsAny<string>())).Throws(new IOException());
            ConfigurationBL configurationBL = new ConfigurationBL(fileWrapperMock.Object);
            Assert.Throws<CustomException>(() => configurationBL.DisconnectTrelloAccount());
        }
    }
}