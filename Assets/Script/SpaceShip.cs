using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public float transparentValue;
    public List<GameObject> shipParts;

    private Transform _transform;
    private Camera cam;
    private SpriteRenderer shootPoint;
    private bool isMoving;
    private bool isFiring;
    private bool invulnerable;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 target;
    
    [Tooltip("Time between each shot")]
    private float fireRate;
    
    private void Start()
    {
        _transform = transform;
        shootPoint = _transform.GetChild(0).GetComponent<SpriteRenderer>();
        cam = Camera.main;
        fireRate = .5f;
    }

    private void Update()
    {
        var direction = GetMousePosition() - _transform.position;
        if (!isMoving)
        {
            var angle = Vector2.SignedAngle(Vector2.right, direction) - 90f;
            var targetRotation = new Vector3(0, 0, angle);
            var lookTo = Quaternion.Euler(targetRotation);
            _transform.rotation = lookTo;
        }
        
        if (Input.GetMouseButtonDown(1) && !isMoving)
        {
            isMoving = true;
            target = GetMousePosition();
        }

        if (isMoving)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target, speed * Time.deltaTime);
            if (_transform.position == target)
            {
                isMoving = false;
            }
        }

        if (fireRate > 0) fireRate -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            isFiring = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
        }
        
        if (isFiring && !isMoving && fireRate <= 0)
        {
            Shoot(direction);
            fireRate = .2f;
        }
    }

    private void Shoot(Vector3 direction)
    {
        var bullet = ObjectPooler.Instance.Spawn("Bullet");
        bullet.GetComponent<Bullet>().dir = direction.normalized;
        bullet.transform.position = shootPoint.transform.position;
        shootPoint.DOFade(1f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            shootPoint.DOFade(0, 0.1f).SetEase(Ease.Linear);
        });
    }
    
    private Vector3 GetMousePosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        return cam.ScreenToWorldPoint(mousePosition);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
    }
}
