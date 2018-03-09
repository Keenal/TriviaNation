using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TGS {

				/// <summary>
				/// Marks random cells as unpassable
				/// </summary>
				public class Obstacles : MonoBehaviour {

								// Use this for initialization
								void Start () {
												TerrainGridSystem tgs = TerrainGridSystem.instance;
												for (int k = 0; k < 1500; k++) {
																int cellIndex = Random.Range (0, tgs.cellCount);
																tgs.CellToggleRegionSurface (cellIndex, true, Color.red);
																tgs.CellSetCanCross (cellIndex, false);
												}
	
								}

				}
}




