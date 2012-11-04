using System;
using System.Linq;

using NUnit.Framework;

using Shouldly;

namespace SolitaireCypher
{
	public class KeyStreamTests
	{
		[TestCase(53, 54)]
		[TestCase(1, 2)]
		[TestCase(17, 18)]
		[TestCase(54, 2)]
		public void MoveLittleJokerOneDown_position_check(int position, int expected)
		{
			var deck = Card.UnkeyedDeck;
			deck.MoveCard(Card.LittleJoker, position);

			KeyStream.MoveLittleJokerOneDown(deck);

			int newIndex = deck.IndexOf(Card.LittleJoker);
			int newPosition = newIndex + 1;
			newPosition.ShouldBe(expected);
		}

		[TestCase("A,D7,D2,B,D9,D4,D1", "D7,A,D2,B,D9,D4,D1")]
		public void MoveLittleJokerOneDown_deck_check(string before, string expected)
		{
			var deck = Card.FromStringList(before);
			KeyStream.MoveLittleJokerOneDown(deck);

			deck.ToStringList().ShouldBe(expected);
		}

		[TestCase(52, 54)]
		[TestCase(53, 2)]
		[TestCase(1, 3)]
		[TestCase(17, 19)]
		[TestCase(54, 3)]
		public void MoveBigJokerTwoDown_position_check(int position, int expected)
		{
			var deck = Card.UnkeyedDeck;
			deck.MoveCard(Card.BigJoker, position);

			KeyStream.MoveBigJokerTwoDown(deck);

			int newIndex = deck.IndexOf(Card.BigJoker);
			int newPosition = newIndex + 1;
			newPosition.ShouldBe(expected);
		}

		[TestCase("D7,A,D2,B,D9,D4,D1", "D7,A,D2,D9,D4,B,D1")]
		[TestCase("C1,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,B,A",
			"C1,B,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,A")]
		public void MoveBigJokerTwoDown_deck_check(string before, string expected)
		{
			var deck = Card.FromStringList(before);
			KeyStream.MoveBigJokerTwoDown(deck);

			deck.ToStringList().ShouldBe(expected);
		}

		[TestCase("A,D7,D2,B,D9,D4,D1", "D7,A,D2,D9,D4,B,D1")]
		[TestCase("D3,A,B,D8,D9,D6", "D3,A,D8,B,D9,D6")]
		public void both_jokers_deck_check(string before, string expected)
		{
			var deck = Card.FromStringList(before);
			KeyStream.MoveLittleJokerOneDown(deck);
			KeyStream.MoveBigJokerTwoDown(deck);

			deck.ToStringList().ShouldBe(expected);
		}

		[TestCase("D2,D4,D6,B,D5,D8,D7,D1,A,D3,D9", "D3,D9,B,D5,D8,D7,D1,A,D2,D4,D6")]
		[TestCase("B,D5,D8,D7,D1,A,D3,D9", "D3,D9,B,D5,D8,D7,D1,A")]
		[TestCase("B,D5,D8,D7,D1,A", "B,D5,D8,D7,D1,A")] // Unchanged
		public void TripleCut_deck_check(string before, string expected)
		{
			var deck = Card.FromStringList(before);
			KeyStream.TripleCut(deck);

			deck.ToStringList().ShouldBe(expected);
		}

		[TestCase("C7,C6,C3,C2,C1,C10,C11,C12,C4,C5,C13,C8,C9", "C5,C13,C8,C7,C6,C3,C2,C1,C10,C11,C12,C4,C9")]
		[TestCase("C7,C6,C3,C2,C1,C10,C11,C12,C4,C5,C13,C8,C9,A", "C7,C6,C3,C2,C1,C10,C11,C12,C4,C5,C13,C8,C9,A")] // Unchanged
		public void CountCut_deck_check(string before, string expected)
		{
			var deck = Card.FromStringList(before);
			KeyStream.CountCut(deck);

			deck.ToStringList().ShouldBe(expected);
		}

		[TestCase("C1,C2,C3", 2)]
		[TestCase("C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,D11,D12,D13,H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,A,B,C1",
			4)]
		[TestCase("S12,A,C1,B,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,D11,D12,D13,H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S13",
			49)]
		public void Value_check(string cards, int expected)
		{
			var stream = new KeyStream(Card.FromStringList(cards));
			stream.Value.ShouldBe(expected);
		}

		[Test]
		public void iterate_one_unkeyed_deck()
		{
			const string first = "C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,D11,D12,D13,H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,A,B,C1";
			const string second = "S12,A,C1,B,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,D11,D12,D13,H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S13";

			var key = Card.UnkeyedDeck;
			var stream = new KeyStream(key);

			stream = stream.Iterate();
			stream.Cards.Count().ShouldBe(key.Count);
			stream.Cards.ToStringList().ShouldBe(first);

			stream = stream.Iterate();
			stream.Cards.Count().ShouldBe(key.Count);
			stream.Cards.ToStringList().ShouldBe(second);
		}

		[Test]
		public void check_unkeyed_deck_outputs()
		{
			var expected = new[]
			{
				4, 49, 10, 53, 24, 8, 51, 44, 6, 4, 33
			};
			const string expectedCharacters = "DWJXHYRFDG";
			const string expectedCharacters20 = "DWJXHYRFDGTMSHPUURXJ";

			var stream = new KeyStream(Card.UnkeyedDeck);
			var actual = stream.GetNext(11, s => s.Value, false).ToList();
			var characters = stream.GetNext(10, s => s.Character).ToList();
			var characters20 = stream.GetNext(20, s => s.Character).ToList();

			actual.ToArray().ShouldBe(expected);
			new string(characters.ToArray()).ShouldBe(expectedCharacters);
			new string(characters20.ToArray()).ShouldBe(expectedCharacters20);
		}

		[TestCase("AAAAAAAAAA", "EXKYIZSGEH")]
		[TestCase("CODEINRUBYLIVELONGER", "GLNCQMJAFFFVOMBJIYCB")]
		[TestCase("YOURCIPHERISWORKINGX", "CLEPKHHNIYCFPWHFDFEH")]
		[TestCase("WELCOMETORUBYQUIZXXX", "ABVAWLWZSYOORYKDUPVH")]
		public void can_encrypt_and_decrypt_from_unkeyed_deck(string plain, string encrypted)
		{
			var stream = new KeyStream(Card.UnkeyedDeck);

			string actualEncrypted = stream.Encrypt(plain);
			actualEncrypted.ShouldBe(encrypted);

			string actualDecrypted = stream.Decrypt(actualEncrypted);
			actualDecrypted.ShouldBe(plain);
		}
	}
}