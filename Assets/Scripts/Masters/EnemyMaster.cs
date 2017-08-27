using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour 
{

    public static EnemyMaster instance
    {
        get { return _instance; }//can also use just get;
        set { _instance = value; }//can also use just set;
    }

    //Creates a class variable to keep track of GameManger
    static EnemyMaster _instance = null;

    [SerializeField]
    int currWave = 0;

    [SerializeField]
    EnemyWaveRule[] levelRules;

    int enemyTotal = 0;
    int enemyDead = 0;

    List<EnemyWaveRule> nextWaveRules;
	// Use this for initialization
	void Start () 
    {
        this.nextWaveRules = new List<EnemyWaveRule>();

        if(instance)
        {
            //GameManager exists,delete copy
            DestroyImmediate(gameObject);
        }
        else
        {
            //assign GameManager to variable "_instance"
            instance = this;
        }   
	}

    public IEnumerator checkNextWaveRulesCorr()
    {
        this.nextWaveRules.Clear();

        for (int count = 0; count < this.levelRules.Length; count++)
        {
            //Add to the next wave if the current one matchs
            if (this.currWave == this.levelRules[count].afterWaveRule)
            {
                this.nextWaveRules.Clear();
                this.nextWaveRules.Add(this.levelRules[count]);
            }
            //Add to the next wave if the wave counts matchs
            else if (this.levelRules[count].waveCounts != 0 && this.currWave % this.levelRules[count].waveCounts == 0)
            {
                if(this.nextWaveRules[0].priority < this.levelRules[count].priority)
                {
                    this.nextWaveRules.Clear();
                }

                if (this.nextWaveRules.Count <= 0)
                {
                    this.nextWaveRules.Add(this.levelRules[count]);
                }
            }
            //Add to the next wave - Default
            else if (this.levelRules[count].afterWaveRule == -1 &&
                    this.levelRules[count].waveCounts == 0)
            {
                this.nextWaveRules.Add(this.levelRules[count]);
            }

            yield return null;
        }

        if (this.nextWaveRules.Count > 0)
        {
            int ruleIndex = Random.Range(0, 1000) % this.nextWaveRules.Count;
            EnemyWaveRule currRule = this.nextWaveRules[ruleIndex];

            this.nextWaveRules.Clear();
            this.nextWaveRules.Add(currRule);
        }
    }

    public void checkNextWaveRules()
    {
        this.nextWaveRules.Clear();

        for (int count = 0; count < this.levelRules.Length; count++)
        {
            //Add to the next wave if the current one matchs
            if (this.currWave == this.levelRules[count].afterWaveRule)
            {
                this.nextWaveRules.Clear();
                this.nextWaveRules.Add(this.levelRules[count]);
            }
            //Add to the next wave if the wave counts matchs
            else if (this.levelRules[count].waveCounts != 0 && this.currWave % this.levelRules[count].waveCounts == 0 && this.currWave != 0)
            {
                if(this.nextWaveRules[0].priority < this.levelRules[count].priority)
                {
                    this.nextWaveRules.Clear();
                }

                if (this.nextWaveRules.Count <= 0)
                {
                    this.nextWaveRules.Add(this.levelRules[count]);
                }
            }
            //Add to the next wave - Default
            else if (this.levelRules[count].afterWaveRule == -1 &&
                this.levelRules[count].waveCounts == 0)
            {
                if (this.nextWaveRules.Count <= 0)
                {
                    this.nextWaveRules.Add(this.levelRules[count]);
                }
            }
        }

        if (this.nextWaveRules.Count > 0)
        {
            int ruleIndex = Random.Range(0, 1000) % this.nextWaveRules.Count;
            EnemyWaveRule currRule = this.nextWaveRules[ruleIndex];

            this.nextWaveRules.Clear();
            this.nextWaveRules.Add(currRule);
        }
    }

    public void addDeadEnemy()
    {
        this.enemyDead++;

        if (this.enemyDead >= this.enemyTotal)
        {
            this.enemyDead = 0;
            this.currWave++;
            StartCoroutine(this.startNextWave());
        }

    }

    public IEnumerator startNextWave()
    {
        this.enemyTotal = 0;

        for (int count = 0; count < this.nextWaveRules.Count; count++)
        {
            for (int countEnemy = 0; countEnemy < this.nextWaveRules[count].prefabs.Length; countEnemy++)
            {
                Vector3 startPos = this.nextWaveRules[count].position[countEnemy];

                for (int countAmount = 0; countAmount < this.nextWaveRules[count].amount[countEnemy]; countAmount++)
                {
                    GameObject enemy = Instantiate(this.nextWaveRules[count].prefabs[countEnemy], startPos, Quaternion.identity);

                    startPos += this.nextWaveRules[count].distance;

                    if (this.nextWaveRules[count].totalEnemys[countEnemy] > 0)
                    {
                        this.enemyTotal += this.nextWaveRules[count].totalEnemys[countEnemy];
                    }
                    else
                    {
                        this.enemyTotal++;
                    }
                }

                yield return null;

            }
        }

        StartCoroutine(checkNextWaveRulesCorr());
    }
}
