using System.Diagnostics;

namespace AnkiClone
{
	public static class UserInterface
	{
		// main menu of the application
		public static void MainMenu()
		{
			Console.Clear();
			Console.WriteLine("AnkiClone v0.6\n");
			Console.WriteLine("1. Review cards");
			Console.WriteLine("2. Manage decks");

			var key = Console.ReadKey(true).Key;
			switch(key)
			{
				case ConsoleKey.D1:
					if (Database.CardsToReview.Any())
					{
						ReviewCards(Database.CardsToReview);
					}
					else
					{
						Console.Clear();
						Console.WriteLine("No cards to review now, please wait for a bit!");
						Console.ReadKey();
					}
					MainMenu();
					break;
				case ConsoleKey.D2:
					ManageDecksMenu();
					break;
			}
		}

		public static void ReviewCards(List<Card> cardsToReview)
		{
			var cardIndex = 1;
			foreach (var card in cardsToReview)
			{
				Console.Clear();
				Console.WriteLine($"Reviewing card {cardIndex}/{cardsToReview.Count}\n");
				Console.WriteLine($"{card.FrontText}");
				Console.ReadKey(true);

				// show the back side of the card
				Console.WriteLine($"{card.BackText}\n\n");
				Console.WriteLine("1 - Again | 2 - Hard | 3 - Good | 4 - Easy");
				var key = Console.ReadKey(true).Key;
				switch(key)
				{
					case ConsoleKey.D1:
						card.DifficultyMultiplier *= 2.7;
						card.NextReviewDate = DateTime.Now;
						break;
					case ConsoleKey.D2:
						card.DifficultyMultiplier *= 1.5;
						break;
					case ConsoleKey.D3:
						card.DifficultyMultiplier *= 0.9;
						break;
					case ConsoleKey.D4:
						card.DifficultyMultiplier *= 0.6;
						break;
				}

				// only change the next review date if the user doesn't choose the "again" option
				if (key != ConsoleKey.D1)
				{
					card.NextReviewDate = DateTime.Now.AddDays(900 / card.DifficultyMultiplier);
				}

				Database.SaveData();
				cardIndex++;
			}
		}

		// list, select, create and delete decks
		public static void ManageDecksMenu()
		{
			Console.Clear();
			Console.WriteLine("List of decks:\n");

			foreach(var d in Database.Decks)
			{
				if (d.Cards.Count == 1)
				{
					Console.Write($"{d.Name}: {d.Cards.Count} card. ");
				}
				else
				{
					Console.Write($"{d.Name}: {d.Cards.Count} cards. ");
				}
				if (d.Cards.Any())
				{
					Console.WriteLine($"Average difficulty: {d.Cards.Average(x => x.DifficultyMultiplier):F0}");
				}
			}

			Console.WriteLine("\nM to modify, C to create, D to delete, E to return to main menu:");
			switch(Console.ReadKey(true).Key)
			{
				case ConsoleKey.M:
					var selectedDeck = SelectDeck();
					if (selectedDeck is not null)
					{
						DeckMenu(selectedDeck);
					}
					else
					{
						ManageDecksMenu();
					}
					break;
				case ConsoleKey.C:
					CreateDeckMenu();
					break;
				case ConsoleKey.D:
					DeleteDeckMenu();
					break;
				case ConsoleKey.E:
					MainMenu();
					break;
			}
		}

		public static Deck SelectDeck()
		{
			Console.Write("\nEnter the name of a deck you want to select: ");
			var deckToSelect = Database.FindDeckByName(Console.ReadLine());
			if (deckToSelect is null)
			{
				Console.WriteLine("Deck with that name does not exist.");
				Console.ReadKey();
				return null;
			}
			else
			{
				return deckToSelect;
			}
		}

		public static void DeleteDeckMenu()
		{
			Console.Write("\nEnter the name of a deck you want to delete: ");
			var deckToDelete = Database.FindDeckByName(Console.ReadLine());
			if (deckToDelete is null)
			{
				Console.WriteLine("Deck with that name does not exist.");
				Console.ReadKey();
				ManageDecksMenu();
			}
			else
			{
				Database.DeleteDeck(deckToDelete);
				ManageDecksMenu();
			}
		}

		public static void CreateDeckMenu()
		{
			Console.Write("\nEnter the name of a deck you want to create: ");
			var name = Console.ReadLine();
			var newDeck = DeckFactory.GetDeck(name);

			if (newDeck is null)
			{
				Console.WriteLine("The deck could not be created.");
				Console.ReadKey();
				ManageDecksMenu();
			}
			else
			{
				Database.AddDeck(newDeck);
				ManageDecksMenu();
			}
		}

		public static void DeckMenu(Deck deck)
		{
			Console.Clear();
			ListCardsInDeck(deck);
			Console.WriteLine("\nC to create, D to delete cards, E to return to main menu:");
			switch (Console.ReadKey(true).Key)
			{
				case ConsoleKey.C:
					CreateCardMenu(deck);
					break;
				case ConsoleKey.D:
					DeleteCardMenu(deck);
					break;
				case ConsoleKey.E:
					MainMenu();
					break;
			}
		}

		public static void ListCardsInDeck(Deck deck)
		{
			Console.WriteLine($"List of cards in deck '{deck.Name}':\n");

			foreach (var c in deck.Cards)
			{
				Console.WriteLine($"front: {c.FrontText}; back: {c.BackText};" +
					$" created at {c.LastReviewDate.ToString("yyyy-MM-dd HH:mm")};" +
					$" difficulty: {c.DifficultyMultiplier}");
			}
		}

		public static void CreateCardMenu(Deck deck)
		{
			Console.Clear();
			Console.WriteLine("Card Creation Menu\n");

			Console.Write("Enter what will be on the front side: ");
			var front = Console.ReadLine();
			Console.Write("Enter what will be on the back side: ");
			var back = Console.ReadLine();

			var card = CardFactory.GetCard(front, back);
			if (card is null)
			{
				Console.WriteLine("The card could not be created.");
			}
			else
			{
				deck.AddCard(card);
				DeckMenu(deck);
			}
		}

		public static void DeleteCardMenu(Deck deck)
		{
			Console.Write("\nDelete the card by it's front message: ");
			var front = Console.ReadLine();
			var cardToDelete = deck.FindCardByFrontMessage(front);
			if (cardToDelete is null)
			{
				Console.WriteLine("Card with that name does not exist.");
				Console.ReadKey();
				DeckMenu(deck);
			}
			else
			{
				deck.DeleteCard(cardToDelete);
				DeckMenu(deck);
			}
		}
	}
}