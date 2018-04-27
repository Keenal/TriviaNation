using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TriviaNation.Models.Abstract;
using System.Timers;

/**
TriviaNation is a networked trivia game designed for use in 
classrooms. Class members are each in control of a nation on 
a map. The goal of the game is to increase the size of the nation 
by winning trivia challenges and defeating other class members 
in contested territories. The focus is on gamifying learning and 
making it an enjoyable experience.


@author Timothy McWatters
@author Keenal Shah
@author Randy Quimby
@author Wesley Easton
@author Wenwen Xu

@version 1.0

CEN3032    "TriviaNation" SEII- Group 1's class project
File Name: Teacher.cs 
*/

namespace TriviaNation.Models
    
{
    class Teacher

    {
        public static System.Timers.Timer aTimer;

        /// <summary>
        /// issues a game timer
        /// </summary>
        public static void countGameTimer()
        {
            int startGame = 2000; // this is 2 second - get the teacher to set this manually
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(startGame); 
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += onTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        
        /// <summary>
        /// elapsed time
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public static void onTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }

        /// <summary>
        /// starts a game timer
        /// </summary>
        public static void startGameTimer() {
            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();

            Console.WriteLine("Terminating the application...");
        }
    }

}
