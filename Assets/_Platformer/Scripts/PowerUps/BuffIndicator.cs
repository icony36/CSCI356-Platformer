using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIndicator : MonoBehaviour
{
    [SerializeField] private Image attackUpImage;
    [SerializeField] private Image jumpUpImage;
    [SerializeField] private Image speedUpImage;
    [SerializeField] private Image skillCoolDownImage;

    private float coolDownRotationFill = 1f;
    private float coolDownOpacity = 1f;
    private float attackUpFill = 0f;
    private float speedUpFill = 0f;
    private float jumpUpFill = 0f;

    private void Start()
    {
        /*attackUpImage.SetActive(false);
        jumpUpImage.SetActive(false);
        speedUpImage.SetActive(false);*/
        skillCoolDownImage.fillAmount = 1;
    }

    private void Update()
    {
        skillCoolDownImage.fillAmount = coolDownRotationFill;
        attackUpImage.fillAmount = attackUpFill;
        jumpUpImage.fillAmount = jumpUpFill;
        speedUpImage.fillAmount = speedUpFill;

        //Color imageColor = skillCoolDownImage.color;
        //imageColor.a =  coolDownOpacity;
        //skillCoolDownImage.color = imageColor;
    }

    /*public void SetIsAttack(bool value)
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
    }*/

    public void SetCoolDownRotationFill(float value)
    {
        coolDownRotationFill = value;
    }

    public void SetAttackUpRotationFill(float value)
    {
        attackUpFill = value;
    }

    public void SetJumpUpRotationFill(float value)
    {
        jumpUpFill = value;
    }

    public void SetSpeedUpRotationFill(float value)
    {
        speedUpFill = value;
    }

    public void SetCoolDownOpacity(float value)
    {
        coolDownOpacity = value;
    }
}
