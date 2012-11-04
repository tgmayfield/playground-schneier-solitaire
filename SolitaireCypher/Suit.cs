using System;

namespace SolitaireCypher
{
	public class Suit
	{
		private readonly string _name;
		private readonly int _value;

		private Suit(string name, int value)
		{
			_name = name;
			_value = value;
		}

		public string Name
		{
			get { return _name; }
		}
		public string Identifier
		{
			get { return Name.Substring(0, 1); }
		}
		public int Value
		{
			get { return _value; }
		}

		public static readonly Suit Clubs = new Suit("Clubs", 0);
		public static readonly Suit Diamonds = new Suit("Diamonds", 1);
		public static readonly Suit Hearts = new Suit("Hearts", 2);
		public static readonly Suit Spades = new Suit("Spades", 3);

		public static readonly Suit[] Suits = new[]
		{
			Clubs,
			Diamonds,
			Hearts,
			Spades,
		};

		public static readonly Suit Jokers = new Suit("Jokers", 4);
	}
}