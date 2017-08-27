using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun1Movement : MonoBehaviour 
{
    [SerializeField]
    Vector3 finalPos;

    Vector3 startPos;
    float startDistance;
    float startTime = 0;
    public bool inPosition = false;

	// Use this for initialization
	void Start () 
    {
        transform.localPosition = this.finalPos;

        Vector3 playerPos = this.finalPos;
        playerPos.x -= 30;

        Debug.Log(playerPos);

        transform.localPosition = playerPos;
        this.startDistance = Vector3.Distance(playerPos, this.startPos);

        this.inPosition = false;
	}
	
	// Update is called once per frame
	public void UpdateMovement () 
    {
        
        if (this.startTime == 0)
        {
            this.startTime = Time.time;
        }

        float distCover = (Time.time - this.startTime) * 3;

        if (this.startDistance == 0)
        {
            this.startDistance = 0.1f;
        }

        float pathJorney = distCover / this.startDistance;

        if (pathJorney > 0.9f)
        {
            this.inPosition = true;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, this.finalPos, pathJorney);
	}
}
