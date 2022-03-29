using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameMenuButton : MonoBehaviour
{
    [SerializeField] GameObject inGameMenu;

    public void OpenAndCloseMenu()
    {
        inGameMenu.SetActive(!inGameMenu.activeSelf);
    }
}
