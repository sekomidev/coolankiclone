using DataManagement;
namespace AnkiClone
{
	public static class Database
	{
		private readonly static string DECKS_SAVE_PATH = 
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnkiClone\decks";

		public static List<Deck> Decks { get; } = LoadDecksFromDisk();

		public static List<Card> CardsToReview
		{
			get
			{
				// find all cards from all decks which are needed to be reviewed
				var a = from d in Decks
						from c in d.Cards
						where DateTime.Now >= c.NextReviewDate
						orderby c.NextReviewDate
						select c;

				return a.ToList();
			}
		}

		private static List<Deck> LoadDecksFromDisk()
		{
			var decks = new List<Deck>();
			foreach (var d in Directory.GetFiles(DECKS_SAVE_PATH))
			{
				decks.Add(DataManager.Load<Deck>(d));
			}
			return decks;
		}

		public static void SaveData()
		{
			foreach(var d in Decks)
			{
				DataManager.Save($"{DECKS_SAVE_PATH}\\{d.Name}.xml", d);
			}
		}

		public static void AddDeck(Deck deck)
		{
			Decks.Add(deck);
			SaveData();
		}

		public static void DeleteDeck(Deck deck)
		{
			Decks.Remove(deck);
			File.Delete($"{DECKS_SAVE_PATH}\\{deck.Name}.xml");
			SaveData();
		}

		public static Deck? FindDeckByName(string name)
		{
			return Decks.Find(x => x.Name == name);
		}
	}
}