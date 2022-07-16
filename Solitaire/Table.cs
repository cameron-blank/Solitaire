using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solitaire
{
    class Table {

        List<PictureBox>[] lists = new List<PictureBox>[13]; //Left to right, top to bottom. 6 on top, 7 on bottom
        public static int[] xPositions = { 12, 174, 375, 550, 725, 900, 12, 174, 336, 498, 660, 822, 984 };
        public static int[] yPositions = { 28, 28, 28, 28, 28, 28, 225, 225, 225, 225, 225, 225, 225};
        int offsetIncrement = 16;

        public Table(PictureBox[] pictureBoxes, PictureBox[] acePictureBoxes, PictureBox[] emptyPictureBoxes) { 
            for(int i = 0; i < 13; i++) {
                lists[i] = new List<PictureBox>();
            }

            for(int i = 0; i < 7; i++) {
                emptyPictureBoxes[i].Location = new Point(xPositions[i + 6], yPositions[i + 6]);
                lists[i + 6].Add(emptyPictureBoxes[i]);
            }

            int count = 1;
            int offset = 0;
            Point point;
            foreach(PictureBox pictureBox in pictureBoxes) {
                if (count == 2 || count == 4 || count == 7 || count == 11 || count == 16 || count == 22 || count == 29)
                    offset = 0;

                if (count <= 1) {
                    point = new Point(12, 225 + offset);
                    lists[6].Add(pictureBox);
                } else if (count <= 3) {
                    point = new Point(174, 225 + offset);
                    lists[7].Add(pictureBox);
                } else if (count <= 6) {
                    point = new Point(336, 225 + offset);
                    lists[8].Add(pictureBox);
                } else if (count <= 10) {
                    point = new Point(498, 225 + offset);
                    lists[9].Add(pictureBox);
                } else if (count <= 15) {
                    point = new Point(660, 225 + offset);
                    lists[10].Add(pictureBox);
                } else if (count <= 21) {
                    point = new Point(822, 225 + offset);
                    lists[11].Add(pictureBox);
                } else if (count <= 28) {
                    point = new Point(984, 225 + offset);
                    lists[12].Add(pictureBox);
                } else {
                    point = new Point(xPositions[0], yPositions[0]);
                    lists[0].Add(pictureBox);
                }

                pictureBox.Location = point;
                count++;
                offset += offsetIncrement;
            }

            for (int i = 0; i < 4; i++) {
                acePictureBoxes[i].Location = new Point(xPositions[i + 2], yPositions[i + 2]);
                lists[i + 2].Add(acePictureBoxes[i]);
            }

        }

        public List<PictureBox>[] getList() {
            return lists;
        }

        public List<PictureBox> moveCards(int sourceList, int destinationList, int numberOfCards) {
            List<PictureBox> sublist = lists[sourceList].GetRange(lists[sourceList].Count() - numberOfCards, numberOfCards);
            lists[sourceList] = lists[sourceList].Except(sublist).ToList();
            //lists[destinationList].AddRange(sublist);
            foreach(PictureBox pictureBox in sublist) {
                pictureBox.BringToFront();
                lists[destinationList].Add(pictureBox);
                //pictureBox.Location = new Point(xPositions[destinationList], getNextOffset(destinationList));
            }
            return sublist;
        }

        public int getNextOffset(int listNumber) {
            int offset = 34;
            if(listNumber >= 6 && lists[listNumber].Count() + 1 > 2) //Was +2
                return lists[listNumber][lists[listNumber].Count() - 1].Location.Y + offset; // Was -2
            else if (listNumber >= 6)
                return lists[listNumber][lists[listNumber].Count() - 1].Location.Y; // Was -2
            else
                return yPositions[listNumber];
        }

        public int getListContaining(PictureBox pictureBox) { 
            for(int i = 0; i < 13; i++) {
                if (lists[i].Contains(pictureBox))
                    return i;
            }
            return 13;
        }

        public bool isLastCardInList(int listNumber, PictureBox pictureBox) {
            return lists[listNumber][lists[listNumber].Count() - 1] == pictureBox;
        }

        public int isInPosition(int listNumber, PictureBox pictureBox) {
            return lists[listNumber].FindIndex(pictureBox2 => pictureBox2 == pictureBox);
        }

        public int getSize(int listNumber) {
            return lists[listNumber].Count();
        }

        public void flipTopCards(Dictionary<PictureBox, Card> cardMapping, Dictionary<PictureBox, Image> imageMapping, Dictionary<PictureBox, bool> booleanMapping) { 
            for(int i = 1; i < 13; i++) { 
                if(lists[i].Count() > 0 && cardMapping[lists[i][lists[i].Count() - 1]].getSuit() != 'O') {
                    lists[i][lists[i].Count() - 1].Image = imageMapping[lists[i][lists[i].Count() - 1]];
                    booleanMapping[lists[i][lists[i].Count() - 1]] = true;
                }
            }
        }

        public int countAceStack() {
            int sum = 0;
            for(int i = 2; i < 6; i++)
                sum += lists[i].Count() - 1;
            return sum;
        }

        public List<PictureBox> getTopCards() {
            List<PictureBox> topCards = new List<PictureBox>();
            for(int i = 6; i < 13; i++) {
                if(lists[i].Count() > 1)
                    topCards.Add(lists[i][lists[i].Count() - 1]);
            }
            if (lists[1].Count() > 1)
                topCards.Add(lists[1][lists[1].Count() - 1]);
            return topCards;
        }

        public PictureBox getTopCard(int listNumber) {
            return lists[listNumber][lists[listNumber].Count() - 1];
        }
        
    }
}
