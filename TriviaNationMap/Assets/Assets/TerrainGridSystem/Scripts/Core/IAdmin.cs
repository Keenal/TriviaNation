﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TGS.Geom;

namespace TGS {
				public interface IAdmin {

								string name { get; set; }

								Region region { get; set; }

								Polygon polygon { get; set; }

								bool visible { get; set; }

				}
}
