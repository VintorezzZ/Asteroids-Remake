using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private readonly Dictionary<PoolType, PoolContainer> _pools = new Dictionary<PoolType, PoolContainer>();
    private void Awake()
    {
        Instance = this;

        foreach (var poolContainer in GetComponentsInChildren<PoolContainer>())
        {
            _pools.Add(poolContainer.poolType, poolContainer);
            poolContainer.Init();
        }
    }

    public static PoolItem Get(PoolType poolType)
    {
        if (!Instance._pools.ContainsKey(poolType))
        {
            Debug.LogError("Unknown pool name: " + poolType);
            return null;
        }

        return Instance._pools[poolType].TakeFromPool();
    }

    public static void Return(PoolItem item)
    {
        if (!Instance._pools.ContainsKey(item.PoolType))
        {
            Debug.LogError("Unknown pool name: " + item.PoolType);
            return;
        }

        Instance._pools[item.PoolType].ReturnToPool(item);
    }
}
