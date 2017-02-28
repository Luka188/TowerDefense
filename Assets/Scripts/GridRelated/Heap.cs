using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T: IHeapItem<T> {
    T[] items;
    int currentItemCout;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }
    public void Add(T item)
    {
        item.HeapIndex = currentItemCout;
        items[currentItemCout] = item;
        SortUp(item);
        currentItemCout++;
    }
    public T RemoveFirstItem()
    {
        T firstItem = items[0];
        currentItemCout--;
        items[0] = items[currentItemCout];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCout;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T Item)
    {
        while (true)
        {
            int childIndexLeft = Item.HeapIndex * 2 + 1;
            int childIndexRight = Item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            if (childIndexLeft< currentItemCout)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCout)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (Item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(Item, items[swapIndex]);
                    
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            
        }
    }
	void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T ItemB)
    {
        items[itemA.HeapIndex] = ItemB;
        items[ItemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = ItemB.HeapIndex;
        ItemB.HeapIndex = itemAIndex;
    }
}
public interface IHeapItem<T>: IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
