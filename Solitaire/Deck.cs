using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solitaire
{
    class Deck
    {
        Card[] deck = new Card[52];
        public Deck() {
            for(int i = 0; i < 13; i++) { 
                for(int j = 0; j < 4; j++) {
                    char suit = 'X';
                    switch(j) {
                        case 0:
                            suit = 'C';
                            break;
                        case 1:
                            suit = 'D';
                            break;
                        case 2:
                            suit = 'H';
                            break;
                        case 3:
                            suit = 'S';
                            break;
                    }

                    deck[i * 4 + j] = new Card(suit, i, j == 1 || j == 2 ? 'R' : 'B');
                }


            }
        }

        public Card[] getDeck() {
            return deck;
        }

        public Card[] shuffle() {
            Random rnd = new Random();
            deck = deck.OrderBy(x => rnd.Next()).ToArray();
            return deck;
        }

    }
}
