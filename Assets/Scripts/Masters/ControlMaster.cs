using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMaster : MonoBehaviour 
{

    public static ControlMaster instance
    {
        get { return _instance; }//can also use just get;
        set { _instance = value; }//can also use just set;
    }

    //Creates a class variable to keep track of GameManger
    static ControlMaster _instance = null;

    public bool gameStarted = false;
    public bool gamePaused  = false;

    int playerScore = 0;

	// Use this for initialization
	void Start () 
    {
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

        EnemyMaster.instance.checkNextWaveRules();
	}
	
    public void startGame()
    {
        this.gameStarted = true;

        StartCoroutine(EnemyMaster.instance.startNextWave());
    }

    public void pause()
    {
        this.gamePaused = !this.gamePaused;
        Time.timeScale = (this.gamePaused) ? 0f : 1f;

        UIMaster.instance.pauseScreen(this.gamePaused);
    }

    public void quitGame()
    {
        Debug.Break();
        Application.Quit();
    }

    public void addScore(int score)
    {
        this.playerScore += score;
        UIMaster.instance.updateScore(this.playerScore);
    }
}
