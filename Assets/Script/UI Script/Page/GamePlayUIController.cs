using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] private Text totalCardNumberText;
    [SerializeField] private Text destroyCardCountText;
    private Vector3 destroyCardCountTextScale;
    private int prevDestroyCardCount;

    private GameUIPresenter UIControll;

    private void Awake()
    {
        UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIPresenter>();
        destroyCardCountTextScale = destroyCardCountText.rectTransform.localScale;
    }

    public void InitPlayUIData(int totalnum)
    {
        totalCardNumberText.text = "00";
        totalCardNumberText.text = "x" + totalnum;
        prevDestroyCardCount = 0;
    }

    public void UpatateDestroyCardCount(int destroycount)
    {
        Sequence sequnce = DOTween.Sequence();
        if(prevDestroyCardCount != destroycount && destroycount != 0) {
            prevDestroyCardCount = destroycount;
            sequnce.Append(destroyCardCountText.DOCounter(0, destroycount, 0.5f).SetDelay(0.5f))
                   .Append(destroyCardCountText.transform.DOScale((destroyCardCountTextScale * 1.5f), 0.1f))
                   .Append(destroyCardCountText.transform.DOScale(destroyCardCountTextScale, 0.1f))
                   .Play();
        }
        else
        {
            //0は初期化処理のためアニメーションはなし
            destroyCardCountText.text = destroycount + "";
        }
    }

    public void UpatateTotalCardCount(int totalcount)
    {
        totalCardNumberText.text = "x" + totalcount;
    }

    public void OnClickPauseButton()
    {
        UIControll.PauseGame();
    }
}
