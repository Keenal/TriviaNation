using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TGS {
				public static class Drawing {

								static Rect dummyRect = new Rect ();

								public static GameObject CreateSurface (string name, Vector3[] surfPoints, int[] indices, Material material) {
												return CreateSurface (name, surfPoints, indices, material, dummyRect, Misc.Vector2one, Misc.Vector2zero, 0, false);
								}

								/// <summary>
								/// Rotates one point around another
								/// </summary>
								/// <param name="pointToRotate">The point to rotate.</param>
								/// <param name="centerPoint">The centre point of rotation.</param>
								/// <param name="angleInDegrees">The rotation angle in degrees.</param>
								/// <returns>Rotated point</returns>
								static Vector2 RotatePoint (Vector2 pointToRotate, Vector2 centerPoint, float angleInDegrees) {
												float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
												float cosTheta = Mathf.Cos (angleInRadians);
												float sinTheta = Mathf.Sin (angleInRadians);
												return new Vector2 (cosTheta * (pointToRotate.x - centerPoint.x) - sinTheta * (pointToRotate.y - centerPoint.y) + centerPoint.x,
																sinTheta * (pointToRotate.x - centerPoint.x) + cosTheta * (pointToRotate.y - centerPoint.y) + centerPoint.y);
								}

								public static GameObject CreateSurface (string name, Vector3[] surfPoints, int[] indices, Material material, Rect rect, Vector2 textureScale, Vector2 textureOffset, float textureRotation, bool rotateInLocalSpace) {
			
												GameObject hexa = new GameObject (name, typeof(MeshRenderer), typeof(MeshFilter));
												hexa.hideFlags = HideFlags.DontSave; // | HideFlags.HideInHierarchy; // TODO: RMC
			
												Mesh mesh = new Mesh ();
												mesh.hideFlags = HideFlags.DontSave;
												mesh.vertices = surfPoints;
												mesh.triangles = indices;
												// uv mapping
												if (material.HasProperty("_MainTex") && material.mainTexture != null) {
																Vector2[] uv = new Vector2[surfPoints.Length];
																for (int k = 0; k < uv.Length; k++) {
																				Vector2 coor = surfPoints [k];
																				Vector2 normCoor;
																				if (rotateInLocalSpace) {
																								normCoor = new Vector2 ((coor.x - rect.xMin) / rect.width, (coor.y - rect.yMin) / rect.height);
																								if (textureRotation != 0) {
																												normCoor = RotatePoint (normCoor, Misc.Vector2half, textureRotation);
																								}
																								normCoor.x = 0.5f + (normCoor.x - 0.5f) / textureScale.x;
																								normCoor.y = 0.5f + (normCoor.y - 0.5f) / textureScale.y;
																								normCoor -= textureOffset;
																				} else {
																								coor.x /= textureScale.x;
																								coor.y /= textureScale.y;
																								if (textureRotation != 0) {
																												coor = RotatePoint (coor, Vector2.zero, textureRotation);
																								}
																								coor -= textureOffset;
																								normCoor = new Vector2 ((coor.x - rect.xMin) / rect.width, (coor.y - rect.yMin) / rect.height);
																				}
																				uv [k] = normCoor;
																}
																mesh.uv = uv;
												}
												mesh.RecalculateNormals ();
												mesh.RecalculateBounds ();
#if !UNITY_5_5_OR_NEWER
			mesh.Optimize ();
#endif
			
												MeshFilter meshFilter = hexa.GetComponent<MeshFilter> ();
												meshFilter.mesh = mesh;
			
												hexa.GetComponent<Renderer> ().sharedMaterial = material;
												return hexa;
			
								}
				}


}



