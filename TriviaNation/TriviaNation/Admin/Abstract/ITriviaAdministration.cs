using System.Collections.Generic;

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
File Name: ITriviaAdministration.cs 
*/
namespace TriviaNation
{
    /// <summary>
    /// Interface for the TriviaAdministration class.  Extends IDataEntry.
    /// </summary>
    public interface ITriviaAdministration : IDataEntry
    {
        void AddQuestion(string question, string answer, string questionType);

        void DeleteQuestion(int questionNumber);

        IEnumerable<IQuestion> ListQuestions();
        IQuestion GetEditableQuestion(int questionNumber);
        void InsertEditedQuestion(IQuestion editedQuestion);
    }
}
