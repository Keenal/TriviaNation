﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TGS.Geom {

				public struct Point: IEqualityComparer<Point> {

								public static double PRECISION = 1e-8;
								public static Point zero = new Point (0, 0);

								double _x;

								public double x {
												get { return _x; }
												set {
																_x = value;
																hashCode = 0;
												}
								}

								double _y;

								public double y {
												get { return _y; }
												set {
																_y = value;
																hashCode = 0;
												}
								}

								public int hashCode;

								public Point (double x, double y) {
												this._x = x;
												this._y = y;
												hashCode = 0;
								}

								public Point (Vector3 p) {
												_x = p.x;
												_y = p.y;
												hashCode = 0;
								}


								public Vector3 vector3 {
												get {
																float xf = (float)Math.Round (x, 7);
																float yf = (float)Math.Round (y, 7);
																return new Vector3 (xf, yf);
												}
								}

								public double magnitude {
												get {
																return Math.Sqrt (x * x + y * y);
												}
								}

								public static double Distance (Point p1, Point p2) {
												return Math.Sqrt ((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
								}

								public override string ToString () {
												return "x:" + x.ToString ("F5") + " y:" + y.ToString ("F5");
								}


								//		public bool Equals(Point p1, Point p2) {
								//			return p1.x == p2.x && p1.y == p2.y;
								//		}

								public bool Equals (Point p0, Point p1) {
												return  (p0.x - p1.x) < PRECISION && (p0.x - p1.x) > -PRECISION &&
												(p0.y - p1.y) < PRECISION && (p0.y - p1.y) > -PRECISION;
								}

								public static bool EqualsBoth (Point p0, Point p1) {
												return  (p0.x - p1.x) < PRECISION && (p0.x - p1.x) > -PRECISION &&
												(p0.y - p1.y) < PRECISION && (p0.y - p1.y) > -PRECISION;
								}


								public override bool Equals (object o) {
												if (o is Point) {
																Point p = (Point)o;
																return p.x == x && p.y == y;
												}
												return false;
								}


								void CalculateHashCode () {
												hashCode = (int)(10e7 * Math.Round (x, 7) + 10e9 * Math.Round (y, 7));
								}

								public override int GetHashCode () {
												if (hashCode == 0)
																CalculateHashCode ();
												return hashCode;
								}

								public int GetHashCode (Point p) {
												return (int)(10e7 * Math.Round (p.x, 7) + 10e9 * Math.Round (p.y, 7));
								}

								public static bool operator == (Point p1, Point p2) {
												return p1.x == p2.x && p1.y == p2.y;
								}

								public static bool operator != (Point p1, Point p2) {
												return p1.x != p2.x || p1.y != p2.y;
								}

								public static Point operator - (Point p1, Point p2) {
												return new Point (p1.x - p2.x, p1.y - p2.y);
								}

								public static Point operator - (Point p) {
												return new Point (-p.x, -p.y);
								}

								public static Point operator + (Point p1, Point p2) {
												return new Point (p1.x + p2.x, p1.y + p2.y);
								}

								public static Point operator * (Point p, double scalar) {
												return new Point (p.x * scalar, p.y * scalar);
								}

								public static Point operator / (Point p, double scalar) {
												return new Point (p.x / scalar, p.y / scalar);
								}

								public double sqrMagnitude {
												get {
																return x * x + y * y;
												}
								}

								public static Point operator - (Vector2 p1, Point p2) {
												return new Point (p1.x - p2.x, p1.y - p2.y);
								}


								public static Point Lerp (Point start, Point end, double t) {
												return start + (end - start) * t;

								}

								public void Normalize () {
												double d = Math.Sqrt (x * x + y * y);
												x /= d;
												y /= d;
								}

								public Point normalized {
												get {
																double d = Math.Sqrt (x * x + y * y);
																return new Point (x / d, y / d);
												}
								}

								public Point Offset (double deltax, double deltay) {
												return new Point (x + deltax, y + deltay);
								}

								public void CropBottom () {
												if (y < -0.5)
																y = -0.5;
								}

								public void CropRight () {
												if (x > 0.5)
																x = 0.5;
								}
				}

}
