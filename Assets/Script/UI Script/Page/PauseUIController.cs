using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystem;

public class PauseUIController : MonoBehaviour
{
    private GameUIPresenter UIControll;

    private void Awake()
    {
        UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIPresenter>();
    }

    public void OnClickReturnTitle()
    {
        UIControll.EventPauseUI(UIEvent.Title);
    }

    public void OnClickQuitGame()
    {
        UIControll.EventPauseUI(UIEvent.Quit);
    }

    public void OnClickRetryGame()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        UIControll.EventPauseUI(UIEvent.Retry);
    }

    public void OnClickReturnGame()
    {
        UIControll.EventPauseUI(UIEvent.Return);
    }
}
