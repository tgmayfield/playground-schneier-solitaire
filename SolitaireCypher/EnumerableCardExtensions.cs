using System;
using System.Collections.Generic;
using System.Linq;

namespace SolitaireCypher
{
	public static class EnumerableCardExtensions
	{
		public static void MoveCard(this List<Card> cards, Card card, int position)
		{
			int currentIndex = cards.IndexOf(card);
			if (currentIndex < 0)
			{
				throw new ArgumentOutOfRangeException("card", card, "Not found in cards list");
			}

			cards.RemoveAt(currentIndex);
			cards.Insert(position - 1, card);
		}

		public static string ToStringList(this IEnumerable<Card> cards)
		{
			return string.Join(",", cards.Select(c => c.Identifier));
		}
	}
}