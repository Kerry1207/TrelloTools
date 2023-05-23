namespace TrelloToolsBeanInterface
{
    public interface ICard
    {
        string Id { get; set; }

        string Name { get; set; }

        string IdBoard { get; set; }
    }
}
