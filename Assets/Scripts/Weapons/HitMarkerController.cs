using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HitMarkerController : MonoBehaviour
{
    Image _hitMark;


    private void Awake()
    {
        _hitMark = GetComponent<Image>();
    }

    public void BodyShot()
    {
        AudioManager.Instance.PlaySFX("BodyShot");

        _hitMark.color = Color.white;
        gameObject.SetActive(true);
        StartCoroutine(DisableHitMark(0.2f));
    }

    public void HeadShot()
    {
        AudioManager.Instance.PlaySFX("HeadShot");

        _hitMark.color = Color.red;
        gameObject.SetActive(true);
        StartCoroutine(DisableHitMark(0.2f));
    }

    public IEnumerator DisableHitMark(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
