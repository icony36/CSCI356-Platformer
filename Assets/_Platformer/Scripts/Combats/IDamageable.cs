using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void InflictDamage(float damageToInflict, Vector3 damageSource);
}
