using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solitaire { 
    class Card {
        char suit; //Clubs = "C", Diamonds = "D", Hearts = "H", Spades = "S"
        int value;
        char colour; //Red = 'R', Black = 'B'

        public Card(char suit, int value, char colour) {
            this.suit = suit;
            this.value = value;
            this.colour = colour;
        }

        public bool follows(Card other) {
            bool follows = true;
            follows &= colour != other.colour;
            follows &= value == other.value - 1;
            return follows;
        }

        public bool followsAceStack(Card other) {
            bool follows = true;
            follows &= value == other.value + 1;
            follows &= suit == other.suit || other.suit == 'O';
            return follows;
        }

        public char getSuit() {
            return suit;
        }

        public char getColour() {
            return colour;
        }

        public int getValue() {
            return value;
        }

    }
}
