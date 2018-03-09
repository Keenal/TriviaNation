//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
//  REMAINS UNCHANGED.
//
//  Email:  gustavo_franco@hotmail.com
//
//  Copyright (C) 2006 Franco, Gustavo 
//
//  Some modifications by Kronnect to reuse grid buffers between calls and to allow different grid configurations in same grid array (uses bitwise differentiator)
//  Also including support for hexagonal grids and some other improvements

using UnityEngine;
using System;
using System.Collections.Generic;

namespace TGS.PathFinding {

				public class PathFinderFastNonSQR : IPathFinder {
								internal struct PathFinderNodeFast {
												public int F;
												// f = gone + heuristic
												public int G;
												public ushort PX;
												// Parent
												public ushort PY;
												public byte Status;
								}

								// Heap variables are initializated to default, but I like to do it anyway
								private int[] mGrid = null;
								private PriorityQueueB<int> mOpen = null;
								private List<PathFinderNode> mClose = new List<PathFinderNode> ();
								private HeuristicFormula mFormula = HeuristicFormula.Manhattan;
								private bool mDiagonals = true;
								private bool mHexagonalGrid = false;
								private int mHEstimate = 2;
								private bool mHeavyDiagonals = false;
								private int mMaxSteps = 2000;
								private int mMaxSearchCost = 100000;
								private PathFinderNodeFast[] mCalcGrid = null;
								private byte mOpenNodeValue = 1;
								private byte mCloseNodeValue = 2;
								private OnCellCross mOnCellCross = null;

								//Promoted local variables to member variables to avoid recreation between calls
								private int mH = 0;
								private int mLocation = 0;
								private int mNewLocation = 0;
								private ushort mLocationX = 0;
								private ushort mLocationY = 0;
								private ushort mNewLocationX = 0;
								private ushort mNewLocationY = 0;
								private int mCloseNodeCounter = 0;
								private ushort mGridX = 0;
								private ushort mGridY = 0;
								private bool mFound = false;
								private sbyte[,] mDirection = new sbyte[8, 2] {
												{ 0, -1 },
												{ 1, 0 },
												{ 0, 1 },
												{ -1, 0 }, {
																1,
																-1
												}, {
																1,
																1
												}, {
																-1,
																1
												}, {
																-1,
																-1
												}
								};
								private sbyte[,] mDirectionHex0 = new sbyte[6, 2] {
												{ 0, -1 },
												{ 1, 0 },
												{ 0, 1 },
												{ -1, 0 }, {
																1,
																1
												}, {
																-1,
																1
												}
								};
								private sbyte[,] mDirectionHex1 = new sbyte[6, 2] {
												{ 0, -1 },
												{ 1, 0 },
												{ 0, 1 },
												{ -1, 0 }, {
																-1,
																-1
												}, {
																1,
																-1
												}
								};
								private int mEndLocation = 0;
								private int mNewG = 0;
								private int mcellGroupMask = -1;


								public PathFinderFastNonSQR (int[] grid, int gridWidth, int gridHeight) { 
												if (grid == null)
																throw new Exception ("Grid cannot be null");

												mGrid = grid;
												mGridX = (ushort)gridWidth;
												mGridY = (ushort)gridHeight;

												if (mCalcGrid == null || mCalcGrid.Length != (mGridX * mGridY))
																mCalcGrid = new PathFinderNodeFast[mGridX * mGridY];

												mOpen = new PriorityQueueB<int> (new ComparePFNodeMatrix (mCalcGrid));
								}

								public void SetCalcMatrix (int[] grid) {
												if (grid == null)
																throw new Exception ("Grid cannot be null");
												if (grid.Length != mGrid.Length) // mGridX != (ushort) (mGrid.GetUpperBound(0) + 1) || mGridY != (ushort) (mGrid.GetUpperBound(1) + 1))
				throw new Exception ("SetCalcMatrix called with matrix with different dimensions. Call constructor instead.");
												mGrid = grid;

												Array.Clear (mCalcGrid, 0, mCalcGrid.Length);
												ComparePFNodeMatrix comparer = (ComparePFNodeMatrix)mOpen.comparer;
												comparer.SetMatrix (mCalcGrid);
								}

								public int MaxSearchCost {
												get { return mMaxSearchCost; }
												set { mMaxSearchCost = value; }
								}

								public int MaxSteps {
												get { return mMaxSteps; }
												set { mMaxSteps = value; }
								}


								public HeuristicFormula Formula {
												get { return mFormula; }
												set { mFormula = value; }
								}

								public bool Diagonals {
												get { return mDiagonals; }
												set { 
																mDiagonals = value; 
																if (mDiagonals)
																				mDirection = new sbyte[8, 2] {
																								{ 0, -1 },
																								{ 1, 0 },
																								{ 0, 1 },
																								{ -1, 0 },
																								{ 1, -1 },
																								{ 1, 1 },
																								{
																												-1,
																												1
																								},
																								{
																												-1,
																												-1
																								}
																				};
																else
																				mDirection = new sbyte[4, 2]{ { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
												}
								}

								public bool HeavyDiagonals {
												get { return mHeavyDiagonals; }
												set { mHeavyDiagonals = value; }
								}

								public bool HexagonalGrid {
												get { return mHexagonalGrid; }
												set { mHexagonalGrid = value; }
								}

								public int HeuristicEstimate {
												get { return mHEstimate; }
												set { mHEstimate = value; }
								}


								public OnCellCross OnCellCross {
												get { return mOnCellCross; }
												set { mOnCellCross = value; }
								}

								public int cellGroupMask {
												get { return mcellGroupMask; }
												set { mcellGroupMask = value; }
								}

								public List<PathFinderNode> FindPath (PathFindingPoint start, PathFindingPoint end, out int totalCost, bool evenLayout) {
												totalCost = 0;
												mFound = false;
												mCloseNodeCounter = 0;
												int evenLayoutValue = evenLayout ? 1 : 0;
												if (mOpenNodeValue > 250) {
																Array.Clear (mCalcGrid, 0, mCalcGrid.Length);
																mOpenNodeValue = 1;
																mCloseNodeValue = 2;
												} else {
																mOpenNodeValue += 2;
																mCloseNodeValue += 2;
												}
												mOpen.Clear ();
												mClose.Clear ();

												mLocation = (start.Y * mGridX) + start.X;
												mEndLocation = (end.Y * mGridX) + end.X;
												mCalcGrid [mLocation].G = 0;
												mCalcGrid [mLocation].F = mHEstimate;
												mCalcGrid [mLocation].PX = (ushort)start.X;
												mCalcGrid [mLocation].PY = (ushort)start.Y;
												mCalcGrid [mLocation].Status = mOpenNodeValue;

												mOpen.Push (mLocation);
												while (mOpen.Count > 0) {
																mLocation = mOpen.Pop ();

																//Is it in closed list? means this node was already processed
																if (mCalcGrid [mLocation].Status == mCloseNodeValue)
																				continue;

																if (mLocation == mEndLocation) {
																				mCalcGrid [mLocation].Status = mCloseNodeValue;
																				mFound = true;
																				break;
																}

																if (mCloseNodeCounter > mMaxSteps) {
																				return null;
																}

																mLocationX = (ushort)(mLocation % mGridX);
																mLocationY = (ushort)(mLocation / mGridX);

																//Lets calculate each successors
																int maxi;
																if (mHexagonalGrid) {
																				maxi = 6;
																} else {
																				maxi = mDiagonals ? 8 : 4;
																}
																for (int i = 0; i < maxi; i++) {
						
																				if (mHexagonalGrid) {
																								if (mLocationX % 2 == evenLayoutValue) {
																												mNewLocationX = (ushort)(mLocationX + mDirectionHex0 [i, 0]);
																												mNewLocationY = (ushort)(mLocationY + mDirectionHex0 [i, 1]);
																								} else {
																												mNewLocationX = (ushort)(mLocationX + mDirectionHex1 [i, 0]);
																												mNewLocationY = (ushort)(mLocationY + mDirectionHex1 [i, 1]);
																								}
																				} else {
																								mNewLocationX = (ushort)(mLocationX + mDirection [i, 0]);
																								mNewLocationY = (ushort)(mLocationY + mDirection [i, 1]);
																				}

																				if (mNewLocationY >= mGridY)
																								continue;

																				if (mNewLocationX >= mGridX)
																								continue;

																				// Unbreakeable?
																				mNewLocation = (mNewLocationY * mGridX) + mNewLocationX;
																				int gridValue = (mGrid [mNewLocation] & mcellGroupMask) != 0 ? 1 : 0;
																				if (gridValue == 0)
																								continue;

																				// Check custom validator
																				if (mOnCellCross != null) {
																								gridValue += mOnCellCross (mNewLocation);
																				}

																				if (mHeavyDiagonals && i > 3)
																								mNewG = mCalcGrid [mLocation].G + (int)(gridValue * 2.41f);
																				else
																								mNewG = mCalcGrid [mLocation].G + gridValue;

																				if (mNewG > mMaxSearchCost)
																								continue;

																				//Is it open or closed?
																				if (mCalcGrid [mNewLocation].Status == mOpenNodeValue || mCalcGrid [mNewLocation].Status == mCloseNodeValue) {
																								// The current node has less code than the previous? then skip this node
																								if (mCalcGrid [mNewLocation].G <= mNewG)
																												continue;
																				}

																				mCalcGrid [mNewLocation].PX = mLocationX;
																				mCalcGrid [mNewLocation].PY = mLocationY;
																				mCalcGrid [mNewLocation].G = mNewG;

																				int dist = Math.Abs (mNewLocationX - end.X);
																				switch (mFormula) {
																				default:
																				case HeuristicFormula.Manhattan:
																								mH = mHEstimate * (dist + Math.Abs (mNewLocationY - end.Y));
																								break;
																				case HeuristicFormula.MaxDXDY:
																								mH = mHEstimate * (Math.Max (dist, Math.Abs (mNewLocationY - end.Y)));
																								break;
																				case HeuristicFormula.DiagonalShortCut:
																								int h_diagonal = Math.Min (dist, Math.Abs (mNewLocationY - end.Y));
																								int h_straight = (dist + Math.Abs (mNewLocationY - end.Y));
																								mH = (mHEstimate * 2) * h_diagonal + mHEstimate * (h_straight - 2 * h_diagonal);
																								break;
																				case HeuristicFormula.Euclidean:
																								mH = (int)(mHEstimate * Math.Sqrt (Math.Pow (dist, 2) + Math.Pow ((mNewLocationY - end.Y), 2)));
																								break;
																				case HeuristicFormula.EuclideanNoSQR:
																								mH = (int)(mHEstimate * (Math.Pow (dist, 2) + Math.Pow ((mNewLocationY - end.Y), 2)));
																								break;
																				case HeuristicFormula.Custom1:
																								PathFindingPoint dxy = new PathFindingPoint (dist, Math.Abs (end.Y - mNewLocationY));
																								int Orthogonal = Math.Abs (dxy.X - dxy.Y);
																								int Diagonal = Math.Abs (((dxy.X + dxy.Y) - Orthogonal) / 2);
																								mH = mHEstimate * (Diagonal + Orthogonal + dxy.X + dxy.Y);
																								break;
																				}

																				mCalcGrid [mNewLocation].F = mNewG + mH;

																				mOpen.Push (mNewLocation);
																				mCalcGrid [mNewLocation].Status = mOpenNodeValue;
																}

																mCloseNodeCounter++;
																mCalcGrid [mLocation].Status = mCloseNodeValue;
												}

												if (mFound) {
																mClose.Clear ();
																int posX = end.X;
																int posY = end.Y;

																PathFinderNodeFast fNodeTmp = mCalcGrid [(end.Y * mGridX) + end.X];
																totalCost = fNodeTmp.G;
																PathFinderNode fNode;
																fNode.F = fNodeTmp.F;
																fNode.G = fNodeTmp.G;
																fNode.H = 0;
																fNode.PX = fNodeTmp.PX;
																fNode.PY = fNodeTmp.PY;
																fNode.X = end.X;
																fNode.Y = end.Y;

																while (fNode.X != fNode.PX || fNode.Y != fNode.PY) {
																				mClose.Add (fNode);
																				posX = fNode.PX;
																				posY = fNode.PY;
																				fNodeTmp = mCalcGrid [(posY * mGridX) + posX];
																				fNode.F = fNodeTmp.F;
																				fNode.G = fNodeTmp.G;
																				fNode.H = 0;
																				fNode.PX = fNodeTmp.PX;
																				fNode.PY = fNodeTmp.PY;
																				fNode.X = posX;
																				fNode.Y = posY;
																} 

																mClose.Add (fNode);

																return mClose;
												}
												return null;
								}

								internal class ComparePFNodeMatrix : IComparer<int> {
												protected PathFinderNodeFast[] mMatrix;

												public ComparePFNodeMatrix (PathFinderNodeFast[] matrix) {
																mMatrix = matrix;
												}

												public int Compare (int a, int b) {
																if (mMatrix [a].F > mMatrix [b].F)
																				return 1;
																else if (mMatrix [a].F < mMatrix [b].F)
																				return -1;
																return 0;
												}

												public void SetMatrix (PathFinderNodeFast[] matrix) {
																mMatrix = matrix;
												}
								}
				}
}
