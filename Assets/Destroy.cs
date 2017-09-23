using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour 
{
    [SerializeField]
    float lifeTime;

	// Use this for initialization
	void Start () 
    {
        Destroy(gameObject, this.lifeTime);
	}
}
