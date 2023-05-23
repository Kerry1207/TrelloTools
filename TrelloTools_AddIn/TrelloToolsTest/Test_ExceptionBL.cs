using TrelloToolsLogic;

namespace TrelloToolsTest
{
    public class Test_ExceptionBL
    {
        private ExceptionBL exceptionBL;
        
        [SetUp]
        public void Setup()
        {
            exceptionBL = new ExceptionBL();
        }

        [Test]
        [Description("Test for retrieve message correctly having its code")]
        public void Test_GetMessage_Success()
        {
            string key = "01";
            string expectErrorMessage = "Error into startup operation about add-in.";

            Assert.That(exceptionBL.GetMessage(key), Is.EqualTo(expectErrorMessage));
        }

        [Test]
        [Description("Test for retrieve message when code didn't map")]
        public void Test_GetMessage_KeyNotDefined()
        {
            string key = "99";

            Assert.Throws<KeyNotFoundException>(() => exceptionBL.GetMessage(key));
        }

    }
}
