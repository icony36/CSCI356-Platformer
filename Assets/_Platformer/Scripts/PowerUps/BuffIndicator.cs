using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIndicator : MonoBehaviour
{
    [SerializeField] private GameObject attackUpImage;
    [SerializeField] private GameObject jumpUpImage;
    [SerializeField] private GameObject speedUpImage;

    private bool isAttack;
    private bool isJump;
    private bool isSpeed;

    void Start()
    {
        isAttack = false;
        isJump = false;
        isSpeed = false;

        attackUpImage.SetActive(isAttack);
        jumpUpImage.SetActive(isJump);
        speedUpImage.SetActive(isSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        attackUpImage.SetActive(isAttack);
        jumpUpImage.SetActive(isJump);
        speedUpImage.SetActive(isSpeed);
    }

    public void SetIsAttack(bool value)
    {
        isAttack = value;
    }

    public void SetIsJump(bool value)
    {
        isJump = value;
    }

    public void SetIsSpeed(bool value)
    {
        isSpeed = value;
    }
}
