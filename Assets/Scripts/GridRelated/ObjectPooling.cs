using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {
    public static ObjectPooling current;
    List<GameObject> objList;
    [SerializeField]
    GameObject todelete;

    public int Max;

    void Awake()
    {
        current = this;
    }


	// Use this for initialization
	void Start () {
        objList = new List<GameObject>();
        for(int i = 0; i < Max; ++i)
        {
            GameObject obj = Instantiate(todelete, transform.position, Quaternion.identity);
            obj.SetActive(false);
            objList.Add(obj);
        }
		
	}
	public GameObject GetObject()
    {
        for(int i = 0;i< Max; i++)
        {
            if (!objList[i].activeInHierarchy)
            {
                return objList[i];
            }
        }
        return null;
    }
}
