using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    feu,
    froid,
    poison,
    normal,
};
public class Unit : MonoBehaviour {
    public float speed = 2;
    public float hp = 100;

    public float fireRes;
    public float frostRes;
    public float poisonRes;
    public float normalRes;

  
    Vector3[] path;
    int targetIndex;
   
    void Awake()
    {
        path = Spawner.path;
        StartCoroutine("FollowPath");
    }
    
    /*
    void Update()
    {
        Node k = grid.NodeFromWorldPoint(transform.position);
        print(k.gridX+ " , "+k.gridY);
    }
    */
    public void DealDamage(float damageTaken,DamageType type)
    {
        if (type == DamageType.feu)
        {
            print("normal" + fireRes);
            print("damage: " + damageTaken + " " + (GetArmor(type)));
        }
        hp -= (damageTaken *GetArmor(type));
        if (hp <= 0)
        {
            Death();
        }
    }

    float GetArmor(DamageType type)
    {
        switch (type){
            case DamageType.feu:
                return (100-fireRes)/100;
            case DamageType.froid:
                return (100-frostRes) /100;
            case DamageType.poison:
                return (100-poisonRes) /100;
            case DamageType.normal:
                return (100-normalRes) /100;
            default:
                return (100-normalRes) /100;
        }
    }
    void Death()
    {
        Spawner.enemyList.Remove(gameObject);
        Destroy(gameObject);
    }
    IEnumerator FollowPath()
    {
        Vector3 currentPoint = path[0];
        while (true)
        {
            if(transform.position == currentPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentPoint = path[targetIndex];
            }
            transform.position =  Vector3.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);
            yield return null;
        }
    }
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(path[i], Vector3.one*0.1f);
                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
