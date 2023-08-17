using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour {

    [SerializeField] private PlayerData playerData;
    [SerializeField] private int trapDamage;
    [SerializeField] private float trapInterval;

    private Animator spikeTrapAnim;
    
    private void Start()
    {
        spikeTrapAnim = GetComponent<Animator>();
        StartCoroutine(OpenCloseTrap());
    }
    IEnumerator OpenCloseTrap()
    {
        spikeTrapAnim.SetTrigger("Open");

        yield return new WaitForSeconds(trapInterval);

        spikeTrapAnim.SetTrigger("Close");

        yield return new WaitForSeconds(trapInterval);

        StartCoroutine(OpenCloseTrap());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Combat targetCombat = other.GetComponent<Combat>();
            if (targetCombat != null)
            {
                // play sfx
                // play vfx

                targetCombat.InflictDamage(trapDamage);
            }
        }
    }
}