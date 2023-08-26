using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrap : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject flamePrefab;
    [SerializeField] private List<Vector3> trapPositions;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float duration;
    [SerializeField] private float damageInterval;
    [SerializeField] private int damage;

    private int posIndex;
    private float elapsedTime = 0f;
    private float elapsedTime2 = 0f;
    private bool trapActivated = false;
    
    private void Start()
    {
        posIndex = 1;
        StartCoroutine(MoveToNextPos());
    }

    private void Update()
    {
        if(trapActivated)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime2 += Time.deltaTime;

            if (elapsedTime > duration) 
            {
                trapActivated = false;
                elapsedTime = 0f;
                flamePrefab.SetActive(false);
                posIndex++;

                if (posIndex == trapPositions.Count)
                {
                    posIndex = 0;
                    trapPositions.Reverse();
                }

                StartCoroutine(MoveToNextPos());          
            }
        }
    }

    IEnumerator MoveToNextPos()
    {
        while (Vector3.Distance(transform.position, trapPositions[posIndex]) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, trapPositions[posIndex], moveSpeed * Time.deltaTime);

            yield return null;
        }

        flamePrefab.SetActive(true);
        trapActivated = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (trapActivated)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (elapsedTime2 > damageInterval)
                {
                    Combat targetCombat = other.GetComponent<Combat>();
                    if (targetCombat != null)
                    {
                        // play sfx
                        // play vfx

                        targetCombat.InflictDamage(damage, transform.position);
                    }
                    elapsedTime2 = 0f;
                }          
            }
        }    
    }
}
