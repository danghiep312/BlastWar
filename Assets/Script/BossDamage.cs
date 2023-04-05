using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


public class BossDamage : MonoBehaviour
{
    public List<SpriteRenderer> objectsToHighlight;
    
    public float transparentValue;
    [HideInInspector]
    public bool separateBoss;
    private GameObject player;

    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
            ObjectPooler.Instance.ReleaseObject(col.gameObject);
        }
    }

    private async void TakeDamage()
    {
        SetTransparent(transparentValue);
        this.PostEvent(EventID.BTakeDamage);
        await UniTask.Delay(TimeSpan.FromSeconds(.1f));
        SetTransparent(1f);
    }

    private void SetTransparent(float value)
    {
        foreach (var sr in objectsToHighlight)
        {
            sr.DOFade(value, 0);
        }
    }
}
