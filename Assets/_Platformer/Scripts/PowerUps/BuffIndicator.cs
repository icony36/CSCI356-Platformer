using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIndicator : MonoBehaviour
{
    [SerializeField] private GameObject attackUpImage;
    [SerializeField] private GameObject jumpUpImage;
    [SerializeField] private GameObject speedUpImage;

    private void Start()
    {
        attackUpImage.SetActive(false);
        jumpUpImage.SetActive(false);
        speedUpImage.SetActive(false);
    }

    public void SetIsAttack(bool value)
    {
        attackUpImage.SetActive(value);
    }

    public void SetIsJump(bool value)
    {
        jumpUpImage.SetActive(value);
    }

    public void SetIsSpeed(bool value)
    {
        speedUpImage.SetActive(value);
    }
}
