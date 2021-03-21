using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;

public class CardPlacementController
{
    private GameObject CardPrefabData;
    private GameObject CardFiled;
    private int horizontalMaxCount = 6;
    private int verticalMaxCount = 3;

    private float verticalCardInterval = 3.0f;
    private float horizontalCardInterval = 1.8f;

    private int cardNum;
    private int kindPattern;

    private Vector3 pointCardPile = new Vector3(-8f, 0.0f, 3.5f);

    private string shuffleTweenID = "Shuffle"; 
    private string placementTweenID = "Placement";
    private Tween CardShuffleUpTween = null;
    private Tween CardShuffleDownTween = null;
    private float shuffleTweenSpeed = 0.1f;

    private GameManager GameManegy;

    public CardPlacementController()
    {
        GameManegy = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public GameObject SetPrefab
    {
        set
        {
            CardPrefabData = value;
        }
    }

    public GameObject SetFiled
    {
        set
        {
            CardFiled = value;
        }
    }

    /// <summary>
    /// ゲームで使用するカード山を作る
    /// </summary>
    /// <param name="cardlist">生成したカード情報を受け取るリスト</param>
    /// <param name="num">カードの総数</param>
    /// <param name="kind">パターンの総数</param>
    /// <returns></returns>
    public bool CreatCardPile(ref List<GameObject> cardlist, int num, int kind)
    {
        if (!CardOptionCheck(num, kind))
        {
            return false;
        }

        cardNum = num;
        kindPattern = kind;

        int pattern = 0;
        int cardset = 0;

        for (int cardidx = 0; cardidx < cardNum; cardidx++)
        {
            //MonoBehaviour継承の変わりに直接呼ぶ
            cardlist.Add( MonoBehaviour.Instantiate(
                                CardPrefabData,
                                pointCardPile,
                                Quaternion.identity));

            cardlist[cardidx].GetComponent<CardItemController>().InitCardData(cardidx, pattern);
            cardset++;
            if(cardset == 2)
            {
                cardset = 0;
                pattern++;
                if(pattern >= kindPattern)
                {
                    pattern = 0;
                }
            }
        }

        return true;
    }
    /// <summary>
    /// Fisher-Yates shuffleアルゴリズムを使用してカードをシャッフルする
    /// </summary>
    /// <param name="cardlist">生成したカードリスト</param>
    public void ShuffleCardPile(ref List<GameObject> cardlist)
    {
        Random rnd = new Random();
        
        for (int i = cardNum; i > 1; --i)
        {
            int a = i - 1;
            int b = Random.Range(0, (cardNum - 1));

            GameObject tmp = cardlist[a];
            cardlist[a] = cardlist[b];
            cardlist[b] = tmp;
        }

        //CardのIndex再配置
        for (int cardidx = 0; cardidx < cardNum; cardidx++)
        {
            cardlist[cardidx].GetComponent<CardItemController>().cardId = cardidx;
        }

        Vector3 pos = cardlist[0].gameObject.transform.position;
        Vector3 offset = new Vector3(0, 0, 0.5f);


        CardShuffleUpTween.Kill();
        CardShuffleUpTween = cardlist[0].gameObject.transform.DOMove((pos + offset), shuffleTweenSpeed);
        CardShuffleUpTween.SetEase(Ease.Linear);
        CardShuffleUpTween.SetLoops(3);

        CardShuffleDownTween.Kill();
        CardShuffleDownTween = cardlist[0].gameObject.transform.DOMove(pos, shuffleTweenSpeed/2);

        Sequence SequenceCardTween1 = DOTween.Sequence();
        SequenceCardTween1
                    .SetId(shuffleTweenID)
                    .Append(CardShuffleUpTween)
                    .Append(CardShuffleDownTween)
                    .Play();
    }

    /// <summary>
    /// カード山のカードをプレイマットに配置する
    /// </summary>
    /// <param name="cardlist">カード山リスト</param>
    /// <returns>成功有無</returns>
    public bool PlacementCrad( ref List<GameObject> cardlist)
    {
        int createline = 0;
        float createstartypoint = 0; 
        float xpointstart = (horizontalCardInterval / 2) - (cardNum / 2.0f * horizontalCardInterval);
        int endlinecount = 0;

        //Y座標の開始位置を取得
        switch ((cardNum-1) / horizontalMaxCount)
        {
            case 0:
                endlinecount = 0;
                endlinecount = cardNum;
                createstartypoint = 0;
                break;
            case 1:
                createline = 1;
                endlinecount = cardNum - horizontalMaxCount;
                createstartypoint = verticalCardInterval / 2.0f; 
                break;
            case 2:
                createline = 2;
                endlinecount = cardNum - (horizontalMaxCount * 2);
                createstartypoint = verticalCardInterval;
                break;
            default:
                return false;
        }

        //カード配置開始
        KillTweentToID(shuffleTweenID);
        KillTweentToID(placementTweenID);
        Sequence SequenceCardTween = DOTween.Sequence();
        SequenceCardTween.SetId(placementTweenID);
        float linecount;
        for ( int cardidx=0; cardidx<cardNum ; cardidx++)
        {

            int line = cardidx / 6;
            linecount = (createline != line) ? horizontalMaxCount : endlinecount;
            xpointstart = (horizontalCardInterval / 2) - (linecount / 2.0f * horizontalCardInterval);

            Vector3 pos  = new Vector3(xpointstart + ((cardidx - (line * horizontalMaxCount)) * horizontalCardInterval), 0.0f, createstartypoint - (line * verticalCardInterval));

            SequenceCardTween.Append(cardlist[cardidx].transform.DOMove(pos, 0.15f))
                    .AppendInterval(0.1f);
                    
        }
        SequenceCardTween.OnComplete(() => GameManegy.CompleteCardPlacement());
        return true;
    }

    private void KillTweentToID(string tweenid)
    {
        var killtween = DOTween.TweensById(tweenid);
        if (killtween != null)
        {
            killtween.ForEach((tween) =>
            {
                if (tween != null)
                {
                    tween.Complete();
                }
            });
        }
    }

    private bool CardOptionCheck(int cardcount, int patterncount)
    {
        if (cardcount == 0 || (cardcount % 2) != 0)
        {
            return false;
        }

        if (cardcount > (horizontalMaxCount * verticalMaxCount))
        {
            return false;
        }

        if ((cardcount / 2) < patterncount)
        {
            return false;
        }

        return true;
    }
}
    