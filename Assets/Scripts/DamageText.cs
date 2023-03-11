using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    System.Action<DamageText> callback;
    TextMeshProUGUI tMP;


    [SerializeField] float lifeTime = 1f;
    float timer = 0f;

    Vector3 damageTextPos;

    bool isCrit;

    [SerializeField] Color normalColor;
    [SerializeField] Color critColor;

    private void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        tMP = GetComponent<TextMeshProUGUI>();
        canvas.sortingOrder = 1;
    }

    public void Launch(System.Action<DamageText> callback, float damageText, bool isCrit, Transform damageTextPos)
    {
        this.callback = callback;
        this.isCrit = isCrit;
        this.damageTextPos = damageTextPos.position;
        StartCoroutine(ShowDamageText(damageText));
    }

    private IEnumerator ShowDamageText(float towerDamage)
    {
        transform.forward = MainCam.Instance.transform.forward;

        if (isCrit) tMP.color = critColor;
        else tMP.color = normalColor;

        timer = 0;

        Vector3 endPos = damageTextPos + Vector3.up * 0.5f;

        while (timer <= lifeTime)
        {
            float t = timer / lifeTime;

            Vector3 newPosition = damageTextPos != null ? Vector3.Lerp(damageTextPos, endPos, GameCanvas.instance.dmgPosCurve.Evaluate(t)) : transform.position;
            transform.position = newPosition;

            float scaleCurve = isCrit ? GameCanvas.instance.critScaleCurve.Evaluate(t) * 0.01f : GameCanvas.instance.dmgScaleCurve.Evaluate(t) * 0.01f;
            transform.localScale = new Vector3(scaleCurve, scaleCurve, scaleCurve);

            float alpha = GameCanvas.instance.dmgAlphaCurve.Evaluate(t);
            Color color = tMP.color;
            color.a = alpha;
            tMP.color = color;

            yield return null;
        }
    }

    private void Update()
    {
        if (timer >= lifeTime)
        {
            callback?.Invoke(this);
        }

        timer += Time.deltaTime;
    }


}
