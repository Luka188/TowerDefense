using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding : MonoBehaviour {

    PathRequestManager requestManager;
    Grid grid;

    void Awake()
    {

        grid = GetComponent<Grid>();
        requestManager = GetComponent<PathRequestManager>();
    }


	IEnumerator FindPath(Vector3 startP, Vector3 endP)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node StartN = grid.NodeFromWorldPoint(startP);
        Node EndN = grid.NodeFromWorldPoint(endP);
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        List<Node> closeSet = new List<Node>();
        openSet.Add(StartN);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirstItem();
           
            closeSet.Add(currentNode);
            if (currentNode == EndN)
            {
                sw.Stop();
                print(sw.ElapsedMilliseconds + "ms");
                RetracePath(StartN, EndN);
                pathSuccess = true;
                break;
            }
            foreach(Node neighboor in grid.GetNeighboors(currentNode))
            {
                if(!neighboor.walkable || closeSet.Contains(neighboor))
                {
                    continue;
                }
                int newMovementCostToNeightbour = currentNode.gCost + GetDistance(currentNode, neighboor);
                if (newMovementCostToNeightbour < neighboor.gCost || !openSet.Contains(neighboor))
                {
                    neighboor.gCost = newMovementCostToNeightbour;
                    neighboor.hCost = GetDistance(neighboor, EndN);
                    neighboor.Parent = currentNode;
                    if (!openSet.Contains(neighboor))
                    {
                        openSet.Add(neighboor);
                    }
                    else
                    {
                        openSet.UpdateItem(neighboor);
                    }
                }
                
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(StartN, EndN);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }
    Vector3[] RetracePath(Node Start,Node End)
    {
        List<Node> path = new List<Node>();
        Node currentNode = End;
        while(currentNode != Start){
            
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Add (Start);
        Vector3[] waypoints = SimplifyPath(path);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionold = Vector2.zero;
        int oldind = -1;
        for(int i = 1; i < path.Count; ++i)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionold )
            {
                if (oldind != i - 1)
                    waypoints.Add(path[i - 1].worldPosition);
                waypoints.Add(path[i].worldPosition);
                oldind = i;
            }

            directionold = directionNew;
        }
        waypoints.Reverse();
        return waypoints.ToArray();
    }
  
    int GetDistance(Node A, Node B)
    {
        int distX = Mathf.Abs(A.gridX - B.gridX);
        int distY = Mathf.Abs(A.gridY - B.gridY);
        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10*(distY - distX);
    }
    public void StartFindPath(Vector3 startpos, Vector3 endpos)
    {
        StartCoroutine(FindPath(startpos, endpos));
    }
}
