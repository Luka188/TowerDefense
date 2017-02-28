using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node Parent;
    int heapIndex;
    public Node(bool w, Vector3 wp,int gX,int gY)
    {
        walkable = w;
        worldPosition = wp;
        gridX = gX;
        gridY = gY;
    }
    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }
    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
