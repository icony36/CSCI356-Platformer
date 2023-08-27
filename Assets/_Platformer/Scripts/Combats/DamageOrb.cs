using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageOrb : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private ParticleSystem hitVFX;

    private Rigidbody rigidBody;
    private AudioManager audioManager;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioManager = AudioManager.Instance;
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == targetTag)
        {
            Combat targetCombat = other.GetComponent<Combat>();

            if (targetCombat != null)
            {
                targetCombat.InflictDamage(attackDamage, transform.position);
            }        
        }

        if (other.tag != "PowerUp")
        {
            if (hitVFX != null)
            {
                audioManager.PlaySFX("Hit", transform.position);
                Instantiate(hitVFX, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
