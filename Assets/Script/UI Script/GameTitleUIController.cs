using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTitleUIController : MonoBehaviour
{
    private GameUIController UIControll;

    // Start is called before the first frame update
    private void Awake()
    {
        UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIController>();
    }

    public void OnClickGameStart()
    {
        UIControll.StartGame();
    }

    public void OnClickGameQuit()
    {
        UIControll.QuitGame();
    }

    public void OnClickGameInfo()
    {
        UIControll.OpenGameInfo();
    }
}
