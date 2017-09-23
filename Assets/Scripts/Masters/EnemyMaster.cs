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
    GameObject[] itemPrefab;

    [SerializeField]
    int[] itemPerc;

    [SerializeField]
    [Range(0f, 20f)]
    int itemAfterEnemys;

    [SerializeField]
    EnemyWaveRule[] levelRules;

    [SerializeField]
    EnemyBossRule[] levelBossRules;

    int enemyTotal = 0;
    int enemyDead = 0;
    int enemyItemCount = 0;

    int currBoss = -1;
    EnemyBossRule.bossStatus currBossStatus;

    int lastWaveId = 0;

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

        for (int count = 0; count < this.levelRules.Length; count++)
        {
            this.levelRules[count].waveId = count;
        }
	}

    public int CurrWave()
    {
        return this.currWave;
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
            else if (this.levelRules[count].afterWaveId != -1 && this.lastWaveId == this.levelRules[count].afterWaveId)
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
                if (this.nextWaveRules.Count <= 0 || this.nextWaveRules[0].priority <= this.levelRules[count].priority)
                {
                    this.nextWaveRules.Add(this.levelRules[count]);
                }
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

    public void addEnemyWave(int enemyWave)
    {
        Debug.Log(this.currWave + " - " + enemyWave);
        if (this.currWave == enemyWave)
        {
            this.enemyDead++;
        }

        if (this.enemyDead >= this.enemyTotal)
        {
            this.enemyDead = 0;
            this.currWave++;
            Debug.Log("Curr Wave - "+this.currWave);
            StartCoroutine(this.startNextWave());
        }

    }

    public IEnumerator startNextWave()
    {
        this.enemyTotal = 0;

        for (int count = 0; count < this.levelBossRules.Length; count++)
        {
            if (this.currWave == this.levelBossRules[count].alertAtWave)
            {
                this.levelBossRules[count].bossGameObject.SetActive(true);
                this.levelBossRules[count].bossGameObject.transform.position = this.levelBossRules[count].startPos;
                this.currBoss = count;
            }
        }

        if (this.currBoss != -1)
        {
            if (!this.levelBossRules[this.currBoss].keepWave)
            {
                yield break;
            }
            else
            {
                
                if (this.currBossStatus != EnemyBossRule.bossStatus.None && this.levelBossRules[this.currBoss].stopAfterSignal)
                {
                    yield break;
                }
            }
        }

        for (int count = 0; count < this.nextWaveRules.Count; count++)
        {
            this.lastWaveId = this.nextWaveRules[count].waveId;

            Debug.Log("WaveId = "+this.nextWaveRules[count].waveId);

            for (int countEnemy = 0; countEnemy < this.nextWaveRules[count].prefabs.Length; countEnemy++)
            {
                Vector3 startPos = this.nextWaveRules[count].position[countEnemy];

                for (int countAmount = 0; countAmount < this.nextWaveRules[count].amount[countEnemy]; countAmount++)
                {
                     GameObject enemy = Instantiate(this.nextWaveRules[count].prefabs[countEnemy]);
                    enemy.transform.position = startPos;
                    EnemyGroup groupControl = enemy.GetComponent<EnemyGroup>();
                    EnemyTemplate enemyControl = enemy.GetComponent<EnemyTemplate>();

                    if (groupControl)
                    {
                        groupControl.SetWave(this.currWave);
                    }
                    else if(enemyControl)
                    {
                        enemyControl.SetWave(this.currWave);
                    }

                    startPos += this.nextWaveRules[count].distance;

                    if (this.nextWaveRules[count].totalEnemys[countEnemy] > 0)
                    {
                        this.enemyTotal += this.nextWaveRules[count].totalEnemys[countEnemy];
                    }
                    else
                    {
                        this.enemyTotal++;

                        if (this.enemyItemCount > this.itemAfterEnemys)
                        {
                            int itemPerc = Random.Range(0, 1000) % 100;
                            GameObject item = null;

                            for (int countItem = 0; countItem < this.itemPerc.Length; countItem++)
                            {
                                if (this.itemPerc[countItem] <= itemPerc)
                                {
                                    item = this.itemPrefab[countItem];
                                    break;
                                }
                            }

                            this.enemyItemCount = 0;

                            if (groupControl)
                            {
                                groupControl.SetItem(item);
                            }
                            else if(enemyControl)
                            {
                                enemyControl.SetItem(item);
                            }
                        }
                    }

                    this.enemyItemCount++;

                    if (this.nextWaveRules[count].delay != 0)
                    {
                        yield return new WaitForSeconds(this.nextWaveRules[count].delay);
                    }
                    else
                    {
                        yield return null;
                    }
                }

                if (this.nextWaveRules[count].delay != 0)
                {
                    yield return new WaitForSeconds(this.nextWaveRules[count].delay);
                }
                else
                {
                    yield return null;
                }

            }
        }

        StartCoroutine(checkNextWaveRulesCorr());
    }

    public void BossStatus(EnemyBossRule.bossStatus status)
    {
        if (this.currBoss != -1)
        {
            this.currBossStatus = status;
        }
    }
}
