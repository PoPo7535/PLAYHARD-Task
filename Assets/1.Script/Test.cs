using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    public HexagonGrid hexList;
    void Start()
    {
        hexList.AddHexLine(10);
    }
}
