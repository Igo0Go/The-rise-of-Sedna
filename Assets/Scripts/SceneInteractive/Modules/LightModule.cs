using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LightModule : InteractiveModule
{
    [SerializeField]
    private List<Light> targetLight;
    [SerializeField]
    private float targetIntensity;

    public override void Activate()
    {
        base.Activate();
        StopAllCoroutines();
        StartCoroutine(SetStateForAllLight(true));
    }

    public override void Deactivate()
    {
        base.Deactivate();
        StopAllCoroutines();
        StartCoroutine(SetStateForAllLight(false));
    }

    private IEnumerator SetStateForAllLight(bool value)
    {
        float t = 0;

        List<(float start, float end)> swithcValues = new List<(float start, float end)> ();

        foreach (Light light in targetLight)
        {
            if (value)
            {
                light.enabled = true;
            }
            swithcValues.Add((light.intensity, value? targetIntensity : 0));
        }

        while(t < 1)
        {
            t += Time.deltaTime;
            for (int i = 0; i < swithcValues.Count; i++)
            {
                targetLight[i].intensity = Mathf.Lerp(swithcValues[i].start, swithcValues[i].end, t);
            }
            yield return null;
        }

        if(!value)
        {
            foreach (Light light in targetLight)
            {
                if (value)
                {
                    light.enabled = false;
                }
            }
        }
    }
}
