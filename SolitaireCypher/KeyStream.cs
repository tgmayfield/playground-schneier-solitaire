using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SolitaireCypher
{
	public class KeyStream
	{
		protected readonly List<Card> state;

		public KeyStream(List<Card> state)
		{
			if (state == null || state.Count == 0)
			{
				throw new ArgumentNullException("state");
			}
			this.state = state;
		}

		public IEnumerable<Card> Cards
		{
			get { return state; }
		}

		public int Value
		{
			get
			{
				var first = state[0];
				int count = first.SuitValue;

				var outputCard = state.Skip(count).First();
				int value = outputCard.SuitValue;

				return value;
			}
		}

		public int CharacterValue
		{
			get
			{
				var value = Value;
				if (value > 26)
				{
					value -= 26;
				}
				return value;
			}
		}

		private const int ZeroCharacter = 'A' - 1;

		public char Character
		{
			get
			{
				int character = CharacterValue + ZeroCharacter;
				char result = (char)character;
				return result;
			}
		}

		public bool ShouldSkipValue
		{
			get { return Value == Card.BigJoker.SuitValue; }
		}

		public KeyStream Iterate()
		{
			var newState = new List<Card>(state);
			MoveLittleJokerOneDown(newState);
			MoveBigJokerTwoDown(newState);
			TripleCut(newState);
			CountCut(newState);

			return new KeyStream(newState);
		}

		private static void MoveDown(List<Card> deck, Card card, int distance)
		{
			int jokerAt = deck.IndexOf(card);
			int newIndex = jokerAt + distance;

			if (newIndex >= deck.Count)
			{
				newIndex -= deck.Count;
				newIndex++;
			}

			deck.RemoveAt(jokerAt);
			deck.Insert(newIndex, card);
		}

		public static void MoveLittleJokerOneDown(List<Card> deck)
		{
			MoveDown(deck, Card.LittleJoker, 1);
		}

		public static void MoveBigJokerTwoDown(List<Card> deck)
		{
			MoveDown(deck, Card.BigJoker, 1);
			MoveDown(deck, Card.BigJoker, 1);
		}

		public static void TripleCut(List<Card> deck)
		{
			int firstJoker = deck.FindIndex(c => c.Suit == Suit.Jokers);
			int lastJoker = deck.FindLastIndex(c => c.Suit == Suit.Jokers);

			var topCut = deck.Take(firstJoker).ToList();
			var bottomCut = deck.Skip(lastJoker + 1).ToList();

			deck.RemoveAll(topCut.Contains);
			deck.RemoveAll(bottomCut.Contains);

			var middleCut = new List<Card>(deck);

			deck.Clear();

			deck.AddRange(bottomCut);
			deck.AddRange(middleCut);
			deck.AddRange(topCut);
		}

		public static void CountCut(List<Card> deck)
		{
			var last = deck.Last();
			if (last.Suit == Suit.Jokers)
			{
				return;
			}

			int count = last.SuitValue;

			var topCut = deck.Take(count).ToList();
			deck.RemoveAll(topCut.Contains);
			var bottomCut = deck.Take(deck.Count - 1).ToList();

			deck.Clear();

			deck.AddRange(bottomCut);
			deck.AddRange(topCut);
			deck.Add(last);
		}

		public IEnumerable<KeyStream> GetNext(int count)
		{
			return GetNext(count, s => s);
		}

		public IEnumerable<T> GetNext<T>(int count, Func<KeyStream, T> getValue, bool checkShouldSkipValue = true)
		{
			var stream = this;
			int returned = 0;
			while (returned < count)
			{
				stream = stream.Iterate();
				if (checkShouldSkipValue && stream.ShouldSkipValue)
				{
					continue;
				}

				returned++;
				yield return getValue(stream);
			}
		}

		private static readonly Regex StripNonAlpha = new Regex("[^A-Z]", RegexOptions.IgnoreCase);

		public string Encrypt(string value)
		{
			value = StripNonAlpha.Replace(value, "").ToUpper();
			while (value.Length % 5 != 0)
			{
				value += "X";
			}

			int[] keys = GetNext(value.Length, s => s.CharacterValue).ToArray();

			var result = new List<char>();
			for (int index = 0; index < value.Length; index++)
			{
				int c = value[index] - ZeroCharacter;
				int key = keys[index];

				c += key;
				if (c > 26)
				{
					c -= 26;
				}

				result.Add((char)(c + ZeroCharacter));
			}

			return new string(result.ToArray());
		}

		public string Decrypt(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value");
			}

			value = value.Replace(" ", "").ToUpper();

			if (StripNonAlpha.IsMatch(value))
			{
				throw new ArgumentOutOfRangeException("value", "Only alpha characters are acceptable");
			}

			int[] keys = GetNext(value.Length, s => s.CharacterValue).ToArray();

			var result = new List<char>();
			for (int index = 0; index < value.Length; index++)
			{
				int c = value[index] - ZeroCharacter;
				int key = keys[index];

				c -= key;
				if (c < 1)
				{
					c += 26;
				}

				result.Add((char)(c + ZeroCharacter));
			}

			return new string(result.ToArray());
		}
	}
}