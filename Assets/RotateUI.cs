using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUI : MonoBehaviour 
{
    [SerializeField]
    [Range(0f, 300f)]
    float speed;

    RectTransform uiTransform;

	// Use this for initialization
	void Start () 
    {
        this.uiTransform = GetComponent<RectTransform>();	
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.uiTransform.Rotate(0, this.speed, 0);
	}
}
