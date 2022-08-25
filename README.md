# Solitaire
This app is meant to be a very simple demonstration of my skills. As of right now, it's on version 1.0.0. My goal is to add updates to further polish it, but it is currently a fully functioning game of Solitaire.

This is a Visual Studio .NET Framework project written in C#. If you want to play the game yourself, you can download the installer [here](https://github.com/cameron-blank/Solitaire/releases/tag/1.0.0).

### Below are gifs of the gameplay demonstrating some of the features of this app:
General Gameplay:
![GitHub General Gameplay](https://user-images.githubusercontent.com/108784504/179337017-1df94fd0-29af-4f50-81ac-59233621bec4.gif)

Right clicking automatically moves cards from the tableau to one of the four foundations (the piles at the top). The choppy gif doesn't exactly do it justice, but the cards move pretty smoothly:
![GitHub General Gameplay](https://user-images.githubusercontent.com/108784504/179337237-42ccd659-44b9-4f4d-bf68-2f4f74ccc159.gif)

Because this game is aimed at high-class individuals, I included a background color customizer:
![GitHub General Gameplay](https://user-images.githubusercontent.com/108784504/179337388-c091e9fa-3b28-4561-9ac7-c578f83b085a.gif)

### A few final notes:
As you can see in the gifs, the game keeps track of your score at the top left. The math is simple: each card put in one of the foundations wins you $5. Each game costs you $52. A new game can be started with the menu in the top left-hand corner:

![image](https://user-images.githubusercontent.com/108784504/179337452-9da3f2cb-a171-4c03-ab1c-60569ac1f511.png)

#### My plans for future updates:
- Implement dragging and dropping of cards. This current version does not allow for this, and you're required to first click on the card you want to move, and then click on where you want it to be placed.
- Improve the visual appeal of the game. This can be done in several ways:
  - Add a slight light-to-dark gradient to the background color.
  - Find nicer looking card pictures.
  - When cards are being snapped from one place to another, give them some weight by having an acceleration at the beginning and a deceleration at the end.
  - Change the size and spacing of the cards when the window size is changed.
  - Add a nice animation that plays when you win.
  
