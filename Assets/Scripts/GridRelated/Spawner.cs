using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField]
    GameObject[] Monsters;
    [SerializeField]
    Transform target;
    Queue<Order> QueueOrder = new Queue<Order>();
    public static Vector3[] path;
    public static List<GameObject> enemyList= new List<GameObject>();

    public static bool isWaveFinished
    {
        get
        {
            return enemyList.Count == 0;
        }
    }

    struct Order
    {
        public int index;
        public int num;
        public float delay;
        public Order( int i, int n, float d)
        {
            index = i;
            num = n;
            delay = d;
        }
    }

	// Use this for initialization
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Spawn(0, 10, 1);
        }
    }
	
	public void Spawn(int index,int num,float delay)
    {
        Order k = new Order(index, num, delay);
        QueueOrder.Enqueue(k);
        StartLevel();
    }
    public void StartLevel()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        
    }
    public void OnPathFound(Vector3[] newpath, bool success)
    {
        if (success)
        {
            path = newpath;
            StartCoroutine("Spawning");
        }
    }
    IEnumerator Spawning()
    {
       
        while (QueueOrder.Count > 0)
        {
            Order k = QueueOrder.Dequeue();
            for(int i = 0; i < k.num; ++i)
            {
                enemyList.Add(Instantiate(Monsters[k.index],transform.position,Quaternion.identity));
                yield return new WaitForSeconds(k.delay);
            }
        }
    }
}
