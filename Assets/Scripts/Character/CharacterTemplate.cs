using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterTemplate : MonoBehaviour 
{
    
    [SerializeField]
    [Range(0f, 10000f)]
    int maxLife;

    [SerializeField]
    GameObject deathEffect;

    [Header("Effects")]
    [SerializeField]
    protected Color flashColor;

    [SerializeField]
    protected int flashTimes = 10;

    protected int flashCount = 0;

    protected bool startFlash = false;
    protected bool takeDamage = true;

    protected Color originalColor;

    protected int currentLife;

	// Use this for initialization
	virtual protected void Start () 
    {
        this.currentLife = this.maxLife;
        this.originalColor = Color.white;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
    }

    abstract protected void OnParticleCollision(GameObject obj);

    abstract protected void OnCollisionEnter(Collision collision);

    abstract protected IEnumerator flashChar();

    protected IEnumerator coolDownDamage()
    {
        this.takeDamage = false;
        yield return new WaitForSeconds(0.3f);
        this.takeDamage = true;
    }


    public void ApplyDamage(int damage)
    {
        if (this.takeDamage)
        {

            Debug.Log("Damage1 - "+this.currentLife, gameObject);
            this.currentLife -= damage;
            Debug.Log("Damage2 - "+this.currentLife, gameObject);

            if (this.currentLife <= 0)
            {
                this.Die();
            }

            StartCoroutine(coolDownDamage());
        }
    }

    public virtual void Die()
    {
        if (this.deathEffect != null)
        {
            Instantiate(this.deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
