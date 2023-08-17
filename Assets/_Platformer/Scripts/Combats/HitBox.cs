using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HitBox : MonoBehaviour
{
    [SerializeField] private string targetTag;
    
    private Collider hitBoxCollider;
    private List<Collider> hitTargetList;

    private int damageToInflict;

    private void Start()
    {
        hitBoxCollider = GetComponent<Collider>();
        hitBoxCollider.enabled = false;

        hitTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag && !hitTargetList.Contains(other))
        {
            Combat targetCombat = other.GetComponent<Combat>();
            if (targetCombat != null)
            {
                // play sfx
                // play vfx

               //Debug.Log(combat.gameObject.name + " is attacking: " +  targetCombat.gameObject.name);

                targetCombat.InflictDamage(damageToInflict);
            }

            hitTargetList.Add(other);
        }
    }

    public void EnableHitBox(int damage)
    {
        hitTargetList.Clear();

        hitBoxCollider.enabled = true;

        damageToInflict = damage;
    }
    public void DisableHitBox()
    {
        hitTargetList.Clear();

        hitBoxCollider.enabled = false;
    }
}

