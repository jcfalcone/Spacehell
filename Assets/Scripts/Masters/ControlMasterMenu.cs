using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlMasterMenu : MonoBehaviour 
{
    AsyncOperation async;

    [SerializeField]
    CanvasGroup loadingGroup;

    [SerializeField]
    Animator cameraAnimator;

	// Use this for initialization
	void Start () {
		
	}

    public void OnClickCredits()
    {
        this.cameraAnimator.CrossFade("CameraMainMenu_Credits", 0f);
    }

    public void OnClickReturn()
    {
        this.cameraAnimator.CrossFade("CameraMainMenu_CreditsReturn", 0f);
    }

    public void OnClickMap(int mapToLoad)
    {
        AudioManager.instance.fadeToAudio(mapToLoad);
        StartCoroutine(startLoadLevel(mapToLoad));
        StartCoroutine(checkProgress());
        StartCoroutine(showLoading());
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

        while(this.loadingGroup.alpha < 1)
        {
            yield return null;
        }

        this.async.allowSceneActivation = true;
        Time.timeScale = 1f;
    }

    IEnumerator showLoading()
    {
        this.loadingGroup.gameObject.SetActive(true);

        while (this.loadingGroup.alpha < 1)
        {
            this.loadingGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    public void quitGame()
    {
        Debug.Break();
        Application.Quit();
    }
}
