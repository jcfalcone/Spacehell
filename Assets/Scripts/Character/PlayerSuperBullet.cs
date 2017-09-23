using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperBullet : MonoBehaviour 
{
    [SerializeField]
    [Range(1f, 9000f)]
    float speed;

    [SerializeField]
    [Range(1f, 9000f)]
    float speedRotation;


    [SerializeField]
    Transform target;

    Rigidbody rb;
    Transform bullet;

	// Use this for initialization
	void Start () 
    {
        this.rb = GetComponent<Rigidbody>();
        this.bullet = transform.GetChild(0);
        Destroy(gameObject, 2f);
	}

    void Update() 
    {
        if (this.target)
        {
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * this.speedRotation);
        }

        this.bullet.Rotate(0, 0, Time.deltaTime * (this.speedRotation * 10));
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        this.rb.velocity = transform.forward * this.speed * Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }
}
