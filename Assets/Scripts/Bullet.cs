using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PoolItem))]
public class Bullet : VisibleObject, IPoolObservable
{
    [SerializeField] float speed = 500f;
    Rigidbody rb;
    public BasePlayer owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Init()
    {
        rb.AddForce(transform.up * speed);
        Invoke(nameof(ReturnToPool), 2f);
    }

    private void Update()
    {
        CheckPosition();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PoolManager.Return(GetComponent<PoolItem>());
    }

    private void ReturnToPool()
    {
        PoolManager.Return(GetComponent<PoolItem>());
    }

    public void OnReturnToPool()
    {
        
    }

    public void OnTakeFromPool()
    {
        Init();
    }
}
