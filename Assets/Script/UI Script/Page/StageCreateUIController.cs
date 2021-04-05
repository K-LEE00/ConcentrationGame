using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

using GameSystem;

public class StageCreateUIController : MonoBehaviour
{
    [SerializeField] private Dropdown totalCardNumList;
    [SerializeField] private Dropdown patternCountList;
    private GameUIPresenter UIControll;

    [SerializeField] private Button CreateButton;
    [SerializeField] private Button ShuffleButton;
    [SerializeField] private Button PlacementButton;

    private int selectTotalCardNumber;
    private int selectKindCardPattern;

    private void Awake()
    {
        UIControll = GameObject.Find("Game UI Canvas").GetComponent<GameUIPresenter>();
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
        totalCardNumList.interactable = false;
        patternCountList.interactable = false;
        CreateButton.interactable = false;
        ShuffleButton.interactable = true;

        UIControll.EventCreateStage(UIEvent.Create, selectTotalCardNumber, selectKindCardPattern);
    }

    public void OnClickCardShuffle()
    {
        ShuffleButton.interactable = false;
        PlacementButton.interactable = true;

        UIControll.EventCreateStage(UIEvent.Shuffle);
    }

    public void OnClickCardPlacement()  
    {

        ResetButtonStatus();
        RefreshData();
        gameObject.GetComponent<Canvas>().enabled = false;

        UIControll.EventCreateStage(UIEvent.Placement);
    }
}
