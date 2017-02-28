using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    Node[,] grid;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unWalkableMask;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Vector3 tL;

    [Header("Towers")]
    public GameObject fireTower;
    void Awake()
    {
        float t1 = Time.time;
        nodeDiameter = nodeRadius* 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y/2;
        for(int x = 0; x<gridSizeX; ++x)
        {
            for(int y = 0; y < gridSizeY; ++y)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unWalkableMask));
                grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }
    public Node NodeFromWorldPoint(Vector3 wp)
    {
  
        float percentX = (wp.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (wp.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        return grid[x, y];
    }
    public void TryCreateTurret(Vector3 v, int diameterX, int diameterY)
    {
    
        float wpX = diameterX * nodeRadius;
        float wpY = diameterY * nodeRadius;
        if (v.x - wpX < -gridWorldSize.x / 2 || v.x + wpX / 2 > gridWorldSize.x / 2 || v.z - wpY < -gridWorldSize.y / 2 || v.z + wpY > gridWorldSize.y / 2)
        {
            print("badplacement");
            return;
        }
        
        Vector3 topLeft = new Vector3(v.x - (wpX - nodeRadius),0, v.z+(wpY - nodeRadius));
        tL = topLeft;
        print(v.x - topLeft.x);
        Vector3 botRight = new Vector3(v.x+(wpX- nodeRadius),0, v.z - (wpY  - nodeRadius));
        Node n = NodeFromWorldPoint(topLeft);
        bool freespace = true;
        print(n.gridX + "," + n.gridY);
        for (int i = 0; i < diameterY ; i++)
        {
            for (int j = 0; j < diameterX; j++)
            {
                if (!grid[n.gridX + j, n.gridY - i].walkable)
                {
                    freespace = false;
                    break;
                }
            }
        }
        print(freespace);
        if (freespace)
        {
            for (int i = 0; i < diameterY ; i++)
            {
                for (int j = 0; j < diameterX; j++)
                {
                    grid[n.gridX + j, n.gridY - i].walkable = false;
                }
            }
        }
        /*
        Node k = NodeFromWorldPoint(new Vector3(v.x - diameter/2, v.y, v.z -diameter/2));
        print(k.gridX + "," + k.gridY);
        print("def" + gridSizeX);
        if (k.gridX+1 >= gridSizeX || k.gridY+1 >= gridSizeY)
        {
            
            print("bad placement");
            return;
        }
        if(k.walkable && grid[k.gridX + 1, k.gridY].walkable && grid[k.gridX, k.gridY + 1].walkable && grid[k.gridX + 1, k.gridY + 1].walkable)
        {
            Instantiate(fireTower, v, Quaternion.identity);
            k.walkable = grid[k.gridX + 1, k.gridY].walkable = grid[k.gridX, k.gridY + 1].walkable = grid[k.gridX + 1, k.gridY + 1].walkable = false;
        }*/

    }
    public List<Node> GetNeighboors(Node node)
    {
        //print(node.gridX+","+node.gridY);
        List<Node> neighboors = new List<Node>();
        //bool notwalkable = false;
        /*
        for (int x =-1;x<= 1; ++x)
        {
            for(int y = -1; y <= 1; ++y)
            {
                if (x+y ==0 || x+y == 2 || x+y == -2)
                    continue;
                
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighboors.Add(grid[checkX, checkY]);
                }
            }
        }
        */

        bool godiag = true;

        for(int x= -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; ++y)
            {
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighboors.Add(grid[checkX, checkY]);
                    if (!grid[checkX, checkY].walkable)
                    {
                        godiag = false;
                    }
                }
            }
        }
        if (godiag)
        {
            return neighboors;
            
        }
       else
        {
            neighboors.Clear();
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (x + y == 0 || x + y == 2 || x + y == -2)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighboors.Add(grid[checkX, checkY]);
                    }
                }
            }
            return neighboors;
        }
        /*
        if (node.gridX == 1 && node.gridY == 27)
        {
            print("NotWalkable? " + notwalkable + "9? "+ neighboors.Count );
        }
        if (notwalkable)
        {
            List<Node> newlist = new List<Node>();
            if(neighboors.Count == 8)
            {
                for(int i = 1; i < 8; i+=2)
                {
                    newlist.Add(neighboors[i]);
                }
            }
            else
            {
                for (int x = -1; x <= 1; x+=1)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        if (x+y == 0 || x+y == 2)
                            continue;

                        int checkX = node.gridX + x;
                        int checkY = node.gridY + y;
                        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                        {
                            newlist.Add(grid[checkX, checkY]);
                        }
                    }
                }
            }
            return newlist;
        }
       */
        return neighboors;
    }
   

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(tL!= null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(tL, Vector3.one * 0.01f);
        }
        
        if(grid != null)
        {
            foreach(Node k in grid)
            {
                Gizmos.color = (k.walkable) ? Color.white : Color.red;
                
                Gizmos.DrawWireCube(k.worldPosition, Vector3.one*(nodeDiameter-.1f));
            }
        }
    }
}
