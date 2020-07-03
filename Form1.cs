using Reno.Piles;
using System;
using System.Drawing;
using System.Windows.Forms;

using Reno.Source;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Reno
{
    public partial class Form1 : Form
    {
        private bool _newGame;                                          //is new game start
        private Deck _deck;                                             //deck ^_^
        private Graphics gr;                                            //drawing element
        private Pile[] _extraField;                                     //the place where we put cards from deck
        private Pile[] _piles;                                          //piles
        private Pile[] _aces;                                           //piles of aces
        private Pile[] _secretPile;                                     //secret pile
        
        private bool _drag;                                              //are we drag card?     
        private int _dragX, _dragY;
        private int _deltaX, _deltaY;
        private Card _dragCard;                                          //card which we drag
        private List<Card> _dragSequenses;
        private int _amountDragCards;

        private int _fromPile;
        private int _fromExtraPile;
        private int _fromSecretPile;
        private int _toAce;
        private int _toPile;


        public Form1()
        {
            InitializeComponent();

            _deck = new Deck();
            _extraField = new Pile[Constants.COUNT_OF_EXTRA_FIELDS];
            _piles = new Pile[Constants.COUNT_OF_PILES];
            _aces = new Pile[Constants.COUNT_OF_ACES];
            _secretPile = new Pile[Constants.COUNT_OF_SECRET_PILES];

            _drag = false;
            _dragSequenses = new List<Card>();
            _amountDragCards = 0;

            _fromPile = -1;
            _fromExtraPile = -1;
            _fromSecretPile = -1;

            _toAce = -1;
            _toPile = -1;

        }

        
        
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_newGame) { ShowAll(e.Graphics); }

        }

        private void NewGame()             //initialize and drawing our fields with cards
        {
            _newGame = false;
            _deck.Reset();
            this.Refresh();
            _newGame = true;
            _deck.Shuffle();
            gr = this.CreateGraphics();
            InitializeFields();
            HandOutCards();
            ShowAll(gr);
        }
        #region INITIALIZATION
        private void InitializeFields()
        {
            initExtraField();
            initPiles();
            initAces();
            initSecretPile();
        }

        private void initExtraField() 
        {
            _extraField[0] = new Pile(Constants.EXTRA_FIELD_POS_X, Constants.EXTRA_FIELD_POS_Y);
        }

        private void initSecretPile()
        {
            _secretPile[0] = new Pile(Constants.SECRET_PILE_X, Constants.SECRET_PILE_Y);
        }

        private void initPiles()
        {
            for (int i = 0; i < Constants.COUNT_OF_PILES; i++) {
                _piles[i] = new Pile(
                    Constants.PILES_POS_X + i * (Constants.CARD_INTERVAL_DX + Constants.CARD_WEIGHT), 
                    Constants.PILES_POS_Y);
            }
        }

        private void initAces()
        {
            for (int i = 0; i < Constants.COUNT_OF_PILES; i++) {
                _aces[i] = new Pile(
                    Constants.ACES_POS_X + i * (Constants.CARD_INTERVAL_DX + Constants.CARD_WEIGHT),
                    Constants.ACES_POS_Y);
            }
        }
        #endregion
        #region HANDLING
        private void HandOutCards() 
        {
            fromDeckToSecretPile();
            fromDeckToPiles();
        }

        private void fromDeckToSecretPile()
        {
            for (int i = 0; i < Constants.COUNT_OF_SECRET_PILES; i++) {
                for (int j = 0; j < Constants.CARDS_IN_SECRET_PILE; j++) {
                    _secretPile[i].Add(_deck.Pop());
                }
                _secretPile[i].Element(_secretPile[i].Count() - 1).Block = true;
            }
        }

        private void fromDeckToPiles()
        {
            if (_newGame)
            {
                for (int i = 0; i < Constants.COUNT_OF_PILES; i++) {
                    for (int j = 0; j < Constants.CARDS_IN_PILE; j++) {
                        _piles[i].Add(_deck.Pop());
                    }
                    _piles[i].Element(_piles[i].Count() - 1).Block = true;
                }
            }
        }

        private void fromDeckToExtraField() 
        {
            if (_newGame)
            {
                for (int i = 0; i < Constants.COUNT_OF_EXTRA_FIELDS; i++) {
                    _extraField[i].Add(_deck.Pop());
                    _extraField[i].LastCard.Block = true;
                }
            }
        }
        #endregion
        #region SHOWING
        private void ShowAll(Graphics grf)
        {
            showDeck(grf);
            showExtraField(grf);
            showPiles(grf);
            showAces(grf);
            showSecretPile(grf);

            if (_drag)
            {
                for (int i = _dragSequenses.Count - 1; i >= 0 ; i--)
                    imageList1.Draw(grf, _dragX, _dragY - i * Constants.CARD_SHIFT, _dragSequenses[i].CardNum());
            }
        }

        private void showExtraField(Graphics grf) 
        {
            if (_extraField[0].Count() == 0)
                imageList1.DrawFobidPile(grf, _extraField[0].X, _extraField[0].Y);
            else
                imageList1.Draw(grf, _extraField[0].X, _extraField[0].Y, _extraField[0].Last);
        }

        private void showSecretPile(Graphics grf)
        {
            if (_secretPile[0].Count() == 0)
                imageList1.DrawFobidPile(grf, _secretPile[0].X, _secretPile[0].Y);
            else {
                if (_secretPile[0].LastCard.Block == false)
                    imageList1.DrawShirt(grf, _secretPile[0].X, _secretPile[0].Y);
                else {
                    imageList1.Draw(grf, _secretPile[0].X, _secretPile[0].Y, _secretPile[0].Last);
                }
            }
        }

        private void showDeck(Graphics grf)
        {
            if (_newGame)
            {
                if (_deck.Count() == 0)
                {
                    imageList1.DrawDeck(grf, Constants.DECK_POS_X, Constants.DECK_POS_Y);
                }
                else {
                    for (int i = _deck.Count(); i > 0; i--)
                        imageList1.Draw(grf, Constants.DECK_POS_X + i / 5, Constants.DECK_POS_Y - i / 5, Constants.SHIRT);
                }
            }
        }

        private void showPiles(Graphics grf)
        {
            for (int i = 0; i < Constants.COUNT_OF_PILES; i++)
            {
                if (_piles[i].Count() == 0)
                    imageList1.DrawEmptyPile(grf, _piles[i].X, _piles[i].Y);
                else
                {
                    for (int j = _piles[i].Count(); j >= 1; j--) {
                        if (_piles[i].Element(_piles[i].Count() - j).Block == false)
                            imageList1.DrawShirt(grf, _piles[i].X, _piles[i].Y + (_piles[i].Count() - j) * Constants.CARD_SHIFT);
                        else
                            imageList1.Draw(grf, _piles[i].X, _piles[i].Y + (_piles[i].Count() - j) * Constants.CARD_SHIFT, _piles[i].Element(_piles[i].Count() - j).CardNum());
                    }
                }
            }
        }

        private void showAces(Graphics grf)
        {
            for (int i = 0; i < Constants.COUNT_OF_ACES; i++)
            {
                if (_aces[i].Count() == 0)
                    imageList1.DrawEmptyPile(grf, _aces[i].X, _aces[i].Y);
                else
                {
                    imageList1.Draw(grf, _aces[i].X, _aces[i].Y, _aces[i].Last);
                }

            }
        }
        #endregion
        #region LOGIC_PART
        private int canDropToAce(int X, int Y)
        {
            _toAce = -1;
            if (_fromPile >= 0 || _fromSecretPile >= 0 || _fromExtraPile >= 0)
            {
                for (int i = 0; i < Constants.COUNT_OF_ACES; i++)
                {
                    if (X >= _aces[i].X && X <= _aces[i].X + Constants.CARD_WEIGHT)
                    {
                        if (Y >= _aces[i].Y + (_aces[i].Count() - 1) * Constants.CARD_SHIFT 
                            && Y <= _aces[i].Y + Constants.CARD_HIGHT + (_aces[i].Count() - 1) * Constants.CARD_SHIFT)
                        {
                            _toAce = i;
                            break;
                        }
                    }
                }
                if (_toAce >= 0)
                {
                    if (_aces[_toAce].Count() == 0)
                    {
                        if (_dragCard.Rank != 0)
                            return _toAce = -1;
                    } else {
                        if (_aces[_toAce].LastCard.Rank + 1 != _dragCard.Rank) {
                            return _toAce = -1;
                        } else { 
                            if (_aces[_toAce].LastCard.Suit != _dragCard.Suit) {
                                return _toAce = -1;
                            }
                            return _toAce;
                        }
                    }

                }
            }
            return _toAce;
        }

        private int canDropToPile(int X, int Y)
        {
            _toPile = -1;
            if (_fromPile >= 0 || _fromSecretPile >= 0 || _fromExtraPile >= 0)
            {  
                for (int i = 0; i < Constants.COUNT_OF_PILES; i++) {
                    if (X >= _piles[i].X && X <= _piles[i].X + Constants.CARD_WEIGHT)
                    {
                        if (Y >= _piles[i].Y + (_piles[i].Count() - 1) * Constants.CARD_SHIFT 
                            && Y <= _piles[i].Y + Constants.CARD_HIGHT + (_piles[i].Count() - 1) * Constants.CARD_SHIFT)
                        {
                            _toPile = i;
                            break;
                        }
                    }
                }
            }
            if (_toPile >= 0 && _toPile != _fromPile)
            {
                if (_piles[_toPile].Count() == 0)
                {
                    if (_fromSecretPile >= 0)
                    {
                        if (_secretPile[_fromSecretPile].Count() == 0 && _dragCard.Rank != 12)
                        {
                            return _toPile = -1;
                        }
                        return _toPile;
                    }
                    return _toPile = -1;
                }
                if (_piles[_toPile].Count() > 0) {
                    if (_piles[_toPile].LastCard.Rank - 1 != _dragCard.Rank)
                        return _toPile = -1;
                    if (_piles[_toPile].LastCard.Suit == Constants.SUIT_BLACK_PIKE || _piles[_toPile].LastCard.Suit == Constants.SUIT_BLACK_CROSS)
                    {
                        if (_dragCard.Suit == Constants.SUIT_BLACK_CROSS || _dragCard.Suit == Constants.SUIT_BLACK_PIKE)
                            return  _toPile = -1;
                        return _toPile;
                    } else if(_piles[_toPile].LastCard.Suit == Constants.SUIT_RED_HEART || _piles[_toPile].LastCard.Suit == Constants.SUIT_RED_RHOMB)
                    {
                        if (_dragCard.Suit == Constants.SUIT_RED_RHOMB || _dragCard.Suit == Constants.SUIT_RED_HEART)
                            return _toPile = -1;
                        return _toPile;
                    }

                }
            }
            return _toPile;
        }
        #endregion
        #region CLICK_PROCESSING

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dragCard = CaptureCard(e.X, e.Y);
                if (_dragCard != null)
                {
                    _deltaX = e.X - _dragX;
                    _deltaY = e.Y - _dragY;
                    _drag = true;
                }
            }
            this.Invalidate();
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (_newGame)
            {
                //the deck is't empty
                if (_deck.Count() != 0)
                {
                    if (e.X >= Constants.DECK_POS_X && e.X <= Constants.DECK_POS_X + Constants.CARD_WEIGHT
                        && e.Y >= Constants.DECK_POS_Y && e.Y <= Constants.DECK_POS_Y + Constants.CARD_HIGHT)
                    {
                        fromDeckToExtraField();
                        Refresh();
                    }
                }
            }
        }

        private Card CaptureCard(int X, int Y)
        {
            if (_newGame)
            {
                for (int i = 0; i < Constants.COUNT_OF_PILES; i++)
                {
                    if (X >= _piles[i].X && X <= _piles[i].X + Constants.CARD_WEIGHT)
                    {
                        if (Y >= _piles[i].Y && Y <= _piles[i].Y + Constants.CARD_HIGHT + (_piles[i].Count() - 1) * Constants.CARD_SHIFT)
                        {
                            if (_piles[i].Count() != 0)
                            {
                                _dragX = _piles[i].X;
                                _dragY = _piles[i].Y + (_piles[i].Count() - 1) * Constants.CARD_SHIFT;
                                _fromPile = i;

                                int numberCard = _piles[i].Count() - 1;
                                int bottomEdgeY = _piles[i].Y + Constants.CARD_HIGHT + (_piles[i].Count() - 1) * Constants.CARD_SHIFT;



                                _amountDragCards = 0;
                                while (bottomEdgeY > Y) {
                                    _dragSequenses.Add(_piles[i].Element(numberCard));
                                    _piles[i].Delete();
                                    bottomEdgeY = _piles[i].Y + (_piles[i].Count()) * Constants.CARD_SHIFT;
                                    numberCard--;
                                    _amountDragCards++;

                                }
                                _dragCard = _dragSequenses[_dragSequenses.Count() - 1];
                                //_piles[i].Delete();
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < Constants.COUNT_OF_EXTRA_FIELDS; i++)
                {
                    if (X >= _extraField[i].X && X <= _extraField[i].X + Constants.CARD_WEIGHT)
                    {
                        if (Y >= _extraField[i].Y && Y <= _extraField[i].Y + Constants.CARD_HIGHT)
                        {
                            if (_extraField[i].Count() != 0)
                            {
                                _dragSequenses.Add(_extraField[i].LastCard);
                                _dragCard = _dragSequenses[0];
                                _dragX = _extraField[i].X;
                                _dragY = _extraField[i].Y;
                                _fromExtraPile = i;
                                _extraField[i].Delete();
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < Constants.COUNT_OF_SECRET_PILES; i++)
                {
                    if (X >= _secretPile[i].X && X <= _secretPile[i].X + Constants.CARD_WEIGHT)
                    {
                        if (Y >= _secretPile[i].Y && Y <= _secretPile[i].Y + Constants.CARD_HIGHT)
                        {
                            if (_secretPile.Count() != 0)
                            {
                                _dragSequenses.Add(_secretPile[i].LastCard);
                                _dragCard = _dragSequenses[0];
                                _dragX = _secretPile[i].X;
                                _dragY = _secretPile[i].Y;
                                _fromSecretPile = i;
                                _secretPile[i].Delete();
                            }
                        }
                    }
                }
            }
            return _dragCard;

        }

        private void Form1_MouseUp_1(object sender, MouseEventArgs e)
        {
            int variant = -1;
            //положим карту на стопку?
            if (canDropToPile(e.X, e.Y) >= 0)
                variant = 0;
            //положим карту на поле тузов?
            if (canDropToAce(e.X, e.Y) >= 0)
                variant = 1;
            //карта находится в движении
            if (_drag)
            {
                //карту взяли из стопки
                if (_fromPile >= 0)
                {
                    switch (variant)
                    {
                        case 0:
                            for (int i = _dragSequenses.Count() - 1; i >= 0; i--) {
                                _piles[_toPile].Add(_dragSequenses[i]);
                            }
                            break;
                        case 1:
                            _aces[_toAce].Add(_dragCard);
                            if (_piles[_fromPile].Count() > 0)
                                _piles[_fromPile].LastCard.Block = true;
                            break;
                        default:
                            for (int i = _dragSequenses.Count() - 1; i >= 0; i--) {
                                _piles[_fromPile].Add(_dragSequenses[i]);
                            }
                            break;
                    }
                }
                if (_fromExtraPile >= 0)
                {
                    switch (variant)
                    {
                        case 0:
                            _piles[_toPile].Add(_dragCard);
                            _piles[_toPile].LastCard.Block = true;
                            break;
                        case 1:
                            _aces[_toAce].Add(_dragCard);
                            break;
                        default:
                            _extraField[0].Add(_dragCard);
                            break;
                    }
                }
                if (_fromSecretPile >= 0) 
                { 
                    switch (variant) 
                    {
                        case 0:
                            _piles[_toPile].Add(_dragCard);
                            //_piles[_toPile].LastCard.Block = true;
                            if (_secretPile[_fromSecretPile].Count() > 0)
                                _secretPile[_fromSecretPile].LastCard.Block = true;
                            break;
                        case 1:
                            _aces[_toAce].Add(_dragCard);
                             if (_secretPile.Count() > 0)
                                    _secretPile[_fromSecretPile].LastCard.Block = true;
                            break;
                        default:
                            _secretPile[_fromSecretPile].Add(_dragCard);
                            break;
                    }
                }
                Invalidate();
                _dragCard = null;
                _fromPile = -1;
                _fromExtraPile = -1;
                _fromSecretPile = -1;
                _toPile = -1;
                _toAce = -1;
                _dragX = 0;
                _dragY = 0;
                _drag = false;
                _amountDragCards = 0;
                _dragSequenses.Clear();
            }
        }

        private void Form1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (_drag)
            {
                _dragX = e.X - _deltaX;
                _dragY = e.Y - _deltaY;
                this.Invalidate();
            }
        }
#endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (_newGame)
            {
                ShowAll(e.Graphics);
            }
        }



        private void gameToolStripMenuItem_Click(object sender, EventArgs e) {  }

   
    }
}
