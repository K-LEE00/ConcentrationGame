using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject ButtonRoot;
    [SerializeField] GameObject ButtonIcon;
    private Vector3 rootScale;

    private void Awake()
    {
        rootScale = ButtonIcon.transform.localScale;
    }

    public void OnPointDownButton()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(ButtonRoot.transform.DOScale((rootScale * 0.9f), 0.05f).SetEase(Ease.InBounce))
            .Join(ButtonIcon.transform.DORotate(new Vector3(0, 0, 360f), 0.5f, RotateMode.FastBeyond360));
    }

    public void OnPointOutButton()
    {
        ButtonRoot.transform.DOScale(rootScale, 0.05f).SetEase(Ease.InBounce);
    }
}
