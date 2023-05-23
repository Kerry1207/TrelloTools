using TrelloToolsBeanInterface;

namespace TrelloToolsBean
{
    public class Card : ICard
    {
        private string id;
        private string name;
        private string idBoard;

        public Card() { }

        public string Id { get => this.id; set => this.id = value; }

        public string Name { get => this.name; set => this.name = value; }

        public string IdBoard { get => this.idBoard; set => this.idBoard = value; }
    }
}
