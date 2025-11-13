using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Grid grid;
    void Start()
    {
        grid.GetCellCenterLocal(Vector3Int.zero);
    }

    [Button]
    public void Foo(Vector3Int vector3Int)
    {
        Debug.Log(grid.GetCellCenterLocal(vector3Int));
        Debug.Log(grid.GetCellCenterWorld(vector3Int));
    }
}
