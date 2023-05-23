namespace TrelloToolsBean
{
    public static class SingletonMail
    {
        private static dynamic mail = null;
        private static readonly object padlock = new object();

        public static dynamic Mail
        {
            get {
                lock(padlock) 
                {
                    if(mail == null)
                    {
                        mail = new System.Dynamic.ExpandoObject();
                    }
                    return mail;
                }
            }
        }
    }
}
