using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    [HideInInspector]
    public Transform target;

    public float range;
    public float delay;
    public float damage;

    private Unit hpunit;
    public DamageType Type;

    public string enemyTag = "Enemy";
    public float projectileSpeed;


    Transform projectileG;
    bool ReadyToFire;

    ParticleSystem.Particle[] m_Particles;
    void Start()
    {
        ReadyToFire = true;
        //InvokeRepeating("UpdateSameTarget", 0f, 0.5f);
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
    /*
    private void LateUpdate()
    {
        if (target != null)
        {
            InitializeIfNeeded();

            // GetParticles is allocation free because we reuse the m_Particles buffer between updates
            int numParticlesAlive = m_System.GetParticles(m_Particles);
            print(numParticlesAlive);
           
            // Change only the particles that are alive
            for (int i = 0; i < numParticlesAlive; i++)
            {
                m_Particles[i].position = Vector3.MoveTowards(m_Particles[i].position, ProjectFocus.position, projectileSpeed * Time.deltaTime);
                //m_Particles[i].position = new Vector3(0, 2, 0);
                if (m_Particles[i].position == ProjectFocus.position)
                {
                    m_Particles[i].remainingLifetime = -1;
                    ReadyToFire = true;
                }

            }

            // Apply the particle changes to the particle system
            m_System.SetParticles(m_Particles, numParticlesAlive);
        }
    }
    



    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }*/

    IEnumerator MyUpdate()
    {
        while (true)
        {
            if (target == null)
            {
                UpdateTarget();
                if (target != null)
                    yield return null;
                yield return new WaitForSeconds(delay);
            }
            else
            {
                if (ReadyToFire)
                {
                    ReadyToFire = false;
                    //m_System.Emit(1);
                    UpdateSameTarget();
                    projectileG = ObjectPooling.current.GetObject().transform;
                    if (projectileG != null)
                    {
                        projectileG.position = transform.position;
                        projectileG.gameObject.SetActive(true);
                    }
                    yield return new WaitForSeconds(delay);
                }
                else {
                    projectileG.position = Vector3.MoveTowards(projectileG.position, target.position, projectileSpeed * Time.deltaTime);
                    if (projectileG.position == target.position)
                    {
                        ReadyToFire = true;
                        projectileG.gameObject.SetActive(false);
                    }
                    yield return null;
                }
                //StartCoroutine(GoingProjectile(partab.Length-1));

                //yield return new WaitForSeconds(delay);

            }
        }
    }

   

   /*
    IEnumerator GoingProjectile(int i)
    {
        print("incoroutine");
        Vector3 dest = target.position;
        while (true)
        {
            partab[i].position = Vector3.MoveTowards(partab[i].position, dest, projectileSpeed*Time.deltaTime);
            partab[i].velocity = Vector3.one;
            Shooting.SetParticles(partab, partab.Length);
            //print(projectile.position);
            if (partab[i].position == dest)
            {
                hpunit.DealDamage(damage, Type);
                partab[i].remainingLifetime = 0;
                break;
            }
            yield return null;
        }
        print("endcor");
   }*/
}
