﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Pram.Entities {
    public class BoxSite : Site {
        public float x_width;
        public float z_width;

        public override Vector3 GetPosition() {
            Vector3 pos = transform.position + new Vector3(Random.Range(-x_width, x_width), 0f, Random.Range(-z_width, z_width));

            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(pos, out myNavHit, 100, -1)) {
                return myNavHit.position;
            }

            return pos;
        }
    }
}
