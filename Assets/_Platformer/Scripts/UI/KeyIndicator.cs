using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyIndicator : MonoBehaviour
{
    [SerializeField] private Text skillKeyText;
    [SerializeField] private Text toggleKeyText;

    public void SetSkillKeyText(string keyName)
    {
        skillKeyText.text = keyName;
    }

    public void SetToggleKeyText(string keyName)
    {
        toggleKeyText.text = keyName;
    }
}
