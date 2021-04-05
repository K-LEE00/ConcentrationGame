using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;

public class GameUIPresenter : MonoBehaviour
{
    [SerializeField] private Canvas TitleCanvas;
    [SerializeField] private Canvas CreateCanvas;
    [SerializeField] private Canvas GamePlayCanvas;
    [SerializeField] private Canvas GameEndCanvas;
    [SerializeField] private Canvas GamePauseCanvas;
    [SerializeField] private Canvas GameInfoCanvas;

    private GameManager GameManegy;
    private GamePlayUIController PlayUI;

    private void Awake()
    {
        GameManegy = GameObject.Find("Game Manager").GetComponent<GameManager>();
        PlayUI = GamePlayCanvas.gameObject.GetComponent<GamePlayUIController>();
        GameInfoCanvas.gameObject.SetActive(false);
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

    /// <summary>
    /// ステージ設定UIページからのイベントを取得する
    /// </summary>
    /// <param name="uievent"></param>
    /// <param name="totalcard">全カード設定データ　（Createdeのみ）</param>
    /// <param name="pattern">全パターン数設定データ　（Createdeのみ）</param>
    public void EventCreateStage(UIEvent uievent, int totalcard = -1, int pattern = -1)
    {
        switch(uievent)
        {
            case UIEvent.Create:
                CreateCardPile(totalcard, pattern);
                break;
            case UIEvent.Shuffle:
                GameManegy.ShuffleCardPile();
                break;
            case UIEvent.Placement:
                GameManegy.PlacementCardPile();
                break;
        }
    }

    /// <summary>
    /// カードを生成をゲームマネージャーに依頼する
    /// 引数をいれない場合既存条件を使用
    /// </summary>
    /// <param name="totalcard">カードの総数</param>
    /// <param name="pattern">カードのパターン数</param>
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

    /// <summary>
    /// タイトル画面からのイベントを取得する
    /// </summary>
    /// <param name="uievent"></param>
    public void EventTitleUI(UIEvent uievent)
    {
        switch (uievent)
        {
            case UIEvent.Start:
                StartGame();
                break;
            case UIEvent.Info:
                OpenGameInfo();
                break;
            case UIEvent.Quit:
                QuitGame();
                break;
        }
    }

    /// <summary>
    /// ゲーム終了画面からのイベントを取得する
    /// </summary>
    /// <param name="uievent"></param>
    public void EventEndUI(UIEvent uievent)
    {
        switch (uievent)
        {
            case UIEvent.Title:
                ReturnTitlePage();
                break;
            case UIEvent.Retry:
                RetryGame();
                break;
        }
    }

    /// <summary>
    /// メニュー画面からのイベントを取得する
    /// </summary>
    /// <param name="uievent"></param>
    public void EventPauseUI(UIEvent uievent)
    {
        switch (uievent)
        {
            case UIEvent.Title:
                ReturnTitlePage();
                break;
            case UIEvent.Quit:
                QuitGame();
                break;
            case UIEvent.Retry:
                RetryGame();
                break;
            case UIEvent.Return:
                PlayScreen();
                break;
        }
    }


    private void ReturnTitlePage()
    {
        GameManegy.RestoreGame();
    }

    private void RetryGame()
    {
        GameManegy.RetryGame(); 
    }

    private void StartGame()
    {
        GameManegy.StartGame();
    }

    private void QuitGame()
    {
        GameManegy.QuitGame();
    }

    public void PauseGame()
    {
        if (GameManegy.CheckPauseStatus())
        {
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

    /// <summary>
    /// 情報表示画面表示
    /// 情報表示画面は表示キャンバス切り替えではなく現在の画面を上に表示を出す
    /// </summary>
    public void OpenGameInfo()
    {
        if (!GameInfoCanvas.gameObject.activeSelf)
        {
            GameInfoCanvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// ステータスに合わせて画面のh表示/非表示切り替え
    /// </summary>
    /// <param name="screen">出力するステータス</param>
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
