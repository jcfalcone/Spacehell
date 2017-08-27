using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour {

    [SerializeField]
    float speed;

    [SerializeField]
    float amplitude;

    [SerializeField]
    float frequency;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 newPos = this.amplitude * (Mathf.Sin(2 * Mathf.PI * this.frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * this.frequency * (Time.time - Time.deltaTime))) * transform.up;

        newPos.x += Time.deltaTime * this.speed;

        transform.position += newPos;
	}
}
