using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiniBoss1 : MonoBehaviour 
{
    [SerializeField]
    [Range(0f,100f)]
    float rotationSpeed;

    [SerializeField]
    [Range(0,10)]
    float rotationCount;

    [SerializeField]
    EnemyMiniBoss1Attack[] attackController;

    Vector3 initEuler;

    int count = -1;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(new Vector3(0, 0, this.rotationSpeed * Time.deltaTime));
	}
}
