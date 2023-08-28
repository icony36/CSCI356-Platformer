using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIndicator : MonoBehaviour
{
    [SerializeField] private Image attackUpImage;
    [SerializeField] private Image attackUpCDImage;
    [SerializeField] private Image jumpUpImage;
    [SerializeField] private Image jumpUpCDImage;
    [SerializeField] private Image speedUpImage;
    [SerializeField] private Image speedUpCDImage;
    [SerializeField] private Image skillCDImage;

    private float coolDownRotationFill = 1f;
    private float attackUpFill = 0f;
    private float speedUpFill = 0f;
    private float jumpUpFill = 0f;

    private void Start()
    {
        attackUpImage.gameObject.SetActive(false);
        attackUpCDImage.gameObject.SetActive(false);

        jumpUpImage.gameObject.SetActive(false);
        jumpUpCDImage.gameObject.SetActive(false);

        speedUpImage.gameObject.SetActive(false);
        speedUpCDImage.gameObject.SetActive(false);

        skillCDImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        skillCDImage.fillAmount = coolDownRotationFill;
        attackUpCDImage.fillAmount = attackUpFill;
        jumpUpCDImage.fillAmount = jumpUpFill;
        speedUpCDImage.fillAmount = speedUpFill;
    }

    public void SetCDRotationFill(float value)
    {
        coolDownRotationFill = value;

        if(coolDownRotationFill >= 1f)
        {
            skillCDImage.gameObject.SetActive(true);
        }
        else if (coolDownRotationFill <= 0f)
        {
            skillCDImage.gameObject.SetActive(false);
        }
    }

    public void SetAttackUpRotationFill(float value)
    {
        attackUpFill = value;

        if (attackUpFill <= 0f)
        {
            attackUpImage.gameObject.SetActive(true);
            attackUpCDImage.gameObject.SetActive(true);
        }
        else if (attackUpFill >= 1f)
        {
            attackUpImage.gameObject.SetActive(false);
            attackUpCDImage.gameObject.SetActive(false);
        }
    }

    public void SetJumpUpRotationFill(float value)
    {
        jumpUpFill = value;

        if (jumpUpFill <= 0f)
        {
            jumpUpImage.gameObject.SetActive(true);
            jumpUpCDImage.gameObject.SetActive(true);
        }
        else if (jumpUpFill >= 1f)
        {
            jumpUpImage.gameObject.SetActive(false);
            jumpUpCDImage.gameObject.SetActive(false);
        }
    }

    public void SetSpeedUpRotationFill(float value)
    {
        speedUpFill = value;

        if (speedUpFill <= 0f)
        {
            speedUpImage.gameObject.SetActive(true);
            speedUpCDImage.gameObject.SetActive(true);
        }
        else if (speedUpFill >= 1f)
        {
            speedUpImage.gameObject.SetActive(false);
            speedUpCDImage.gameObject.SetActive(false);
        }
    }
}
