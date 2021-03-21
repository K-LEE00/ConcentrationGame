using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIController : MonoBehaviour
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

    public void OnClickQuitGame()
    {
        UIControll.QuitGame();
    }

    public void OnClickRetryGame()
    {
        UIControll.RetryGame();
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void OnClickReturnGame()
    {
        UIControll.PlayScreen();
    }
}
