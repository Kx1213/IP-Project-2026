DDA

Overview

COOKED is an interactive VR cooking game. Our main goal is to let secondary school student to not only strengthen their theory knowledge, but also practise their practical skill without the cost of ingredients nor the risk of burning their house down.
COOKED itegrated with Firebase functions like user authentication, quiz, leaderboard, etc. Players follow cooking steps in a virtual environment and take quizzes to earn points. A web-based leaderboard tracks users’ points in real-time.

This project consists of:

Unity VR Game
-Authentication
-Instructions guidance ui
-Quiz system integrated with Firebase Realtime Database.
-Door-locking system controlled by authentication and quiz completion.
-Point system throughout the whole game

Web-based Firebase Leaderboard
-Login, registration, and password reset via Firebase Authentication.
-Real-time leaderboard displaying top users and points.
-Clean UI with a background image, responsive layout, and modern styling.

Features

Unity VR Game
-Instruction guidance ui: Guides players through the whole game
-Quiz system:
  -Retrieve questions from Firebase Realtime Database
  -Randomized questions per quiz session
  -Earns 100 points per correct quiz answer (200 points for each correct practical steps)
  -Updates user points in Firebase
-Door Lock Can't enter the quiz room without logging in or signing up, and can't leave the quiz room nor enter the practical room without finishing the quiz. Ensure the correct gameflow
-Firebase Authentication: Log in and Sign up with proper user error message 
-Firebase Realtime Databse: Stores questions for quiz, user accumulated points, user email and user username

Website
-Log in and Sign Up: Email and password Firebase authentication
-Forget Password: Send a password reset email from firebase
-Real-time Leaderboard: Display users' points and username, sorted by points
-Multiplatform: Works on both computers and phones
-Styling: Cooking theme design that are easy to read and intuitive

Setup Instructions

Website
1. Unzip the zipped file
2. Make sure there are 3 files in it. Website Background.jpg, logo.png and IP Website.html
3. Open the IP Website.html with Internet browser, Chrome recommended 

