using UnityEngine;
using UnityEngine.Pool;
using Utility;

public class BubblePool : Singleton<BubblePool>
{
    public  ObjectPool<Bubble> Pool { get; private set; }

    public Bubble _bulletPrefab;
    public void Awake()
    {
        Pool = new ObjectPool<Bubble>(
            createFunc: () => {
                var bubble = Instantiate(_bulletPrefab);
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
