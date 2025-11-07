using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSource soundSource2D;
    [SerializeField]
    private AudioSource soundSource3D;

    private void Awake()
    {
        AudioPack.audioSystem = this;
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource2D.PlayOneShot(clip);
    }
    public void PlaySoundInPoint(AudioClip clip, Vector3 point)
    {
        soundSource3D.transform.position = point;
        soundSource3D.PlayOneShot(clip);
    }
}

public static class AudioPack
{
    public static AudioSystem audioSystem;
}

[System.Serializable]
public class FootStepSystem
{
    [SerializeField, Min(1)]
    private float stepTargetValue = 1;

    [SerializeField]
    private AudioClip clip;

    private float stepValue = 0;

    public void ResetStepValue()
    { 
        stepValue = 0;
    }
    public void AddStepValue(float delta)
    {
        stepValue += delta;
        if (stepValue > stepTargetValue)
        {
            stepValue = 0;
        }
    }
    public void Step()
    {
        if(stepValue == 0)
        {
            AudioPack.audioSystem.PlaySound(clip);
        }
    }
}
