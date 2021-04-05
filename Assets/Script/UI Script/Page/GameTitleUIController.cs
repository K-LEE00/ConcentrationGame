using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystem;

public class GameTitleUIController : MonoBehaviour
{
    private GameUIPresenter UIControll;

    // Start is called before the first frame update
    private void Awake()
    {
        UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIPresenter>();
    }

    public void OnClickGameStart()
    {
        UIControll.EventTitleUI(UIEvent.Start);
    }

    public void OnClickGameQuit()
    {
        UIControll.EventTitleUI(UIEvent.Quit);
    }

    public void OnClickGameInfo()
    {
        UIControll.EventTitleUI(UIEvent.Info);
    }
}
