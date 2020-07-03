using Reno.Piles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reno.Source
{
    static class Methods
    {
        public static void DrawCard(this ImageList imageList, Graphics g, int x, int y, int index)
        {
            imageList.Draw(g, x, y, index);
        }

        public static void DrawDeck(this ImageList imageList, Graphics g, int x, int y)
        {
            imageList.Draw(g, x, y, Constants.DECK_SHIRT);
        }

        public static void DrawFobidPile(this ImageList imageList, Graphics g, int x, int y)
        {
            imageList.Draw(g, x, y, Constants.FORBID_PLACE);
        }

        public static void DrawEmptyPile(this ImageList imageList, Graphics g, int x, int y)
        {
            imageList.Draw(g, x, y, Constants.EMPTY_PILE);
        }

        public static void DrawShirt(this ImageList imageList, Graphics g, int x, int y)
        {
            imageList.Draw(g, x, y, Constants.SHIRT);
        }
    }
}
