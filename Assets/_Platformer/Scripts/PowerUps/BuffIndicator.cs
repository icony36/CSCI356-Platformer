using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIndicator : MonoBehaviour
{
    [SerializeField] private GameObject AttackUpImage;
    [SerializeField] private GameObject JumpUpImage;
    [SerializeField] private GameObject SpeedUpImage;

    public bool isAttack;
    public bool isJump;
    public bool isSpeed;

    void Start()
    {
        isAttack = false;
        isJump = false;
        isSpeed = false;

        AttackUpImage.SetActive(isAttack);
        JumpUpImage.SetActive(isJump);
        SpeedUpImage.SetActive(isSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        AttackUpImage.SetActive(isAttack);
        JumpUpImage.SetActive(isJump);
        SpeedUpImage.SetActive(isSpeed);
    }

    public void setIsAttack(bool value)
    {
        isAttack = value;
    }

    public void setIsJump(bool value)
    {
        isJump = value;
    }

    public void setIsSpeed(bool value)
    {
        isSpeed = value;
    }
}
