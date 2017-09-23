using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterTemplate 
{
    public enum playerGun
    {
        TripleShip
    }

    [Header("Movement")]
    [SerializeField]
    [Range(0f, 1000f)]
    float speed = 5;

    float curSpeedX;
    float curSpeedY;

    [SerializeField]
    Vector3 verticalAngle = new Vector3(0f, 0f, 50f);


    [SerializeField]
    float maxForceParticle;

    [Header("Model")]
    [SerializeField]
    Transform spaceShip;

    Renderer spaceShipRender;

    [Header("Sound")]
    [SerializeField]
    AudioSource playerAudioSource;

    [SerializeField]
    AudioClip superBulletSFX;

    [SerializeField]
    AudioSource shootingAudioSource;

    [SerializeField]
    AudioSource charingAudioSource;

    [Header("Bullets")]
    [SerializeField]
    PlayerBullet[] bullets;


    [SerializeField]
    float superBulletWaitTime;

    float superBulletTotalTime;

    [SerializeField]
    PlayerSuperBulletList[] superBullets;

    [Header("Powerup")]
    [SerializeField]
    GameObject[] powerUpModel;

    [SerializeField]
    gun1Movement[] powerUpControl;

    [SerializeField]
    PlayerPowerupBullet[] powrUpBullet;

    bool powerUpOn = false;

    Rigidbody rb;

    Vector3 vectorZero = Vector3.zero;
    Quaternion startRotation;

    int currBullet = 0;

    Vector3 startPos;
    public Vector3 addVelocity;
    float startDistance;
    float startTime = 0;

    bool useSuperBullet = false;

    Vector3 vector3One = Vector3.one;


	// Use this for initialization
	void Start () 
    {
        base.Start();
        this.rb = GetComponent<Rigidbody>();
        this.startRotation = this.spaceShip.localRotation;
        this.spaceShipRender = this.spaceShip.GetComponent<Renderer>();

        this.startPos = transform.position;

        Vector3 playerPos = transform.position;
        playerPos.x -= 30;

        transform.position = playerPos;
        this.startDistance = Vector3.Distance(playerPos, this.startPos);

        this.curSpeedX = this.speed;
        this.curSpeedY = this.speed;

        this.addVelocity = vectorZero;

        UIMaster.instance.SetLifeUI(this.currentLife);
	}

    void Update()
    {
        if (!ControlMaster.instance.gameStarted)
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

            if (pathJorney > 0.2f)
            {
                UIMaster.instance.ShowTutorial();
                ControlMaster.instance.startGame();
            }

            transform.position = Vector3.Lerp(transform.position, this.startPos, (pathJorney  * Time.deltaTime) * 10);

            return;
            
        }

        if (!ControlMaster.instance.gamePaused)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                ControlMaster.instance.pause();
            }

            this.checkFire();
            this.checkBullets();
            this.movePowerUp();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (!ControlMaster.instance.gameStarted)
        {
            return;
        }

        /*if (!ControlMaster.instance.gameStarted)
        {
            return;
        }*/

        float btnVertical   = -Input.GetAxis("Vertical");
        float btnHorizontal = Input.GetAxis("Horizontal");

        Vector3 velocity = vectorZero;

        //Debug.Log(transform.position.y + " ( " + Mathf.Abs((transform.position.y + 18f) / 5f) + " = "+Mathf.Lerp(this.curSpeedY, 0, Mathf.Abs((transform.position.y + 18f) / 4f))+" ) - ( " + Mathf.Abs((transform.position.y - 23f) / 2f) + " )");


        if (transform.position.y < -18f && btnVertical > 0)
        {
            this.curSpeedY = Mathf.Lerp(this.curSpeedY, 0, Time.deltaTime * 2);
        }
        else if (transform.position.y > 21f && btnVertical < 0)
        {
            this.curSpeedY = Mathf.Lerp(this.curSpeedY, 0, Time.deltaTime * 2);
        }
        else
        {
            this.curSpeedY = Mathf.Lerp(this.curSpeedY, this.speed, Time.deltaTime);
        }

        if (transform.position.x < -38f && btnHorizontal < 0)
        {
            this.curSpeedX = Mathf.Lerp(this.curSpeedX, 0, Time.deltaTime * 2);
        }
        else if (transform.position.x > 39.5f &&  btnHorizontal > 0)
        {
            this.curSpeedX = Mathf.Lerp(this.curSpeedX, 0, Time.deltaTime * 2);
        }
        else
        {
            this.curSpeedX = this.speed;
        }

        if ((transform.position.y < -23f && btnVertical > 0) ||
            (transform.position.y > 25f && btnVertical < 0))
        {
            btnVertical = 0;
        }

        if ((transform.position.x < -44f && btnHorizontal < 0) ||
            (transform.position.x > 42.5f &&  btnHorizontal > 0))
        {
            btnHorizontal = 0;
        }

        if ((transform.position.y < -23f) || (transform.position.y > 25f))
        {
            this.addVelocity.y = 0;
        }

        if ((transform.position.x < -44f) || (transform.position.x > 42.5f))
        {
            this.addVelocity.x = 0;
        }

        Quaternion targetRotation = this.startRotation;

        if(btnVertical != 0)
        {
            Vector3 newEulerAngle = this.verticalAngle;
            newEulerAngle.z *= btnVertical;
            targetRotation = Quaternion.Euler(this.verticalAngle * btnVertical);
        }

        this.spaceShip.localRotation = Quaternion.Lerp (this.spaceShip.localRotation, targetRotation, Time.fixedDeltaTime * speed);

        if (this.powerUpOn)
        {
            for (int count = 0; count < this.powerUpModel.Length; count++)
            {
                this.powerUpModel[count].transform.localRotation = this.spaceShip.localRotation;
            }
        }

        velocity = transform.right * btnVertical * this.curSpeedY * Time.fixedDeltaTime;
        velocity += transform.forward * btnHorizontal * this.curSpeedX * Time.fixedDeltaTime;

        velocity += this.addVelocity;

        this.rb.velocity = velocity;
        this.addVelocity = Vector3.Lerp(this.addVelocity, vectorZero, Time.fixedDeltaTime * 4f);
	}

    override protected void OnParticleCollision (GameObject obj) 
    {   
        //gameobject  is the emitter
        //obj is the object hitted (colided) with particle
        //but how to acces that exact particle what caused collision (and destroy it)  

        bool applyDamage = obj.transform.CompareTag("Enemy") || obj.transform.CompareTag("EnemyBoss");

        if (obj.transform.parent != null)
        {
            applyDamage = obj.transform.parent.CompareTag("Enemy") || obj.transform.CompareTag("EnemyBoss");
        }

        if (applyDamage)
        {
            ParticleSystem particleSystem = obj.GetComponent<ParticleSystem>();
            ParticleCollisionEvent[] collisions = new ParticleCollisionEvent[particleSystem.GetSafeCollisionEventSize()];
            int numberOfCollisions = particleSystem.GetCollisionEvents(this.gameObject, collisions);

            for (int count = 0; count < numberOfCollisions; count++)
            {
                Vector3 force = collisions[count].velocity * 1;
                this.addVelocity += force;
                this.addVelocity = Vector3.ClampMagnitude(this.addVelocity, maxForceParticle);
                //this.rb.AddForce(force);
            }

            if (!this.startFlash)
            {
                this.ApplyDamage(1);
                UIMaster.instance.SetLifeUI(this.currentLife);
                this.flashCount = this.flashTimes;
            }

            if (!this.startFlash && this.flashCount > 0)
            {
                this.startFlash = true;
                StartCoroutine(flashChar());
            }
        }
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("EnemyBoss"))
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;

            dir.z = 0;

            this.addVelocity += dir * 50f;
            this.ApplyDamage(1);
            UIMaster.instance.SetLifeUI(this.currentLife);
            this.flashCount = this.flashTimes;

            if (!this.startFlash && this.flashCount > 0)
            {
                this.startFlash = true;
                StartCoroutine(flashChar());
            }
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.transform.CompareTag("Enemy"))
        {
            if (!this.startFlash)
            {
                this.ApplyDamage(1);
                UIMaster.instance.SetLifeUI(this.currentLife);
                this.flashCount = this.flashTimes;
            }

            if (!this.startFlash && this.flashCount > 0)
            {
                this.startFlash = true;
                StartCoroutine(flashChar());
            }
        }
    }

    override protected IEnumerator flashChar()
    {
        while (this.flashCount > 0)
        {
            this.spaceShipRender.material.color = this.flashColor;
            yield return new WaitForSeconds(0.1f);
            this.spaceShipRender.material.color = this.originalColor;
            yield return new WaitForSeconds(0.1f);

            this.flashCount--;
        }

        this.startFlash = false;
    }
    void checkFire()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && this.superBullets[this.currBullet].bulletCharging.isPlaying)
        {
            this.superBullets[this.currBullet].bulletCharging.Stop();

            if (this.charingAudioSource.isPlaying)
            {
                this.charingAudioSource.Stop();
            }

            if (this.useSuperBullet)
            {
                this.playerAudioSource.PlayOneShot(this.superBulletSFX);

                GameObject tempBullet = Instantiate(this.superBullets[this.currBullet].prefab, transform.position + (transform.forward * 5), transform.rotation);

                Collider[] enemys = Physics.OverlapSphere(transform.position, 100f, this.superBullets[this.currBullet].enemyLayer);

                if (enemys.Length > 0)
                {
                    tempBullet.GetComponent<PlayerSuperBullet>().setTarget(enemys[Random.Range(0, 1000) % enemys.Length].transform);
                }

                this.useSuperBullet = false;
                this.superBulletTotalTime = 0;
            }
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (!this.superBullets[this.currBullet].bulletCharging.isPlaying)
            {
                this.superBullets[this.currBullet].bulletCharging.Play(true);
            }

            if (this.superBulletTotalTime > this.superBulletWaitTime)
            {
                this.useSuperBullet = true;
            }

            this.superBullets[this.currBullet].bulletCharging.transform.localScale = vector3One * Mathf.Clamp01(this.superBulletTotalTime / this.superBulletWaitTime);

            this.superBulletTotalTime += Time.deltaTime;

            if (!this.charingAudioSource.isPlaying)
            {
                this.charingAudioSource.Play();
            }

            if (this.shootingAudioSource.isPlaying)
            {
                this.shootingAudioSource.Stop();
            }

            this.stopBullets();
        }
        else if (Input.GetButton("Fire1"))
        {
            if (!this.shootingAudioSource.isPlaying)
            {
                this.shootingAudioSource.Play();
            }

            if (!this.bullets[this.currBullet].bullet.isEmitting)
            {
                this.bullets[this.currBullet].bullet.Play(true);
            }

            if (this.powerUpOn)
            {
                if (!this.powrUpBullet[this.currBullet].bullet[0].isEmitting)
                {
                    for (int count = 0; count < this.powrUpBullet[this.currBullet].bullet.Length; count++)
                    {
                        this.powrUpBullet[this.currBullet].bullet[count].Play(true);
                    }
                }
            }
        }
        else
        {
            if (this.shootingAudioSource.isPlaying)
            {
                this.shootingAudioSource.Stop();
            }

            this.stopBullets();
        }
    }

    void checkBullets()
    {

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            this.spaceShipRender.material = this.bullets[0].spaceshipMaterial;
            this.gameObject.layer         = this.bullets[0].spaceshipLayer;

            if (this.currBullet != 0)
            {
                this.stopBullets();
                this.superBulletTotalTime = 0;
                this.useSuperBullet = false;

                for (int count = 0; count < this.superBullets.Length; count++)
                {
                    this.superBullets[count].bulletCharging.Stop();
                }
            }

            this.currBullet = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            this.spaceShipRender.material = this.bullets[1].spaceshipMaterial;
            this.gameObject.layer         = this.bullets[1].spaceshipLayer;

            if (this.currBullet != 1)
            {
                this.stopBullets();
                this.superBulletTotalTime = 0;
                this.useSuperBullet = false;

                for (int count = 0; count < this.superBullets.Length; count++)
                {
                    this.superBullets[count].bulletCharging.Stop();
                }
            }

            this.currBullet = 1;
        }
    }

    void stopBullets()
    {
        if (!this.bullets[this.currBullet].bullet.isStopped)
        {
            this.bullets[this.currBullet].bullet.Stop(false, ParticleSystemStopBehavior.StopEmitting);

            if (this.powerUpOn)
            {
                for (int count = 0; count < this.powrUpBullet[this.currBullet].bullet.Length; count++)
                {
                    this.powrUpBullet[this.currBullet].bullet[count].Stop(false, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
    }

    void AddLife(int amount)
    {
        this.currentLife += amount;
        UIMaster.instance.SetLifeUI(this.currentLife);
    }

    void AddPowerUp(playerGun type)
    {
        this.powerUpOn = true;

        switch (type)
        {
            case playerGun.TripleShip:
                for (int count = 0; count < this.powerUpModel.Length; count++)
                {
                    this.powerUpModel[count].SetActive(true);
                    this.powerUpModel[count].transform.rotation = Quaternion.identity;
                }

                break;
            default:
                break;
        }
    }

    void movePowerUp()
    {
        if (!this.powerUpControl[0].inPosition)
        {
            for (int count = 0; count < this.powerUpControl.Length; count++)
            {
                this.powerUpControl[count].UpdateMovement();
            }
        }
    }

    override public void Die()
    {
        base.Die();
        UIMaster.instance.ShowFinal();
    }

    override public void ApplyDamage(int damage)
    {
        if (this.powerUpOn)
        {
            this.powerUpOn = false;

            for (int count = 0; count < this.powerUpModel.Length; count++)
            {
                this.powerUpModel[count].SetActive(false);
            }

            StartCoroutine(coolDownDamage());

            return;
        }

        base.ApplyDamage(damage);
    }
}
