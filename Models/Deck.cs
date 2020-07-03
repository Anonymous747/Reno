using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reno.Piles
{
    class Deck
    {
        private Stack<Card> _deck; //the list of card

        public Deck()
        {
            _deck = new Stack<Card>(); 
        }

        public void Add(Card card) {
            _deck.Push(card);
        }

        public Card Pop() {
            Card card = new Card(_deck.Pop().CardNum());
            return card;
        }

        public Card Peek() {
            Card card = new Card(_deck.Peek().CardNum());
            return card;
        }

        public void Shuffle() {
            List<int> cards = new List<int>();
            
            //fill the deck
            for (int i = 0; i < 52; i++) {
                cards.Add(i);
            }

            List<int> tempList = new List<int>();

            Random rnd = new Random();

            for (int i = 0; i < 52; i++) {
                int rndIndex = rnd.Next(cards.Count);
                tempList.Add(cards.ElementAt(rndIndex));
                cards.RemoveAt(rndIndex);
            }

            for (int i = 0; i < tempList.Count; i++) {
                Card card = new Card(tempList.ElementAt(i));
                _deck.Push(card);
            }
        }

        public void Reset() {
            int count = _deck.Count();
            for (int i = 0; i < count; i++) {
                _deck.Pop();
            }
        }

        public int Count() {
            return _deck.Count();
        }
    }
}
