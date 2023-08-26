using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private HitBox SkillHitbox;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float skillMult;

    private void Start()
    {
        StartCoroutine(ActivateSkill());
    }

    private IEnumerator ActivateSkill()
    {
        SkillHitbox.EnableHitBox(Mathf.Round(playerData.attackDamage * skillMult));

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
