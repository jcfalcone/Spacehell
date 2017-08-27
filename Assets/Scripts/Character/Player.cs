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

    [SerializeField]
    Vector3 verticalAngle = new Vector3(0f, 0f, 50f);

    [Header("Model")]
    [SerializeField]
    Transform spaceShip;

    Renderer spaceShipRender;


    [Header("Bullets")]
    [SerializeField]
    PlayerBullet[] bullets;

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
    float startDistance;
    float startTime = 0;

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
                ControlMaster.instance.startGame();
            }

            transform.position = Vector3.Lerp(transform.position, this.startPos, (pathJorney  * Time.deltaTime) * 10);

            return;
            
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ControlMaster.instance.pause();
        }

        this.checkFire();
        this.checkBullets();
        this.movePowerUp();
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (!ControlMaster.instance.gameStarted)
        {
            return;
        }

        float btnVertical   = -Input.GetAxis("Vertical");
        float btnHorizontal = Input.GetAxis("Horizontal");

        Vector3 velocity = vectorZero;


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
        Quaternion targetRotation = this.startRotation;

        if(btnVertical != 0)
        {
            Vector3 newEulerAngle = this.verticalAngle;
            newEulerAngle.z *= btnVertical;
            targetRotation = Quaternion.Euler(this.verticalAngle * btnVertical);
        }

        this.spaceShip.localRotation = Quaternion.Lerp (this.spaceShip.localRotation, targetRotation, Time.deltaTime * speed);

        if (this.powerUpOn)
        {
            for (int count = 0; count < this.powerUpModel.Length; count++)
            {
                this.powerUpModel[count].transform.localRotation = this.spaceShip.localRotation;
            }
        }

        velocity  = transform.right * btnVertical * speed * Time.fixedDeltaTime;
        velocity += transform.forward * btnHorizontal * speed * Time.fixedDeltaTime;

        this.rb.velocity = velocity;
	}

    override protected void OnParticleCollision (GameObject obj) 
    {   
        //gameobject  is the emitter
        //obj is the object hitted (colided) with particle
        //but how to acces that exact particle what caused collision (and destroy it)  
        Debug.Log("Hit",obj);

        bool applyDamage = obj.transform.CompareTag("Enemy");

        if (obj.transform.parent != null)
        {
            applyDamage = obj.transform.parent.CompareTag("Enemy");
        }

        if (applyDamage)
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

    override protected void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            this.ApplyDamage(1);
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
        if (Input.GetButton("Fire1"))
        {
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
}
