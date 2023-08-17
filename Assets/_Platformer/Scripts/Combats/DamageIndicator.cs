using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]  private Text text;

    private float lifetime = 0.6f;
    private float minDist = 2f;
    private float maxDist = 3f;
    private float verticalOffset = 1f;

    private Vector3 iniPos;
    private Vector3 targetPos;
    private float timer;

    void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);

        float direction = Random.Range(0f, 360f);
        iniPos = transform.position + Vector3.up * verticalOffset;

        float dist = Random.Range(minDist, maxDist);

        targetPos = iniPos + Vector3.up * dist;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float fraction = lifetime / 2f;

        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
        else if (timer > fraction)
        {
            text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));
        }

        transform.position = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
    }

    public void SetDamageText(float damage)
    {
        text.text = damage.ToString();
    }
}