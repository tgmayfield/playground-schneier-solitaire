using System;

using NUnit.Framework;

using Shouldly;

using System.Linq;

namespace SolitaireCypher
{
	[TestFixture]
	public class CardTests
	{
		[Test]
		public void verify_joker_values()
		{
			Card.LittleJoker.SuitValue.ShouldBe(53);
			Card.BigJoker.SuitValue.ShouldBe(53);
		}

		[TestCase("C1", "Clubs", 1)]
		[TestCase("D12", "Diamonds", 12)]
		[TestCase("s9", "Spades", 9)]
		[TestCase("h10", "Hearts", 10)]
		[TestCase("", null, 0)]
		[TestCase("1", null, 0)]
		[TestCase("J53", null, 0)]
		[TestCase("C13", "Clubs", 13)]
		[TestCase("C14", null, 0)]
		[TestCase("C0", null, 0)]
		[TestCase("C1a", null, 0)]
		[TestCase("C1.0", null, 0)]
		public void can_get_card_from_string(string value, string expectedSuit, int expectedValue)
		{
			Card card;

			if (expectedSuit == null)
			{
				try
				{
					card = Card.FromString(value);
				}
				catch (ArgumentNullException)
				{
					card = null;
				}
				catch (ArgumentOutOfRangeException)
				{
					card = null;
				}

				card.ShouldBe(null);
			}
			else
			{
				card = Card.FromString(value);
				card.Suit.Name.ShouldBe(expectedSuit);
				card.Value.ShouldBe(expectedValue);
			}
		}

		[TestCase("A,C7,D2,B,D9,S4,H1")]
		public void can_go_to_and_from_string_list(string value)
		{
			var cards = Card.FromStringList(value);
			var list = cards.ToStringList();

			list.ShouldBe(value);
		}

		[Test]
		public void can_go_to_and_from_unkeyed_deck()
		{
			var cards = Card.UnkeyedDeck;

			var list = cards.ToStringList();
			var back = Card.FromStringList(list);

			var orig = cards.Select(c => c.Identifier).ToArray();
			var actual = back.Select(c => c.Identifier).ToArray();

			actual.ShouldBe(orig);
		}
	}
}