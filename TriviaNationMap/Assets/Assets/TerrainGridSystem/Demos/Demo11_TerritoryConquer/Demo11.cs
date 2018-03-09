using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TGS {
				public class Demo11 : MonoBehaviour {

								TerrainGridSystem tgs;
								GUIStyle labelStyle;

								void Start () {
												// setup GUI styles
												labelStyle = new GUIStyle ();
												labelStyle.alignment = TextAnchor.MiddleCenter;
												labelStyle.normal.textColor = Color.black;

												// Get a reference to Terrain Grid System's API
												tgs = TerrainGridSystem.instance;

												// Set colors for frontiers
												tgs.territoryDisputedFrontierColor = Color.yellow;
												tgs.TerritorySetFrontierColor (0, Color.red);
												tgs.TerritorySetFrontierColor (1, Color.blue);

												// Color for neutral territory
												tgs.TerritoryToggleRegionSurface (2, true, new Color (0.2f, 0.2f, 0.2f));
												tgs.TerritorySetNeutral (2, true);

												// listen to events
												tgs.OnCellClick += (cellIndex, buttonIndex) => changeCellOwner (cellIndex);
								}

								void OnGUI () {
												GUI.Label (new Rect (0, 5, Screen.width, 30), "Click on any cell to transfer it to the opposite territory.", labelStyle);
												GUI.Label (new Rect (0, 20, Screen.width, 30), "Note that territories can't be split between two or more areas.", labelStyle);
												GUI.Label (new Rect (0, 35, Screen.width, 30), "If you need separate areas, just color cells with same 'territory color' and don't use territories.", labelStyle);
												GUI.Label (new Rect (0, 50, Screen.width, 30), "(Territory in gray is marked as neutral)", labelStyle);
								}

								void changeCellOwner (int cellIndex) {
												int currentTerritory = tgs.cells [cellIndex].territoryIndex;
												// Looks for a neighbour territory
												List<Cell> neighbours = tgs.CellGetNeighbours(cellIndex);
												for (int k=0;k<neighbours.Count;k++) {
																if (neighbours[k].territoryIndex != currentTerritory) {
																				currentTerritory = neighbours[k].territoryIndex;
																				break;
																}
												}
												tgs.CellSetTerritory (cellIndex, currentTerritory);
								}


				}
}
