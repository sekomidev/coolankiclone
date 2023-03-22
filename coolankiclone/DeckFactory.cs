namespace AnkiClone
{
	// kind of useless for now, but i'm planning to use this class
	// to implement name filters and stuff
	public static class DeckFactory
	{
		public static Deck? GetDeck(string name)
		{
			if(name == String.Empty || Database.Decks.Exists(x => x.Name == name))
			{
				return null;
			}
			return new Deck(name);
		}
	}
}