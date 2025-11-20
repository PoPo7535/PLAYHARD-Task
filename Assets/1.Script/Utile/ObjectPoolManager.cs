using System;
using UnityEngine;
using UnityEngine.Pool;
using Utility;

public class ObjectPoolManager : LocalSingleton<ObjectPoolManager>
{
    public ObjectPool<Bubble> BubblePool { get; private set; }
    public ObjectPool<BubbleStar> BubbleStarPool { get; private set; }
    public ObjectPool<BubbleScore> BubbleScorePool { get; private set; }
    public ObjectPool<BubbleAttack> BubbleAttackPool { get; private set; }
    [SerializeField] private Bubble _bulletPrefab;
    [SerializeField] private BubbleStar _bulletStarPrefab;
    [SerializeField] private BubbleScore _BubbleScorePrefab;
    [SerializeField] private BubbleAttack _BubbleAttackPrefab;

    public void Awake()
    {
        BubblePool = InitPool(_bulletPrefab, Bubble.Scale, Quaternion.identity);
        BubbleStarPool = InitPool(_bulletStarPrefab, Bubble.Scale, Quaternion.identity);
        BubbleScorePool = InitPool(_BubbleScorePrefab, Bubble.Scale, Quaternion.identity);
        BubbleAttackPool = InitPool(_BubbleAttackPrefab, Vector3.one, Quaternion.identity);
        // InitBubblePool();
        // InitBubbleStarPool();
    }

    private ObjectPool<T> InitPool<T>(T preFab, Vector3 scale, Quaternion quaternion) where T : MonoBehaviour
    {
        return new ObjectPool<T>(
            createFunc: () => {
                var preFabObj = Instantiate(preFab);
                return preFabObj;
            },
            actionOnGet: (obj) => 
            {
                obj.gameObject.SetActive(true);
                obj.transform.localScale = scale;
                obj.transform.rotation = quaternion;
            },
            actionOnRelease: (obj) => {
                obj.gameObject.SetActive(false);
            },
            actionOnDestroy: (obj) => {
                Destroy(obj.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 200
        );
    }
    
    // private void InitBubbleStarPool()
    // {
    //     BubbleStarPool = new ObjectPool<BubbleStar>(
    //         createFunc: () => {
    //             var bubble = Instantiate(_bulletStarPrefab);
    //             return bubble;
    //         },
    //         actionOnGet: (bullet) => 
    //         {
    //             bullet.gameObject.SetActive(true);
    //             bullet.transform.localScale = Bubble.Scale;
    //             bullet.transform.rotation = Quaternion.identity;
    //         },
    //         actionOnRelease: (bullet) => {
    //             bullet.gameObject.SetActive(false);
    //         },
    //         actionOnDestroy: (bullet) => {
    //             Destroy(bullet.gameObject);
    //         },
    //         collectionCheck: false,
    //         defaultCapacity: 10,
    //         maxSize: 200
    //     );
    // }
    // private void InitBubblePool()
    // {
    //     BubblePool = new ObjectPool<Bubble>(
    //         createFunc: () => {
    //             var bubble = Instantiate(_bulletPrefab);
    //             return bubble;
    //         },
    //         actionOnGet: (bullet) => 
    //         {
    //             bullet.gameObject.SetActive(true);
    //             bullet.transform.localScale = Bubble.Scale;
    //             bullet.transform.rotation = Quaternion.identity;
    //         },
    //         actionOnRelease: (bullet) => {
    //             bullet.gameObject.SetActive(false);
    //         },
    //         actionOnDestroy: (bullet) => {
    //             Destroy(bullet.gameObject);
    //         },
    //         collectionCheck: false,
    //         defaultCapacity: 10,
    //         maxSize: 200
    //     );
    // }
}
