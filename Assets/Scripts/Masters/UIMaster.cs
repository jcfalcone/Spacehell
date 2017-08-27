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
    Text scoreTxt;

    [SerializeField]
    GameObject pauseScreenObj;

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
    }

    public void pauseScreen(bool enable)
    {
        this.pauseScreenObj.SetActive(enable);
    }

    public void updateScore(int currScore)
    {
        this.scoreTxt.text = currScore.ToString("0000000");
    }
}
