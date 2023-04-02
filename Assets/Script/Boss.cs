using System;
using UnityEngine;


public abstract class Boss : MonoBehaviour
{
    protected float health;
    protected float maxHealth;
    protected float damage;
    protected float moveSpeed;

    public abstract void Fire();
    
    public abstract void Move();
    
    public abstract void TakeDamage(float damage);
}
