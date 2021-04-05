using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [HeaderAttribute("Game Object")]
    [SerializeField] private GameObject CardPrefabData;
    [SerializeField] private GameObject CardFiled;
    private CardPlacementController CardPlacement;
    private GameUIPresenter UIControll;
    private List<GameObject> PlayCardList = new List<GameObject>();

    [HeaderAttribute("Game Status")]
    [Disable] [SerializeField] private int totalCardNumber;
    [Disable] [SerializeField] private int kindPattern;
    [Disable] [SerializeField] private float delayTimeDecisionPhase;

    [HeaderAttribute("Game Status - Monitor")]
    [Disable] [SerializeField] private GameStatus gameStatus;
    [Disable] [SerializeField] private int destroyCardCount;
    [Disable] [SerializeField] private int selectIndexFirstCard;
    [Disable] [SerializeField] private int selectIndexSecondCard;


    public GameStatus NowStatus
    {
        get
        {
            return gameStatus;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            CardPlacement = new CardPlacementController();
            CardPlacement.SetCardPrefab = CardPrefabData;

            UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIPresenter>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void InitGameStatus()
    {
        UIControll.InitPlayUI(0, totalCardNumber);
        destroyCardCount = 0;
        selectIndexFirstCard = -1;
        selectIndexSecondCard = -1;
    }
    
    private void DestroyCardData()
    {
        int cardnum = PlayCardList.Count;
        if (cardnum > 0)
        {
            for( int i=0; i< cardnum; i++)
            {
                Destroy(PlayCardList[i]);
            }
        }
        PlayCardList.Clear();
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitGameStatus();
        ChangeGameStatus(GameStatus.Title);
        UIControll.ChangeUIScreen(gameStatus);

    }

    /// <summary>
    /// 選択したカードの処理を行う
    /// </summary>
    /// <param name="selectcardindex">選択したカードのIndex</param>
    public void DecisionSeclectCard( int selectcardindex)
    {
        switch (gameStatus)
        {
            case GameStatus.FirstPhase:
                DecisionFirstPhase(selectcardindex);
                break;
            case GameStatus.SecondPhase:
                DecisionSecondPhase(selectcardindex);
                break;
            default:
                break;
        }
    }

    private void DecisionFirstPhase(int selectcardindex)
    {
        selectIndexFirstCard = selectcardindex;

        ChangeGameStatus(GameStatus.SecondPhase);
    }

    private void DecisionSecondPhase(int selectcardindex)
    {
        selectIndexSecondCard = selectcardindex;
        ChangeGameStatus(GameStatus.DecisionPhase);

        DecisionOpenCard();
    }

    /// <summary>
    /// 選択した二つのカードを比較
    /// </summary>
    private void DecisionOpenCard()
    {
        CardItemController firstcard = PlayCardList[selectIndexFirstCard].GetComponent<CardItemController>();
        CardItemController secondcard = PlayCardList[selectIndexSecondCard].GetComponent<CardItemController>();

        if(firstcard.GetCardPattern == secondcard.GetCardPattern)
        {
            firstcard.DestroyCardPrint(delayTimeDecisionPhase);
            secondcard.DestroyCardPrint(delayTimeDecisionPhase);

            destroyCardCount += 2;
            UIControll.UpdateDestroyCardCount(destroyCardCount);

            if ( destroyCardCount >= totalCardNumber)
            {
                ChangeGameStatus(GameStatus.GameEnd);
                DOTween.Sequence()
                    .AppendCallback(() => UIControll.ChangeUIScreen(gameStatus))
                    .SetDelay(2.0f);
            }
            else
            {
                ChangeGameStatus(GameStatus.FirstPhase);
            }
        }
        else
        {
            firstcard.CloseCard(delayTimeDecisionPhase);
            secondcard.CloseCard(delayTimeDecisionPhase);

            StartCoroutine(delayTimerNextPhase());
        }
    }

    IEnumerator delayTimerNextPhase()
    {
        yield return new WaitForSeconds(delayTimeDecisionPhase);

        ChangeGameStatus(GameStatus.FirstPhase);
    }

    private void ChangeGameStatus(GameStatus status)
    {
        gameStatus = status;
    }

    /// <summary>
    /// カード山を生成する（既存条件）
    /// </summary>
    public void CreateCardPile()
    {
        CardPlacement.CreatCardPile(ref PlayCardList, totalCardNumber, kindPattern);
    }

    /// <summary>
    /// カード山を生成する（新規条件）
    /// </summary>
    /// <param name="totalcard">総カード数</param>
    /// <param name="pattern">パターン数</param>
    public void CreateCardPile( int totalcard, int pattern)
    {
        if (totalcard >= 4 && pattern >= 2)
        {
            totalCardNumber = totalcard;
            kindPattern = pattern;

            CardPlacement.CreatCardPile(ref PlayCardList, totalCardNumber, kindPattern);
        }
        else
        {
            Debug.Log("Card Pile Data Error");
        }

    }

    public void ShuffleCardPile()
    {
        CardPlacement.ShuffleCardPile(ref PlayCardList);
    }

    
    public void PlacementCardPile()
    {
        bool placemnet = CardPlacement.PlacementCrad(ref PlayCardList);
        if (placemnet)
        {
            ChangeGameStatus(GameStatus.CardPlacement);
        }
        else
        {
            //エラーの場合はタイトルに戻す
            RestoreGame();
        }
    }

    public void CompleteCardPlacement()
    {
        ChangeGameStatus(GameStatus.FirstPhase);
        UIControll.PlayScreen(totalCardNumber);
    }

    public void RetryGame()
    {
        //オブジェクト削除
        DestroyCardData();

        //ステータス初期化
        InitGameStatus();
        
        //同条件でカード再配置
        CreateCardPile();
        ShuffleCardPile();
        ChangeGameStatus(GameStatus.CardPlacement);
        PlacementCardPile();
        
        //画面更新
        //ChangeGameStatus(GameStatus.FirstPhase);
        //UIControll.ChangeUIScreen(gameStatus);
    }

    public void RestoreGame()
    {
        //オブジェクト削除
        DestroyCardData();

        //ステータス初期化
        InitGameStatus();

        //画面更新
        ChangeGameStatus(GameStatus.Title);
        UIControll.ChangeUIScreen(gameStatus);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        //エディター上での実行終了
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //実環境でのプログラム終了
        UnityEngine.Application.Quit();
#endif
    }

    public void StartGame()
    {        
        //ステータス初期化 
        InitGameStatus();
        ChangeGameStatus(GameStatus.StageCreate);
        UIControll.ChangeUIScreen(GameStatus.StageCreate);
    }

    public bool CheckPauseStatus()
    {
        switch (gameStatus)
        {
            case GameStatus.FirstPhase:
            case GameStatus.SecondPhase:
            case GameStatus.DecisionPhase:
                return true;
            default:
                return false;
        }
    }
}
