using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HitBox : MonoBehaviour
{
    [Tooltip("This will be overridden by target tag in combat component.")]
    [SerializeField] private string targetTag;
    
    private Collider hitBoxCollider;
    private List<Collider> hitTargetList;

    private float damageToInflict;
    private Combat combat;

    private void Start()
    {
        hitBoxCollider = GetComponent<Collider>();
        hitBoxCollider.enabled = false;

        hitTargetList = new List<Collider>();
        
        if(transform.parent != null )
        {
            combat = GetComponentInParent<Combat>();
            targetTag = combat?.TargetTag;
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !hitTargetList.Contains(other))
        {
            Combat targetCombat = other.GetComponent<Combat>();
            if (targetCombat != null)
            {
               //Debug.Log(combat.gameObject.name + " is attacking: " +  targetCombat.gameObject.name);

                targetCombat.InflictDamage(damageToInflict, transform.position);
            }

            hitTargetList.Add(other);
        }
    }

    public void EnableHitBox(float damage)
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

