using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalUILifes : MonoBehaviour 
{
    [SerializeField]
    Player player;

	// Use this for initialization
	/*public void StartPoints () 
    {
        StartCoroutine(convertLifeInPoints());
	}*/
	
    IEnumerator convertLifeInPoints()
    {
        while (player.currentLife > 0)
        {
            player.currentLife--;
            ControlMaster.instance.addScore(ControlMaster.instance.lifePoints);
            UIMaster.instance.SetLifeUI(player.currentLife);
            yield return new WaitForSeconds(0.8f);
        }
    }
}
