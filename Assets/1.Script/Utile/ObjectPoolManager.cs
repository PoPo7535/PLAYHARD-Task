using System;
using UnityEngine;
using UnityEngine.Pool;
using Utility;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public ObjectPool<Bubble> BubblePool { get; private set; }
    public Bubble _bulletPrefab;

    public void Awake()
    {
        InitBubblePool();
    }

    private void InitBubblePool()
    {
        BubblePool = new ObjectPool<Bubble>(
            createFunc: () => {
                var bubble = Instantiate(_bulletPrefab);
                return bubble;
            },
            actionOnGet: (bullet) => 
            {
                bullet.gameObject.SetActive(true);
                bullet.transform.localScale = Bubble.Scale;
                bullet.transform.rotation = Quaternion.identity;
            },
            actionOnRelease: (bullet) => {
                bullet.gameObject.SetActive(false);
            },
            actionOnDestroy: (bullet) => {
                Destroy(bullet.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 200
        );
    }
}
