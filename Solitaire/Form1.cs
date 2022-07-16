using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Solitaire
{
    public partial class Form1 : Form
    {
        PictureBox[] pictureBoxes;
        PictureBox[] acePictureBoxes;
        PictureBox[] emptyPictureBoxes;
        Dictionary<PictureBox, Card> mapping = new Dictionary<PictureBox, Card>();
        Dictionary<PictureBox, Image> imageMapping = new Dictionary<PictureBox, Image>();
        Dictionary<PictureBox, bool> booleanMapping = new Dictionary<PictureBox, bool>();
        Dictionary<PictureBox, Dictionary<PictureBox, int[]>> xIncrements = new Dictionary<PictureBox, Dictionary<PictureBox, int[]>>();
        Dictionary<PictureBox, Dictionary<PictureBox, int[]>> yIncrements = new Dictionary<PictureBox, Dictionary<PictureBox, int[]>>();
        Dictionary<PictureBox, Point> sourcePoint = new Dictionary<PictureBox, Point>();
        Dictionary<PictureBox, Point> destinationPoint = new Dictionary<PictureBox, Point>();
        Dictionary<PictureBox, int> timerCount = new Dictionary<PictureBox, int>();
        Dictionary<PictureBox, List<PictureBox>> cardsToMove = new Dictionary<PictureBox, List<PictureBox>>();
        Table table;
        PictureBox selectedCard = null;
        int previousScore = -52;
        int currentScore = -52;
        bool isEasterEggEnabled = false;

        public Form1()
        {
            InitializeComponent();
            pictureBoxes = new PictureBox [] {  pictureBox1, pictureBox2, pictureBox3, pictureBox4,
                                                pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                                                pictureBox9, pictureBox10, pictureBox11, pictureBox12,
                                                pictureBox13, pictureBox14, pictureBox15, pictureBox16,
                                                pictureBox17, pictureBox18, pictureBox19, pictureBox20,
                                                pictureBox21, pictureBox22, pictureBox23, pictureBox24,
                                                pictureBox25, pictureBox26, pictureBox27, pictureBox28,
                                                pictureBox29, pictureBox30, pictureBox31, pictureBox32,
                                                pictureBox33, pictureBox34, pictureBox35, pictureBox36,
                                                pictureBox37, pictureBox38, pictureBox39, pictureBox40,
                                                pictureBox41, pictureBox42, pictureBox43, pictureBox44,
                                                pictureBox45, pictureBox46, pictureBox47, pictureBox48,
                                                pictureBox49, pictureBox50, pictureBox51, pictureBox52};
            acePictureBoxes = new PictureBox[] { pictureBox56, pictureBox55, pictureBox54, pictureBox53};
            emptyPictureBoxes = new PictureBox[] {   pictureBox57, pictureBox58, pictureBox59, pictureBox60, 
                                                     pictureBox61, pictureBox62, pictureBox63 };
            createMapping();
            createImageMapping();
            createBooleanMapping();
            createAceSpots();
            createEmptySpots();
            shufflePictureBoxes();
            table = new Table(pictureBoxes, acePictureBoxes, emptyPictureBoxes);
            flipAll();
            table.flipTopCards(mapping, imageMapping, booleanMapping);
        }

        private void onClick(object sender, EventArgs e) {
            PictureBox pictureBox = (PictureBox)sender;
            unselectAll();
            int listNumber = table.getListContaining(pictureBox);
            bool isCard = booleanMapping.ContainsKey(pictureBox);
            bool isFlipped = isCard ? booleanMapping[pictureBox] : true;

            if (mapping[pictureBox].getSuit() == 'O' && selectedCard == null);
            else if (listNumber > 1 && listNumber < 6) {
                if (selectedCard != null && mapping[selectedCard].followsAceStack(mapping[pictureBox]) && isFlipped) {
                    int sourceList = table.getListContaining(selectedCard);
                    int destinationList = table.getListContaining(pictureBox);
                    if (table.isLastCardInList(sourceList, selectedCard)) {
                        sourcePoint[selectedCard] = selectedCard.Location;
                        destinationPoint[selectedCard] = new Point(Table.xPositions[destinationList], table.getNextOffset(destinationList));
                        moveTimer.Start();
                        cardsToMove[selectedCard] = table.moveCards(sourceList, destinationList, 1);
                    }
                    unselectAll();
                    selectedCard = null;
                } else if(selectedCard == null) {
                    selectedCard = pictureBox;
                    pictureBox.BorderStyle = pictureBox.BorderStyle == BorderStyle.None ? BorderStyle.Fixed3D : BorderStyle.None;
                } else {
                    unselectAll();
                    selectedCard = null;
                }
                currentScore = previousScore + table.countAceStack() * 5;
                scoreTextBox.Text = "Score: ";
                scoreTextBox.Text += currentScore < 0 ? "-" : "";
                scoreTextBox.Text += "$" + Math.Abs(currentScore).ToString();

            } else if (listNumber >= 6 && emptyPictureBoxes.Contains(pictureBox) && selectedCard != null && isFlipped) {
                int sourceList = table.getListContaining(selectedCard);
                int destinationList = table.getListContaining(pictureBox);

                if (mapping[selectedCard].getValue() == 12) {
                    sourcePoint[selectedCard] = selectedCard.Location;
                    destinationPoint[selectedCard] = new Point(Table.xPositions[destinationList], table.getNextOffset(destinationList));
                    moveTimer.Start();
                    cardsToMove[selectedCard] = table.moveCards(sourceList, destinationList, table.getSize(sourceList) - table.isInPosition(sourceList, selectedCard));
                }
                unselectAll();
                selectedCard = null;
            }
            else if(selectedCard != pictureBox && (isFlipped || listNumber == 0)) {
                pictureBox.BorderStyle = pictureBox.BorderStyle == BorderStyle.None ? BorderStyle.Fixed3D : BorderStyle.None;
                if (table.getList()[0].Contains(pictureBox)) { //If pictureBox is in the main pile
                    sourcePoint[pictureBox] = new Point(Table.xPositions[0], Table.yPositions[0]);
                    destinationPoint[pictureBox] = new Point(Table.xPositions[1], Table.yPositions[1]);
                    moveTimer.Start();
                    cardsToMove[pictureBox] = table.moveCards(0, 1, 1);
                    unselectAll();
                } else if (listNumber == 1 && selectedCard != null) {
                    selectedCard = null;
                    unselectAll();
                } else {
                    if (selectedCard != null) {
                        int sourceList = table.getListContaining(selectedCard);
                        int destinationList = table.getListContaining(pictureBox);
                        if (table.isLastCardInList(destinationList, pictureBox)){ //Selected card is last card in list
                            if (mapping[selectedCard].follows(mapping[pictureBox])) { //Can selectedCard be placed on pictureBox
                                sourcePoint[selectedCard] = selectedCard.Location;
                                destinationPoint[selectedCard] = new Point(Table.xPositions[destinationList], table.getNextOffset(destinationList));
                                moveTimer.Start();
                                cardsToMove[selectedCard] = table.moveCards(sourceList, destinationList, table.getSize(sourceList) - table.isInPosition(sourceList, selectedCard));
                            }
                        }
                        unselectAll();
                        selectedCard = null;

                    }
                    else
                        selectedCard = pictureBox;
                }
            } else {
                unselectAll();
                selectedCard = null;
            }
            table.flipTopCards(mapping, imageMapping, booleanMapping);
        }

        private void unselectAll() {
            foreach(PictureBox pictureBox in pictureBoxes)
                pictureBox.BorderStyle = BorderStyle.None;
            foreach(PictureBox pictureBox in acePictureBoxes)
                pictureBox.BorderStyle = BorderStyle.None;
            foreach(PictureBox pictureBox in emptyPictureBoxes)
                pictureBox.BorderStyle = BorderStyle.None;
        }

        private void createMapping() {
            Card[] deck = new Deck().getDeck();
            int index = 0;
            foreach(PictureBox pictureBox in pictureBoxes) {
                mapping.Add(pictureBox, deck[index++]);
            }
        }

        private void shufflePictureBoxes() {
            Random rnd = new Random();
            pictureBoxes = pictureBoxes.OrderBy(x => rnd.Next()).ToArray();
            foreach(PictureBox pictureBox in pictureBoxes) {
                pictureBox.BringToFront();
            }
        }

        private void createAceSpots() {
            mapping.Add(pictureBox53, new Card('O', -1, 'A'));
            mapping.Add(pictureBox54, new Card('O', -1, 'B'));
            mapping.Add(pictureBox55, new Card('O', -1, 'C'));
            mapping.Add(pictureBox56, new Card('O', -1, 'D'));
        }

        private void createEmptySpots() {
            mapping.Add(pictureBox57, new Card('O', 11, 'E'));
            mapping.Add(pictureBox58, new Card('O', 11, 'F'));
            mapping.Add(pictureBox59, new Card('O', 11, 'G'));
            mapping.Add(pictureBox60, new Card('O', 11, 'H'));
            mapping.Add(pictureBox61, new Card('O', 11, 'I'));
            mapping.Add(pictureBox62, new Card('O', 11, 'J'));
            mapping.Add(pictureBox63, new Card('O', 11, 'K'));
        }

        private void createImageMapping() {
            foreach (PictureBox pictureBox in pictureBoxes)
                imageMapping[pictureBox] = pictureBox.Image;
        }

        private void createBooleanMapping() {
            foreach (PictureBox pictureBox in pictureBoxes)
                booleanMapping[pictureBox] = false;
        }

        private void flipAll() { 
            foreach(PictureBox pictureBox in pictureBoxes)
                pictureBox.Image = Properties.Resources.back_of_card1;
        }

        private void formClick(object sender, EventArgs e) {
            unselectAll();
            selectedCard = null;
        }

        private void resetButton_Click(object sender, EventArgs e) {
            selectedCard = null;
            shufflePictureBoxes();
            table = new Table(pictureBoxes, acePictureBoxes, emptyPictureBoxes);
            flipAll();
            table.flipTopCards(mapping, imageMapping, booleanMapping);
            previousScore = currentScore - 52;
            currentScore = previousScore;
            scoreTextBox.Text = "Score: ";
            scoreTextBox.Text += currentScore < 0 ? "-" : "";
            scoreTextBox.Text += "$" + Math.Abs(currentScore).ToString();
        }

        private void openColorDialog(object sender, EventArgs e) {
            colorDialog1.Color = BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
                BackColor = colorDialog1.Color;
        }

        private void moveTimer_Tick(object sender, EventArgs e){
            foreach (PictureBox firstCard in destinationPoint.Keys) {
                if (!timerCount.ContainsKey(firstCard))
                    timerCount[firstCard] = 0;

                int deltaX = destinationPoint[firstCard].X - sourcePoint[firstCard].X;
                int deltaY = destinationPoint[firstCard].Y - sourcePoint[firstCard].Y;

                if (timerCount[firstCard] == 0) {
                    int count = 0;
                    xIncrements[firstCard] = new Dictionary<PictureBox, int[]>();
                    yIncrements[firstCard] = new Dictionary<PictureBox, int[]>();
                    foreach (PictureBox pictureBox in cardsToMove[firstCard]) {
                        xIncrements[firstCard].Add(pictureBox, new int[10]);
                        yIncrements[firstCard].Add(pictureBox, new int[10]);
                        for (int i = 0; i < 9; i++) {
                            xIncrements[firstCard][pictureBox][i] = pictureBox.Location.X + deltaX * (i + 1) / 10;
                            yIncrements[firstCard][pictureBox][i] = pictureBox.Location.Y + deltaY * (i + 1) / 10;
                        }
                        xIncrements[firstCard][pictureBox][9] = destinationPoint[firstCard].X;
                        yIncrements[firstCard][pictureBox][9] = destinationPoint[firstCard].Y + 38 * count++;
                    }
                }

                foreach (PictureBox pictureBox in cardsToMove[firstCard]) {
                    pictureBox.Location = new Point(xIncrements[firstCard][pictureBox][timerCount[firstCard]], yIncrements[firstCard][pictureBox][timerCount[firstCard]]);
                }
                timerCount[firstCard]++;
                if (cardsToMove[firstCard][0].Location.Equals(destinationPoint[firstCard]) || timerCount[firstCard] == 10) {
                    cardsToMove.Remove(firstCard);
                    if(cardsToMove.Count() == 0) {
                        destinationPoint = new Dictionary<PictureBox, Point>();
                        sourcePoint = new Dictionary<PictureBox, Point>();
                        moveTimer.Stop();
                    }
                    timerCount.Remove(firstCard);
                }
            }
        }

        private void automaticMove() {
            bool addedCards;
            do {
                addedCards = false;
                List<PictureBox> topCards = table.getTopCards();
                foreach (PictureBox card in topCards) {
                    int sourceList = table.getListContaining(card);
                    int destinationList = 0;
                    for (int i = 5; i > 1; i--) {
                        destinationList = mapping[card].followsAceStack(mapping[table.getTopCard(i)]) ? i : destinationList;
                        addedCards |= mapping[card].followsAceStack(mapping[table.getTopCard(i)]);
                    }
                    if (destinationList != 0) {
                        cardsToMove[card] = table.moveCards(sourceList, destinationList, 1);
                        sourcePoint[card] = card.Location;
                        destinationPoint[card] = new Point(Table.xPositions[destinationList], Table.yPositions[destinationList]);
                    }
                    table.flipTopCards(mapping, imageMapping, booleanMapping);
                }
            } while(addedCards);
            if(cardsToMove.Count() != 0)
                moveTimer.Start();
            currentScore = previousScore + table.countAceStack() * 5;
            scoreTextBox.Text = "Score: ";
            scoreTextBox.Text += currentScore < 0 ? "-" : "";
            scoreTextBox.Text += "$" + Math.Abs(currentScore).ToString();
        }

        private void rightClick(object sender, MouseEventArgs e) { 
            switch(e.Button) {
                case MouseButtons.Left:
                    break;
                case MouseButtons.Right:
                    automaticMove();
                    break;

            }
        }

        private void uno(object sender, EventArgs e) {
            foreach(PictureBox pictureBox in booleanMapping.Keys) {
                if (!booleanMapping[pictureBox] && !isEasterEggEnabled)
                    pictureBox.Image = Properties.Resources.easter_egg;
                else if (!booleanMapping[pictureBox])
                    pictureBox.Image = Properties.Resources.back_of_card1;
            }

            isEasterEggEnabled = !isEasterEggEnabled;
        }
    }
}
