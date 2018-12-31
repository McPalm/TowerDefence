using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Movement
{
    public class WaypointMesh : MonoBehaviour
    {
        public float TrackSpeed = 1f;

        public List<GameObject> waypoints;
        public float length;
        public List<GameObject> trackNodes;

        List<Path> paths;

        private void Start()
        {
            paths = new List<Path>();
            float distance = 0;
            for (int i = 1; i < waypoints.Count; i++)
            {
                var path = new Path(distance,
                    waypoints[i - 1].transform.position,
                    waypoints[i].transform.position
                    );
                distance += path.length;
                paths.Add(path);
            }
            length = paths[paths.Count - 1].endDistance;
            for (int i = 0; i <= distance; i++)
            {
                var node = new GameObject("Node " + i);
                node.AddComponent<Building.Obstruction>();
                node.transform.position = DistanceToPosition(i);
                node.transform.SetParent(transform);
                trackNodes.Add(node);
            }
        }


        public Vector3 DistanceToPosition(float distance)
        {
            var path = DistanceToPath(distance);
            if (path == null)
                return waypoints[waypoints.Count - 1].transform.position;

            return path.DistanceToPosition(distance);
        }

        Path DistanceToPath(float distance)
        {
            foreach (var item in paths)
            {
                if (distance < item.endDistance)
                    return item;
            }

            return null;
        }

        private void OnDrawGizmos()
        {
            // draw line between each waypoint
            for (int i = 0; i < waypoints.Count-1; i++)
            {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
            }   
        }

        private class Path
        {
            public Vector3 start;
            public Vector3 end;
            public float startDistance;
            public float length;
            public float endDistance;

            public Path(float startDistance, Vector3 current, Vector3 next)
            {
                this.startDistance = startDistance;
                start = current;
                end = next;
                length = (current - next).magnitude;
                endDistance = startDistance + length;
            }

            public Vector3 DistanceToPosition(float distance)
            {
                distance -= startDistance;
                distance /= length;

                return Vector3.Lerp(start, end, distance);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(WaypointMesh))]
    public class WaypontMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            if(GUILayout.Button("Add Waypoint"))
            {
                WaypointMesh mesh = (WaypointMesh)target;
                GameObject o = new GameObject();
                mesh.waypoints.Add(o);
                o.transform.SetParent(mesh.transform);
                o.AddComponent<Snap>();

                if(mesh.waypoints.Count == 1)
                {
                    o.name = "Start";
                    o.transform.position = Vector3.zero;
                }
                else
                {
                    o.name = "Goal";
                    if (mesh.waypoints.Count > 2)
                        mesh.waypoints[mesh.waypoints.Count - 2].name = "Waypoint " + (mesh.waypoints.Count - 2);
                    o.transform.position = mesh.waypoints[mesh.waypoints.Count - 2].transform.position + Vector3.up;
                }
            }

            DrawDefaultInspector();
        }

        private void OnSceneGUI()
        {
            foreach (var item in ((WaypointMesh)target).waypoints)
            {
                item.transform.position = Handles.PositionHandle(item.transform.position, Quaternion.identity);
                Handles.Label(item.transform.position, item.name);
            }
        }
    }
#endif
}
