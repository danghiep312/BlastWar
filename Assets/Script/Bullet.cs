using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float speed;
    private Vector3 dir;
    
    public Vector3 Dir
    {
        get => dir;
        set => dir = value;
    }

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
    
}
