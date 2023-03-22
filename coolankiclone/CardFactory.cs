namespace AnkiClone
{
	public static class CardFactory
	{
		public static Card? GetCard(string front, string back)
		{
			if (front == String.Empty || back == String.Empty)
			{
				return null;
			}
			return new Card(front, back);
		}
	}
}
