using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private Image hpFill;

    private int _hp = 100;

    private int HP
    {
        get => _hp;
        set
        {
            hpFill.fillAmount = value / 100f;
            _hp = value;
        }
    } 
    public bool IsDead => _hp <= 0;

    public void Start()
    {
        _ = BubbleLineRefill(GridDefine.BossLeftLine);
        _ = BubbleLineRefill(GridDefine.BossRightLine);
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
    }

    public async UniTask BubbleLineRefill()
    {
        var (result1, result2) = await UniTask.WhenAll(
            BubbleLineRefill(GridDefine.BossLeftLine),
            BubbleLineRefill(GridDefine.BossRightLine)
        );
        await new WaitUntil(() => result1 && result2);
    }
    private static async UniTask<bool> BubbleLineRefill(Vector2Int[] line, float dur = 0.1f)
    {
        if (HexagonGrid.I.IsValid(line.Last()))
            return true;

        // var type = 0 == Random.Range(0, 8) ? BubbleType.Boom : Bubble.GetRandomBubbleType;
        var type = Bubble.GetRandomBubbleType;
        var bubble = HexagonGrid.I.SetBubble(null, line[0], type);
        if (0 == Random.Range(0, 4)) 
            bubble.SetAttackBubble();
        var count = 1;
        for (int i = line.Length - 2; i >= 0; i--)
        {
            if (false == HexagonGrid.I.IsValid(line[i])) 
                continue;
            ++count;
            if (i != 0)
                HexagonGrid.I.MoveCellBubble(line[i], line[i + 1], dur: dur);
            else
                HexagonGrid.I.MoveCellBubble(line[i], line[i + 1], () => _ = BubbleLineRefill(line), dur: dur);
        }

        await UniTask.Delay(count * (int)(dur * 1000));
        return true;
    }
}
