using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryGroup : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private VictoryTrigger victoryTrigger;

    private void Awake()
    {
        if (victoryTrigger != null)
        {
            victoryTrigger.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (boss == null || victoryTrigger == null) { return; }

        if (boss.CurrentState == Bot.BotState.Dead)
        {
            victoryTrigger.gameObject.SetActive(true);
        }
    }
}
