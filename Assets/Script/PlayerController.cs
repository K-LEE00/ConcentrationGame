using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;

public class PlayerController : MonoBehaviour
{
    private Camera MainCamera;
    private GameManager GameManegy;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        GameManegy = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManegy.NowStatus)
        {
            case GameStatus.Title:
                //カード操作イベントなし
                break;
            case GameStatus.StageCreate:
                //カード操作イベントなし
                break;
            case GameStatus.FirstPhase:
                SelectCardFirstStep();
                break;
            case GameStatus.SecondPhase:
                SelectCardSecondStep();
                break;
            case GameStatus.DecisionPhase:
                //カード操作イベントなし
                break;
            case GameStatus.GameEnd:
                //カード操作イベントなし
                break;
            default:
                break;
        }
    }

    private void SelectCardFirstStep()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                DecisionSelectCard(hit.collider.gameObject);
            }
        }
    }

    private void SelectCardSecondStep()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                DecisionSelectCard(hit.collider.gameObject);
            }
        }
    }

    /// <summary>
    /// 選択したカードを確認する
    /// </summary>
    /// <param name="selectcard">選択カードのGameObject</param>
    private void DecisionSelectCard(GameObject selectcard )
    {
        if (selectcard.tag == "Card")
        {
            CardItemController card = selectcard.GetComponent<CardItemController>();
            if (card.NowStatus == CardStatus.Close)
            {
                int selectindex = card.OpenCard();
                GameManegy.DecisionSeclectCard(selectindex);
            }
        }
    }
}
