using TrelloToolsBeanInterface;

namespace TrelloToolsBean
{
    public class FileResponse : IFileResponse
    {
        private string id;
        private int bytes;

        public string Id { get => this.id; set => this.id = value; }

        public int Bytes { get => this.bytes; set => this.bytes = value; }
    }
}
