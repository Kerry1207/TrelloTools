using System.Collections.Generic;

namespace TrelloToolsBean
{
    public class CustomException : System.Exception
    {
        private string code;

        public CustomException(string code) : base(code) { }

        public string Code { get => this.code; set => this.code = value; }
    }
}
