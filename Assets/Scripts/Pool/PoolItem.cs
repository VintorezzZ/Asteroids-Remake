using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour
{
    public bool IsFree { get; private set; }
    public PoolType PoolType { get;  set; }

    private readonly List<IPoolObservable> _observableComponents = new List<IPoolObservable>();
 
    public void Init(PoolType poolType)
    {
        IsFree = true;
        PoolType = poolType;
        
        _observableComponents.AddRange(GetComponentsInChildren<IPoolObservable>());
    }

    public void TakeFromPool()
    {
        IsFree = false;

        foreach (var observableComponent in _observableComponents)
        {
            observableComponent.OnTakeFromPool();
        }
    }

    public void ReturnToPool()
    {
        IsFree = true;
        
        foreach (var observableComponent in _observableComponents)
        {
            observableComponent.OnReturnToPool();
        }
    }
}
