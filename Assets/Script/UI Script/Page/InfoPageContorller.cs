using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPageContorller : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnClickInfoPageClose()
    {
        gameObject.SetActive(false);
    }
}
