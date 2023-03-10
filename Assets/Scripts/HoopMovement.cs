using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoopMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    void Start()
    {
        Move();
    }

    private void Move()
    {
        transform.DOMoveX(-2.47f, moveSpeed, false).SetSpeedBased().SetEase(Ease.Linear).OnComplete(delegate ()
        {
            transform.DOMoveX(2.47f, moveSpeed, false).SetSpeedBased().SetEase(Ease.Linear).OnComplete(delegate ()
            {
                Move();
            });
        });
    }
}
