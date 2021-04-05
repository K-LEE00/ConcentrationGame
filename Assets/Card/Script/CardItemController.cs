using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;
using DG.Tweening;

public class CardItemController : MonoBehaviour
{
    [HeaderAttribute("Card - Object")]
    [SerializeField] private Material cardFrontMaterial;
    [SerializeField] private Material desCardFrontMaterial;
    [SerializeField] private GameObject effectPosition;
    [SerializeField] private GameObject monsterPosition;
    [SerializeField] private GameObject fogEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private List<GameObject> MonsterPatternList = new List<GameObject>();
    private GameObject monsterPattern;

    [HeaderAttribute("Card - Status")]
    [Disable] public int cardId;
    [Disable] [SerializeField] private int pattern;
    [Disable] [SerializeField] private GameObject cardFront;
    [Disable] [SerializeField] private CardStatus cardStatus;


    private Vector3 openRotateVector = new Vector3(0, 0, 180);
    private Vector3 closeRotateVector = new Vector3(0, 0, 0);

    /// <summary>
    /// カードの今の状態を取得
    /// </summary>
    public CardStatus NowStatus
    {
        get
        {
            return cardStatus;
        }
    }

    /// <summary>
    /// 選択したカードのパターン取得
    /// </summary>
    public int GetCardPattern
    {
        get
        {
            return pattern;
        }
    }

    /// <summary>
    /// カードの初期情報を登録する
    /// </summary>
    /// <param name="id">カードのindex</param>
    /// <param name="pt">カードが持つパターン</param>
    public void InitCardData(int id, int pt)
    {
        cardId = id;
        pattern = pt;

        cardStatus = CardStatus.Close;

        monsterPattern = Instantiate(MonsterPatternList[pattern], monsterPosition.transform.position , monsterPosition.transform.rotation , monsterPosition.transform);
        
        monsterPattern.SetActive(false);
    }

    /// <summary>
    /// カードを開く
    /// </summary>
    /// <returns>選択したカードのindex</returns>
    public int OpenCard()
    {
        if (!monsterPattern.activeSelf)
        {
            cardStatus = CardStatus.Open;

            Sequence sequence = DOTween.Sequence()
                                            .Append(gameObject.transform.DORotate(openRotateVector, 0.5f))
                                            .AppendCallback(() => ActiveCardPattern(true));

            return cardId;
        }
        else
        {
            return -1;
        }
    }

    private void ActiveCardPattern(bool value)
    {
        monsterPattern.SetActive(value);
    }

    /// <summary>
    /// カードを閉じる
    /// </summary>
    /// <param name="delaytime">アニメーション演出用で用意された遅延時間</param>
    public void CloseCard(float delaytime)
    {
        StartCoroutine(CardCloseTimer(delaytime));
    }

    IEnumerator CardCloseTimer(float delaytime)
    {
        yield return new WaitForSeconds(delaytime/2);

        if (fogEffect != null)
        {
            Instantiate(fogEffect, effectPosition.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(delaytime/2);

        monsterPattern.SetActive(false);
        gameObject.transform.DORotate(closeRotateVector, 0.5f);

        cardStatus = CardStatus.Close;
    }

    /// <summary>
    /// 当てられたカードをゲームから除外
    /// </summary>
    /// <param name="delaytime">アニメーション演出用で用意された遅延時間</param>
    public void DestroyCardPrint(float delaytime)
    {
        StartCoroutine(CardDestroyTimer(delaytime));
    }

    IEnumerator CardDestroyTimer(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);

        monsterPattern.SetActive(false);
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, effectPosition.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(delaytime / 5);

        cardFront.GetComponent<Renderer>().material = desCardFrontMaterial;
    }
}

