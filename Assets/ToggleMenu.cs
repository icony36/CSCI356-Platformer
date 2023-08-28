using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;

    public bool IsOpened { get; private set; }

    private void Awake()
    {
        menuUI.SetActive(false);

        IsOpened = false;
    }

    public void OpenMenu()
    {
        menuUI.SetActive(true);
        IsOpened = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void CloseMenu()
    {
        menuUI.SetActive(false);
        IsOpened = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ToggleMenuUI()
    {
        IsOpened = !IsOpened;
        menuUI.SetActive(IsOpened);
    }
}
