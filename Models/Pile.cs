using Reno.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reno.Piles
{
    class Pile
    {
        private int _x;              //location on X axis
        private int _y;              //location on Y axis
        private List<Card> _pile;    //cards in pile

        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        public int Last { get {
                if (_pile != null && _pile.Count > 0)
                    return _pile.Last().CardNum();
                return Constants.FORBID_PLACE;
            } 
        }

        public Card LastCard {
            get {
                if (_pile != null && _pile.Count > 0)
                    return _pile.Last();
                else return null;
            }
        }

        public Pile(int x, int y)
        {
            this._x = x;
            this._y = y;
            this._pile = new List<Card>();
        }

        public void Add(Card card) {
            _pile.Add(card);
        }

        public Card Delete() {
            Card card = _pile.ElementAt(_pile.Count - 1);
            _pile.RemoveAt(_pile.Count - 1);
            return card;
        }

        public Card Element(int i) {
            return _pile.ElementAt(i);
        }

        public int Count() {
            return _pile.Count();
        }


    }
}
