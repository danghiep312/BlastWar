using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


public class BossDamage : MonoBehaviour
{
    public List<SpriteRenderer> objectsToHighlight;

    [HideInInspector]
    public float health;

    public float maxHealth;
    public float transparentValue;
    [HideInInspector]
    public bool separateBoss;
    private GameObject player;

    private bool dead;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1f);
            ObjectPooler.Instance.ReleaseObject(col.gameObject);
        }
    }

    public async void TakeDamage(float damage)
    {
        health -= damage;
        SetTransparent(transparentValue);
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
