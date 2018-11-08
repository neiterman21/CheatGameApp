using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheatGameApp.Model
{
    public sealed class Deck : List<Card>
    {
        public static readonly Deck FullDeck = Deck.Parse("4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4");

        public int GetCardCount(int number)
        {
            return this.Count(c => c.Number == number);
        }
        public Dictionary<int, List<Card>> ToDictionary()
        {
            Dictionary<int, List<Card>> cardsByNumber = new Dictionary<int, List<Card>>();

            foreach (var card in this)
            {
                List<Card> cards;

                if (cardsByNumber.TryGetValue(card.Number, out cards))
                    cards.Add(card);
                else
                    cardsByNumber.Add(card.Number, new List<Card>() { card });
            }

            return cardsByNumber;
        }
        /// <summary>
        /// Retunrs an array of ints where for each index the count of cards with the index+1 count is set
        /// </summary>
        /// <returns></returns>
        public int[] GetCounts()
        {
            int[] counts = new int[Card.MaxNumber];
            for (int i = 0; i < Card.MaxNumber; i++)
                counts[i] = GetCardCount(i + 1);
            return counts;
        }
        public string ToShortString()
        {
            return string.Join(", ", GetCounts());
        }
        public static Deck Parse(string s)
        {
            Deck deck = new Deck();
            s = s.Replace(" ", "");
            string[] values = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int length = Math.Min(Card.MaxNumber, values.Length);
            for (int i = 0; i < length; i++)
            {
                int count = int.Parse(values[i]);
                for (int c = 0; c < count; c++)
                {
                    Card card = new Card((i + 1), (CardType)c);
                    deck.Add(card);
                }
            }

            return deck;
        }
    }
}
