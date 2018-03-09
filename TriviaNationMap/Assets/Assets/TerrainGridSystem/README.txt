************************************
*        TERRAIN GRID SYSTEM       *
* (C) Copyright 2015-2017 Kronnect * 
*            README FILE           *
************************************


How to use this asset
---------------------
Firstly, you should run the Demo Scene provided to get an idea of the overall functionality.
Later, you should read the documentation and experiment with the tool.

Hint: to use the asset, drag the TerrainGridSystem prefab from Resources/Prefabs folder to your scene and assign your terrain to it.


Demo Scene
----------
There's one demo scene, located in "Demos" folder. Just go there from Unity, open "Demo1" scene and run it.


Documentation/API reference
---------------------------
The PDF is located in the Doc folder. It contains instructions on how to use the prefab and the API so you can control it from your code.


Support
-------
Please read the documentation PDF and browse/play with the demo scene and sample source code included before contacting us for support :-)

* Support: contact@kronnect.me
* Website-Forum: http://kronnect.me
* Twitter: @KronnectGames


Future updates
--------------

All our assets follow an incremental development process by which a few beta releases are published on our support forum (kronnect.com).
We encourage you to signup and engage our forum. The forum is the primary support and feature discussions medium.

Of course, all updates of Terrain Grid System will be eventually available on the Asset Store.


Version history
---------------

Version 6.2 Current Release
- Added highlight effect none (allows selection of cells and territories but does not show highlight effect)
- Added neutral territories (API: TerritorySetNeutral / TerritoryIsNeutral). Neutral territories do not dispute frontiers.
- [Fix] Fixed rare issue with territories surfaces when changing cell ownership
- [Fix] Removed harmless error message related to material lacking a mainTexture property

Version 6.1 2018-JAN-9
- Added new highlight effect options (texture additive, texture multiply, texture color, texture scale)

Version 6.0 2018-DEC-28
- Editor: added toggle to enable/disable grid editing options in Scene View
- API: Ability to assign cells to groups with CellSetGroup/CellGetGroup
- API: FindPath method now accepts cell group mask as argument to consider only certain cell groups
- API: Added CellGetLineOfSight() for Line of Sight computation. Example in demo scene 12.
- API: CellGetNeighbours now accepts a range/distance. Range example in demo scene 12.
- API: CellColorTemp, temporarily colors a cell or list of cells
- API: Added CellGetHexagonDistance
- Highlight Fade now accepts a range for increased effect flexibility
- Added Hightlight Speed option
- Max number of territories increased to 512
- Performance improvements

Version 5.2 2017-NOV-21
- New cell border thickness option (uses geometry shader, SM 4.0 required)
- Minor usability and performance improvements
- [Fix] Fixed issue with mesh vertex limit on very large colored surfaces
- [Fix] Fixed issue when clicking on cells in SceneView deselecting TGS

Version 5.1 2017-NOV-13
- New demo scene 17: using a texture to color cells
- Improved event system with new events: OnCellMouseDown, OnCellMouseUp, OnCellClick, OnTerritoryMouseDown, OnTerritoryMouseUp, OnTerritoryClick
- [Fix] Fixed "Respect Other UI" regression issues on mobile

Version 5.0.1 2017-NOV-6
- Added "Near Clip Fade" toggle to inspector
- API: Added TerritoryGetPosition, TerritoryGetRectWorldSpace, TerritoryGetVertexCount, TerritoryGetVertexPosition
- [Fix] Fixed highlighting issue on WebGL platform
- [Fix] Fixed highlighting issue when using CellFadeOut

Version 5.0 2017-OCT-4
- TGS now requires Unity 5.5 or later
- New demo scene 16: simple matching game
- API: new variants for CellBlink and CellFlash
- API: added CellGetRect to obtain the rectangle enclosing any given cell in local or worldspace

Version 4.9.2 2017-SEP-1
- New demo scene 15: create grid dynamically at runtime
- Cell tag field can now be modified in the Grid Editor
- [Fix] Fixed LoadConfiguration issue with cell colors

Version 4.9.1 2017-JUL-16
- [Fix] Fixed TerritogyGetNeighbours returning 0 elements

Version 4.9 2017-JUN-27
- Added EvenLayout option to hexagonal grid
- Added CellFlash, CellBlink, TerritoryFlash and TerritoryBlink new effects

Version 4.8 2017-MAY-31
- Added Max Movement Cost to path finding functions
- Minor optimizations (less GC when creating cells)
- [Fix] Fixed grid scale regression bug introduced in 4.7
- [Fix] Fixed territory disputed frontiers color issue

Version 4.7 2017-MAY-23
- Added new parameter to CellToggleRegionSurface and TerritoryToggleRegionSurface to specify if colored surface is shown on top of objects or on the ground
- [Fix] Fixed path finding missing some cells under heavy usage
- [Fix] Fixed OnCellEnter/OnCellExit firing null errors

Version 4.6.1 2017-MAR-14
- [Fix] Fixed cell visible property being ignored during redraw call
- [Fix] Fixed RespectOtherUI on mobile

Version 4.6 2017-MAR-07
- New demo scene 14 "PathFinding over terrain"
- Updated demo scene 10b with option to show neighbour cells
- Added Max Slope parameter to hide cells hanging over terrain edges
- Added Minimum Altitude parameter to hide cells under certain altitude
- Added gridMeshDepthOffset and gridSurfaceDepthOffset for improved control of zfighting issues
- Added gridCenterWorldPosition to simplify grid centering (also simplified gridCenter concept to be in the range of -0.5 to 0.5 irrespective of grid scale)
- [Fix] Fixed some issues when coloring certain hexagonal cells
- [Fix] Fixed/handled compatibility warnings with Unity 5.6

Version 4.5 2017-JAN-13
- New demo scene 10b: Grid around character
- Upped max cell count
- [Fix] Fixed internal territories issues with overlapping edges

Version 4.4 16-DEC-2016
- Added RespectOtherUI to prevent pointer interactions when it's over an UI element in front of the grid
- [Fix] Fixed depth offset parameter not being applied correctly
- [Fix] Fixed texture list empty in Editor
- [Fix] Fixed Unity 5.5 compatibility issues

Version 4.3 16-NOV-2016
- New events: OnTerritoryHighlight/OnCellHighlight with option to cancel highlight
- New territoryDisputedFrontiersColor property.
- Ability to set individual territory frontier color using TerritorySetFrontierColor
- Editor changes are now registered so Unity asks for changes to be saved

Version 4.2 26-SEP-2016
- Ability to define territories by using a color texture
- Support for enclaves (territories surrounding other territories)
- Public API reorganization

Version 4.1 - 20-SEP-2016
- Ability to add color textures to define territories
- New demo scene #13

Version 4.0 - 10-SEP-2016
  - A* Path Finding system. New demo scene 12.
  - Faster cells and territory line shaders
  - Added new properties to the inspector to control near clip fade amount and falloff
  - PathFinding now works with POT and non POT grid sizes
  - [Fix] Fixed near clip fade effect with orthographic camera

Version 3.2 - 30-AUG-2016
  - Ability to add two or more grids to same terrain
  - New Elevation Base property to allow set higher heights to grid over terrain
  - Updated demo scene 6b showing how to get the row/column of the cell beneath a gameobject and fade neigbhours/range of cells
  - [Fix] Minor fix to CellGetAtPoint method which returned null when positions passed crossed the terrain

Version 3.1 - 24-JUL-2016
  - New option in inspector to specify the minimum distance to camera for a cell to be selectable
  - [Fix] Changed hexagonal topology so all rows contains same number of cells

Version 3.0 - 02-JUL-2016
  - New grid editor section with option to load configurations

Version 2.2 - 23-JUN-2016
  - New demo scene #11 showing how to transfer cells to another territory
  - Ability to hide territories outer borders
  - New API: CellSetTerritory.
  - Redraw() method now accepts a reuseTerrainData parameter to speed up updates if terrain has not changed
  - [Fix] Fixed lower boundary of territory in hexagonal grid

Version 2.1 - 03-JUN-2016
  - New demo scene #10 showing how to position the grid inside the terrain using gridCenter and gridScale properties.
  - Cells will be visible if at least one vertex if visible when applying mask.
  - Canvas texture now works with territories also

Version 2.0 - 24-MAY-2016
  - Added mask property to define cells visibility.
  - CellGetAtPosition now can accept world space coordinates.
  - Option to prevent highlighting of invisible cells
  - [Fix] Fixed bug in territory frontiers line shader

Version 1.4.0 - 12-MAY-2016
  - Added grid center and scale properties.
  - [Fix] CellGetPosition and CellGetVertexPosition now takes into account terrain height

Version 1.3.2 - 15-APR-2016
  - [Fix] Fixed compatibility with Orthographic Camera
  - [Fix] Fixed CellMerge out of bounds error

Version 1.3.1 - 04-APR-2016
  - Grid configuration now supports specifying exact column and row number for box and hexagonal topologies
  - Added new Demo7 scene.
  - Added cellRowCount, cellColumnCount
  - Added CelGetAtPosition(column, row)
  - Added CellGetVertexCount, CellGetVertexPosition
  - [Fix] Fixed CellGetCenterPosition in stand-alone mode

Version 1.2 - 04-JAN-2016
  - Added new Demo5 scene with cell fading example.
  - Can fade out cells and territories with a single function call.
  - Added new Demo6 scene with cell position and vertices locating example.
  - Some internal performance optimizations

Version 1.1 - 26-NOV-2015
  - Added new Demo3 and Demo4 scenes.
  - Can assign a canvas texture for all cells
  - Added new events: OnTerrainEnter, OnTerrainExit, OnTerrainClick, OnCellEnter, OnCellExit, OnCellClick.
  - Added cell visibility field.
  - Added CellSetTag and CellGetWithTag methods.
  - Cells can be customized with individual or canvas textures.

Version 1.0 - Initial launch 7/10/2015







