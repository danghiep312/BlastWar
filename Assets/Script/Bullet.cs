using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private SpriteRenderer sr;
    public float speed;
    public Vector3 dir;
    

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += dir * (speed * Time.deltaTime);
        if (!sr.isVisible)
        {
            ObjectPooler.Instance.ReleaseObject(gameObject);
        }
    }

    public void Init(Vector3 dir, float speed)
    {
        this.dir = dir;
        this.speed = speed;
    }

}
