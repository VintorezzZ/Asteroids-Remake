using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
        CancelInvoke(nameof(ReturnToPool));
        PoolManager.Return(GetComponent<PoolItem>());
    }

    private void ReturnToPool()
    {
        PoolManager.Return(GetComponent<PoolItem>());
    }

    public void OnReturnToPool()
    {
        rb.velocity = Vector3.zero;
        owner = null;
    }

    public void OnTakeFromPool()
    {
        Init();
    }
}
