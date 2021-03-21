using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPageUIController : MonoBehaviour
{
    private GameUIController UIControll;

    private void Awake()
    {
        UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIController>();
    }

    public void OnClickReturnTitle()
    {
        UIControll.ReturnTitlePage();
    }

    public void OnClickGameRetry()
    {
        UIControll.RetryGame();
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}
