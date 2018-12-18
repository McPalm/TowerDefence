using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MeshManager : MonoBehaviour
    {
        public WaypointMesh[] waypointMeshes;

        int count;

        public WaypointMesh GetMesh()
        {
            count++;
            count %= waypointMeshes.Length;
            return waypointMeshes[count];
        }
    }
}