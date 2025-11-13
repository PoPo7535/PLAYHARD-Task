using UnityEngine;

public class Test : MonoBehaviour
{
    public Grid grid;
    void Start()
    {
        grid.GetCellCenterLocal(Vector3Int.zero);
    }

    void Update()
    {
        
    }
}
