using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiniBoss1 : EnemyTemplate 
{

    public enum MiniBossState
    {
        StartAnimation,
        RotationLaser,
        SmallAttack,
        Gravity,
        RotateSmallAttack,
        Dead
    }

    [Header("MiniBoss")]
    [SerializeField]
    [Range(0f,100f)]
    float rotationSpeed;

    [SerializeField]
    [Range(0,10)]
    float rotationCount;

    [SerializeField]
    int[] partLife;

    [SerializeField]
    EnemyMiniBoss1Attack[] attackController;

    [SerializeField]
    float gravityTotalTime;

    float gravityTime;


    [Header("Sound")]
    [SerializeField]
    AudioClip chargingSound;

    [SerializeField]
    AudioClip deathSound;

    [SerializeField]
    AudioSource smallGunSound;

    [SerializeField]
    AudioSource gravityGunSound;

    AudioSource bossAudio;

    [Header("Timer")]
    [SerializeField]
    [Range(0f, 10f)]
    float waitBetweenAttacks = 0;

    float totalWaitBetweenAttacks = 0;

    [Header("Physics")]
    [SerializeField]
    public float mass;

    public bool kill;

    MiniBossState currBossState;

    Vector3 initEuler;

    int count = -1;

    bool checkDirection = false;
    bool startAttack = true;
    bool countAttack = true;

    bool gravityAttack = true;

    int countSmallAttack = 0;

    Vector3 oriDestine;

    Vector3 vectorZero = Vector3.zero;

    float percLife = -1;

    GameObject player;

    int totalLife;
    int currLife;

	// Use this for initialization
    override protected void Start () 
    {

        this.player = GameObject.FindGameObjectWithTag("Player");

        this.destinePos.z = transform.position.z;

        this.oriDestine = this.destinePos;

        this.destinePos = transform.position;

        this.oriDestine.z = this.destinePos.z;

        Vector3 newPos = this.destinePos;//Camera.main.ViewportToWorldPoint(new Vector3(Screen.width, 0, 0));

        transform.position = newPos;
        this.startDistance = Vector3.Distance(newPos, this.destinePos);

        //this.StartAttack(MiniBossState.SmallAttack);

        this.currBossState = MiniBossState.StartAnimation;
        //this.currBossState = MiniBossState.SmallAttack;

        this.totalWaitBetweenAttacks = this.waitBetweenAttacks;

        this.oriDestine = this.destinePos;

        base.Start();

        this.oriDestine.x = 0;
        this.oriDestine.z = this.player.transform.position.z;
        this.destinePos.x += 200;

        for (int count = 0; count < this.partLife.Length; count++)
        {
            this.totalLife += this.partLife[count];
        }

        this.currLife = this.totalLife;

        this.bossAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (this.totalWaitBetweenAttacks < this.waitBetweenAttacks)
        {
            this.totalWaitBetweenAttacks += Time.deltaTime;
            return;
        }

        if (this.kill)
        {
            for(int count = 0; count < 4; count++)
            {
                this.applyDamagePart(count, 999);
            }

            this.kill = false;
        }

        if (this.currBossState == MiniBossState.StartAnimation)
        {
            if (initMove)
            {
                initialMovement();
            }
        }
        else if (this.currBossState == MiniBossState.RotationLaser)
        {
            if (startAttack)
            {
                this.StartAttack(MiniBossState.RotationLaser);
                this.startAttack = false;
                this.CheckSound(this.currBossState);
            }
            else
            {
                if (this.count < this.rotationCount)
                {
                    transform.Rotate(new Vector3(0, 0, this.rotationSpeed * Time.deltaTime));

                    if (transform.rotation.eulerAngles.z < 5f && this.countAttack)
                    {
                        this.count++;
                        this.countAttack = false;

                        if (this.count >= this.rotationCount)
                        {
                            this.StopAttack();
                            transform.rotation = Quaternion.identity;
                            this.currBossState = MiniBossState.SmallAttack;
                            this.startAttack = true;
                            this.count = 0;
                            this.totalWaitBetweenAttacks = 0;
                        }
                    }
                    else if (transform.rotation.eulerAngles.z > 5f && !this.countAttack)
                    {
                        this.countAttack = true;
                    }
                }
            }
        }
        else if (this.currBossState == MiniBossState.SmallAttack)
        {
            if (startAttack)
            {
                this.StartAttack(MiniBossState.SmallAttack);
                this.CheckSound(this.currBossState);
                this.startAttack = false;
            }

            if (this.countSmallAttack < 10)
            {
                transform.Rotate(new Vector3(0, 0, this.rotationSpeed * 2 * Time.deltaTime));

                if ((!this.checkDirection && transform.rotation.eulerAngles.z < 10f && this.rotationSpeed > 0) ||
                    (!this.checkDirection && transform.rotation.eulerAngles.z > 350 && this.rotationSpeed < 0))
                {
                    this.checkDirection = true;
                    this.countSmallAttack++;
                }
                   

                if ((this.checkDirection && transform.rotation.eulerAngles.z > 30 && this.rotationSpeed > 0) ||
                    (this.checkDirection && transform.rotation.eulerAngles.z < 330 && this.rotationSpeed < 0))
                {
                    this.rotationSpeed *= -1;
                    this.checkDirection = false;
                    this.countSmallAttack++;
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime);

                Vector3 distance = Quaternion.identity.eulerAngles - transform.rotation.eulerAngles;

                if (distance.magnitude < 0.1f)
                {
                    this.StopAttack();
                    transform.rotation = Quaternion.identity;
                    this.currBossState = MiniBossState.Gravity;
                    this.startAttack = true;
                    this.countSmallAttack = 0;
                    this.totalWaitBetweenAttacks = 0;
                }
            }
        }
        else if (this.currBossState == MiniBossState.Gravity)
        {
            if (startAttack)
            {
                this.StartAttack(MiniBossState.Gravity, gravityAttack);
                this.StartAttack(MiniBossState.RotateSmallAttack, !gravityAttack);
                this.CheckSound(this.currBossState);
                this.startAttack = false;

                this.gravityAttack = !this.gravityAttack;
            }
            else
            {
                this.gravityTime += Time.deltaTime;

                if (this.gravityTime > this.gravityTotalTime)
                {
                    this.StopAttack();
                    transform.rotation = Quaternion.identity;
                    this.currBossState = MiniBossState.RotationLaser;
                    this.startAttack = true;
                    this.gravityTime = 0;
                    this.totalWaitBetweenAttacks = 0;
                }
            }
        }
        else if(this.currBossState == MiniBossState.Dead)
        {
            bool allInPosition = true;

            for (int count = 0; count < this.attackController.Length; count++)
            {
                this.attackController[count].transform.position = Vector3.MoveTowards(this.attackController[count].transform.position, vectorZero, Time.deltaTime * 25);

                if (this.attackController[count].transform.position != vectorZero)
                {
                    allInPosition = false;
                }
            }

            if (allInPosition)
            {
                UIMaster.instance.ShowFinal();
                Instantiate(this.deathEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
            }
        }
    }

    override protected void initialMovement()
    {
        if (this.startTime == 0)
        {
            this.startTime = Time.time;
        }

        float distCover = (Time.time - this.startTime);

        if (this.startDistance == 0)
        {
            this.startDistance = 0.1f;
        }

        float pathJorney = (distCover / this.startDistance) * 0.6f;

        if (this.destinePos != this.oriDestine)
        {
            if (pathJorney > 0.35f)
            {
                this.startTime = 0;
                //initMove = false;
                Vector3 newPos = transform.position;
                newPos.z = this.oriDestine.z;
                transform.position = newPos;
                transform.localScale = Vector3.one;
                EnemyMaster.instance.BossStatus(EnemyBossRule.bossStatus.StartBoss);
                this.destinePos = this.oriDestine;
                AudioManager.instance.fadeToAudio(ControlMaster.instance.currLevel + 1, 5f);
                //this.bullet.Play();
            }
        }
        else
        {
            if(this.destinePos == transform.position)
            {

                if (this.percLife == -1f)
                {
                    this.percLife = 0f;
                    UIMaster.instance.showLifeBar(true);
                    UIMaster.instance.SetLifeBar(this.percLife);
                }
                else
                {
                    this.percLife = Mathf.Lerp(this.percLife, 1f, Time.deltaTime * 2);
                    UIMaster.instance.SetLifeBar(this.percLife);
                }

                if (this.percLife >= 0.99f)
                {
                    this.percLife = 1f;
                    UIMaster.instance.SetLifeBar(this.percLife);
                    this.StartAttack(MiniBossState.SmallAttack);
                    this.currBossState = MiniBossState.SmallAttack;
                    initMove = false;
                    this.startTime = 0;
                }

            }
        }

        //transform.position = Vector3.Lerp(transform.position, this.destinePos, (pathJorney  * Time.deltaTime));
        transform.position = Vector3.MoveTowards(transform.position, this.destinePos, Time.deltaTime * 7f);
    }

    public override void Die()
    {
        UIMaster.instance.showLifeBar(false);
        this.StartAttack(MiniBossState.Dead);
        this.currBossState = MiniBossState.Dead;

        this.CheckSound(this.currBossState);
        this.bossAudio.PlayOneShot(this.deathSound);
    }


    public void applyDamagePart(int partId, int damage)
    {
        if (this.currBossState == MiniBossState.StartAnimation)
        {
            return;
        }

        this.partLife[partId] -= damage;

        this.currLife -= damage;

        UIMaster.instance.SetLifeBar((float)this.currLife / (float)this.totalLife);

        if (this.partLife[partId] <= 0)
        {
            this.attackController[partId].Die();
        }

        bool allDead = true;

        for (int count = 0; count < this.partLife.Length; count++)
        {
            if (this.partLife[count] > 0)
            {
                allDead = false;
            }
        }

        if (allDead)
        {
            this.Die();
        }
    }

    void StartAttack(EnemyMiniBoss1.MiniBossState attackType)
    {
        for (int count = 0; count < this.attackController.Length; count++)
        {
            this.attackController[count].StartAttack(attackType);
        }
    }

    void StartAttack(EnemyMiniBoss1.MiniBossState attackType, bool redBlack)
    {
        for (int count = 0; count < this.attackController.Length; count++)
        {
            if ((redBlack && count % 2 == 0) || (!redBlack && count % 2 == 1))
            {
                this.attackController[count].StartAttack(attackType);
            }
        }
    }

    void StopAttack()
    {
        for (int count = 0; count < this.attackController.Length; count++)
        {
            this.attackController[count].StopAttack();
        }
    }

    void CheckSound(EnemyMiniBoss1.MiniBossState attackType)
    {
        if (attackType == MiniBossState.RotationLaser)
        {
            this.bossAudio.PlayOneShot(this.chargingSound);
            this.smallGunSound.Stop();
            this.gravityGunSound.Stop();
        }
        else if (attackType == MiniBossState.SmallAttack)
        {
            this.bossAudio.Stop();
            this.smallGunSound.Play();
            this.gravityGunSound.Stop();
        }
        else if (this.currBossState == MiniBossState.Gravity)
        {
            this.bossAudio.Stop();
            this.smallGunSound.Stop();
            this.gravityGunSound.Play();
        }
        else if (this.currBossState == MiniBossState.Dead)
        {
            this.bossAudio.Stop();
            this.smallGunSound.Stop();
            this.gravityGunSound.Stop();
        }
    }
}
