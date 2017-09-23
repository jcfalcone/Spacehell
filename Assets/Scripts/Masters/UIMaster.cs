using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour 
{

    public static UIMaster instance
    {
        get { return _instance; }//can also use just get;
        set { _instance = value; }//can also use just set;
    }

    //Creates a class variable to keep track of GameManger
    static UIMaster _instance = null;

    [SerializeField]
    Text lifesTxt;

    [SerializeField]
    Text lifesFinalTxt;

    [SerializeField]
    Text scoreTxt;

    [SerializeField]
    Text finalScoreTxt;

    [SerializeField]
    GameObject pauseScreenBack;

    [SerializeField]
    GameObject pauseScreenObj;

    [Header("Loading")]
    [SerializeField]
    public CanvasGroup loadingGroup;

    [Header("Tutorial")]
    [SerializeField]
    GameObject tutorialObj;

    [Header("Player Canvas")]
    [SerializeField]
    GameObject playerStatic;

    [SerializeField]
    GameObject playerDynamic;

    [Header("Final")]
    [SerializeField]
    GameObject playerFinal;

    [Header("Boss")]
    [SerializeField]
    GameObject[] lifeCanvas;

    [SerializeField]
    Image lifeBar;

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
	}
	
    public void SetLifeUI(int totalLife)
    {

        this.lifesTxt.gameObject.SetActive(true);

        this.lifesTxt.text = "X "+totalLife.ToString();
        this.lifesFinalTxt.text = "X "+totalLife.ToString();
    }

    public void pauseScreen(bool enable)
    {
        this.pauseScreenObj.SetActive(enable);
        this.pauseScreenBack.SetActive(enable);
    }

    public void updateScore(int currScore)
    {
        this.scoreTxt.text = currScore.ToString("0000000");
        this.finalScoreTxt.text = currScore.ToString("0000000");
    }

    public void showLifeBar(bool status)
    {
        for (int count = 0; count < this.lifeCanvas.Length; count++)
        {
            this.lifeCanvas[count].SetActive(status);
        }
    }

    public void SetLifeBar(float perc)
    {
        this.lifeBar.fillAmount = perc;
    }

    public void ShowFinal()
    {
        this.playerStatic.SetActive(false);
        this.playerDynamic.SetActive(false);
        this.playerFinal.SetActive(true);
    }

    public void ShowTutorial()
    {
        if (this.tutorialObj)
        {
            this.tutorialObj.SetActive(true);
        }
    }

    public IEnumerator showLoading()
    {
        this.loadingGroup.gameObject.SetActive(true);

        while (this.loadingGroup.alpha < 1)
        {
            this.loadingGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }
}
