using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reno.Piles
{
    class Card
    {
        private int _suit;   //the suit of card
        private int _rank;   //the rank of card
        private bool _block; //is the cart in game

        public Card(int suit, int rank)
        {
            this._suit = suit;
            this._rank = rank;
        }
        
        public Card(int num)
        {
            this._suit = num / 13;
            this._rank = num % 13;
        }

        public int Suit { get { return _suit; } }
        public int Rank { get { return _rank; } }
        public bool Block { get { return _block; } set { _block = value; } }

        public int CardNum() {
            return _suit * 13 + _rank;
        }

    }
}
