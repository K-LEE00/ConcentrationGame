using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class StageCreateUIController : MonoBehaviour
{
    [SerializeField] private Dropdown totalCardNumList;
    [SerializeField] private Dropdown patternCountList;
    private GameUIController UIControll;

    [SerializeField] private Button CreateButton;
    [SerializeField] private Button ShuffleButton;
    [SerializeField] private Button PlacementButton;

    private int selectTotalCardNumber;
    private int selectKindCardPattern;

    enum CreateStatus
    {
        Idle,
        CardCreate,
        CardShuffle,
    }

    [Disable] [SerializeField] private CreateStatus nowState = CreateStatus.Idle;

    private void Awake()
    {
        UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIController>();
        selectTotalCardNumber = 0;
        selectKindCardPattern = 0;
    }

    private void Start()
    {
        ResetButtonStatus();
        //リスト更新
        RefreshData();
    }

    public void ResetButtonStatus()
    {
        totalCardNumList.interactable = true;
        patternCountList.interactable = true;
        CreateButton.interactable = true;
        ShuffleButton.interactable = false;
        PlacementButton.interactable = false;
    }

    public void RefreshData()
    {
        RefreshDropdown(totalCardNumList);
        RefreshDropdown(patternCountList);
    }

    public void ChangeValueTotalCardNumber(int value)
    {
        int dropdownvalue = value;
        int maxcard = 4 + (dropdownvalue * 2);
        int maxpattern = maxcard / 2;
        //UIControll.TotalCardNumber = maxcard;
        selectTotalCardNumber = maxcard;

        //パターンリスト更新
        patternCountList.options.Clear();
        for (int patterncount = 2; patterncount <= maxpattern ; patterncount++)
        {
            patternCountList.options.Add(new Dropdown.OptionData { text = patterncount.ToString() });
        }
        RefreshDropdown(patternCountList);
    }

    public void ChangeValueCardPatternCount(int value)
    {
        int dropdownvalue = value;
        int selectpattern = 2 + dropdownvalue;

        //UIControll.KindPatternCount = selectpattern;
        selectKindCardPattern = selectpattern;
    }
    
    private void RefreshDropdown(Dropdown list)
    {
        list.value = -1;
    }

    public void OnClickCardCreate()
    {
        if (nowState != CreateStatus.Idle )
        {
            return;
        }
            
        UIControll.CreateCardPile( selectTotalCardNumber, selectKindCardPattern);
        nowState = CreateStatus.CardCreate;

        totalCardNumList.interactable = false;
        patternCountList.interactable = false;
        CreateButton.interactable = false;
        ShuffleButton.interactable = true;
    }

    public void OnClickCardShuffle()
    {
        if (nowState != CreateStatus.CardCreate)
        {
            return;
        }

        UIControll.ShuffleCardPile();
        nowState = CreateStatus.CardShuffle;

        ShuffleButton.interactable = false;
        PlacementButton.interactable = true;
    }

    public void OnClickCardPlacement()
    {
        //カード山をシャッフルした以降のみ開始可能
        if (nowState != CreateStatus.CardShuffle)
        {
            return;
        }

        UIControll.PlacementCard();
        nowState = CreateStatus.Idle;


        ResetButtonStatus();
        RefreshData();
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}
