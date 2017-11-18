using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WayPointPath : MonoBehaviour
{
    [HideInInspector]
    public List<FollowPath> paths = null;

    [SerializeField]
    private int selectedPath = -1;

    public void AddWayPoint()
    {
        if (paths == null)
        {
            paths = new List<FollowPath>();
            NewPath();

        }
        else if (paths.Count == 0)
        {
            paths = new List<FollowPath>();
            NewPath();
        }

        Transform point = new GameObject("Waypoint: " + paths[selectedPath].waypoints.Count).transform;

        paths[selectedPath].Add(point.transform);
    }

    public void NewPath()
    {
        FollowPath newpath = new FollowPath();
        paths.Add(newpath);
        selectedPath++;
    }

    public void DeletePath()
    {
        paths.RemoveAt(selectedPath);
        selectedPath--;
    }

    void OnDrawGizmos()
    {
        if (paths != null)
        {
            for (int x = 0; x < paths.Count; x++)
            {
                if (paths[x].waypoints.Count > 1)
                {
                    Gizmos.color = Color.blue;

                    for (int i = 0; i < paths[x].waypoints.Count - 1; i++)
                    {
                        Handles.Label(paths[x].waypoints[i].position, "WayPoint: " + i);
                        Gizmos.DrawLine(paths[x].waypoints[i].position, paths[x].waypoints[i + 1].position);
                        Gizmos.DrawSphere(paths[x].waypoints[i].position, 3);
                    }

                    Handles.Label(paths[x].waypoints[paths[x].waypoints.Count - 1].position, "WayPoint: " + (paths[x].waypoints.Count - 1));
                    Gizmos.DrawSphere(paths[x].waypoints[paths[x].waypoints.Count - 1].position, 3);
                    Gizmos.DrawLine(paths[x].waypoints[paths[x].waypoints.Count - 1].position, paths[x].waypoints[0].position);
                }
            }
        }
    }
}

public class FollowPath
{
    public Transform holder;
    public List<Transform> waypoints = null;

    public FollowPath()
    {
        waypoints = new List<Transform>();
        holder = new GameObject("Path" + GameObject.FindGameObjectsWithTag("Path").Length).transform;
        holder.tag = "Path";
    }

    public void Add(Transform _point)
    {
        waypoints.Add(_point);
        _point.parent = holder;
    }
}