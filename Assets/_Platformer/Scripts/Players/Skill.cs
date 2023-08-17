using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private HitBox SkillHitbox;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float skillMult;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateSkill());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ActivateSkill()
    {
        SkillHitbox.EnableHitBox(playerData.attackDamage * skillMult);

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
