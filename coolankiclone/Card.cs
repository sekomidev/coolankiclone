namespace AnkiClone
{
	public class Card
	{
		public DateTime LastReviewDate { get; set; }
		public DateTime NextReviewDate { get; set; }
		public string FrontText { get; set; }
		public string BackText { get; set; }
		public double DifficultyMultiplier { get; set; }
		public Card()
		{
			LastReviewDate = DateTime.Now;
			NextReviewDate = DateTime.Now;
			DifficultyMultiplier = 1000;
			FrontText = String.Empty;
			BackText = String.Empty;
		}
		public Card(string front, string back) : this()
		{
			FrontText = front;
			BackText = back;
		}
	}
}
