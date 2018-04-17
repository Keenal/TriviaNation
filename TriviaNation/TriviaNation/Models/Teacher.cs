using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaNation.Models.Abstract;
using System.Timers;

namespace TriviaNation.Models

{
    class Teacher

    {
        public static System.Timers.Timer aTimer;

        // game time limit 

        private string gameStartTime;
        private string gameEndTime;

        // start the game timer 
            // log in as a teacher, open the game and set the time, players can play the game only after the teacher has set the time

        public static void startGameTimer()
        {

            int startGame = 5000; // this is 2 second - get the teacher to set this manually
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(startGame); 
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += onTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        

        public static void onTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }

        // end the game timer
            // after the given time, a display message tells that the time is up and the screen should take them to the leaderboard screen


        // question time limit
        // select the question pack
        // select a player
    }

}
