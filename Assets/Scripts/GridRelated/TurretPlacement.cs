using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacement : MonoBehaviour {
    [SerializeField]
    Transform cube;
    [SerializeField]
    GameObject FireTurret;

    Vector3 currentvect;
    public int dimX;
    public int dimY;
    Grid grille;
	void Start()
    {
        grille = GetComponent<Grid>();
        cube.localScale = new Vector3(dimX * 0.5f, cube.localScale.y, dimY * 0.5f);
    }
    void Update()
    {
        if (Spawner.isWaveFinished)
        {
            Plane plane = new Plane(Vector3.up, 0);

            float dist;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 v3;
            if (plane.Raycast(ray, out dist))
            {
                v3 = ray.GetPoint(dist);
                v3.x = NearFive(v3.x);
                v3.z = NearFive(v3.z);
                currentvect = v3;
                cube.position = v3;
                if (Input.GetMouseButtonDown(0))
                {
                    grille.TryCreateTurret(v3, dimX,dimY);

                }
                if (Input.GetMouseButtonDown(1))
                {
                    
                }

            }
        }
        
    }
 
    float NearFive(float k)
    {
        int neg = 1;
        if (k < 0)
            neg = -1;
        k = Mathf.Abs(k);
        float m =(k % 1);
        if(m>0.25f&&m < 0.75f)
        {
            return (Mathf.FloorToInt(k) + 0.5f)*neg;
        }
        else if (m <= 0.25)
        {
            return Mathf.FloorToInt(k)*neg;
        }
        else
        {
            return Mathf.CeilToInt(k)*neg;
        }
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (currentvect != null)
            Gizmos.DrawWireCube(currentvect, Vector3.one * 0.4f);
    }
}
