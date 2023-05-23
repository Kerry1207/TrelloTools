using Moq;
using System.Net;
using TrelloToolsAccessInterfaces;
using TrelloToolsBean;
using TrelloToolsBeanInterface;
using TrelloToolsLogic;
using TrelloToolsLogicInterfaces;
using View = TrelloToolsBean.View;

namespace TrelloToolsTest
{
    public class Test_FunctionalitiesBL
    {
        private dynamic mail;
        private Mock<IWebRequestAC> webRequestMock;
        private Mock<IFileWrapper> fWrapperMock;
        private Mock<IUtilities> utilitiesMock;
        private byte[] fileContent;

        [SetUp]
        public void Setup()
        {
            webRequestMock = new Mock<IWebRequestAC>();
            fWrapperMock = new Mock<IFileWrapper>();
            utilitiesMock = new Mock<IUtilities>();
            fileContent = new byte[1024];
            mail = new System.Dynamic.ExpandoObject();
            mail.Details = new System.Dynamic.ExpandoObject();
            mail.Details.ConversationTopic = "Test";
        }

        [Test]
        [Description("Test when email's attaching to card correctly")]
        public void Test_AttachEmailToCard_Success()
        {
            string cardId = "01abcdef";
            string response = "{ \"id\": \"test\", \"bytes\": 1024 }";

            webRequestMock.Setup(x => x.SendRequestWithFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(Task.FromResult(response));
            fWrapperMock.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Returns(fileContent);
            utilitiesMock.Setup(x => x.NormalizeString(It.IsAny<string>())).Returns("Test");
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "testKey" }, { "token", "testToken" } });
            utilitiesMock.Setup(x => x.GetOrCreateEmailSingletonInstance()).Returns(mail);
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, fWrapperMock.Object, utilitiesMock.Object, null, null);
            Assert.That(functionalitiesBL.AttachEmail(cardId).GetAwaiter().GetResult(), Is.EqualTo(true));
        }

        [Test]
        [Description("Test when email didn't attach to card because of incorrect API credentials")]
        public void Test_AttachEmail_IncorrectAPICredentials()
        {
            string cardId = "01abcdef";

            webRequestMock.Setup(x => x.SendRequestWithFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new WebException("The remote server returned an error: (401) Unauthorized."));
            fWrapperMock.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Returns(fileContent);
            utilitiesMock.Setup(x => x.NormalizeString(It.IsAny<string>())).Returns("Test");
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "testKey" }, { "token", "not_valid" } });
            utilitiesMock.Setup(x => x.GetOrCreateEmailSingletonInstance()).Returns(mail);
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, fWrapperMock.Object, utilitiesMock.Object, null, null);
            Assert.ThrowsAsync<CustomException>(() => functionalitiesBL.AttachEmail(cardId));
        }

        [Test]
        [Description("Test when user want to attach email to card, but the message wasn't found into path")]
        public void Test_AttachEmail_EmailNotFound()
        {
            string cardId = "01abcdef";
            string response = "{ \"id\": \"test\", \"bytes\": 1024 }";

            webRequestMock.Setup(x => x.SendRequestWithFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(Task.FromResult(response));
            fWrapperMock.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Throws(new IOException());
            utilitiesMock.Setup(x => x.NormalizeString(It.IsAny<string>())).Returns("Test");
            utilitiesMock.Setup(x => x.GetOrCreateEmailSingletonInstance()).Returns(mail);
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, fWrapperMock.Object, utilitiesMock.Object, null, null);
            Assert.ThrowsAsync<CustomException>(() => functionalitiesBL.AttachEmail(cardId));
        }

        [Test]
        [Description("Test when user would attach email to card, but add-in can't to retrieve metadata about current email opened")]
        public void Test_AttachEmail_MetadataUnavailable()
        {
            string cardId = "01abcdef";
            string response = "{ \"id\": \"test\", \"bytes\": 1024 }";

            webRequestMock.Setup(x => x.SendRequestWithFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(Task.FromResult(response));
            fWrapperMock.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Throws(new IOException());
            utilitiesMock.Setup(x => x.NormalizeString(It.IsAny<string>())).Returns("Test");
            utilitiesMock.Setup(x => x.GetOrCreateEmailSingletonInstance()).Returns(null);
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, fWrapperMock.Object, utilitiesMock.Object, null, null);
            Assert.ThrowsAsync<CustomException>(() => functionalitiesBL.AttachEmail(cardId));
        }

        [Test]
        [Description("Test when user would attach email to card, but user have deleted configuration file")]
        public void Test_AttachEmail_ConfigFileCancellation()
        {
            string cardId = "01abcdef";
            string response = "{ \"id\": \"test\", \"bytes\": 1024 }";

            webRequestMock.Setup(x => x.SendRequestWithFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(Task.FromResult(response));
            fWrapperMock.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Returns(fileContent);
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Throws(new System.Exception());
            utilitiesMock.Setup(x => x.GetOrCreateEmailSingletonInstance()).Returns(mail);
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, fWrapperMock.Object, utilitiesMock.Object, null, null);
            Assert.ThrowsAsync<CustomException>(() => functionalitiesBL.AttachEmail(cardId));
        }

        [Test]
        [Description("Test when the card was correctly added to Trello without attach email")]
        public void Test_AddCardToBoard_SuccessWithoutAttachEmail()
        {
            string idList = "firstCard";
            string name = "My First Card";
            string body = "Hello, this is my first card wrote with TrelloTools Add-in!";
            bool isAttachMail = false;
            List<IView> views = new List<IView>() { new View("1ababab2", "firstCard"), new View("2adadad3", "secondCard") };
            string response = "{ \"Id\": \"1abcdef2\", \"Name\": \"My first Card\", \"IdBoard\": \"3abcdef4\" }";

            webRequestMock.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(Task.FromResult(response));
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "testKey" }, { "token", "testToken" } });
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, null, utilitiesMock.Object, views, null);
            Assert.That(functionalitiesBL.AddCardToBoard(idList, name, body, isAttachMail), Is.EqualTo(true));
        }

        [Test]
        [Description("Test when the card was correctly added to Trello with attach email")]
        public void Test_AddCardToBoard_SuccessWithAttachEmail()
        {
            string idList = "firstCard";
            string name = "My First Card";
            string body = "Hello, this is my first card wrote with TrelloTools Add-in!";
            bool isAttachMail = true;
            List<IView> views = new List<IView>() { new View("1ababab2", "firstCard"), new View("2adadad3", "secondCard") };
            string response = "{ \"id\": \"1abcdef2\", \"name\": \"My first Card\", \"IdBoard\": \"3abcdef4\" }";
            string fileResponse = "{ \"id\": \"test\", \"bytes\": 1024 }";

            webRequestMock.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(Task.FromResult(response));
            webRequestMock.Setup(x => x.SendRequestWithFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(Task.FromResult(fileResponse));
            fWrapperMock.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Returns(fileContent);
            utilitiesMock.Setup(x => x.NormalizeString(It.IsAny<string>())).Returns("Test");
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "testKey" }, { "token", "testToken" } });
            utilitiesMock.Setup(x => x.GetOrCreateEmailSingletonInstance()).Returns(mail);
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, fWrapperMock.Object, utilitiesMock.Object, views, null);
            Assert.That(functionalitiesBL.AddCardToBoard(idList, name, body, isAttachMail), Is.EqualTo(true));
        }

        [Test]
        [Description("Test when user would to add a card into specific board, but Trello API credentials aren't correct")]
        public void Test_AddCardToBoard_IncorrectAPICredentials()
        {
            string idList = "firstCard";
            string name = "My First Card";
            string body = "Hello, this is my first card wrote with TrelloTools Add-in!";
            bool isAttachMail = false;
            List<IView> views = new List<IView>() { new View("1ababab2", "firstCard"), new View("2adadad3", "secondCard") };

            webRequestMock.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ThrowsAsync(new WebException("The remote server returned an error: (401) Unauthorized."));
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "not_valid" }, { "token", "testToken" } });
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, null, utilitiesMock.Object, views, null);
            Assert.Throws<CustomException>(() => functionalitiesBL.AddCardToBoard(idList, name, body, isAttachMail));
        }

        [Test]
        [Description("Test when user would to add a card into specific board but user have deleted configuration file")]
        public void Test_AddCardToBoard_ConfigFileCancellation()
        {
            string idList = "firstCard";
            string name = "My First Card";
            string body = "Hello, this is my first card wrote with TrelloTools Add-in!";
            bool isAttachMail = false;
            List<IView> views = new List<IView>() { new View("1ababab2", "firstCard"), new View("2adadad3", "secondCard") };

            utilitiesMock.Setup(x => x.RetrieveCredentials()).Throws(new System.Exception());
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(null, null, utilitiesMock.Object, null, null);
            Assert.Throws<CustomException>(() => functionalitiesBL.AddCardToBoard(idList, name, body, isAttachMail));
        }

        [Test]
        [Description("Test when user choose board from dropdown and retrieved views names correctly")]
        public void Test_RetrieveView_Success()
        {
            string boardName = "secondBoard";
            List<IBoard> boards = new List<IBoard>() { new Board("1ababab2", "firstBoard"), new Board("2adadad3", "secondBoard") };
            string response = "[ { \"id\": \"1a\", \"name\": \"First View\" }, { \"id\": \"2b\", \"name\": \"Second View\" } ]";
            List<string> expectedViews = new List<string>() { "First View", "Second View"};

            webRequestMock.Setup(x => x.GetRequest(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(response);
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "testKey" }, { "token", "testToken" } });
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, null, utilitiesMock.Object, null, boards);
            CollectionAssert.AreEqual(expectedViews, functionalitiesBL.RetrieveView(boardName));
        }

        [Test]
        [Description("Test when user choose board from dropdown but the API credentials are incorrect")]
        public void Test_RetrieveView_IncorrectAPICredentials()
        {
            string boardName = "secondBoard";
            List<IBoard> boards = new List<IBoard>() { new Board("1ababab2", "firstBoard"), new Board("2adadad3", "secondBoard") };

            webRequestMock.Setup(x => x.GetRequest(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new WebException("The remote server returned an error: (401) Unauthorized."));
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "not_valid" }, { "token", "testToken" } });
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, null, utilitiesMock.Object, null, boards);
            Assert.Throws<CustomException>(() => functionalitiesBL.RetrieveView(boardName));
        }

        [Test]
        [Description("Test when user choose board for adding card, but user have deleted configuration file")]
        public void Test_RetrieveView_ConfigFileCancellation()
        {
            string boardName = "secondBoard";
            List<IBoard> boards = new List<IBoard>() { new Board("1ababab2", "firstBoard"), new Board("2adadad3", "secondBoard") };

            utilitiesMock.Setup(x => x.RetrieveCredentials()).Throws(new System.Exception());
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(null, null, utilitiesMock.Object, null, boards);
            Assert.Throws<CustomException>(() => functionalitiesBL.RetrieveView(boardName));
        }

        [Test]
        [Description("Test when user click 'Add Card' button and the application retrieve board names correctly")]
        public void Test_RetrieveBoard_Correctly()
        {
            string response = "[ { \"id\": \"1a\", \"name\": \"First Board\" }, { \"id\": \"2b\", \"name\": \"Second Board\" } ]";
            List<string> expectedBoards = new List<string>() { "First Board", "Second Board" };

            webRequestMock.Setup(x => x.GetRequest(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(response);
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "testKey" }, { "token", "testToken" } });
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, null, utilitiesMock.Object, null, null);
            CollectionAssert.AreEqual(expectedBoards, functionalitiesBL.RetrieveBoards());
        }

        [Test]
        [Description("Test when user click 'Add Card' button and the application try to retrieve board names but API credentials are incorrect")]
        public void Test_RetrieveBoard_IncorrectAPICredentials()
        {
            webRequestMock.Setup(x => x.GetRequest(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new WebException("The remote server returned an error: (401) Unauthorized."));
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Returns(new Dictionary<string, string>() { { "key", "not_valid" }, { "token", "testToken" } });
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(webRequestMock.Object, null, utilitiesMock.Object, null, null);
            Assert.Throws<CustomException>(() => functionalitiesBL.RetrieveBoards());
        }

        [Test]
        [Description("Test when user click 'Add Card' button and the application try to retrieve board names, but user have deleted configuration file")]
        public void Test_RetrieveBoard_ConfigFileCancellation()
        {
            utilitiesMock.Setup(x => x.RetrieveCredentials()).Throws(new System.Exception());
            FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL(null, null, utilitiesMock.Object, null, null);
            Assert.Throws<CustomException>(() => functionalitiesBL.RetrieveBoards());
        }
    }
}