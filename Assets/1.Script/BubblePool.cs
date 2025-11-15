using UnityEngine;
using UnityEngine.Pool;
using Utility;

public class BubblePool : Singleton<BubblePool>
{
    private ObjectPool<Bubble> bulletPool;
    public Bubble _bulletPrefab;
    public void Awake()
    {
        bulletPool = new ObjectPool<Bubble>(
            createFunc: () => {
                var bubble = Instantiate(_bulletPrefab);
                bubble.Init(bulletPool);
                return bubble;
            },
            actionOnGet: (bullet) => {
                bullet.gameObject.SetActive(true);
            },
            actionOnRelease: (bullet) => {
                bullet.gameObject.SetActive(false);
            },
            actionOnDestroy: (bullet) => {
                Destroy(bullet.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
    }
}
