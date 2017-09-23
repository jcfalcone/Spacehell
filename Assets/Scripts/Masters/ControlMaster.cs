using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    public int currLevel;


    [SerializeField]
    public int lifePoints;


    [SerializeField]

    AsyncOperation async;

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
        AudioManager.instance.fadeToAudio(this.currLevel);
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

    public void pause(bool showUI)
    {
        this.gamePaused = !this.gamePaused;
        Time.timeScale = (this.gamePaused) ? 0f : 1f;

        if (showUI)
        {
            UIMaster.instance.pauseScreen(this.gamePaused);
        }
    }

    public void quitGame()
    {
        Debug.Break();
        Application.Quit();
    }

    public void retryLevel(bool pause)
    {
        if (pause)
        {
            this.pause();
        }

        StartCoroutine(startLoadLevel(1));
        StartCoroutine(checkProgress());
        StartCoroutine(UIMaster.instance.showLoading());
    }

    IEnumerator startLoadLevel(int mapToLoad)
    {
        this.async = Application.LoadLevelAsync(mapToLoad);
        this.async.allowSceneActivation = false;

        yield return async;
    }

    IEnumerator checkProgress()
    {
        while (this.async.progress < 0.9f)
        {
            yield return null;
        }

        while(UIMaster.instance.loadingGroup.alpha < 1)
        {
            yield return null;
        }

        this.async.allowSceneActivation = true;
        Time.timeScale = 1f;
    }

    public void mainMenuLevel()
    {
        if (this.gamePaused)
        {
            this.pause();
        }

        AudioManager.instance.fadeToAudio(0);
        StartCoroutine(startLoadLevel(0));
        StartCoroutine(checkProgress());
        StartCoroutine(UIMaster.instance.showLoading());
    }

    public void addScore(int score)
    {
        this.playerScore += score;
        UIMaster.instance.updateScore(this.playerScore);
    }
}
