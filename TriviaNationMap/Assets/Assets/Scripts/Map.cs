using UnityEngine;
using System.Data;
using I18N;
using I18N.West;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TriviaNation;
using System;

namespace TGS
{
	public class Map : MonoBehaviour
    {
		public Texture2D textureForCells;
		TerrainGridSystem tgs;

		void Start ()
        {
            // Get a reference to TGS system's API
            Debug.Log("TEST\n\n");
            Console.WriteLine("TEST\n\n");

			tgs = TerrainGridSystem.instance;

            // Read texture colors
            Color32[] colors = textureForCells.GetPixels32();

			// Iterate cells and picks the corresponding color in the texture
			int cellCount = tgs.cells.Count;
			for (int k=0; k< cellCount; k++) {
				Vector2 cellCenter = tgs.cells[k].center;

                // Convert the center to texture coordinates
                // The center is in the range of -0.5..0.5, so we add 0.5
                //and multiply by the texture width in pixels to get the X texture coordinate

				int px = (int)((cellCenter.x + 0.5f) * textureForCells.width);
				// Same for Y
				int py = (int)((cellCenter.y + 0.5f) * textureForCells.height);

				// Now get the color
				Color32 color = colors[py * textureForCells.width + px];

				// And assign it to the cell
				tgs.CellToggleRegionSurface(k, true, color);
			}
            tgs.TerritorySetVisible(1, false);
            //tgs.TerritorySetNeutral(1, true);
            tgs.TerritoryToggleRegionSurface(1, false, Color.clear);
            
            

            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            // Corrected for interface use (also required for my classes ~Randy)
            IDataBaseTable QT = new QuestionTable();
            QT.CreateTable(QT.TableName, QT.TableCreationString);
            Debug.Log("The table exists: " + QT.TableExists(QT.TableName));
            IQuestion question = new Questions();
            /* This (ITriviaAdministration) IS an IDataEntry interface.  
             * Interface inherits the IDataEntry inteferface. For future 
             * implementation. Will make interfaces granular, versatile, 
             * and easy to change. 
             */
            ITriviaAdministration admin = new TriviaAdministration(question, QT);

            // Same as InsertRowIntoTable method call had here before.
            admin.AddQuestion("Test", "Yup", "Question Type: MC (Test)");
            admin.AddQuestion("Working?", "Affirmitive", "Question Type: T/F (Test)");
            admin.AddQuestion("No more objects necessary?", "Fer Shizzle", "Question Type: Matching (Test)");

            Debug.Log("The number of rows in this table are: " + QT.RetrieveNumberOfRowsInTable());

            // Takes the place of all the RetriveTableRow method calls for output
            string test = admin.ListQuestions();
            Debug.Log(test);

            Debug.Log("The number of cols in this table are: " + QT.RetriveNumberOfColsInTable());

            // Replaces the "QT.DeleteRowFromTable("This is question1");"
            admin.DeleteQuestion(1);

            Debug.Log("The number of rows in this table are now: " + QT.RetrieveNumberOfRowsInTable());

            // Takes the place of all the RetrieveTableRow method calls for output
            test = admin.ListQuestions();
            Debug.Log(test);
        }

	}
}
