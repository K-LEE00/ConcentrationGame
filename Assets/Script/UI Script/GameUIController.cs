using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Canvas TitleCanvas;
    [SerializeField] private Canvas CreateCanvas;
    [SerializeField] private Canvas GamePlayCanvas;
    [SerializeField] private Canvas GameEndCanvas;
    [SerializeField] private Canvas GamePauseCanvas;
    [SerializeField] private Canvas GameInfoCanvas;

    private GameManager GameManegy;
    private GamePlayUIController PlayUI;
    private StageCreateUIController controllCreateUI;

    private void Awake()
    {
        GameManegy = GameObject.Find("Game Manager").GetComponent<GameManager>();
        PlayUI = GamePlayCanvas.gameObject.GetComponent<GamePlayUIController>();
        controllCreateUI = GamePlayCanvas.gameObject.GetComponent<StageCreateUIController>();
    }

    public void UpdateDestroyCardCount(int count)
    {
        PlayUI.UpatateDestroyCardCount(count);
    }

    public void InitPlayUI(int now, int total)
    {
        PlayUI.UpatateDestroyCardCount(now);
        PlayUI.UpatateTotalCardCount(total);
    }

    public void CreateCardPile( int totalcard = -1, int pattern = -1)
    {
        if( totalcard > 1 && pattern > 1 )
        {
            GameManegy.CreateCardPile(totalcard, pattern);
        }
        else
        {
            GameManegy.CreateCardPile();
        }
    }

    public void ShuffleCardPile()
    {
        GameManegy.ShuffleCardPile();
    }

    public void PlacementCard()
    {

        GameManegy.PlacementCardPile();
    }

    public void ReturnTitlePage()
    {
        GameManegy.RestoreGame();
    }

    public void RetryGame()
    {
        GameManegy.RetryGame(); 
    }

    public void StartGame()
    {
        GameManegy.StartGame();
    }

    public void QuitGame()
    {
        GameManegy.QuitGame();
    }

    public void PauseGame()
    {
        //GameManegy.QuitGame();
        if (GameManegy.CheckPauseStatus())
        {
            Debug.Log("Pause Menu Call");
            ChangeUIScreen(GameStatus.GamePause);
        }
        else
        {
            Debug.Log("Not Pause Menu Status");
        }
    }
    
    public void PlayScreen(int max = -1)
    {
        if( max < 1)
        {
            //UIではPhase間の画面差がないためFirstで戻す
            ChangeUIScreen(GameStatus.FirstPhase);
        }
        else
        {
            ChangeUIScreen(GameStatus.FirstPhase);
            InitPlayUI(0, max);
        }
    }

    public void OpenGameInfo()
    {
        GameInfoCanvas.gameObject.SetActive(true);
    }

    public void ChangeUIScreen(GameStatus screen)
    {
        switch (screen)
        {
            case GameStatus.Title:
                TitleCanvas.enabled = true;
                CreateCanvas.enabled = false;
                GamePlayCanvas.enabled = false;
                GameEndCanvas.enabled = false;
                GamePauseCanvas.enabled = false;
                break;
            case GameStatus.StageCreate:
                TitleCanvas.enabled = false;
                CreateCanvas.enabled = true;
                GamePlayCanvas.enabled = false;
                GameEndCanvas.enabled = false;
                GamePauseCanvas.enabled = false;
                break;
            case GameStatus.FirstPhase:
            case GameStatus.SecondPhase:
            case GameStatus.DecisionPhase:
                TitleCanvas.enabled = false;
                CreateCanvas.enabled = false;
                GamePlayCanvas.enabled = true;
                GameEndCanvas.enabled = false;
                GamePauseCanvas.enabled = false;
                break;
            case GameStatus.GameEnd:
                TitleCanvas.enabled = false;
                CreateCanvas.enabled = false;
                GamePlayCanvas.enabled = false;
                GameEndCanvas.enabled = true;
                GamePauseCanvas.enabled = false;
                break;
            case GameStatus.GamePause:
                TitleCanvas.enabled = false;
                CreateCanvas.enabled = false;
                GamePlayCanvas.enabled = true;
                GameEndCanvas.enabled = false;
                GamePauseCanvas.enabled = true;
                break;
        }
    }
}
