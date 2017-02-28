using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCount : MonoBehaviour {
    [SerializeField]
    Text tx;
    int i = 0;
    float timedelta;
	// Update is called once per frame
	void Update () {
        ++i;
        timedelta += Time.deltaTime;
        if(timedelta>1)
        {
            timedelta = timedelta % 1;
            tx.text = i.ToString();
            i = 0;
        }
	}
}
