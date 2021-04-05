using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystem;

public class GameEndPageUIController : MonoBehaviour
{
    private GameUIPresenter UIControll;

    private void Awake()
    {
        UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIPresenter>();
    }

    public void OnClickReturnTitle()
    {
        UIControll.EventEndUI(UIEvent.Title);
    }

    public void OnClickGameRetry()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        UIControll.EventEndUI(UIEvent.Retry);
    }
}
