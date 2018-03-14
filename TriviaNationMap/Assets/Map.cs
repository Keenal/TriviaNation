using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
            QuestionTable QT = new QuestionTable();
            QT.CreateTable();
            Debug.Log("The table exists" + QT.TableExists());
            IDataEntry question1 = new Question("This is question1", "This is answer1");
            Debug.Log(question1);
            IDataEntry question2 = new Question("This is question2", "This is answer2");
            Debug.Log(question2);
            IDataEntry question3 = new Question("This is question3", "This is answer3");
            Debug.Log(question3);
            QT.InsertRowIntoTable(question1);
            QT.InsertRowIntoTable(question2);
            QT.InsertRowIntoTable(question3);
            Debug.Log("The number of rows in this table are: " + QT.RetrieveNumberOfRowsInTable());
            Debug.Log(QT.RetrieveTableRow(1));
            Debug.Log(QT.RetrieveTableRow(2));
            Debug.Log(QT.RetrieveTableRow(3));
            Debug.Log("The number of cols in this table are: " + QT.RetriveNumberOfColsInTable());
            QT.DeleteRowFromTable("This is question1");
            Debug.Log("The number of rows in this table are now: " + QT.RetrieveNumberOfRowsInTable());
            Debug.Log(QT.RetrieveTableRow(1));
            Debug.Log(QT.RetrieveTableRow(2));
            Debug.Log(QT.RetrieveTableRow(3));

            Console.WriteLine("Press any key to end the program");
            //Console.ReadKey();
        }

	}
}
