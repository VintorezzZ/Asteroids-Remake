using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObservable
{
    void OnReturnToPool();
    void OnTakeFromPool();
}
