using UnityEngine;
using System.Collections.Generic;

public class BottleNeckActor : Actor
{
    [SerializeField]
    private List<Actor> actorsToActivation;

    public override void Activate()
    {
        Check();
    }

    private void Check()
    {
        foreach (var actor in actorsToActivation)
        {
            if(!actor.IsActive)
            {
                return;
            }
        }

        base.Activate();
    }
}
