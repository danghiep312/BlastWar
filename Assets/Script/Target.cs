using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    private Transform _transform;
    private Vector3 mousePosition;
    private Camera cam;
    private GameObject player;
    [SerializeField] private Color inRangeColor;
    [SerializeField] private Color originalColor; 
    private SpriteRenderer sr;
    private Transform shadow;
    [HideInInspector]
    public bool canCharge;

    private void Start()
    {
        Cursor.visible = false;
        _transform = transform;
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
       
        shadow = transform.GetChild(0);
        shadow.DORotate(Vector3.forward * 360f, 2f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
        transform.DORotate(Vector3.forward * -360f, 4f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void Update() 
    {
        _transform.position = GetMousePosition();
        if (player != null && Vector3.Distance(player.transform.position, _transform.position) < 5) {
            sr.color = inRangeColor;
            canCharge = true;
        } else {
            sr.color = originalColor;
            canCharge = false;
        }
    }

    private Vector3 GetMousePosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        return cam.ScreenToWorldPoint(mousePosition);
    }
}