
using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Boss1 : Boss
{
    private GameObject player;
    public Transform gun;
    public Transform bumper;
    
    [Header("Pivot")]
    public Transform shotForwardPos;
    public Transform shotRightPos;
    public Transform shotLeftPos;
    public Transform shotRightPosOffset;
    public Transform shotLeftPosOffset;
    public Transform topLeft;
    public Transform topRight;
    public GameObject explosion;
    
    [Header("Rate")]
    private float nextFire;
    private float fireRate;
    private Transform target;

    private void Start()
    {
	    bumper.DORotate(Vector3.forward * 360f, 4f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
	    health = maxHealth = 60f;
	    player = GameObject.FindGameObjectWithTag("Player");
	    nextFire = Time.time + 1.4f;
	    fireRate = 1.21f;
	    moveSpeed = 14f;
	    target = Random.Range(0, 1) > 0.5f ? topLeft : topRight;
    }

    private void Update()
    {
	    if (player == null) return;
	    gun.transform.LookAt(player.transform);
	    gun.transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, player.transform.position - gun.transform.position, Vector3.forward));
		if (Time.time > nextFire)
		{
			//this.gun.GetComponent<Animator>().SetTrigger("shoot");
			Fire();
			nextFire = Time.time + fireRate;
		}
		
		
	}

    public override void Move()
    {
	    if (health <= maxHealth * 2.83 / 5.0)
	    {
		    if (Vector3.Distance(transform.position, target.position) > 0.1f)
		    {
			    transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
		    }
		    else
		    {
			    target = target == topLeft ? topRight : topLeft;
		    }
	    }
	    if (health >= maxHealth * 3.6 / 5f)
	    {
		    fireRate = 1.21f;
		    return;
	    }
	    if (health >= maxHealth * 2f / 5f)
	    {
		    fireRate = 1.1f;
	    }
    }

    public override void TakeDamage(float damage)
    {
	    health -= damage;
    }


    public override void Fire()
	{
		var forwardBullet = BulletSpawn(shotForwardPos);
		var rightBullet = BulletSpawn(shotRightPos);
		var leftBullet = BulletSpawn(shotLeftPos);

		if (health <= maxHealth * 2.75 / 5.0)
		{
			var rightBulletOffset = BulletSpawn(shotRightPosOffset);
			var leftBulletOffset = BulletSpawn(shotLeftPosOffset);
		}
	}

	private GameObject BulletSpawn(Transform pivot)
	{
		var dir = pivot.position - gun.position;
		var bullet = ObjectPooler.Instance.Spawn("Bullet");
		bullet.transform.position = pivot.position;
		bullet.GetComponent<Bullet>().Init(dir.normalized, 160f);
		return bullet;
	}
    
}
