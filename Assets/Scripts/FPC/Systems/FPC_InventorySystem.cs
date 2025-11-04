using UnityEngine;
using System;

public class FPC_InventorySystem : MonoBehaviour
{
    [SerializeField]
    private InventaryDB db;

    private void Awake()
    {
        InventarySystem.Instance.SetDb(db);
    }
}
