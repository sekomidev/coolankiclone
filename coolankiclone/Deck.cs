namespace AnkiClone
{
	public class Deck
	{
		public string Name { get; set; }
		public List<Card> Cards = new List<Card>();

		public Deck()
		{
			Name = String.Empty;
		}

		public Deck(string name)
		{
			Name = name;
		}

		public void AddCard(Card card)
		{
			Cards.Add(card);
			Database.SaveData();
		}

		public void DeleteCard(Card card)
		{
			Cards.Remove(card);
			Database.SaveData();
		}

		public Card? FindCardByFrontMessage(string front)
		{
			return Cards.Find(c => c.FrontText == front);
		}
	}
}