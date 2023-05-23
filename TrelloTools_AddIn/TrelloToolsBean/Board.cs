using TrelloToolsBeanInterface;

namespace TrelloToolsBean
{
    public class Board : IBoard
    {
        private string id;
        private string name;

        public Board(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public string Id { get => this.id; set => this.id = value; }

        public string Name { get => this.name; set => this.name = value; }
    }
}
