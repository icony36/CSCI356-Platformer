using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class BuffIndicator : MonoBehaviour
{
    [SerializeField] private GameObject attackUpImage;
    [SerializeField] private GameObject jumpUpImage;
    [SerializeField] private GameObject speedUpImage;
    [SerializeField] private Image skillCoolDownImage;

    private float coolDownRotationFill = 1f;
    private float coolDownOpacity = 1f;

    private void Start()
    {
        attackUpImage.SetActive(false);
        jumpUpImage.SetActive(false);
        speedUpImage.SetActive(false);
        skillCoolDownImage.fillAmount = 1;
    }

    private void Update()
    {
        skillCoolDownImage.fillAmount = coolDownRotationFill;

        Color imageColor = skillCoolDownImage.color;
        imageColor.a =  coolDownOpacity;
        skillCoolDownImage.color = imageColor;
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

    public void SetCoolDownRotationFill(float value)
    {
        coolDownRotationFill = value;
    }

    public void SetCoolDownOpacity(float value)
    {
        coolDownOpacity = value;
    }
}
