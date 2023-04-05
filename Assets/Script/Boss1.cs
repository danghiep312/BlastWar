
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Boss1 : MonoBehaviour
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
    
    [Header("Stat")]
    [ShowInInspector]
    private float health;
    private float maxHealth;
    private float moveSpeed;

    private void Start()
    {
	    this.RegisterListener(EventID.BTakeDamage, (param) => TakeDamage());
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
	    gun.transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, player.transform.position - gun.transform.position, Vector3.forward));
	    
	    if (Time.time > nextFire)
		{
			Fire();
			nextFire = Time.time + fireRate;
		}
	    Move();
    }

    private void Move()
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

    public void TakeDamage()
    {
	    health--;
    }

    private async void Explosion()
    {
	    explosion.SetActive(true);
	    await UniTask.Delay(TimeSpan.FromSeconds(.15f));
	    explosion.SetActive(false);
    }

    private void Fire()
	{
		Explosion();
		BulletSpawn(shotForwardPos);
		BulletSpawn(shotRightPos);
		BulletSpawn(shotLeftPos);

		if (health <= maxHealth * 2.75 / 5.0)
		{
			BulletSpawn(shotRightPosOffset);
			BulletSpawn(shotLeftPosOffset);
		}
	}

	private GameObject BulletSpawn(Transform pivot)
	{
		var dir = pivot.position - gun.position;
		dir.z = 0;
		var bullet = ObjectPooler.Instance.Spawn("EnemyBullet");
		bullet.transform.position = pivot.position;
		
		bullet.GetComponent<Bullet>().Init(dir.normalized, 20f);
		return bullet;
	}

}
