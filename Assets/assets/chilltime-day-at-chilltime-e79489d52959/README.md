# Chilltime - Day at Chilltime / Remote Technical Exercise

This exercise is related to a Technical exercise for candidates applying to a Software Development position at Chilltime. This exercise can be done remotely or at our office locations.

The exercise is related to a Card Matching puzzle, as shown below.

![Annex I](https://bitbucket.org/chilltime/day-at-chilltime/raw/d10ab1e15c1fd8732a165f20d52569ed9de50008/annex/annex_1.png)

The objective of the puzzle is to identify **three** identical cards in the **minimum amount of moves and time**.

You can use any of these technologies to complete the exercise: `React JS (preferred)`, `Unity 3D (preferred)`, `React Native` or `HTML (Javascript, jQuery, ...)`.

## Summary of the exercise:
- Exercise A: Card Matching puzzle, matching three identical cards and receiving a score. **(Minimum requirement)**
- Exercise B: Leaderboard system that shows score, number of moves and times of each user.
- Exercise C: Persistent state of the game that allows multiple players with different puzzles.


## Recommended Software for this Exercise:
- Visual Studio Code - https://code.visualstudio.com/Download
- Node JS - https://nodejs.org/en/download

## Setup

To start the exercise download the files from the git repository to a local folder:
https://bitbucket.org/chilltime/day-at-chilltime/downloads/

Open the project folder in `Visual Studio Code` and then run the following two commands in the Command Prompt of the directory:
```
npm install
npm start
```

## Exercise Questions

### A. Development - Frontend
1. Prepare the Interface according to the image shown in Annex I below.
2. Cards should be randomly positioned at start and hidden.
3. A person can then click on three cards, if those cards are correctly matched then they should remain visible.
4. On every try (of 3 cards) the number of moves should increased by one.
5. The time elapsed should continue to count as the game continues.
6. The final score of the player should be: (Number of Moves x 5) + Total elapsed time in seconds.
7. Once the player completes all of the possible matches, the board should say "Congratulations" with a "Play Again!" button.

#### Annex I:
![Annex I](https://bitbucket.org/chilltime/day-at-chilltime/raw/d10ab1e15c1fd8732a165f20d52569ed9de50008/annex/annex_1.png)

### B. Leaderboard - Frontend
1. Create a Interface for the Leaderboard, shown in Annex II below.
2. Each line of the leaderboard should contain the Player Name, the number of moves, total time elapsed and final score.ed.

#### Annex II:
![Annex II](https://bitbucket.org/chilltime/day-at-chilltime/raw/a8b8abbd7455557a3d26fe624e1afc9cde15aa4b/annex/annex_2.png)

### C. Development - Backend
1. Create a interface as shown in Annex III below, where a player can add his Codename and have a Puzzle board generated for him.
2. A persistent state of the game should be saved for each player, this can be a local variable (local storage, session, cookie) or a remote server (python, php, node) with a local data file or database.
3. If a user adds a codename that already exists, than the previous incomplete Puzzle Board should appear.
4. At the end of the game, the score of the player should be added to the Leaderboard.

#### Annex III:
![Annex III](https://bitbucket.org/chilltime/day-at-chilltime/raw/a8b8abbd7455557a3d26fe624e1afc9cde15aa4b/annex/annex_3.png)


## Final notes:

If you did this exercise remotely, create a private git repository or Google Drive directory with your code and add it to your application process or send it careers@chilltime.com
