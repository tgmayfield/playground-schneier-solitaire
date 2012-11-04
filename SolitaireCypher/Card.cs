using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SolitaireCypher
{
	public class Card
	{
		private readonly Suit _suit;
		private readonly int _value;
		private readonly string _identifier;

		public Card(Suit suit, int value, string identifier = null)
		{
			_suit = suit;
			_value = value;

			if (identifier == null)
			{
				identifier = string.Concat(suit.Identifier, value.ToString());
			}

			_identifier = identifier;
		}

		public Suit Suit
		{
			get { return _suit; }
		}
		public int Value
		{
			get { return _value; }
		}
		public string Identifier
		{
			get { return _identifier; }
		}

		public int SuitValue
		{
			get { return Suit.Value * 13 + Value; }
		}

		public override string ToString()
		{
			return Identifier;
		}

		public static readonly Card LittleJoker = new Card(Suit.Jokers, 1, "A");
		public static readonly Card BigJoker = new Card(Suit.Jokers, 1, "B");

		public static readonly Card[] Jokers = new[]
		{
			LittleJoker,
			BigJoker,
		};

		public static readonly List<Card> UnkeyedDeck = Suit.Suits
			.SelectMany(
				suit => Enumerable.Range(1, 13),
				(suit, value) => new Card(suit, value))
			.Concat(Jokers)
			.ToList();

		private static readonly Regex NonNumeric = new Regex("[^0-9]", RegexOptions.Compiled);

		/// <param name="identifier">Expected C1 for Ace of Clubs, S13 for King of Spades</param>
		public static Card FromString(string identifier)
		{
			if (string.IsNullOrEmpty(identifier))
			{
				throw new ArgumentNullException("identifier");
			}

			if (identifier.Equals("A", StringComparison.InvariantCultureIgnoreCase))
			{
				return LittleJoker;
			}
			if (identifier.Equals("B", StringComparison.InvariantCultureIgnoreCase))
			{
				return BigJoker;
			}

			string suitIdentifier = identifier.Substring(0, 1).ToUpper();
			Suit match = Suit.Suits.SingleOrDefault(x => x.Identifier == suitIdentifier);
			if (match == null)
			{
				throw new ArgumentOutOfRangeException("identifier", identifier, string.Format("Could not find a suit with identifier '{0}'", suitIdentifier));
			}

			string remaining = identifier.Substring(1);
			if (NonNumeric.IsMatch(remaining))
			{
				throw new ArgumentOutOfRangeException("identifier", identifier, string.Format("The characters following the suit identifier aren't all numeric: '{0}'", remaining));
			}

			int value = int.Parse(remaining);
			if (value < 1 || value > 13)
			{
				throw new ArgumentOutOfRangeException("identifier", identifier, string.Format("Value does not correspond to a suit identifier: {0}", value));
			}

			return new Card(match, value);
		}

		public static List<Card> FromStringList(string identifiers)
		{
			var split = identifiers.Split(',')
				.Select(s => s.Trim())
				.Where(s => !string.IsNullOrEmpty(s))
				.ToArray();

			var result = new List<Card>();

			for (int index = 0; index < split.Length; index++)
			{
				try
				{
					var card = FromString(split[index]);
					result.Add(card);
				}
				catch (Exception ex)
				{
					throw new ArgumentOutOfRangeException("identifiers", string.Format("Could not parse identifier at position {0}: {1}", index + 1, ex.Message));
				}
			}

			return result;
		}
	}
}