using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public Transform target;
    public float range;
    public float delay;
    public float damage;

    private Unit hpunit;
   public DamageType Type;

    public string enemyTag = "Enemy";
    void Start()
    {
        InvokeRepeating("UpdateSameTarget", 0f, 0.5f);
        StartCoroutine("MyUpdate");
    }
	// Update is called once per frame
	
    void UpdateTarget()
    {
        for(int i = 0;i<Spawner.enemyList.Count;++i)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, Spawner.enemyList[i].transform.position);
            if(distanceToEnemy<range)
            {
                target = Spawner.enemyList[i].transform;
                hpunit = target.GetComponent<Unit>();
                return;
            }
        }

    }
    void UpdateSameTarget()
    {
        if (target == null)
            UpdateTarget();
        
        else if (Vector3.Distance(transform.position, target.position) > range)
        {
            target = null;
            UpdateTarget();
        }
       
    }
   
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(target.position, Vector3.one);
        }
    }
    IEnumerator MyUpdate()
    {
        while (true)
        {
            if (target == null)
                yield return new WaitForSeconds(delay);
            else
            {
                hpunit.DealDamage(damage,Type);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
