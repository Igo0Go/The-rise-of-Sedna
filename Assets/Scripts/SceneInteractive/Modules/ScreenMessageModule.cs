using System;
using UnityEngine;

public class ScreenMessageModule : InteractiveModule
{
    [SerializeField]
    private ScreenMessagePack pack;

    private event Action<ScreenMessagePack> MessageActivated;

    private void Awake()
    {
        MessageActivated += FindFirstObjectByType<ScreenMessageSystem>().ShowNewMessage;
    }

    public override void Activate()
    {
        base.Activate();
        MessageActivated?.Invoke(pack);
    }
}
