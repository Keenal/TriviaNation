using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using TGS.Geom;
using TGS.PathFinding;

namespace TGS {
	
				/* Event definitions */

				public delegate int PathFindingEvent (int cellIndex);


				public partial class TerrainGridSystem : MonoBehaviour {

								/// <summary>
								/// Fired when path finding algorithmn evaluates a cell. Return the increased cost for cell.
								/// </summary>
								public event PathFindingEvent OnPathFindingCrossCell;

	
								[SerializeField]
								HeuristicFormula
												_pathFindingHeuristicFormula = HeuristicFormula.EuclideanNoSQR;

								/// <summary>
								/// The path finding heuristic formula to estimate distance from current position to destination
								/// </summary>
								public PathFinding.HeuristicFormula pathFindingHeuristicFormula {
												get { return _pathFindingHeuristicFormula; }
												set {
																if (value != _pathFindingHeuristicFormula) {
																				_pathFindingHeuristicFormula = value;
																				isDirty = true;
																}
												}
								}

								[SerializeField]
								int
												_pathFindingMaxCost = 200000;

								/// <summary>
								/// The maximum search cost of the path finding execution.
								/// </summary>
								public int pathFindingMaxCost {
												get { return _pathFindingMaxCost; }
												set {
																if (value != _pathFindingMaxCost) {
																				_pathFindingMaxCost = value;
																				isDirty = true;
																}
												}
								}

		
								[SerializeField]
								int
												_pathFindingMaxSteps = 2000;

								/// <summary>
								/// The maximum number of steps that a path can return.
								/// </summary>
								public int pathFindingMaxSteps {
												get { return _pathFindingMaxSteps; }
												set {
																if (value != _pathFindingMaxSteps) {
																				_pathFindingMaxSteps = value;
																				isDirty = true;
																}
												}
								}

		
		
								[SerializeField]
								bool
												_pathFindingUseDiagonals = true;

								/// <summary>
								/// If path can include diagonals between cells
								/// </summary>
								public bool pathFindingUseDiagonals {
												get { return _pathFindingUseDiagonals; }
												set {
																if (value != _pathFindingUseDiagonals) {
																				_pathFindingUseDiagonals = value;
																				isDirty = true;
																}
												}
								}

								[SerializeField]
								bool
												_pathFindingHeavyDiagonals = false;

								/// <summary>
								/// If diagonals have extra cost.
								/// </summary>
								public bool pathFindingHeavyDiagonals {
												get { return _pathFindingHeavyDiagonals; }
												set {
																if (value != _pathFindingHeavyDiagonals) {
																				_pathFindingHeavyDiagonals = value;
																				isDirty = true;
																}
												}
								}


								#region Public Path Finding functions

								/// <summary>
								/// Returns an optimal path from startPosition to endPosition with options.
								/// </summary>
								/// <returns>The route consisting of a list of cell indexes.</returns>
								/// <param name="startPosition">Start position in map coordinates (-0.5...0.5)</param>
								/// <param name="endPosition">End position in map coordinates (-0.5...0.5)</param>
								/// <param name="maxSearchCost">Maximum search cost for the path finding algorithm. A value of 0 will use the global default defined by pathFindingMaxCost</param>
								/// <param name="maxSteps">Maximum steps for the path. A value of 0 will use the global default defined by pathFindingMaxSteps</param>
								public List<int> FindPath (int cellIndexStart, int cellIndexEnd, int maxSearchCost = 0, int maxSteps = 0, int cellGroupMask = -1) {
												int dummy;
												return FindPath (cellIndexStart, cellIndexEnd, out dummy, maxSearchCost, maxSteps, cellGroupMask);
								}

								/// <summary>
								/// Returns an optimal path from startPosition to endPosition with options.
								/// </summary>
								/// <returns>The route consisting of a list of cell indexes.</returns>
								/// <param name="startPosition">Start position in map coordinates (-0.5...0.5)</param>
								/// <param name="endPosition">End position in map coordinates (-0.5...0.5)</param>
								/// <param name="totalCost">The total accumulated cost for the path</param>
								/// <param name="maxSearchCost">Maximum search cost for the path finding algorithm. A value of 0 will use the global default defined by pathFindingMaxCost</param>
								/// <param name="maxSteps">Maximum steps for the path. A value of 0 will use the global default defined by pathFindingMaxSteps</param>
								public List<int> FindPath (int cellIndexStart, int cellIndexEnd, out int totalCost, int maxSearchCost = 0, int maxSteps = 0, int cellGroupMask = -1) {
												totalCost = 0;
												Cell startCell = cells [cellIndexStart];
												Cell endCell = cells [cellIndexEnd];
												PathFindingPoint startingPoint = new PathFindingPoint (startCell.column, startCell.row);
												PathFindingPoint endingPoint = new PathFindingPoint (endCell.column, endCell.row);
												List<int> routePoints = null;
			
												// Minimum distance for routing?
												if (Mathf.Abs (endingPoint.X - startingPoint.X) > 0 || Mathf.Abs (endingPoint.Y - startingPoint.Y) > 0) {
																ComputeRouteMatrix ();
																finder.Formula = _pathFindingHeuristicFormula;
																finder.MaxSearchCost = maxSearchCost > 0 ? maxSearchCost : _pathFindingMaxCost;
																finder.MaxSteps = maxSteps > 0 ? maxSteps : _pathFindingMaxSteps;
																finder.Diagonals = _pathFindingUseDiagonals;
																finder.HeavyDiagonals = _pathFindingHeavyDiagonals;
																finder.HexagonalGrid = _gridTopology == GRID_TOPOLOGY.Hexagonal;
																finder.cellGroupMask = cellGroupMask;
																if (OnPathFindingCrossCell != null) {
																				finder.OnCellCross = FindRoutePositionValidator;
																} else {
																				finder.OnCellCross = null;
																}
																List<PathFinderNode> route = finder.FindPath (startingPoint, endingPoint, out totalCost, _evenLayout);
																if (route != null) {
																				routePoints = new List<int> (route.Count);
																				for (int r = route.Count - 2; r >= 0; r--) {
																								routePoints.Add (route [r].PY * _cellColumnCount + route [r].PX);
																				}
																				routePoints.Add (cellIndexEnd);
																} else {
																				return null;	// no route available
																}
												}
												return routePoints;
								}

								#endregion


	
				}
}

