//#define SHOW_DEBUG_GIZMOS
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TGS.Geom;
using TGS.Poly2Tri;

namespace TGS {
				[ExecuteInEditMode]
				[Serializable]
				public partial class TerrainGridSystem : MonoBehaviour {
								
								// internal fields
								const double MIN_VERTEX_DISTANCE = 0.002;
								const double SQR_MIN_VERTEX_DIST = MIN_VERTEX_DISTANCE * MIN_VERTEX_DISTANCE;
								const double SQR_MAX_VERTEX_DIST = 10 * SQR_MIN_VERTEX_DIST;
								const string SKW_NEAR_CLIP_FADE = "NEAR_CLIP_FADE";
								const string SKW_TEX_HIGHLIGHT_ADDITIVE = "TGS_TEX_HIGHLIGHT_ADDITIVE";
								const string SKW_TEX_HIGHLIGHT_MULTIPLY = "TGS_TEX_HIGHLIGHT_MULTIPLY";
								const string SKW_TEX_HIGHLIGHT_COLOR = "TGS_TEX_HIGHLIGHT_COLOR";
								const string SKW_TEX_HIGHLIGHT_SCALE = "TGS_TEX_HIGHLIGHT_SCALE";

								Rect canvasRect = new Rect (-0.5f, -0.5f, 1, 1);

								// Custom inspector stuff
								public const int MAX_TERRITORIES = 512;
								public int maxCellsSqrt = 100;
								public bool isDirty;
								public const int MAX_CELLS_FOR_CURVATURE = 500;
								public const int MAX_CELLS_FOR_RELAXATION = 1000;

								// Materials and resources
								Material territoriesMat, cellsThinMat, cellsGeoMat, hudMatTerritoryOverlay, hudMatTerritoryGround, hudMatCellOverlay, hudMatCellGround;
								Material territoriesDisputedMat;
								Material coloredMatGround, coloredMatOverlay;
								Material texturizedMatGround, texturizedMatOverlay;

								// Cell mesh data
								const string CELLS_LAYER_NAME = "Cells";
								Vector3[][] cellMeshBorders;
								int[][] cellMeshIndices;
								Dictionary<Segment,Region> cellNeighbourHit;
								float meshStep;
								bool recreateCells, recreateTerritories;
								Dictionary<int,Cell> cellTagged;
								bool needUpdateTerritories;

								// Territory mesh data
								const string TERRITORIES_LAYER_NAME = "Territories";
								Dictionary<Segment,Region> territoryNeighbourHit;
								List<Segment> territoryFrontiers;
								List<Territory> _sortedTerritories;
								List<TerritoryMesh> territoryMeshes;

								// Common territory & cell structures
								List<Vector3> frontiersPoints;
								Dictionary<Segment, bool> segmentHit;
								List<TriangulationPoint> steinerPoints;
								Dictionary<TriangulationPoint, int> surfaceMeshHit;
								List<Vector3> meshPoints;
								int[] triNew;
								int triNewIndex;
								int newPointsCount;

								// Terrain data
								float[,] terrainHeights;
								float[] terrainRoughnessMap;
								float[] tempTerrainRoughnessMap;
								int terrainRoughnessMapWidth, terrainRoughnessMapHeight;
								int heightMapWidth, heightMapHeight;
								const int TERRAIN_CHUNK_SIZE = 8;
								float effectiveRoughness;
								// = _gridRoughness * terrainHeight

								// Placeholders and layers
								GameObject territoryLayer;
								GameObject _surfacesLayer;

								GameObject surfacesLayer {
												get {
																if (_surfacesLayer == null)
																				CreateSurfacesLayer ();
																return _surfacesLayer;
												}
								}

								GameObject _highlightedObj;
								GameObject cellLayer;

								// Caches
								Dictionary<int, GameObject> surfaces;
								Dictionary<Cell, int> _cellLookup;
								int lastCellLookupCount = -1;
								Dictionary<Territory, int> _territoryLookup;
								int lastTerritoryLookupCount = -1;
								Dictionary<Color, Material> coloredMatCacheGround;
								Dictionary<Color, Material> coloredMatCacheOverlay;
								Dictionary<Color, Material> frontierColorCache;
								Color[] factoryColors;
								bool refreshCellMesh, refreshTerritoriesMesh;
								List<Cell> sortedCells;
								bool shouldRedraw;

								// Z-Figther & LOD
								Vector3 lastLocalPosition, lastCamPos, lastPos;
								float lastGridElevation, lastGridCameraOffset;
								float terrainWidth;
								float terrainHeight;
								float terrainDepth;

								// Interaction
								static TerrainGridSystem _instance;
								public bool mouseIsOver;
								Territory _territoryHighlighted;
								int	_territoryHighlightedIndex = -1;
								Cell _cellHighlighted;
								int _cellHighlightedIndex = -1;
								float highlightFadeStart;
								int _territoryLastClickedIndex = -1, _cellLastClickedIndex = -1;
								int _territoryLastOverIndex = -1, _cellLastOverIndex = -1;
								Territory _territoryLastOver;
								Cell _cellLastOver;
								bool canInteract;
								List<string> highlightKeywords;

								// Misc
								int _lastVertexCount = 0;
								Color[] mask;
								bool useEditorRay;
								Ray editorRay;

								public int lastVertexCount { get { return _lastVertexCount; } }

								Dictionary<Cell, int>cellLookup {
												get {
																if (_cellLookup != null && cells.Count == lastCellLookupCount)
																				return _cellLookup;
																if (_cellLookup == null) {
																				_cellLookup = new Dictionary<Cell,int> ();
																} else {
																				_cellLookup.Clear ();
																}
																lastCellLookupCount = cells.Count;
																for (int k = 0; k < lastCellLookupCount; k++) {
																				_cellLookup.Add (cells [k], k);
																}
																return _cellLookup;
												}
								}


								bool territoriesAreUsed {
												get { return (_showTerritories || _colorizeTerritories || _highlightMode == HIGHLIGHT_MODE.Territories); }
								}

								List<Territory>sortedTerritories {
												get {
																if (_sortedTerritories.Count != territories.Count) {
																				_sortedTerritories.AddRange (territories);
																				_sortedTerritories.Sort (delegate(Territory x, Territory y) {
																								return x.region.rect2DArea.CompareTo (y.region.rect2DArea);
																				});
																}
																return _sortedTerritories;
												}
												set {
																_sortedTerritories = value;
												}
								}

								Dictionary<Territory, int>territoryLookup {
												get {
																if (_territoryLookup != null && territories.Count == lastTerritoryLookupCount)
																				return _territoryLookup;
																if (_territoryLookup == null) {
																				_territoryLookup = new Dictionary<Territory,int> ();
																} else {
																				_territoryLookup.Clear ();
																}
																int terrCount = territories.Count;
																for (int k = 0; k < terrCount; k++) {
																				_territoryLookup.Add (territories [k], k);
																}
																lastTerritoryLookupCount = territories.Count;
																return _territoryLookup;
												}
								}

								Material cellsMat {
												get {
																if (_cellBorderThickness > 1)
																				return cellsGeoMat;
																else
																				return cellsThinMat;
												}
								}


								#region Gameloop events

								void OnEnable () {
												if (cells == null || territories == null) {
																Init ();
												}
												if (hudMatTerritoryOverlay != null && hudMatTerritoryOverlay.color != _territoryHighlightColor) {
																hudMatTerritoryOverlay.color = _territoryHighlightColor;
												}
												if (hudMatTerritoryGround != null && hudMatTerritoryGround.color != _territoryHighlightColor) {
																hudMatTerritoryGround.color = _territoryHighlightColor;
												}
												if (hudMatCellOverlay != null && hudMatCellOverlay.color != _cellHighlightColor) {
																hudMatCellOverlay.color = _cellHighlightColor;
												}
												if (hudMatCellGround != null && hudMatCellGround.color != _cellHighlightColor) {
																hudMatCellGround.color = _cellHighlightColor;
												}
												if (territoriesMat != null && territoriesMat.color != _territoryFrontierColor) {
																territoriesMat.color = _territoryFrontierColor;
												}
												if (_territoryDisputedFrontierColor == new Color (0, 0, 0, 0))
																_territoryDisputedFrontierColor = _territoryFrontierColor;
												if (territoriesDisputedMat != null && territoriesDisputedMat.color != _territoryDisputedFrontierColor) {
																territoriesDisputedMat.color = _territoryDisputedFrontierColor;
												}
												if (cellsThinMat != null && cellsThinMat.color != _cellBorderColor) {
																cellsThinMat.color = _cellBorderColor;
												}
												if (cellsGeoMat != null && cellsGeoMat.color != _cellBorderColor) {
																cellsGeoMat.color = _cellBorderColor;
												}
								}

								void Update () {

												FitToTerrain ();  		// Verify if there're changes in container and adjust the grid mesh accordingly

												if (shouldRedraw) {
																Redraw ();
												}

												if (Application.isMobilePlatform) {
																if (Input.touchCount != 1)
																				return;
												}
												bool startPressing = Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began;

												// Check whether the points is on an UI element, then avoid user interaction
												if (respectOtherUI) {
																if (!canInteract && Application.isMobilePlatform && !startPressing)
																				return;

																canInteract = true;
																if (UnityEngine.EventSystems.EventSystem.current != null) {
																				if (Application.isMobilePlatform && Input.touchCount > 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId)) {
																								canInteract = false;
																				} else if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject (-1))
																								canInteract = false;
																}
																if (!canInteract) {
																				HideTerritoryRegionHighlight ();
																				HideCellRegionHighlight ();
																				return;
																}
												}

												if ((!Application.isMobilePlatform && !Input.GetMouseButton (0)) || startPressing) { // on mobile only check when touch start and on desktop do not check if dragging
																CheckMousePos (); 		// Verify if mouse enter a territory boundary 
												}
												TriggerEvents (); // Listen to pointer events
								}

								void LateUpdate () {
												UpdateHighlightFade (); 	// Fades current selection
								}

								#endregion



								#region Initialization

								public void Init () {
												if (territoriesMat == null) {
																territoriesMat = Instantiate (Resources.Load <Material> ("Materials/Territory")) as Material;
																territoriesMat.hideFlags = HideFlags.DontSave;
												}
												if (territoriesDisputedMat == null) {
																territoriesDisputedMat = Instantiate (territoriesMat) as Material;
																territoriesDisputedMat.hideFlags = HideFlags.DontSave;
																territoriesDisputedMat.color = _territoryDisputedFrontierColor;
												}
												if (cellsThinMat == null) {
																cellsThinMat = Instantiate (Resources.Load <Material> ("Materials/Cell"));
																cellsThinMat.hideFlags = HideFlags.DontSave;
												}
												if (cellsGeoMat == null) {
																cellsGeoMat = Instantiate (Resources.Load <Material> ("Materials/CellGeo"));
																cellsGeoMat.hideFlags = HideFlags.DontSave;
												}
												if (hudMatTerritoryOverlay == null) {
																hudMatTerritoryOverlay = Instantiate (Resources.Load <Material> ("Materials/HudTerritoryOverlay")) as Material;
																hudMatTerritoryOverlay.hideFlags = HideFlags.DontSave;
												}
												if (hudMatTerritoryGround == null) {
																hudMatTerritoryGround = Instantiate (Resources.Load <Material> ("Materials/HudTerritoryGround")) as Material;
																hudMatTerritoryGround.hideFlags = HideFlags.DontSave;
												}
												if (hudMatCellOverlay == null) {
																hudMatCellOverlay = Instantiate (Resources.Load <Material> ("Materials/HudCellOverlay")) as Material;
																hudMatCellOverlay.hideFlags = HideFlags.DontSave;
												}
												if (hudMatCellGround == null) {
																hudMatCellGround = Instantiate (Resources.Load <Material> ("Materials/HudCellGround")) as Material;
																hudMatCellGround.hideFlags = HideFlags.DontSave;
												}
												if (coloredMatGround == null) {
																coloredMatGround = Instantiate (Resources.Load <Material> ("Materials/ColorizedRegionGround")) as Material;
																coloredMatGround.hideFlags = HideFlags.DontSave;
												}
												if (coloredMatOverlay == null) {
																coloredMatOverlay = Instantiate (Resources.Load <Material> ("Materials/ColorizedRegionOverlay")) as Material;
																coloredMatOverlay.hideFlags = HideFlags.DontSave;
												}
												if (texturizedMatGround == null) {
																texturizedMatGround = Instantiate (Resources.Load <Material> ("Materials/TexturizedRegionGround"));
																texturizedMatGround.hideFlags = HideFlags.DontSave;
												}
												if (texturizedMatOverlay == null) {
																texturizedMatOverlay = Instantiate (Resources.Load <Material> ("Materials/TexturizedRegionOverlay"));
																texturizedMatOverlay.hideFlags = HideFlags.DontSave;
												}
												coloredMatCacheGround = new Dictionary<Color, Material> ();
												coloredMatCacheOverlay = new Dictionary<Color, Material> ();
												frontierColorCache = new Dictionary<Color, Material> ();

												UnityEngine.Random.InitState (seed);

												if (factoryColors == null || factoryColors.Length < MAX_TERRITORIES) {
																factoryColors = new Color[MAX_TERRITORIES];
																for (int k = 0; k < factoryColors.Length; k++)
																				factoryColors [k] = new Color (UnityEngine.Random.Range (0.0f, 0.5f), UnityEngine.Random.Range (0.0f, 0.5f), UnityEngine.Random.Range (0.0f, 0.5f));
												}
												if (_sortedTerritories == null)
																_sortedTerritories = new List<Territory> (MAX_TERRITORIES);

												if (textures == null || textures.Length < 32)
																textures = new Texture2D[32];

												ReadMaskContents ();
												Redraw ();

												if (territoriesTexture != null) {
																CreateTerritories (territoriesTexture, territoriesTextureNeutralColor);
												}
								}

								void CreateSurfacesLayer () {
												Transform t = transform.Find ("Surfaces");
												if (t != null) {
																DestroyImmediate (t.gameObject);
												}
												_surfacesLayer = new GameObject ("Surfaces");
												_surfacesLayer.transform.SetParent (transform, false);
												_surfacesLayer.transform.localPosition = Misc.Vector3zero; // Vector3.back * 0.01f;
												_surfacesLayer.layer = gameObject.layer;
								}

								void DestroySurfaces () {
												HideTerritoryRegionHighlight ();
												HideCellRegionHighlight ();
												if (segmentHit != null)
																segmentHit.Clear ();
												if (surfaces != null)
																surfaces.Clear ();
												if (_surfacesLayer != null)
																DestroyImmediate (_surfacesLayer);
								}

								void ReadMaskContents () {
												if (_gridMask == null)
																return;
												mask = _gridMask.GetPixels ();
								}


								#endregion


								#region Map generation


								void SetupIrregularGrid () {
												Point[] centers = new Point[_numCells];
												for (int k = 0; k < centers.Length; k++) {
																centers [k] = new Point (UnityEngine.Random.Range (-0.49f, 0.49f), UnityEngine.Random.Range (-0.49f, 0.49f));
												}
			
												VoronoiFortune voronoi = new VoronoiFortune ();
												for (int k = 0; k < goodGridRelaxation; k++) {
																voronoi.AssignData (centers);
																voronoi.DoVoronoi ();
																if (k < goodGridRelaxation - 1) {
																				for (int j = 0; j < _numCells; j++) {
																								Point centroid = voronoi.cells [j].centroid;
																								centers [j] = (centers [j] + centroid) / 2;
																				}
																}
												}

												// Make cell regions: we assume cells have only 1 region but that can change in the future
												float curvature = goodGridCurvature;
												for (int k = 0; k < voronoi.cells.Length; k++) {
																VoronoiCell voronoiCell = voronoi.cells [k];
																Cell cell = new Cell (voronoiCell.center.vector3);
																Region cr = new Region (cell);
																if (curvature > 0) {
																				cr.polygon = voronoiCell.GetPolygon (3, curvature);
																} else {
																				cr.polygon = voronoiCell.GetPolygon (1, 0);
																}
																if (cr.polygon != null) {
																				// Add segments
																				int segCount = voronoiCell.segments.Count;
																				for (int i = 0; i < segCount; i++) {
																								Segment s = voronoiCell.segments [i];
																								if (!s.deleted) {
																												if (curvature > 0) {
																																cr.segments.AddRange (s.subdivisions);
																												} else {
																																cr.segments.Add (s);
																												}
																								}
																				}

																				cell.polygon = cr.polygon.Clone ();
																				cell.region = cr;
																				cells.Add (cell);
																}
												}
								}

								void SetupBoxGrid (bool strictQuads) {

												int qx = _cellColumnCount;
												int qy = _cellRowCount;

												double stepX = 1.0 / qx;
												double stepY = 1.0 / qy;

												double halfStepX = stepX * 0.5;
												double halfStepY = stepY * 0.5;

												Segment[,,] sides = new Segment[qx, qy, 4]; // 0 = left, 1 = top, 2 = right, 3 = bottom
												int subdivisions = goodGridCurvature > 0 ? 3 : 1;
												for (int j = 0; j < qy; j++) {
																for (int k = 0; k < qx; k++) {
																				Point center = new Point ((double)k / qx - 0.5 + halfStepX, (double)j / qy - 0.5 + halfStepY);
																				Cell cell = new Cell (new Vector2 ((float)center.x, (float)center.y));
																				cell.column = k;
																				cell.row = j;

																				Segment left = k > 0 ? sides [k - 1, j, 2] : new Segment (center.Offset (-halfStepX, -halfStepY), center.Offset (-halfStepX, halfStepY), true);
																				sides [k, j, 0] = left;

																				Segment top = new Segment (center.Offset (-halfStepX, halfStepY), center.Offset (halfStepX, halfStepY), j == qy - 1);
																				sides [k, j, 1] = top;

																				Segment right = new Segment (center.Offset (halfStepX, halfStepY), center.Offset (halfStepX, -halfStepY), k == qx - 1);
																				sides [k, j, 2] = right;

																				Segment bottom = j > 0 ? sides [k, j - 1, 1] : new Segment (center.Offset (halfStepX, -halfStepY), center.Offset (-halfStepX, -halfStepY), true);
																				sides [k, j, 3] = bottom;

																				Region cr = new Region (cell);
																				if (subdivisions > 1) {
																								cr.segments.AddRange (top.Subdivide (subdivisions, _gridCurvature));
																								cr.segments.AddRange (right.Subdivide (subdivisions, _gridCurvature));
																								cr.segments.AddRange (bottom.Subdivide (subdivisions, _gridCurvature));
																								cr.segments.AddRange (left.Subdivide (subdivisions, _gridCurvature));
																				} else {
																								cr.segments.Add (top);
																								cr.segments.Add (right);
																								cr.segments.Add (bottom);
																								cr.segments.Add (left);
																				}
																				Connector connector = new Connector ();
																				connector.AddRange (cr.segments);
																				cr.polygon = connector.ToPolygon (); // FromLargestLineStrip();
																				if (cr.polygon != null) {
																								cell.region = cr;
																								cells.Add (cell);
																				}
																}
												}
								}

								void SetupHexagonalGrid () {
			
												double qx = 1.0 + (_cellColumnCount - 1.0) * 3.0 / 4.0;
												double qy = _cellRowCount + 0.5;
												int qy2 = _cellRowCount;
												int qx2 = _cellColumnCount;
			
												double stepX = 1.0 / qx;
												double stepY = 1.0 / qy;
			
												double halfStepX = stepX * 0.5;
												double halfStepY = stepY * 0.5;
												int evenLayout = _evenLayout ? 1 : 0;

												Segment[,,] sides = new Segment[qx2, qy2, 6]; // 0 = left-up, 1 = top, 2 = right-up, 3 = right-down, 4 = down, 5 = left-down
												int subdivisions = goodGridCurvature > 0 ? 3 : 1;
												for (int j = 0; j < qy2; j++) {
																for (int k = 0; k < qx2; k++) {
																				Point center = new Point ((double)k / qx - 0.5 + halfStepX, (double)j / qy - 0.5 + stepY);
																				center.x -= k * halfStepX / 2;
																				Cell cell = new Cell (new Vector2 ((float)center.x, (float)center.y));
																				cell.row = j;
																				cell.column = k;

																				double offsetY = (k % 2 == evenLayout) ? 0 : -halfStepY;
					
																				Segment leftUp = (k > 0 && offsetY < 0) ? sides [k - 1, j, 3] : new Segment (center.Offset (-halfStepX, offsetY), center.Offset (-halfStepX / 2, halfStepY + offsetY), k == 0 || (j == qy2 - 1 && offsetY == 0));
																				sides [k, j, 0] = leftUp;
					
																				Segment top = new Segment (center.Offset (-halfStepX / 2, halfStepY + offsetY), center.Offset (halfStepX / 2, halfStepY + offsetY), j == qy2 - 1);
																				sides [k, j, 1] = top;
					
																				Segment rightUp = new Segment (center.Offset (halfStepX / 2, halfStepY + offsetY), center.Offset (halfStepX, offsetY), k == qx2 - 1 || (j == qy2 - 1 && offsetY == 0));
																				sides [k, j, 2] = rightUp;
					
																				Segment rightDown = (j > 0 && k < qx2 - 1 && offsetY < 0) ? sides [k + 1, j - 1, 0] : new Segment (center.Offset (halfStepX, offsetY), center.Offset (halfStepX / 2, -halfStepY + offsetY), (j == 0 && offsetY < 0) || k == qx2 - 1);
																				sides [k, j, 3] = rightDown;
					
																				Segment bottom = j > 0 ? sides [k, j - 1, 1] : new Segment (center.Offset (halfStepX / 2, -halfStepY + offsetY), center.Offset (-halfStepX / 2, -halfStepY + offsetY), true);
																				sides [k, j, 4] = bottom;
					
																				Segment leftDown;
																				if (offsetY < 0 && j > 0 && k > 0) {
																								leftDown = sides [k - 1, j - 1, 2];
																				} else if (offsetY == 0 && k > 0) {
																								leftDown = sides [k - 1, j, 2];
																				} else {
																								leftDown = new Segment (center.Offset (-halfStepX / 2, -halfStepY + offsetY), center.Offset (-halfStepX, offsetY), true);
																				}
																				sides [k, j, 5] = leftDown;
					
																				cell.center += Vector2.up * (float)offsetY;

																				Region cr = new Region (cell);
																				if (subdivisions > 1) {
																								if (!top.deleted)
																												cr.segments.AddRange (top.Subdivide (subdivisions, _gridCurvature));
																								if (!rightUp.deleted)
																												cr.segments.AddRange (rightUp.Subdivide (subdivisions, _gridCurvature));
																								if (!rightDown.deleted)
																												cr.segments.AddRange (rightDown.Subdivide (subdivisions, _gridCurvature));
																								if (!bottom.deleted)
																												cr.segments.AddRange (bottom.Subdivide (subdivisions, _gridCurvature));
																								if (!leftDown.deleted)
																												cr.segments.AddRange (leftDown.Subdivide (subdivisions, _gridCurvature));
																								if (!leftUp.deleted)
																												cr.segments.AddRange (leftUp.Subdivide (subdivisions, _gridCurvature));
																				} else {
																								if (!top.deleted)
																												cr.segments.Add (top);
																								if (!rightUp.deleted)
																												cr.segments.Add (rightUp);
																								if (!rightDown.deleted)
																												cr.segments.Add (rightDown);
																								if (!bottom.deleted)
																												cr.segments.Add (bottom);
																								if (!leftDown.deleted)
																												cr.segments.Add (leftDown);
																								if (!leftUp.deleted)
																												cr.segments.Add (leftUp);
																				}
																				Connector connector = new Connector ();
																				connector.AddRange (cr.segments);
																				cr.polygon = connector.ToPolygon ();
																				if (cr.polygon != null) {
																								cell.region = cr;
																								cells.Add (cell);
																				}
																}
												}
								}

								void CreateCells () {
												UnityEngine.Random.InitState (seed);

												_numCells = Mathf.Max (_numTerritories, 2, cellCount); 
												if (cells == null) {
																cells = new List<Cell> (_numCells);
												} else {
																cells.Clear ();
												}
												if (cellTagged == null)
																cellTagged = new Dictionary<int, Cell> ();
												else
																cellTagged.Clear ();
												lastCellLookupCount = -1;

												switch (_gridTopology) {
												case GRID_TOPOLOGY.Box:
																SetupBoxGrid (true);
																break;
												case GRID_TOPOLOGY.Hexagonal:
																SetupHexagonalGrid ();
																break;
												default:
																SetupIrregularGrid ();
																break; // case GRID_TOPOLOGY.Irregular:
												}

												CellsFindNeighbours ();
												CellsUpdateBounds ();

												// Update sorted cell list
												sortedCells = new List<Cell> (cells);
												sortedCells.Sort ((cell1, cell2) => {
																return cell1.region.rect2DArea.CompareTo (cell2.region.rect2DArea);
												});

												ClearLastOver ();

												recreateCells = false;

								}

								/// <summary>
								/// Takes the center of each cell and checks the alpha component of the mask to confirm visibility
								/// </summary>
								void CellsApplyVisibilityFilters () {
												int cellsCount = cells.Count;
												if (gridMask != null && mask != null) {
																int tw = gridMask.width;
																int th = gridMask.height;
																for (int k = 0; k < cellsCount; k++) {
																				Cell cell = cells [k];
																				int pointCount = cell.region.points.Count;
																				bool visible = false;
																				for (int v = 0; v < pointCount; v++) {
																								Vector2 p = cell.region.points [v];
																								float y = p.y + 0.5f;
																								float x = p.x + 0.5f;
																								int ty = (int)(y * th);
																								int tx = (int)(x * tw);
																								if (ty >= 0 && ty < th && tx >= 0 && tx < tw && mask [ty * tw + tx].a > 0) {
																												visible = true;
																												break;
																								}
																				}
																				cell.visible = visible;
																}
												}

												if (_terrain != null) {
																if (_cellsMaxSlope < 1f) {
																				for (int k = 0; k < cellsCount; k++) {
																								Cell cell = cells [k];
																								if (!cell.visible)
																												continue;
																								Vector3 norm = _terrain.terrainData.GetInterpolatedNormal (cell.center.x + 0.5f, cell.center.y + 0.5f);
																								if (Mathf.Abs (norm.y) < _cellsMaxSlope) {
																												cell.visible = false;
																								}
																				}
																}
																if (_cellsMinimumAltitude != 0f) {
																				for (int k = 0; k < cellsCount; k++) {
																								Cell cell = cells [k];
																								if (!cell.visible)
																												continue;
																								Vector3 wpos = GetWorldSpacePosition (cell.scaledCenter);
																								if (wpos.y < _cellsMinimumAltitude)
																												cell.visible = false;
																				}
																}
												}

												ClearLastOver ();
												needRefreshRouteMatrix = true;
								}

								void CellsUpdateBounds () {
												// Update cells polygon
												for (int k = 0; k < cells.Count; k++) {
																CellUpdateBounds (cells [k]);
												}
								}

								void CellUpdateBounds (Cell cell) {
												cell.polygon = cell.region.polygon;
												if (cell.polygon.contours.Count == 0)
																return;

												List<Vector2> points = cell.polygon.contours [0].GetVector2Points (_gridCenter, _gridScale);
												cell.region.points = points;
												// Update bounding rect
												float minx, miny, maxx, maxy;
												minx = miny = float.MaxValue;
												maxx = maxy = float.MinValue;
												int pointsCount = points.Count;
												for (int p = 0; p < pointsCount; p++) {
																Vector3 point = points [p];
																if (point.x < minx)
																				minx = point.x;
																if (point.x > maxx)
																				maxx = point.x;
																if (point.y < miny)
																				miny = point.y;
																if (point.y > maxy)
																				maxy = point.y;
												}
												float rectWidth = maxx - minx;
												float rectHeight = maxy - miny;
												cell.region.rect2D = new Rect (minx, miny, rectWidth, rectHeight);
												cell.region.rect2DArea = rectWidth * rectHeight;
												cell.scaledCenter = GetScaledVector (cell.center);
								}


								/// <summary>
								/// Must be called after changing one cell geometry.
								/// </summary>
								void UpdateCellGeometry (Cell cell, TGS.Geom.Polygon poly) {
												// Copy new polygon definition
												cell.region.polygon = poly;
												cell.polygon = cell.region.polygon.Clone ();
												// Update segments list
												cell.region.segments.Clear ();
												List<Segment> segmentCache = new List<Segment> (cellNeighbourHit.Keys);
												int segmentsCacheCount = segmentCache.Count;
												int pointsCount = poly.contours [0].points.Count;
												for (int k = 0; k < pointsCount; k++) {
																Segment s = poly.contours [0].GetSegment (k);
																bool found = false;
																// Search this segment in the segment cache
																for (int j = 0; j < segmentsCacheCount; j++) {
																				Segment o = segmentCache [j];
																				if ((Point.EqualsBoth (o.start, s.start) && Point.EqualsBoth (o.end, s.end)) || (Point.EqualsBoth (o.end, s.start) && Point.EqualsBoth (o.start, s.end))) {
																								cell.region.segments.Add (o);
//						((Cell)cellNeighbourHit[o].entity).territoryIndex = cell.territoryIndex; // updates the territory index of this segment in the cache 
																								found = true;
																								break;
																				}
																}
																if (!found)
																				cell.region.segments.Add (s);
												}
												// Refresh neighbours
												CellsUpdateNeighbours ();
												// Refresh rect2D
												CellUpdateBounds (cell);

												// Refresh territories
												if (territoriesAreUsed) {
																FindTerritoryFrontiers ();
																UpdateTerritoryBoundaries ();
												}

												if (cell == _cellLastOver) {
																ClearLastOver ();
												}
								}

								void CellsUpdateNeighbours () {
												int cellCount = cells.Count;
												for (int k = 0; k < cellCount; k++) {
																cells [k].region.neighbours.Clear ();
												}
												CellsFindNeighbours ();
								}

								void CellsFindNeighbours () {
			
												if (cellNeighbourHit == null) {
																cellNeighbourHit = new Dictionary<Segment, Region> (50000);
												} else {
																cellNeighbourHit.Clear ();
												}
												int cellCount = cells.Count;
												for (int k = 0; k < cellCount; k++) {
																Cell cell = cells [k];
																Region region = cell.region;
																int numSegments = region.segments.Count;
																for (int i = 0; i < numSegments; i++) {
																				Segment seg = region.segments [i];
																				Region neighbour;
																				if (cellNeighbourHit.TryGetValue (seg, out neighbour)) {
																								if (neighbour != region) {
																												if (!region.neighbours.Contains (neighbour)) {
																																region.neighbours.Add (neighbour);
																																neighbour.neighbours.Add (region);
																												}
																								}
																				} else {
																								cellNeighbourHit.Add (seg, region);
																				}
																}
												}
								}

								void FindTerritoryFrontiers () {

												if (territories == null || territories.Count == 0)
																return;

												if (territoryFrontiers == null) {
																territoryFrontiers = new List<Segment> (cellNeighbourHit.Count);
												} else {
																territoryFrontiers.Clear ();
												}
												if (territoryNeighbourHit == null) {
																territoryNeighbourHit = new Dictionary<Segment, Region> (50000);
												} else {
																territoryNeighbourHit.Clear ();
												}
												int terrCount = territories.Count;
												Connector[] connectors = new Connector[terrCount];
												for (int k = 0; k < terrCount; k++) {
																connectors [k] = new Connector ();
																Territory territory = territories [k];
																territory.cells.Clear ();
																if (territory.region == null) {
																				Region territoryRegion = new Region (territory);
																				territory.region = territoryRegion;
																}
																territories [k].region.neighbours.Clear ();
												}

												int cellCount = cells.Count;
												for (int k = 0; k < cellCount; k++) {
																Cell cell = cells [k];
																if (cell.territoryIndex >= terrCount)
																				continue;
																bool validCell = cell.visible && cell.territoryIndex >= 0;
																if (validCell)
																				territories [cell.territoryIndex].cells.Add (cell);
																Region region = cell.region;
																int numSegments = region.segments.Count;
																for (int i = 0; i < numSegments; i++) {
																				Segment seg = region.segments [i];
																				if (seg.border) {
																								if (validCell) {
																												territoryFrontiers.Add (seg);
																												int territory1 = cell.territoryIndex;
																												connectors [territory1].Add (seg);
																												seg.territoryIndex = territory1;
																								}
																								continue;
																				}
																				Region neighbour;
																				if (territoryNeighbourHit.TryGetValue (seg, out neighbour)) {
																								Cell neighbourCell = (Cell)neighbour.entity;
																								int territory1 = cell.territoryIndex;
																								int territory2 = neighbourCell.territoryIndex;
																								if (territory2 != territory1) {
																												territoryFrontiers.Add (seg);
																												if (validCell) {
																																connectors [territory1].Add (seg);
																																bool territory1IsNeutral = territories [territory1].neutral;
																																// check segment ownership
																																if (territory2 >= 0) {
																																				bool territory2IsNeutral = territories [territory2].neutral;
																																				if (territory1IsNeutral && territory2IsNeutral) {
																																								seg.territoryIndex = territory1;
																																				} else if (territory1IsNeutral && !territory2IsNeutral) {
																																								seg.territoryIndex = territory2;
																																				} else if (!territory1IsNeutral && territory2IsNeutral) {
																																								seg.territoryIndex = territory1;
																																				} else {
																																								seg.territoryIndex = -1; // if segment belongs to a visible cell and valid territory2, mark this segment as disputed. Otherwise make it part of territory1
																																				}
																																} else {
																																				seg.territoryIndex = territory1;
																																}

																																if (seg.territoryIndex < 0) {
																																				// add territory neigbhours
																																				Region territory1Region = territories [territory1].region;
																																				Region territory2Region = territories [territory2].region;
																																				if (!territory1Region.neighbours.Contains (territory2Region)) {
																																								territory1Region.neighbours.Add (territory2Region);
																																				}
																																				if (!territory2Region.neighbours.Contains (territory1Region)) {
																																								territory2Region.neighbours.Add (territory1Region);
																																				}
																																}
																												}
																												if (territory2 >= 0) {
																																connectors [territory2].Add (seg);
																												}
																								}
																				} else {
																								territoryNeighbourHit [seg] = region;
																								seg.territoryIndex = cell.territoryIndex;
																				}
																}
												}

												for (int k = 0; k < terrCount; k++) {
																if (territories [k].cells.Count > 0) {
																				territories [k].polygon = connectors [k].ToPolygonFromLargestLineStrip ();
																} else {
																				territories [k].polygon = null;
																}
												}
								}

								void AddSegmentToMesh (Vector3 p0, Vector3 p1) {
												float h0 = _terrain.SampleHeight (transform.TransformPoint (p0));
												float h1 = _terrain.SampleHeight (transform.TransformPoint (p1));
												if (_gridNormalOffset > 0) {
																Vector3 invNormal = transform.InverseTransformVector (_terrain.terrainData.GetInterpolatedNormal (p0.x + 0.5f, p0.y + 0.5f));
																p0 += invNormal * _gridNormalOffset;
																invNormal = transform.InverseTransformVector (_terrain.terrainData.GetInterpolatedNormal (p1.x + 0.5f, p1.y + 0.5f));
																p1 += invNormal * _gridNormalOffset;
												}
												p0.z -= h0;
												p1.z -= h1;
												frontiersPoints.Add (p0);
												frontiersPoints.Add (p1);
								}

								/// <summary>
								/// Subdivides the segment in smaller segments
								/// </summary>
								/// <returns><c>true</c>, if segment was drawn, <c>false</c> otherwise.</returns>
								void SurfaceSegmentForMesh (Vector3 p0, Vector3 p1) {

												// trace the line until roughness is exceeded
												float dist = (float)Math.Sqrt ((p1.x - p0.x) * (p1.x - p0.x) + (p1.y - p0.y) * (p1.y - p0.y));
												Vector3 direction = p1 - p0;

												int numSteps = Mathf.FloorToInt (meshStep * dist);
												Vector3 t0 = p0;
												float h0 = _terrain.SampleHeight (transform.TransformPoint (t0));
												if (_gridNormalOffset > 0) {
																Vector3 invNormal = transform.InverseTransformVector (_terrain.terrainData.GetInterpolatedNormal (t0.x + 0.5f, t0.y + 0.5f));
																t0 += invNormal * _gridNormalOffset;
												}
												t0.z -= h0;
												Vector3 ta = t0;
												float h1, ha = h0;
												for (int i = 1; i < numSteps; i++) {
																Vector3 t1 = p0 + direction * i / numSteps;
																h1 = _terrain.SampleHeight (transform.TransformPoint (t1));
																if (h0 < h1 || h0 - h1 > effectiveRoughness) {
																				frontiersPoints.Add (t0);
																				if (t0 != ta) {
																								if (_gridNormalOffset > 0) {
																												Vector3 invNormal = transform.InverseTransformVector (_terrain.terrainData.GetInterpolatedNormal (ta.x + 0.5f, ta.y + 0.5f));
																												ta += invNormal * _gridNormalOffset;
																								}
																								ta.z -= ha;
																								frontiersPoints.Add (ta);
																								frontiersPoints.Add (ta);
																				}
																				if (_gridNormalOffset > 0) {
																								Vector3 invNormal = transform.InverseTransformVector (_terrain.terrainData.GetInterpolatedNormal (t1.x + 0.5f, t1.y + 0.5f));
																								t1 += invNormal * _gridNormalOffset;
																				}
																				t1.z -= h1;
																				frontiersPoints.Add (t1);
																				t0 = t1;
																				h0 = h1;
																}
																ta = t1;
																ha = h1;
												}
												// Add last point
												h1 = _terrain.SampleHeight (transform.TransformPoint (p1));
												if (_gridNormalOffset > 0) {
																Vector3 invNormal = transform.InverseTransformVector (_terrain.terrainData.GetInterpolatedNormal (p1.x + 0.5f, p1.y + 0.5f));
																p1 += invNormal * _gridNormalOffset;
												}
												p1.z -= h1;
												frontiersPoints.Add (t0);
												frontiersPoints.Add (p1);
								}

								void GenerateCellsMesh () {
			
												if (segmentHit == null) {
																segmentHit = new Dictionary<Segment, bool> (50000);
												} else {
																segmentHit.Clear ();
												}

												if (frontiersPoints == null) {
																frontiersPoints = new List<Vector3> (100000);
												} else {
																frontiersPoints.Clear ();
												}

												int cellCount = cells.Count;
												if (_terrain == null) {
																for (int k = 0; k < cellCount; k++) {
																				Cell cell = cells [k];
																				if (cell.visible) {
																								Region region = cell.region;
																								int numSegments = region.segments.Count;
																								for (int i = 0; i < numSegments; i++) {
																												Segment s = region.segments [i];
																												if (!segmentHit.ContainsKey (s)) {
																																segmentHit.Add (s, true);
																																frontiersPoints.Add (GetScaledVector (s.startToVector3));
																																frontiersPoints.Add (GetScaledVector (s.endToVector3));
																												}
																								}
																				}
																}
												} else {
																meshStep = (2.0f - _gridRoughness) / (float)MIN_VERTEX_DISTANCE;
																for (int k = 0; k < cellCount; k++) {
																				Cell cell = cells [k];
																				if (cell.visible) {
																								Region region = cell.region;
																								int numSegments = region.segments.Count;
																								for (int i = 0; i < numSegments; i++) {
																												Segment s = region.segments [i];
																												if (!segmentHit.ContainsKey (s)) {
																																segmentHit.Add (s, true);
																																SurfaceSegmentForMesh (GetScaledVector (s.start.vector3),
																																				GetScaledVector (s.end.vector3));
																												}
																								}
																				}
																}
												}

												int meshGroups = (frontiersPoints.Count / 65000) + 1;
												int meshIndex = -1;
												if (cellMeshIndices == null || cellMeshIndices.GetUpperBound (0) != meshGroups - 1) {
																cellMeshIndices = new int[meshGroups][];
																cellMeshBorders = new Vector3[meshGroups][];
												}
												if (frontiersPoints.Count == 0) {
																cellMeshBorders [0] = new Vector3[0];
																cellMeshIndices [0] = new int[0];
												} else {
																int frontiersPointsCount = frontiersPoints.Count;
																for (int k = 0; k < frontiersPointsCount; k += 65000) {
																				int max = Mathf.Min (frontiersPointsCount - k, 65000); 
																				++meshIndex;
																				if (cellMeshBorders [meshIndex] == null || cellMeshBorders [0].GetUpperBound (0) != max - 1) {
																								cellMeshBorders [meshIndex] = new Vector3[max];
																								cellMeshIndices [meshIndex] = new int[max];
																				}
																				for (int j = 0; j < max; j++) {
																								cellMeshBorders [meshIndex] [j] = frontiersPoints [j + k];
																								cellMeshIndices [meshIndex] [j] = j;
																				}
																}
												}
								}

								void CreateTerritories () {

												_numTerritories = Mathf.Clamp (_numTerritories, 1, cellCount);

												if (!_colorizeTerritories && !_showTerritories && _highlightMode != HIGHLIGHT_MODE.Territories) {
																if (territories != null)
																				territories.Clear ();
																if (territoryLayer != null)
																				DestroyImmediate (territoryLayer);
																return;
												}

												if (territories == null) {
																territories = new List<Territory> (_numTerritories);
												} else {
																territories.Clear ();
												}

												CheckCells ();
												// Freedom for the cells!...
												int cellsCount = cells.Count;
												for (int k = 0; k < cellsCount; k++) {
																cells [k].territoryIndex = -1;
												}
												UnityEngine.Random.InitState (seed);

												for (int c = 0; c < _numTerritories; c++) {
																Territory territory = new Territory (c.ToString ());
																territory.fillColor = factoryColors [c];
																int territoryIndex = territories.Count;
																int p = UnityEngine.Random.Range (0, cellsCount);
																int z = 0;
																while ((cells [p].territoryIndex != -1 || !cells [p].visible) && z++ <= cellsCount) {
																				p++;
																				if (p >= cellsCount)
																								p = 0;
																}
																if (z > cellsCount)
																				break; // no more territories can be found - this should not happen
																Cell cell = cells [p];
																cell.territoryIndex = territoryIndex;
																territory.center = cell.center;
																territory.cells.Add (cell);
																territories.Add (territory);
												}

												// Continue conquering cells
												int[] territoryCellIndex = new int[territories.Count];

												// Iterate one cell per country (this is not efficient but ensures balanced distribution)
												bool remainingCells = true;
												while (remainingCells) {
																remainingCells = false;
																int terrCount = territories.Count;
																for (int k = 0; k < terrCount; k++) {
																				Territory territory = territories [k];
																				int territoryCellsCount = territory.cells.Count;
																				for (int p = territoryCellIndex [k]; p < territoryCellsCount; p++) {
																								Region cellRegion = territory.cells [p].region;
																								int nCount = cellRegion.neighbours.Count;
																								for (int n = 0; n < nCount; n++) {
																												Region otherRegion = cellRegion.neighbours [n];
																												Cell otherProv = (Cell)otherRegion.entity;
																												if (otherProv.territoryIndex == -1 && otherProv.visible) {
																																otherProv.territoryIndex = k;
																																territory.cells.Add (otherProv);
																																territoryCellsCount++;
																																remainingCells = true;
																																p = territoryCellsCount;
																																break;
																												}
																								}
																								if (p < territoryCellsCount) {// no free neighbours left for this cell
																												territoryCellIndex [k]++;
																								}
																				}
																}
												}

												FindTerritoryFrontiers ();
												UpdateTerritoryBoundaries ();

												recreateTerritories = false;

								}

								void UpdateTerritoryBoundaries () {
												if (territories == null)
																return;

												// Update territory region
												int terrCount = territories.Count;
												for (int k = 0; k < terrCount; k++) {
																Territory territory = territories [k];
																if (territory.polygon == null) {
																				continue;
																}

																Region territoryRegion = territory.region;
																territoryRegion.points = territory.polygon.contours [0].GetVector2Points (_gridCenter, _gridScale);
																territory.scaledCenter = GetScaledVector (territory.center);

																List<Point> points = territory.polygon.contours [0].points;
																int pointCount = points.Count;
																territoryRegion.segments.Clear ();
																for (int j = 0; j < pointCount; j++) {
																				Point p0 = points [j];
																				Point p1;
																				if (j == pointCount - 1) {
																								p1 = points [0];
																				} else {
																								p1 = points [j + 1];
																				}
																				territoryRegion.segments.Add (new Segment (p0, p1));
																}

																// Update bounding rect
																float minx, miny, maxx, maxy;
																minx = miny = float.MaxValue;
																maxx = maxy = float.MinValue;
																int terrPointCount = territoryRegion.points.Count;
																for (int p = 0; p < terrPointCount; p++) {
																				Vector3 point = territoryRegion.points [p];
																				if (point.x < minx)
																								minx = point.x;
																				if (point.x > maxx)
																								maxx = point.x;
																				if (point.y < miny)
																								miny = point.y;
																				if (point.y > maxy)
																								maxy = point.y;
																}
																float rectWidth = maxx - minx;
																float rectHeight = maxy - miny;
																territoryRegion.rect2D = new Rect (minx, miny, rectWidth, rectHeight);
																territoryRegion.rect2DArea = rectWidth * rectHeight;
												}

												_sortedTerritories.Clear ();
								}

								void GenerateTerritoriesMesh () {
												if (territories == null)
																return;

												if (frontiersPoints == null)
																frontiersPoints = new List<Vector3> (10000);

												int terrCount = territories.Count;
												if (territoryMeshes == null) {
																territoryMeshes = new List<TerritoryMesh> (terrCount + 1);
												} else {
																territoryMeshes.Clear ();
												}

												if (territoryFrontiers == null)
																return;

												TerritoryMesh tm;
												for (int k = 0; k < terrCount; k++) {
																tm = new TerritoryMesh ();
																tm.territoryIndex = k;
																if (GenerateTerritoryMesh (tm)) {
																				territoryMeshes.Add (tm);
																}
												}

												// Generate disputed frontiers
												tm = new TerritoryMesh ();
												tm.territoryIndex = -1;
												if (GenerateTerritoryMesh (tm)) {
																territoryMeshes.Add (tm);
												}
								}

								/// <summary>
								/// Generates the territory mesh.
								/// </summary>
								/// <returns>True if something was produced.
								bool GenerateTerritoryMesh (TerritoryMesh tm) {

												frontiersPoints.Clear ();

												int territoryFrontiersCount = territoryFrontiers.Count;
												if (_terrain == null) {
																for (int k = 0; k < territoryFrontiersCount; k++) {
																				Segment s = territoryFrontiers [k];
																				if (s.territoryIndex != tm.territoryIndex)
																								continue;
																				if (!s.border || _showTerritoriesOuterBorder) {
																								frontiersPoints.Add (GetScaledVector (s.startToVector3));
																								frontiersPoints.Add (GetScaledVector (s.endToVector3));
																				}
																}
												} else {
																meshStep = (2.0f - _gridRoughness) / (float)MIN_VERTEX_DISTANCE;
																for (int k = 0; k < territoryFrontiersCount; k++) {
																				Segment s = territoryFrontiers [k];
																				if (s.territoryIndex != tm.territoryIndex)
																								continue;
																				if (!s.border || _showTerritoriesOuterBorder) {
																								SurfaceSegmentForMesh (GetScaledVector (s.start.vector3), GetScaledVector (s.end.vector3));
																				}
																}
				
												}
			
												int meshGroups = (frontiersPoints.Count / 65000) + 1;
												int meshIndex = -1;
												if (tm.territoryMeshIndices == null || tm.territoryMeshIndices.GetUpperBound (0) != meshGroups - 1) {
																tm.territoryMeshIndices = new int[meshGroups][];
																tm.territoryMeshBorders = new Vector3[meshGroups][];
												}
												int frontiersPointsCount = frontiersPoints.Count;
												for (int k = 0; k < frontiersPointsCount; k += 65000) {
																int max = Mathf.Min (frontiersPointsCount - k, 65000); 
																++meshIndex;
																if (tm.territoryMeshBorders [meshIndex] == null || tm.territoryMeshBorders [meshIndex].GetUpperBound (0) != max - 1) {
																				tm.territoryMeshBorders [meshIndex] = new Vector3[max];
																				tm.territoryMeshIndices [meshIndex] = new int[max];
																}
																for (int j = 0; j < max; j++) {
																				tm.territoryMeshBorders [meshIndex] [j] = frontiersPoints [j + k];
																				tm.territoryMeshIndices [meshIndex] [j] = j;
																}
												}

												return frontiersPoints.Count > 0;
								}

								void FitToTerrain () {
												if (_terrain == null || Camera.main == null)
																return;

												// Fit to terrain
												Vector3 terrainSize = _terrain.terrainData.size;
												terrainWidth = terrainSize.x;
												terrainHeight = terrainSize.y;
												terrainDepth = terrainSize.z;
												transform.localRotation = Quaternion.Euler (90, 0, 0);
												transform.localScale = new Vector3 (terrainWidth, terrainDepth, 1);
												effectiveRoughness = _gridRoughness * terrainHeight;

												Vector3 camPos = Camera.main.transform.position;
												bool refresh = camPos != lastCamPos || transform.position != lastPos || gridElevationCurrent != lastGridElevation || _gridCameraOffset != lastGridCameraOffset;
												if (refresh) {
																Vector3 localPosition = new Vector3 (terrainWidth * 0.5f, 0.01f + gridElevationCurrent, terrainDepth * 0.5f);
																if (_gridCameraOffset > 0) {
																				localPosition += (camPos - transform.position).normalized * (camPos - transform.position).sqrMagnitude * _gridCameraOffset * 0.001f;
																} 
																transform.localPosition = localPosition;
																lastPos = transform.position;
																lastCamPos = camPos;
																lastGridElevation = gridElevationCurrent;
																lastGridCameraOffset = _gridCameraOffset;
												}
								}

								bool UpdateTerrainReference (Terrain terrain, bool reuseTerrainData) {

												_terrain = terrain;
												MeshRenderer quad = GetComponent<MeshRenderer> ();
												if (_terrain == null) {
																if (quad == null) {
																				#if UNITY_EDITOR
																				// Check if user attached TGS directly to the terrain
																				GameObject obj = Instantiate (Resources.Load<GameObject> ("Prefabs/TerrainGridSystem")) as GameObject;
																				obj.name = "TerrainGridSystem";
																				TerrainGridSystem tgs = obj.GetComponent<TerrainGridSystem> ();
																				tgs.terrain = GetComponent<Terrain> ();
																				UnityEditor.Selection.activeGameObject = tgs.gameObject;
																				DestroyImmediate (this);
																				#endif
																				return false;
																}
																if (!quad.enabled)
																				quad.enabled = true;
																if (transform.parent != null) {
																				transform.SetParent (null);
																				transform.localScale = new Vector3 (100, 100, 1);
																				transform.localRotation = Quaternion.Euler (0, 0, 0);
																}
																MeshCollider mc = GetComponent<MeshCollider> ();
																if (mc == null)
																				gameObject.AddComponent<MeshCollider> ();
												} else {
																transform.SetParent (_terrain.transform, false);
																if (quad.enabled) {
																				quad.enabled = false;
																}
																if (_terrain.GetComponent<TerrainTrigger> () == null) {
																				_terrain.gameObject.AddComponent<TerrainTrigger> ();
																}
																MeshCollider mc = GetComponent<MeshCollider> ();
																if (mc != null)
																				DestroyImmediate (mc);
																lastCamPos = Camera.main.transform.position - Vector3.up; // just to force update on first frame
																FitToTerrain ();
																lastCamPos = Camera.main.transform.position - Vector3.up; // just to force update on first update as well
																if (CalculateTerrainRoughness (reuseTerrainData)) {
																				refreshCellMesh = true;
																				refreshTerritoriesMesh = true;
																				// Clear geometry
																				if (cellLayer != null) {
																								DestroyImmediate (cellLayer);
																				}
																				if (territoryLayer != null) {
																								DestroyImmediate (territoryLayer);
																				}
																}

												}
												return true;
								}

								/// <summary>
								/// Calculates the terrain roughness.
								/// </summary>
								/// <returns><c>true</c>, if terrain roughness has changed, <c>false</c> otherwise.</returns>
								bool CalculateTerrainRoughness (bool reuseTerrainData) {
												if (reuseTerrainData && _terrain.terrainData.heightmapWidth == heightMapWidth && _terrain.terrainData.heightmapHeight == heightMapHeight && terrainHeights != null && terrainRoughnessMap != null) {
																return false;
												}
												heightMapWidth = _terrain.terrainData.heightmapWidth;
												heightMapHeight = _terrain.terrainData.heightmapHeight;
												terrainHeights = _terrain.terrainData.GetHeights (0, 0, heightMapWidth, heightMapHeight);
												terrainRoughnessMapWidth = heightMapWidth / TERRAIN_CHUNK_SIZE;
												terrainRoughnessMapHeight = heightMapHeight / TERRAIN_CHUNK_SIZE;
												if (terrainRoughnessMap == null) {
																terrainRoughnessMap = new float[terrainRoughnessMapHeight * terrainRoughnessMapWidth];
																tempTerrainRoughnessMap = new float[terrainRoughnessMapHeight * terrainRoughnessMapWidth];
												} else {
																for (int k = 0; k < terrainRoughnessMap.Length; k++) {
																				terrainRoughnessMap [k] = 0;
																				tempTerrainRoughnessMap [k] = 0;
																}
												}

#if SHOW_DEBUG_GIZMOS
			if (GameObject.Find ("ParentDot")!=null) DestroyImmediate(GameObject.Find ("ParentDot"));
			GameObject parentDot = new GameObject("ParentDot");
			parentDot.hideFlags = HideFlags.DontSave;
			parentDot.transform.position = Misc.Vector3zero;
#endif

												float maxStep = (float)TERRAIN_CHUNK_SIZE / heightMapWidth;
												float minStep = 1.0f / heightMapWidth;
												for (int y = 0, l = 0; l < terrainRoughnessMapHeight; y += TERRAIN_CHUNK_SIZE,l++) {
																int linePos = l * terrainRoughnessMapWidth;
																for (int x = 0, c = 0; c < terrainRoughnessMapWidth; x += TERRAIN_CHUNK_SIZE,c++) {
																				int j0 = y == 0 ? 1 : y;
																				int j1 = y + TERRAIN_CHUNK_SIZE;
																				int k0 = x == 0 ? 1 : x;
																				int k1 = x + TERRAIN_CHUNK_SIZE;
																				float maxDiff = 0;
																				for (int j = j0; j < j1; j++) {
																								for (int k = k0; k < k1; k++) {
																												float diff = terrainHeights [j, k] - terrainHeights [j, k - 1];
																												if (diff > maxDiff || -diff > maxDiff)
																																maxDiff = Mathf.Abs (diff);
																												diff = terrainHeights [j, k] - terrainHeights [j + 1, k - 1];
																												if (diff > maxDiff || -diff > maxDiff)
																																maxDiff = Mathf.Abs (diff);
																												diff = terrainHeights [j, k] - terrainHeights [j + 1, k];
																												if (diff > maxDiff || -diff > maxDiff)
																																maxDiff = Mathf.Abs (diff);
																												diff = terrainHeights [j, k] - terrainHeights [j + 1, k + 1];
																												if (diff > maxDiff || -diff > maxDiff)
																																maxDiff = Mathf.Abs (diff);
																												diff = terrainHeights [j, k] - terrainHeights [j, k + 1];
																												if (diff > maxDiff || -diff > maxDiff)
																																maxDiff = Mathf.Abs (diff);
																												diff = terrainHeights [j, k] - terrainHeights [j - 1, k + 1];
																												if (diff > maxDiff || -diff > maxDiff)
																																maxDiff = Mathf.Abs (diff);
																												diff = terrainHeights [j, k] - terrainHeights [j - 1, k];
																												if (diff > maxDiff || -diff > maxDiff)
																																maxDiff = Mathf.Abs (diff);
																												diff = terrainHeights [j, k] - terrainHeights [j - 1, k - 1];
																												if (diff > maxDiff || -diff > maxDiff)
																																maxDiff = Mathf.Abs (diff);
																								}
																				}
																				maxDiff /= (_gridRoughness * 5.0f);
																				maxDiff = Mathf.Lerp (minStep, maxStep, (1.0f - maxDiff) / (1.0f + maxDiff));
																				tempTerrainRoughnessMap [linePos + c] = maxDiff; 
																}
												}

												// collapse chunks with low gradient
												float flatThreshold = maxStep * (1.0f - _gridRoughness * 0.1f);
												for (int j = 0; j < terrainRoughnessMapHeight; j++) {
																int jPos = j * terrainRoughnessMapWidth;
																for (int k = 0; k < terrainRoughnessMapWidth - 1; k++) {
																				if (tempTerrainRoughnessMap [jPos + k] >= flatThreshold) {
																								int i = k + 1;
																								while (i < terrainRoughnessMapWidth && tempTerrainRoughnessMap [jPos + i] >= flatThreshold)
																												i++;
																								while (k < i && k < terrainRoughnessMapWidth)
																												tempTerrainRoughnessMap [jPos + k] = maxStep * (i - k++);
																				}
																}
												}

												// spread min step
												for (int l = 0; l < terrainRoughnessMapHeight; l++) {
																int linePos = l * terrainRoughnessMapWidth;
																int prevLinePos = linePos - terrainRoughnessMapWidth;
																int postLinePos = linePos + terrainRoughnessMapWidth;
																for (int c = 0; c < terrainRoughnessMapWidth; c++) {
																				minStep = tempTerrainRoughnessMap [linePos + c];
																				if (l > 0) {
																								if (tempTerrainRoughnessMap [prevLinePos + c] < minStep)
																												minStep = tempTerrainRoughnessMap [prevLinePos + c];
																								if (c > 0)
																								if (tempTerrainRoughnessMap [prevLinePos + c - 1] < minStep)
																												minStep = tempTerrainRoughnessMap [prevLinePos + c - 1];
																								if (c < terrainRoughnessMapWidth - 1)
																								if (tempTerrainRoughnessMap [prevLinePos + c + 1] < minStep)
																												minStep = tempTerrainRoughnessMap [prevLinePos + c + 1];
																				}
																				if (c > 0 && tempTerrainRoughnessMap [linePos + c - 1] < minStep)
																								minStep = tempTerrainRoughnessMap [linePos + c - 1];
																				if (c < terrainRoughnessMapWidth - 1 && tempTerrainRoughnessMap [linePos + c + 1] < minStep)
																								minStep = tempTerrainRoughnessMap [linePos + c + 1];
																				if (l < terrainRoughnessMapHeight - 1) {
																								if (tempTerrainRoughnessMap [postLinePos + c] < minStep)
																												minStep = tempTerrainRoughnessMap [postLinePos + c];
																								if (c > 0)
																								if (tempTerrainRoughnessMap [postLinePos + c - 1] < minStep)
																												minStep = tempTerrainRoughnessMap [postLinePos + c - 1];
																								if (c < terrainRoughnessMapWidth - 1)
																								if (tempTerrainRoughnessMap [postLinePos + c + 1] < minStep)
																												minStep = tempTerrainRoughnessMap [postLinePos + c + 1];
																				}
																				terrainRoughnessMap [linePos + c] = minStep;
																}
												}


#if SHOW_DEBUG_GIZMOS
			for (int l=0;l<terrainRoughnessMapHeight-1;l++) {
				for (int c=0;c<terrainRoughnessMapWidth-1;c++) {
					if (terrainRoughnessMap[l,c]<0.005f) {
						GameObject marker = Instantiate(Resources.Load<GameObject>("Prefabs/Dot"));
						marker.transform.SetParent(parentDot.transform, false);
						marker.hideFlags = HideFlags.DontSave;
						marker.transform.localPosition = new Vector3(500 * ((float)c / 64 + 0.5f/64) , 1, 500* ((float)l / 64 +  0.5f/64));
						marker.transform.localScale = Misc.Vector3one * 350/512.0f;
					}
				}
			}
#endif

												return true;
								}

								void UpdateMaterialDepthOffset () {
												if (territories != null) {
																int territoriesCount = territories.Count;
																for (int c = 0; c < territoriesCount; c++) {
																				int cacheIndex = GetCacheIndexForTerritoryRegion (c);
																				GameObject surf;
																				if (surfaces.TryGetValue (cacheIndex, out surf)) {
																								if (surf != null) {
																												surf.GetComponent<Renderer> ().sharedMaterial.SetInt ("_Offset", _gridSurfaceDepthOffset);
																								}
																				}
																}
												}
												if (cells != null) {
																int cellsCount = cells.Count;
																for (int c = 0; c < cellsCount; c++) {
																				int cacheIndex = GetCacheIndexForCellRegion (c);
																				GameObject surf;
																				if (surfaces.TryGetValue (cacheIndex, out surf)) {
																								if (surf != null) {
																												surf.GetComponent<Renderer> ().sharedMaterial.SetInt ("_Offset", _gridSurfaceDepthOffset);
																								}
																				}
																}
												}
												float depthOffset = _gridMeshDepthOffset / 10000.0f;
												cellsThinMat.SetFloat ("_Offset", depthOffset);
												cellsGeoMat.SetFloat ("_Offset", depthOffset);
												territoriesMat.SetFloat ("_Offset", depthOffset);
												territoriesDisputedMat.SetFloat ("_Offset", depthOffset);
												foreach (Material mat in frontierColorCache.Values) {
																mat.SetFloat ("_Offset", depthOffset);
												}
												hudMatCellOverlay.SetInt ("_Offset", _gridSurfaceDepthOffset);
												hudMatCellGround.SetInt ("_Offset", _gridSurfaceDepthOffset - 1);
												hudMatTerritoryOverlay.SetInt ("_Offset", _gridSurfaceDepthOffset);
												hudMatTerritoryGround.SetInt ("_Offset", _gridSurfaceDepthOffset - 1);
								}

								void UpdateMaterialNearClipFade () {
												float nearClipFade;
												float nearClipFadeFallOff;
												if (_terrain == null) {
																nearClipFade = 0f;
																nearClipFadeFallOff = 0.0001f;
												} else {
																nearClipFade = _nearClipFade;
																nearClipFadeFallOff = _nearClipFadeFallOff;
												}
												if (_nearClipFadeEnabled) {
																cellsThinMat.EnableKeyword (SKW_NEAR_CLIP_FADE); 
																cellsThinMat.SetFloat ("_NearClip", nearClipFade);
																cellsThinMat.SetFloat ("_FallOff", nearClipFadeFallOff);
																cellsGeoMat.EnableKeyword (SKW_NEAR_CLIP_FADE); 
																cellsGeoMat.SetFloat ("_NearClip", nearClipFade);
																cellsGeoMat.SetFloat ("_FallOff", nearClipFadeFallOff);
																territoriesMat.EnableKeyword (SKW_NEAR_CLIP_FADE);
																territoriesMat.SetFloat ("_NearClip", nearClipFade);
																territoriesMat.SetFloat ("_FallOff", nearClipFadeFallOff);
																territoriesDisputedMat.EnableKeyword (SKW_NEAR_CLIP_FADE);
																territoriesDisputedMat.SetFloat ("_NearClip", nearClipFade);
																territoriesDisputedMat.SetFloat ("_FallOff", nearClipFadeFallOff);
																foreach (Material mat in frontierColorCache.Values) {
																				mat.EnableKeyword (SKW_NEAR_CLIP_FADE);
																				mat.SetFloat ("_NearClip", nearClipFade);
																				mat.SetFloat ("_FallOff", nearClipFadeFallOff);
																}
												} else {
																cellsThinMat.DisableKeyword (SKW_NEAR_CLIP_FADE);
																cellsGeoMat.DisableKeyword (SKW_NEAR_CLIP_FADE);
																territoriesMat.DisableKeyword (SKW_NEAR_CLIP_FADE);
																territoriesDisputedMat.EnableKeyword (SKW_NEAR_CLIP_FADE);
																foreach (Material mat in frontierColorCache.Values) {
																				mat.DisableKeyword (SKW_NEAR_CLIP_FADE);
																}
												}
								}


								void UpdateHighlightEffect () {
												if (highlightKeywords == null) {
																highlightKeywords = new List<string> ();
												} else {
																highlightKeywords.Clear ();
												}
												switch (_highlightEffect) {
												case HIGHLIGHT_EFFECT.TextureAdditive:
																highlightKeywords.Add (SKW_TEX_HIGHLIGHT_ADDITIVE);
																break;
												case HIGHLIGHT_EFFECT.TextureMultiply:
																highlightKeywords.Add (SKW_TEX_HIGHLIGHT_MULTIPLY);
																break;
												case HIGHLIGHT_EFFECT.TextureColor:
																highlightKeywords.Add (SKW_TEX_HIGHLIGHT_COLOR);
																break;
												case HIGHLIGHT_EFFECT.TextureScale:
																highlightKeywords.Add (SKW_TEX_HIGHLIGHT_SCALE);
																break;
												}
												string[] keywords = highlightKeywords.ToArray ();
												hudMatCellGround.shaderKeywords = keywords;
												hudMatCellOverlay.shaderKeywords = keywords;
												hudMatTerritoryGround.shaderKeywords = keywords;
												hudMatTerritoryOverlay.shaderKeywords = keywords;
								}

								#endregion

								#region Drawing stuff

								int GetCacheIndexForTerritoryRegion (int territoryIndex) {
												return territoryIndex; // * 1000 + regionIndex;
								}

								Material hudMatTerritory { get { return _overlayMode == OVERLAY_MODE.Overlay ? hudMatTerritoryOverlay : hudMatTerritoryGround; } }

								Material hudMatCell { get { return _overlayMode == OVERLAY_MODE.Overlay ? hudMatCellOverlay : hudMatCellGround; } }

								Material GetColoredTexturedMaterial (Color color, Texture2D texture, bool overlay) {
												Dictionary<Color, Material> matCache = overlay ? coloredMatCacheOverlay : coloredMatCacheGround;
												Material mat;
												if (texture == null && matCache.TryGetValue (color, out mat)) {
																return mat;
												} else {
																Material customMat;
																if (texture != null) {
																				mat = overlay ? texturizedMatOverlay : texturizedMatGround;
																				customMat = Instantiate (mat);
																				customMat.name = mat.name;
																				customMat.mainTexture = texture;
																} else {
																				mat = overlay ? coloredMatOverlay : coloredMatGround;
																				customMat = Instantiate (mat);
																				customMat.name = mat.name;
																				matCache [color] = customMat;
																}
																customMat.color = color;
																customMat.hideFlags = HideFlags.DontSave;
																customMat.SetFloat ("_Offset", _gridSurfaceDepthOffset);
																return customMat;
												}
								}

								Material GetFrontierColorMaterial (Color color) {
												if (color == territoriesMat.color)
																return territoriesMat;

												Material mat;
												if (frontierColorCache.TryGetValue (color, out mat)) {
																return mat;
												} else {
																Material customMat = Instantiate (territoriesMat) as Material;
																customMat.name = territoriesMat.name;
																customMat.color = color;
																customMat.hideFlags = HideFlags.DontSave;
																frontierColorCache [color] = customMat;
																return customMat;
												}
								}

								void ApplyMaterialToSurface (GameObject obj, Material sharedMaterial) {
												if (obj != null) {
																Renderer r = obj.GetComponent<Renderer> ();
																if (r != null)
																				r.sharedMaterial = sharedMaterial;
												}
								}

								void DrawColorizedTerritories () {
												if (territories == null)
																return;
												int territoriesCount = territories.Count;
												for (int k = 0; k < territoriesCount; k++) {
																Territory territory = territories [k];
																Region region = territory.region;
																if (region.customMaterial != null) {
																				TerritoryToggleRegionSurface (k, true, region.customMaterial.color, (Texture2D)region.customMaterial.mainTexture, region.customTextureScale, region.customTextureOffset, region.customTextureRotation, region.customRotateInLocalSpace);
																} else {
																				Color fillColor = territories [k].fillColor;
																				fillColor.a *= colorizedTerritoriesAlpha;
																				TerritoryToggleRegionSurface (k, true, fillColor);
																}
												}
								}

								public void GenerateMap () {
												recreateCells = true;
												recreateTerritories = true;
												if (cells != null)
																cells.Clear ();
												if (territories != null)
																territories.Clear ();
												Redraw ();
												if (territoriesTexture != null) {
																CreateTerritories (territoriesTexture, territoriesTextureNeutralColor);
												}
												// Reload configuration if component exists
												TGSConfig[] configs = GetComponents<TGSConfig> ();
												for (int k = 0; k < configs.Length; k++) {
																if (configs [k].enabled)
																				configs [k].LoadConfiguration ();
												}
								}

								void ReloadMask () {
												ReadMaskContents (); 
												if (_gridMask == null || mask == null) {
																int cellsCount = cells.Count;
																for (int k = 0; k < cellsCount; k++) {
																				cells [k].visible = true;
																}
												}	
												CellsApplyVisibilityFilters ();
												recreateTerritories = true;
												Redraw ();
												if (territoriesTexture != null) {
																CreateTerritories (territoriesTexture, territoriesTextureNeutralColor);
												}
								}

								/// <summary>
								/// Refresh grid.
								/// </summary>
								public void Redraw () {
												Redraw (false);
								}

								/// <summary>
								/// Refresh grid. Set reuseTerrainData to true to avoid computation of terrain heights and slope (useful if terrain is not changed).
								/// </summary>
								public void Redraw (bool reuseTerrainData) {

												if (!gameObject.activeInHierarchy)
																return;

												shouldRedraw = false;

												// Initialize surface cache
												if (surfaces != null) {
																List<GameObject> cached = new List<GameObject> (surfaces.Values);
																int cachedCount = cached.Count;
																for (int k = 0; k < cachedCount; k++) {
																				if (cached [k] != null)
																								DestroyImmediate (cached [k]);
																}
												} else {
																surfaces = new Dictionary<int, GameObject> ();
												}
												DestroySurfaces ();
												ClearLastOver ();

												if (!UpdateTerrainReference (_terrain, reuseTerrainData))
																return;
												refreshCellMesh = true;
												_lastVertexCount = 0;
												CheckCells ();
												if (_showCells) {
																DrawCellBorders ();
																DrawColorizedCells ();
												}

												refreshTerritoriesMesh = true;
												CheckTerritories ();
												if (_showTerritories) {
																DrawTerritoryBorders ();
												}
												if (_colorizeTerritories) {
																DrawColorizedTerritories ();
												}
												UpdateMaterialDepthOffset ();
												UpdateMaterialNearClipFade ();
												UpdateHighlightEffect ();
								}

								void CheckCells () {
												if (!_showCells && !_showTerritories && !_colorizeTerritories && _highlightMode == HIGHLIGHT_MODE.None)
																return;
												if (cells == null || recreateCells) {
																CreateCells ();
																refreshCellMesh = true;
												}
												if (refreshCellMesh) {
																CellsApplyVisibilityFilters ();
																GenerateCellsMesh ();
																refreshCellMesh = false;
																refreshTerritoriesMesh = true;
												}
								}

								void DrawCellBorders () {

												if (cellLayer != null) {
																DestroyImmediate (cellLayer);
												} else {
																Transform t = transform.Find (CELLS_LAYER_NAME);
																if (t != null)
																				DestroyImmediate (t.gameObject);
												}
												if (cells.Count == 0)
																return;

												cellLayer = new GameObject (CELLS_LAYER_NAME);
												cellLayer.hideFlags = HideFlags.DontSave;
												cellLayer.transform.SetParent (transform, false);
												cellLayer.transform.localPosition = Vector3.back * 0.001f;
		
												for (int k = 0; k < cellMeshBorders.Length; k++) {
																GameObject flayer = new GameObject ("flayer");
																flayer.hideFlags = HideFlags.DontSave;
																flayer.transform.SetParent (cellLayer.transform, false);
																flayer.transform.localPosition = Misc.Vector3zero;
																flayer.transform.localRotation = Quaternion.Euler (Misc.Vector3zero);
			
																Mesh mesh = new Mesh ();
																mesh.vertices = cellMeshBorders [k];
																mesh.SetIndices (cellMeshIndices [k], MeshTopology.Lines, 0);

																mesh.RecalculateBounds ();
																mesh.hideFlags = HideFlags.DontSave;
			
																MeshFilter mf = flayer.AddComponent<MeshFilter> ();
																mf.sharedMesh = mesh;
																_lastVertexCount += mesh.vertexCount;
			
																MeshRenderer mr = flayer.AddComponent<MeshRenderer> ();
																mr.receiveShadows = false;
																mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
																mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
																mr.sharedMaterial = cellsMat;
												}
												cellLayer.SetActive (_showCells);
												cellsGeoMat.SetFloat ("_Thickness", _cellBorderThickness - 0.8f);
								}

								void DrawColorizedCells () {
												int cellsCount = cells.Count;
												for (int k = 0; k < cellsCount; k++) {
																Cell cell = cells [k];
																Region region = cell.region;
																if (region.customMaterial != null && cell.visible) {
																				CellToggleRegionSurface (k, true, region.customMaterial.color, false, (Texture2D)region.customMaterial.mainTexture, region.customTextureScale, region.customTextureOffset, region.customTextureRotation, region.customRotateInLocalSpace);
																}
												}
								}

								void CheckTerritories () {
												if (!territoriesAreUsed)
																return;
												if (territories == null || recreateTerritories) {
																CreateTerritories ();
																refreshTerritoriesMesh = true;
												} else if (needUpdateTerritories) {
																FindTerritoryFrontiers ();
																UpdateTerritoryBoundaries ();
																needUpdateTerritories = false;
																refreshTerritoriesMesh = true;
												}
			
												if (refreshTerritoriesMesh) {
																GenerateTerritoriesMesh ();
																refreshTerritoriesMesh = false;
												}
			
								}

								void DrawTerritoryBorders () {

												if (territoryLayer != null) {
																DestroyImmediate (territoryLayer);
												} else {
																Transform t = transform.Find (TERRITORIES_LAYER_NAME);
																if (t != null)
																				DestroyImmediate (t.gameObject);
												}
												if (territories.Count == 0)
																return;

												territoryLayer = new GameObject (TERRITORIES_LAYER_NAME);
												territoryLayer.hideFlags = HideFlags.DontSave;
												territoryLayer.transform.SetParent (transform, false);
												territoryLayer.transform.localPosition = Vector3.back * 0.001f;

												for (int t = 0; t < territoryMeshes.Count; t++) {
																TerritoryMesh tm = territoryMeshes [t];

																for (int k = 0; k < tm.territoryMeshBorders.Length; k++) {
																				GameObject flayer = new GameObject ("flayer");
																				flayer.hideFlags = HideFlags.DontSave;
																				flayer.transform.SetParent (territoryLayer.transform, false);
																				flayer.transform.localPosition = Vector3.back * 0.001f;
																				flayer.transform.localRotation = Quaternion.Euler (Misc.Vector3zero);
				
																				Mesh mesh = new Mesh ();
																				mesh.vertices = tm.territoryMeshBorders [k];
																				mesh.SetIndices (tm.territoryMeshIndices [k], MeshTopology.Lines, 0);

																				mesh.RecalculateBounds ();
																				mesh.hideFlags = HideFlags.DontSave;
				
																				MeshFilter mf = flayer.AddComponent<MeshFilter> ();
																				mf.sharedMesh = mesh;
																				_lastVertexCount += mesh.vertexCount;

																				MeshRenderer mr = flayer.AddComponent<MeshRenderer> ();
																				mr.receiveShadows = false;
																				mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
																				mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

																				Material mat;
																				if (tm.territoryIndex < 0) {
																								mat = territoriesDisputedMat;
																				} else {
																								Color frontierColor = territories [tm.territoryIndex].frontierColor;
																								if (frontierColor.a == 0 && frontierColor.r == 0 && frontierColor.g == 0 && frontierColor.b == 0) {
																												mat = territoriesMat;
																								} else {
																												mat = GetFrontierColorMaterial (frontierColor);
																								}
																				}
																				mr.sharedMaterial = mat;
																}
												}

												territoryLayer.SetActive (_showTerritories);

								}

								void PrepareNewSurfaceMesh (int pointCount) {
												if (meshPoints == null) {
																meshPoints = new List<Vector3> (pointCount);
												} else {
																meshPoints.Clear ();
												}
												triNew = new int[pointCount];
												if (surfaceMeshHit == null)
																surfaceMeshHit = new Dictionary<TriangulationPoint, int> (20000);
												else
																surfaceMeshHit.Clear ();
			
												triNewIndex = -1;
												newPointsCount = -1;
								}

								void AddPointToSurfaceMeshWithNormalOffset (TriangulationPoint p) {
												int tri;
												if (surfaceMeshHit.TryGetValue (p, out tri)) {
																triNew [++triNewIndex] = tri;
												} else {
																Vector3 np = new Vector3 (p.Xf - 2, p.Yf - 2, -p.Zf);
																np += transform.InverseTransformVector (_terrain.terrainData.GetInterpolatedNormal (np.x + 0.5f, np.y + 0.5f)) * _gridNormalOffset;
																meshPoints.Add (np);
																surfaceMeshHit.Add (p, ++newPointsCount);
																triNew [++triNewIndex] = newPointsCount;
												}
								}

								void AddPointToSurfaceMeshWithoutNormalOffset (TriangulationPoint p) {
												int tri;
												if (surfaceMeshHit.TryGetValue (p, out tri)) {
																triNew [++triNewIndex] = tri;
												} else {
																Vector3 np = new Vector3 (p.Xf - 2, p.Yf - 2, -p.Zf);
																meshPoints.Add (np);
																surfaceMeshHit.Add (p, ++newPointsCount);
																triNew [++triNewIndex] = newPointsCount;
												}
								}

								Poly2Tri.Polygon GetPolygon (Region region, out List<PolygonPoint> ppoints, bool reduce = false) {
												// Calculate region's surface points
												ppoints = null;
												int numSegments = region.segments.Count;
												if (numSegments == 0)
																return null;
			
												Connector connector = new Connector ();
												if (_terrain == null) {
																for (int i = 0; i < numSegments; i++) {
																				Segment s = region.segments [i];
																				connector.Add (GetScaledSegment (s));
																}
												} else {
																for (int i = 0; i < numSegments; i++) {
																				Segment s = region.segments [i];
																				SurfaceSegmentForSurface (GetScaledSegment (s), connector);
																}
												}
												Geom.Polygon surfacedPolygon = connector.ToPolygonFromLargestLineStrip ();
												if (surfacedPolygon == null)
																return null;
			
												List<Point> surfacedPoints = surfacedPolygon.contours [0].points;
			
												int spCount = surfacedPoints.Count;
												ppoints = new List<PolygonPoint> (spCount);
												double midx = 0, midy = 0;
												for (int k = 0; k < spCount; k++) {
																double x = surfacedPoints [k].x + 2 + k / 1000000.0;	// these additions are required to prevent issues with polytri library
																double y = surfacedPoints [k].y + 2 + k / 1000000.0;
																if (!IsTooNearPolygon (x, y, ppoints)) {
																				float h = _terrain != null ? _terrain.SampleHeight (transform.TransformPoint ((float)x - 2, (float)y - 2, 0)) : 0;
																				ppoints.Add (new PolygonPoint (x, y, h));
																				midx += x;
																				midy += y;
																}
												}
												int ppointsCount = ppoints.Count;
												if (ppointsCount < 3)
																return null;
												if (reduce) {
																midx /= ppointsCount;
																midy /= ppointsCount;
																for (int k = 0; k < ppointsCount; k++) {
																				PolygonPoint p = ppoints [k];
																				double DX = midx - p.X;
																				double DY = midy - p.Y;
																				ppoints [k] = new PolygonPoint (p.X + DX * 0.0001, p.Y + DY * 0.0001);
																}
												}
												return new Poly2Tri.Polygon (ppoints);
								}

								GameObject GenerateRegionSurface (Region region, int cacheIndex, Material material, Vector2 textureScale, Vector2 textureOffset, float textureRotation, bool rotateInLocalSpace) {
												List<PolygonPoint> ppoints;
												Poly2Tri.Polygon poly = GetPolygon (region, out ppoints);
												if (poly == null)
																return null;

												// Support for internal territories
												if (_allowTerritoriesInsideTerritories && region.entity is Territory) {
																int terrCount = territories.Count;
																for (int ot = 0; ot < terrCount; ot++) {
																				Territory oter = territories [ot];
																				if (oter.region != region && region.Contains (oter.region)) {
																								List<PolygonPoint> dummyPoints;
																								Poly2Tri.Polygon oterPoly = GetPolygon (oter.region, out dummyPoints, true);
																								if (oterPoly != null)
																												poly.AddHole (oterPoly);
																				}
																}
												}

												if (_terrain != null) {
				
																if (steinerPoints == null) {
																				steinerPoints = new List<TriangulationPoint> (6000);
																} else {
																				steinerPoints.Clear ();
																}
				
																float stepX = 1.0f / heightMapWidth;
																float smallStep = 1.0f / heightMapWidth;
																float y = region.rect2D.yMin + smallStep;
																float ymax = region.rect2D.yMax - smallStep;
																float[] acumY = new float[terrainRoughnessMapWidth];
																int steinerPointsCount = 0;
																while (y < ymax) {
																				int j = (int)((y + 0.5f) * terrainRoughnessMapHeight); // * heightMapHeight)) / TERRAIN_CHUNK_SIZE;
																				if (j >= terrainRoughnessMapHeight)
																								j = terrainRoughnessMapHeight - 1;
																				else if (j < 0)
																								j = 0;
																				int jPos = j * terrainRoughnessMapWidth;
																				float sy = y + 2;
																				float xin, xout;
																				GetFirstAndLastPointInRow (sy, ppoints, out xin, out xout);
//																				float xin = GetFirstPointInRow (sy, ppoints) + smallStep;
//																				float xout = GetLastPointInRow (sy, ppoints) - smallStep;
																				xin += smallStep;
																				xout -= smallStep;
																				int k0 = -1;
																				for (float x = xin; x < xout; x += stepX) {
																								int k = (int)((x + 0.5f) * terrainRoughnessMapWidth); //)) / TERRAIN_CHUNK_SIZE;
																								if (k >= terrainRoughnessMapWidth)
																												k = terrainRoughnessMapWidth - 1;
																								else if (k < 0)
																												k = 0;
																								if (k0 != k) {
																												k0 = k;
																												stepX = terrainRoughnessMap [jPos + k];
																												if (acumY [k] >= stepX)
																																acumY [k] = 0;
																												acumY [k] += smallStep;
																								}
																								if (acumY [k] >= stepX) {
																												// Gather precision height
																												float h = _terrain.SampleHeight (transform.TransformPoint (x, y, 0));
																												float htl = _terrain.SampleHeight (transform.TransformPoint (x - smallStep, y + smallStep, 0));
																												if (htl > h)
																																h = htl;
																												float htr = _terrain.SampleHeight (transform.TransformPoint (x + smallStep, y + smallStep, 0));
																												if (htr > h)
																																h = htr;
																												float hbr = _terrain.SampleHeight (transform.TransformPoint (x + smallStep, y - smallStep, 0));
																												if (hbr > h)
																																h = hbr;
																												float hbl = _terrain.SampleHeight (transform.TransformPoint (x - smallStep, y - smallStep, 0));
																												if (hbl > h)
																																h = hbl;
																												steinerPoints.Add (new PolygonPoint (x + 2, sy, h));		
																												steinerPointsCount++;
																								}
																				}
																				y += smallStep;
																				if (steinerPointsCount > 80000) {
																								break;
																				}
																}
																poly.AddSteinerPoints (steinerPoints);
												}

												P2T.Triangulate (poly);

												string cacheIndexSTR = cacheIndex.ToString ();
												// Deletes potential residual surface
												Transform t = surfacesLayer.transform.Find (cacheIndexSTR);
												if (t != null) {
																DestroyImmediate (t.gameObject);
												}
												Rect rect = (canvasTexture != null && material != null && material.mainTexture == canvasTexture) ? canvasRect : region.rect2D;

												// Calculate & optimize mesh data
												int triCount = poly.Triangles.Count;
												GameObject parentSurf = null;

												for (int triBase = 0; triBase < triCount; triBase += 65500) {
																int meshTriCount = 65500;
																if (triBase + meshTriCount > triCount) {
																				meshTriCount = triCount - triBase;
																}

																PrepareNewSurfaceMesh (meshTriCount * 3);

																if (_terrain != null && _gridNormalOffset > 0) {
																				for (int k = 0; k < meshTriCount; k++) {
																								DelaunayTriangle dt = poly.Triangles [k + triBase];
																								AddPointToSurfaceMeshWithNormalOffset (dt.Points [0]);
																								AddPointToSurfaceMeshWithNormalOffset (dt.Points [2]);
																								AddPointToSurfaceMeshWithNormalOffset (dt.Points [1]);
																				}
																} else {
																				for (int k = 0; k < meshTriCount; k++) {
																								DelaunayTriangle dt = poly.Triangles [k + triBase];
																								AddPointToSurfaceMeshWithoutNormalOffset (dt.Points [0]);
																								AddPointToSurfaceMeshWithoutNormalOffset (dt.Points [2]);
																								AddPointToSurfaceMeshWithoutNormalOffset (dt.Points [1]);
																				}
																}
																string surfName = triBase == 0 ? cacheIndexSTR : "splitMesh";
																GameObject surf = Drawing.CreateSurface (surfName, meshPoints.ToArray (), triNew, material, rect, textureScale, textureOffset, textureRotation, rotateInLocalSpace);
																_lastVertexCount += meshPoints.Count;
																if (triBase == 0) {
																				surf.transform.SetParent (surfacesLayer.transform, false);
																				if (surfaces.ContainsKey (cacheIndex)) {
																								surfaces.Remove (cacheIndex);
																				}
																				surfaces.Add (cacheIndex, surf);
																} else {
																				surf.transform.SetParent (parentSurf.transform, false);
																}
																surf.transform.localPosition = Misc.Vector3zero;
																surf.layer = gameObject.layer;
																parentSurf = surf;
												}
												return parentSurf;
								}


								#endregion


								#region Internal API

								public string GetMapData () {
												return "";
								}

								float goodGridRelaxation {
												get {
																if (_numCells >= MAX_CELLS_FOR_RELAXATION) {
																				return 1;
																} else {
																				return _gridRelaxation;
																}
												}
								}

								float goodGridCurvature {
												get {
																if (_numCells >= MAX_CELLS_FOR_CURVATURE) {
																				return 0;
																} else {
																				return _gridCurvature;
																}
												}
								}

								int FastConvertToInt (string s) {
												int value = 0;
												int start, sign;
												if (s [0] == '-') {
																start = 1;
																sign = -1;
												} else {
																start = 0;
																sign = 1;
												}
												for (int i = start; i < s.Length; i++) {
																value = value * 10 + (s [i] - '0');
												}
												return value * sign;
								}

								/// <summary>
								/// Issues a selection check based on a given ray. Used by editor to manipulate cells from Scene window.
								/// Returns true if ray hits the grid.
								/// </summary>
								public bool CheckRay (Ray ray) {
												useEditorRay = true;
												editorRay = ray;
												return CheckMousePos ();
								}


								#endregion


								#region Highlighting

								void OnMouseEnter () {
												mouseIsOver = true;
												ClearLastOver ();
								}

								void OnMouseExit () {
												// Make sure it's outside of grid
												Vector3 mousePos = Input.mousePosition;
												Ray ray = Camera.main.ScreenPointToRay (mousePos);
												RaycastHit[] hits = Physics.RaycastAll (ray.origin, ray.direction, 5000);
												if (hits.Length > 0) {
																for (int k = 0; k < hits.Length; k++) {
																				if (hits [k].collider.gameObject == gameObject)
																								return; 
																}
												}
												mouseIsOver = false;
												ClearLastOver ();
								}

								void ClearLastOver () {
												_cellLastOver = null;
												_cellLastOverIndex = -1;
												_territoryLastOver = null;
												_territoryLastOverIndex = -1;
								}

								bool GetLocalHitFromMousePos (out Vector3 localPoint) {
			
												Ray ray;
												localPoint = Misc.Vector3zero;

												if (useEditorRay && !Application.isPlaying) {
																ray = editorRay;
												} else {
																if (!mouseIsOver && !Application.isMobilePlatform)
																				return false;
																Vector3 mousePos = Input.mousePosition;
																if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height) {
																				localPoint = Misc.Vector3zero;
																				return false;
																}
																ray = Camera.main.ScreenPointToRay (mousePos);
												}
												RaycastHit[] hits = Physics.RaycastAll (ray, 5000);
												if (hits.Length > 0) {
																if (_terrain != null) {
																				float minDistance = _highlightMinimumTerrainDistance * _highlightMinimumTerrainDistance;
																				for (int k = 0; k < hits.Length; k++) {
																								if (hits [k].collider.gameObject == _terrain.gameObject) {
																												if ((hits [k].point - ray.origin).sqrMagnitude > minDistance) {
																																localPoint = _terrain.transform.InverseTransformPoint (hits [k].point);
																																float w = _terrain.terrainData.size.x;
																																float d = _terrain.terrainData.size.z;
																																localPoint.x = localPoint.x / w - 0.5f;
																																localPoint.y = localPoint.z / d - 0.5f;
																																return true;
																												}
																								}
																				}
																} else {
																				for (int k = 0; k < hits.Length; k++) {
																								if (hits [k].collider.gameObject == gameObject) {
																												localPoint = transform.InverseTransformPoint (hits [k].point);
																												return true;
																								}
																				}
																}
												}
												return false;
								}

								bool GetLocalHitFromWorldPosition (ref Vector3 position) {
												if (_terrain != null) {
																Ray ray = new Ray (position - transform.forward * 100, transform.forward);
																RaycastHit[] hits = Physics.RaycastAll (ray, 5000);
																bool goodHit = false;
																if (hits.Length > 0) {
																				for (int k = 0; k < hits.Length; k++) {
																								if (hits [k].collider.gameObject == _terrain.gameObject) {
																												Vector3 localPoint = _terrain.transform.InverseTransformPoint (hits [k].point);
																												float w = _terrain.terrainData.size.x;
																												float d = _terrain.terrainData.size.z;
																												position.x = localPoint.x / w - 0.5f;
																												position.y = localPoint.z / d - 0.5f;
																												position.z = 0;
																												goodHit = true;
																												break;
																								}
																				}
																}
																if (!goodHit)
																				return false;
												} else {
																position = transform.InverseTransformPoint (position);
												}
												return true;
								}

								bool CheckMousePos () {
												if (_highlightMode == HIGHLIGHT_MODE.None || (!Application.isPlaying && !useEditorRay))
																return false;
			
												Vector3 localPoint;
												bool goodHit = GetLocalHitFromMousePos (out localPoint);
												if (!goodHit) {
																HideTerritoryRegionHighlight ();
																return false;
												}

												// verify if last highlighted territory remains active
												bool sameTerritoryHighlight = false;
												float sameTerritoryArea = float.MaxValue;
												if (_territoryLastOver != null) {
																if (_territoryLastOver.visible && _territoryLastOver.region.Contains (localPoint.x, localPoint.y)) { 
																				sameTerritoryHighlight = true;
																				sameTerritoryArea = _territoryLastOver.region.rect2DArea;
																}
												}
												int newTerritoryHighlightedIndex = -1;

												// mouse if over the grid - verify if hitPos is inside any territory polygon
												if (territories != null) {
																int terrCount = sortedTerritories.Count;
																for (int c = 0; c < terrCount; c++) {
																				Region sreg = _sortedTerritories [c].region;
																				if (sreg != null && _sortedTerritories [c].visible && _sortedTerritories [c].cells != null && _sortedTerritories [c].cells.Count > 0) {
																								if (sreg.Contains (localPoint.x, localPoint.y)) {
																												newTerritoryHighlightedIndex = TerritoryGetIndex (_sortedTerritories [c]);
																												sameTerritoryHighlight = newTerritoryHighlightedIndex == _territoryLastOverIndex;
																												break;
																								}
																								if (sreg.rect2DArea > sameTerritoryArea)
																												break;
																				}
																}	
												}

												// verify if last highlited cell remains active
												bool sameCellHighlight = false;
												if (_cellLastOver != null) {
																if (_cellLastOver.region.Contains (localPoint.x, localPoint.y)) { 
																				sameCellHighlight = true;
																}
												}
												int newCellHighlightedIndex = -1;

												if (!sameCellHighlight) {
																if (_highlightMode == HIGHLIGHT_MODE.Cells || !Application.isPlaying) {
																				if (_territoryLastOver != null) {
																								int cellsCount = _territoryLastOver.cells.Count;
																								for (int p = 0; p < cellsCount; p++) {
																												Cell cell = _territoryLastOver.cells [p];
																												if (cell.region.Contains (localPoint.x, localPoint.y)) {
																																newCellHighlightedIndex = CellGetIndex (cell);
																																break;
																												}
																								}
																				} else {
																								int sortedCellsCount = sortedCells.Count;
																								for (int p = 0; p < sortedCellsCount; p++) {
																												Cell cell = sortedCells [p];
																												if (cell.region.Contains (localPoint.x, localPoint.y)) {
																																newCellHighlightedIndex = CellGetIndex (cell);
																																break;
																												}
																								}
																				}
																}
												}

												if (!sameTerritoryHighlight) {
																if (_territoryLastOverIndex >= 0 && OnTerritoryExit != null)
																				OnTerritoryExit (_territoryLastOverIndex);
																if (newTerritoryHighlightedIndex >= 0 && OnTerritoryEnter != null)
																				OnTerritoryEnter (newTerritoryHighlightedIndex);
																_territoryLastOverIndex = newTerritoryHighlightedIndex;
																if (_territoryLastOverIndex >= 0)
																				_territoryLastOver = territories [_territoryLastOverIndex];
																else
																				_territoryLastOver = null;
												}
												if (!sameCellHighlight) {
																if (_cellLastOverIndex >= 0 && OnCellExit != null)
																				OnCellExit (_cellLastOverIndex);
																if (newCellHighlightedIndex >= 0 && OnCellEnter != null)
																				OnCellEnter (newCellHighlightedIndex);
																_cellLastOverIndex = newCellHighlightedIndex;
																if (newCellHighlightedIndex >= 0)
																				_cellLastOver = cells [newCellHighlightedIndex];
																else
																				_cellLastOver = null;
												}

												if (_highlightMode == HIGHLIGHT_MODE.Cells || !Application.isPlaying) {
																if (!sameCellHighlight) {
																				if (newCellHighlightedIndex >= 0 && (cells [newCellHighlightedIndex].visible || _cellHighlightNonVisible)) {
																								HighlightCellRegion (newCellHighlightedIndex, false);
																				} else {
																								HideCellRegionHighlight ();
																				}
																}
												} else if (_highlightMode == HIGHLIGHT_MODE.Territories) {
																if (!sameTerritoryHighlight) {
																				if (newTerritoryHighlightedIndex >= 0 && territories [newTerritoryHighlightedIndex].visible) {
																								HighlightTerritoryRegion (newTerritoryHighlightedIndex, false);
																				} else {
																								HideTerritoryRegionHighlight ();
																				}
																}
												}

												return true;
								}

								void UpdateHighlightFade () {
												if (_highlightFadeAmount == 0)
																return;

												if (_highlightedObj != null) {
																float newAlpha = _highlightFadeMin + Mathf.PingPong (Time.time * _highlightFadeSpeed - highlightFadeStart, _highlightFadeAmount - _highlightFadeMin);
																Material mat = highlightMode == HIGHLIGHT_MODE.Territories ? hudMatTerritory : hudMatCell;
																mat.SetFloat ("_FadeAmount", newAlpha);
																float newScale = _highlightScaleMin + Mathf.PingPong (Time.time * highlightFadeSpeed, _highlightScaleMax - _highlightScaleMin);
																mat.SetFloat ("_Scale", 1f / (newScale + 0.0001f));
																_highlightedObj.GetComponent<Renderer> ().sharedMaterial = mat;
												}
								}

								void TriggerEvents () {
												if (Input.GetMouseButtonDown (0)) {
																// record last clicked cell/territory
																_cellLastClickedIndex = _cellLastOverIndex;
																_territoryLastClickedIndex = _territoryLastOverIndex;

																if (_territoryLastOverIndex >= 0 && OnTerritoryMouseDown != null) {
																				OnTerritoryMouseDown (_territoryLastOverIndex, 0);
																}
																if (_cellLastOverIndex >= 0 && OnCellMouseDown != null) {
																				OnCellMouseDown (_cellLastOverIndex, 0);
																}
												}
												if (Input.GetMouseButtonUp (0)) {
																if (_territoryLastOverIndex >= 0) {
																				if (_territoryLastOverIndex == _territoryLastClickedIndex && OnTerritoryClick != null) {
																								OnTerritoryClick (_territoryLastOverIndex, 0);
																				}
																				if (OnTerritoryMouseUp != null) {
																								OnTerritoryMouseUp (_territoryLastOverIndex, 0);
																				}
																}
																if (_cellLastOverIndex >= 0) {
																				if (_cellLastOverIndex == _cellLastClickedIndex && OnCellClick != null) {
																								OnCellClick (_cellLastOverIndex, 0);
																				}
																				if (OnCellMouseUp != null) {
																								OnCellMouseUp (_cellLastOverIndex, 0);
																				}

																}
												}
								}

								#endregion

	
								#region Geometric functions

								Vector3 GetWorldSpacePosition (Vector2 localPosition) {
												if (_terrain != null) {
																Vector3 localCenter = new Vector3 ((localPosition.x + 0.5f) * terrainWidth, 0, (localPosition.y + 0.5f) * terrainDepth);
																localCenter.y = _terrain.SampleHeight (_terrain.transform.TransformPoint (localCenter));
																return _terrain.transform.TransformPoint (localCenter);
												} else {
																return transform.TransformPoint (new Vector2 (localPosition.x, localPosition.y));
												}
								}

								Vector3 GetScaledVector (Vector3 p) {
												p.x *= _gridScale.x;
												p.x += _gridCenter.x;
												p.y *= _gridScale.y;
												p.y += _gridCenter.y;
												return p;
								}

								Point GetScaledPoint (Point p) {
												p.x *= _gridScale.x;
												p.x += _gridCenter.x;
												p.y *= _gridScale.y;
												p.y += _gridCenter.y;
												return p;
								}

								Segment GetScaledSegment (Segment s) {
												Segment ss = new Segment (s.start, s.end, s.border);
												ss.start = GetScaledPoint (ss.start);
												ss.end = GetScaledPoint (ss.end);
												return ss;
								}


								#endregion

		
		
								#region Territory stuff

								void HideTerritoryRegionHighlight () {
												HideCellRegionHighlight ();
												if (_territoryHighlighted == null)
																return;
												if (_highlightedObj != null) {
																if (_territoryHighlighted.region.customMaterial != null) {
																				ApplyMaterialToSurface (_highlightedObj, _territoryHighlighted.region.customMaterial);
																} else if (_highlightedObj.GetComponent<SurfaceFader> () == null) {
																				_highlightedObj.SetActive (false);
																}
																_highlightedObj = null;
												}
												_territoryHighlighted = null;
												_territoryHighlightedIndex = -1;
								}

								/// <summary>
								/// Highlights the territory region specified. Returns the generated highlight surface gameObject.
								/// Internally used by the Map UI and the Editor component, but you can use it as well to temporarily mark a territory region.
								/// </summary>
								/// <param name="refreshGeometry">Pass true only if you're sure you want to force refresh the geometry of the highlight (for instance, if the frontiers data has changed). If you're unsure, pass false.</param>
								GameObject HighlightTerritoryRegion (int territoryIndex, bool refreshGeometry) {
												if (_territoryHighlighted != null)
																HideTerritoryRegionHighlight ();
												if (territoryIndex < 0 || territoryIndex >= territories.Count)
																return null;

												if (_highlightEffect != HIGHLIGHT_EFFECT.None) {
																if (OnTerritoryHighlight != null) {
																				bool cancelHighlight = false;
																				OnTerritoryHighlight (territoryIndex, ref cancelHighlight);
																				if (cancelHighlight)
																								return null;
																}

																int cacheIndex = GetCacheIndexForTerritoryRegion (territoryIndex); 
																bool existsInCache = surfaces.ContainsKey (cacheIndex);
																if (refreshGeometry && existsInCache) {
																				GameObject obj = surfaces [cacheIndex];
																				surfaces.Remove (cacheIndex);
																				DestroyImmediate (obj);
																				existsInCache = false;
																}
																if (existsInCache) {
																				_highlightedObj = surfaces [cacheIndex];
																				if (_highlightedObj == null) {
																								surfaces.Remove (cacheIndex);
																				} else {
																								if (!_highlightedObj.activeSelf)
																												_highlightedObj.SetActive (true);
																								Renderer rr = _highlightedObj.GetComponent<Renderer> ();
																								if (rr.sharedMaterial != hudMatTerritory)
																												rr.sharedMaterial = hudMatTerritory;
																				}
																} else {
																				_highlightedObj = GenerateTerritoryRegionSurface (territoryIndex, hudMatTerritory, Misc.Vector2one, Misc.Vector2zero, 0, false);
																}
																// Reuse territory texture
																Territory territory = territories [territoryIndex];
																if (territory.region.customMaterial != null) {
																				hudMatTerritory.mainTexture = territory.region.customMaterial.mainTexture;
																} else {
																				hudMatTerritory.mainTexture = null;
																}
																highlightFadeStart = Time.time;
												}

												_territoryHighlightedIndex = territoryIndex;
												_territoryHighlighted = territories [territoryIndex];

												return _highlightedObj;
								}

								GameObject GenerateTerritoryRegionSurface (int territoryIndex, Material material, Vector2 textureScale, Vector2 textureOffset, float textureRotation, bool rotateInLocalSpace) {
												if (territoryIndex < 0 || territoryIndex >= territories.Count)
																return null;
												Region region = territories [territoryIndex].region;
												int cacheIndex = GetCacheIndexForTerritoryRegion (territoryIndex); 
												return GenerateRegionSurface (region, cacheIndex, material, textureScale, textureOffset, textureRotation, rotateInLocalSpace);
								}

								void UpdateColorizedTerritoriesAlpha () {
												if (territories == null)
																return;
												int territoriesCount = territories.Count;
												for (int c = 0; c < territoriesCount; c++) {
																Territory territory = territories [c];
																int cacheIndex = GetCacheIndexForTerritoryRegion (c);
																GameObject surf;
																if (surfaces.TryGetValue (cacheIndex, out surf)) {
																				if (surf != null) {
																								Color newColor = surf.GetComponent<Renderer> ().sharedMaterial.color;
																								newColor.a = territory.fillColor.a * _colorizedTerritoriesAlpha;
																								surf.GetComponent<Renderer> ().sharedMaterial.color = newColor;
																				}
																}
												}
								}

								Territory GetTerritoryAtPoint (Vector3 position, bool worldSpace) {
												if (worldSpace) {
																if (!GetLocalHitFromWorldPosition (ref position))
																				return null;
												}
												int territoriesCount = territories.Count;
												for (int p = 0; p < territoriesCount; p++) {
																Territory territory = territories [p];
																if (territory.region.Contains (position.x, position.y)) {
																				return territory;
																}
												}
												return null;
								}

								void TerritoryAnimate (FADER_STYLE style, int territoryIndex, Color color, float duration) {
												if (territoryIndex < 0 || territoryIndex >= territories.Count)
																return;
												int cacheIndex = GetCacheIndexForTerritoryRegion (territoryIndex);
												GameObject territorySurface = null;
												surfaces.TryGetValue (cacheIndex, out territorySurface);
												if (territorySurface == null) {
																territorySurface = TerritoryToggleRegionSurface (territoryIndex, true, color);
																territories [territoryIndex].region.customMaterial = null;
												} else {
																territorySurface.SetActive (true);
												}
												Renderer renderer = territorySurface.GetComponent<Renderer> ();
												Material oldMaterial = renderer.sharedMaterial;
												Material fadeMaterial = Instantiate (hudMatTerritory);
												Region region = territories [territoryIndex].region;
												fadeMaterial.color = region.customMaterial != null ? region.customMaterial.color : oldMaterial.color;
												fadeMaterial.mainTexture = oldMaterial.mainTexture;
												fadeMaterial.hideFlags = HideFlags.DontSave;
												fadeMaterial.name = oldMaterial.name;
												renderer.sharedMaterial = fadeMaterial;
												SurfaceFader.Animate (style, this, territorySurface, region, fadeMaterial, color, duration);
								}


								#endregion


								#region Cell stuff

								int GetCacheIndexForCellRegion (int cellIndex) {
												return 1000000 + cellIndex; // * 1000 + regionIndex;
								}

								/// <summary>
								/// Highlights the cell region specified. Returns the generated highlight surface gameObject.
								/// Internally used by the Map UI and the Editor component, but you can use it as well to temporarily mark a territory region.
								/// </summary>
								/// <param name="refreshGeometry">Pass true only if you're sure you want to force refresh the geometry of the highlight (for instance, if the frontiers data has changed). If you're unsure, pass false.</param>
								GameObject HighlightCellRegion (int cellIndex, bool refreshGeometry) {
												if (_cellHighlighted != null)
																HideCellRegionHighlight ();
												if (cellIndex < 0 || cellIndex >= cells.Count)
																return null;

												if (_highlightEffect != HIGHLIGHT_EFFECT.None) {
																
																if (OnCellHighlight != null) {
																				bool cancelHighlight = false;
																				OnCellHighlight (cellIndex, ref cancelHighlight);
																				if (cancelHighlight)
																								return null;
																}

																int cacheIndex = GetCacheIndexForCellRegion (cellIndex); 
																GameObject obj;
																bool existsInCache = surfaces.TryGetValue (cacheIndex, out obj);
																if (refreshGeometry && existsInCache) {
																				surfaces.Remove (cacheIndex);
																				DestroyImmediate (obj);
																				existsInCache = false;
																}
																if (existsInCache) {
																				_highlightedObj = surfaces [cacheIndex];
																				if (_highlightedObj != null) {
																								_highlightedObj.SetActive (true);
																								_highlightedObj.GetComponent<Renderer> ().sharedMaterial = hudMatCell;
																				} else {
																								surfaces.Remove (cacheIndex);
																				}
																} else {
																				_highlightedObj = GenerateCellRegionSurface (cellIndex, hudMatCell, Misc.Vector2one, Misc.Vector2zero, 0, false);
																}
																// Reuse cell texture
																Cell cell = cells [cellIndex];
																if (cell.region.customMaterial != null) {
																				hudMatCell.mainTexture = cell.region.customMaterial.mainTexture;
																} else {
																				hudMatCell.mainTexture = null;
																}
																highlightFadeStart = Time.time;
												}

												_cellHighlighted = cells [cellIndex];
												_cellHighlightedIndex = cellIndex;
												return _highlightedObj;
								}

								void HideCellRegionHighlight () {
												if (_cellHighlighted == null)
																return;
												if (_highlightedObj != null) {
																if (cellHighlighted.region.customMaterial != null) {
																				ApplyMaterialToSurface (_highlightedObj, _cellHighlighted.region.customMaterial);
																} else if (_highlightedObj.GetComponent<SurfaceFader> () == null) {
																				_highlightedObj.SetActive (false);
																}
																_highlightedObj = null;
												}
												_cellHighlighted = null;
												_cellHighlightedIndex = -1;
								}

								void SurfaceSegmentForSurface (Segment s, Connector connector) {

												// trace the line until roughness is exceeded
												double dist = s.magnitude; // (float)Math.Sqrt ( (p1.x-p0.x)*(p1.x-p0.x) + (p1.y-p0.y)*(p1.y-p0.y));
												Point direction = s.end - s.start;
			
												int numSteps = (int)(dist / MIN_VERTEX_DISTANCE);
												Point t0 = s.start;
												float h0 = _terrain.SampleHeight (transform.TransformPoint (t0.vector3));
												Point ta = t0;
												float h1;
												for (int i = 1; i < numSteps; i++) {
																Point t1 = s.start + direction * i / numSteps;
																h1 = _terrain.SampleHeight (transform.TransformPoint (t1.vector3));
																if (h0 < h1 || h0 - h1 > effectiveRoughness) { //-effectiveRoughness) {
																				if (t0 != ta) {
																								Segment s0 = new Segment (t0, ta, s.border);
																								connector.Add (s0);
																								Segment s1 = new Segment (ta, t1, s.border);
																								connector.Add (s1);
																				} else {
																								Segment s0 = new Segment (t0, t1, s.border);
																								connector.Add (s0);
																				}
																				t0 = t1;
																				h0 = h1;
																}
																ta = t1;
												}
												// Add last point
												Segment finalSeg = new Segment (t0, s.end, s.border);
												connector.Add (finalSeg);

								}

								//								float GetFirstPointInRow (float y, List<PolygonPoint>points) {
								//												int max = points.Count - 1;
								//												float minx = 1000;
								//												for (int k = 0; k <= max; k++) {
								//																PolygonPoint p0 = points [k];
								//																PolygonPoint p1;
								//																if (k == max) {
								//																				p1 = points [0];
								//																} else {
								//																				p1 = points [k + 1];
								//																}
								//																// if line crosses the horizontal line
								//																if (p0.Y >= y && p1.Y <= y || p0.Y <= y && p1.Y >= y) {
								//																				float x;
								//																				if (p1.Xf == p0.Xf) {
								//																								x = p0.Xf;
								//																				} else {
								//																								float a = (p1.Xf - p0.Xf) / (p1.Yf - p0.Yf);
								//																								x = p0.Xf + a * (y - p0.Yf);
								//																				}
								//																				if (x < minx)
								//																								minx = x;
								//																}
								//												}
								//												return minx - 2;
								//								}
								//
								//								float GetLastPointInRow (float y, List<PolygonPoint>points) {
								//												int max = points.Count - 1;
								//												float maxx = -1000;
								//												for (int k = 0; k <= max; k++) {
								//																PolygonPoint p0 = points [k];
								//																PolygonPoint p1;
								//																if (k == max) {
								//																				p1 = points [0];
								//																} else {
								//																				p1 = points [k + 1];
								//																}
								//																// if line crosses the horizontal line
								//																if (p0.Yf >= y && p1.Yf <= y || p0.Yf <= y && p1.Yf >= y) {
								//																				float x;
								//																				if (p1.X == p0.Xf) {
								//																								x = p0.Xf;
								//																				} else {
								//																								float a = (p1.Xf - p0.Xf) / (p1.Yf - p0.Yf);
								//																								x = p0.Xf + a * (y - p0.Yf);
								//																				}
								//																				if (x > maxx)
								//																								maxx = x;
								//																}
								//												}
								//												return maxx - 2;
								//								}

								void GetFirstAndLastPointInRow (float y, List<PolygonPoint>points, out float first, out float last) {
												int max = points.Count - 1;
												float minx = float.MaxValue;
												float maxx = float.MinValue;
												for (int k = 0; k <= max; k++) {
																PolygonPoint p0 = points [k];
																PolygonPoint p1;
																if (k == max) {
																				p1 = points [0];
																} else {
																				p1 = points [k + 1];
																}
																// if line crosses the horizontal line
																if (p0.Yf >= y && p1.Yf <= y || p0.Yf <= y && p1.Yf >= y) {
																				float x;
																				if (p1.X == p0.Xf) {
																								x = p0.Xf;
																				} else {
																								float a = (p1.Xf - p0.Xf) / (p1.Yf - p0.Yf);
																								x = p0.Xf + a * (y - p0.Yf);
																				}
																				if (x > maxx)
																								maxx = x;
																				if (x < minx)
																								minx = x;
																}
												}
												first = minx - 2f;
												last = maxx - 2f;
								}

								bool IsTooNearPolygon (double x, double y, List<PolygonPoint> points) {
												int pointsCount = points.Count;
												for (int j = 0; j < pointsCount; j++) {
																PolygonPoint p1 = points [j];
																if ((x - p1.X) * (x - p1.X) + (y - p1.Y) * (y - p1.Y) < SQR_MIN_VERTEX_DIST) {
																				return true;
																}
												}
												return false;
								}

								GameObject GenerateCellRegionSurface (int cellIndex, Material material, Vector2 textureScale, Vector2 textureOffset, float textureRotation, bool rotateInLocalSpace) {
												if (cellIndex < 0 || cellIndex >= cells.Count)
																return null;
												Region region = cells [cellIndex].region;
												int cacheIndex = GetCacheIndexForCellRegion (cellIndex); 
												return GenerateRegionSurface (region, cacheIndex, material, textureScale, textureOffset, textureRotation, rotateInLocalSpace);
								}

								Cell GetCellAtPoint (Vector3 position, bool worldSpace) {
												// Compute local point
												if (worldSpace) {
																if (!GetLocalHitFromWorldPosition (ref position))
																				return null;
												}

												int cellsCount = cells.Count;
												for (int p = 0; p < cellsCount; p++) {
																Cell cell = cells [p];
																if (cell == null || cell.region == null || cell.region.points == null || !cell.visible)
																				continue;
																if (cell.region.Contains (position.x, position.y)) {
																				return cell;
																}
												}
												return null;
								}

								void CellAnimate (FADER_STYLE style, int cellIndex, Color initialColor, Color color, float duration) {
												if (cellIndex < 0 || cellIndex >= cells.Count)
																return;
												int cacheIndex = GetCacheIndexForCellRegion (cellIndex);
												GameObject cellSurface = null;
												surfaces.TryGetValue (cacheIndex, out cellSurface);
												if (cellSurface == null) {
																cellSurface = CellToggleRegionSurface (cellIndex, true, color, false);
																cells [cellIndex].region.customMaterial = null;
												} else {
																cellSurface.SetActive (true);
												}
												Renderer renderer = cellSurface.GetComponent<Renderer> ();
												Material oldMaterial = renderer.sharedMaterial;
												Material fadeMaterial = Instantiate (hudMatCell);
												Region region = cells [cellIndex].region;
												fadeMaterial.color = (initialColor.a == 0 && region.customMaterial != null) ? region.customMaterial.color : initialColor;
												fadeMaterial.mainTexture = oldMaterial.mainTexture;
												fadeMaterial.hideFlags = HideFlags.DontSave;
												renderer.sharedMaterial = fadeMaterial;
			
												SurfaceFader.Animate (style, this, cellSurface, region, fadeMaterial, color, duration);
								}

								#endregion

				}
}