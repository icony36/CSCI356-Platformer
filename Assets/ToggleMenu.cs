using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;

    private void Awake()
    {
        menuUI.SetActive(false);
    }

    public void OpenMenu()
    {
        menuUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void CloseMenu()
    {
        menuUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
