using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSource soundSource2D;
    [SerializeField]
    private AudioSource soundSource3D;
    [SerializeField]
    private AudioSource musicSource;

    private void Awake()
    {
        AudioPack.audioSystem = this;
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource2D.PlayOneShot(clip);
    }
    public void PlaySoundInPoint(AudioClip clip, Vector3 point, float soundRange)
    {
        soundSource3D.transform.position = point;
        soundSource3D.PlayOneShot(clip);
        InvokeSoundEvent(point, soundRange);
    }
    public void InvokeSoundEvent(Vector3 point, float soundRange)
    {
        SoundEventInPoint?.Invoke(point, soundRange);
    }

    public event Action<Vector3, float> SoundEventInPoint;
}

public static class AudioPack
{
    public static AudioSystem audioSystem;
}

[System.Serializable]
public class FootStepSystem
{
    [SerializeField]
    private Transform stepMarker;
    [SerializeField, Min(1)]
    private float stepTargetValue = 1;

    [SerializeField]
    List<SoundItem> stepSounds;

    private float stepValue = 0;

    public void ResetStepValue()
    { 
        stepValue = 0;
        stepMarker.transform.localScale = Vector3.one * 0.01f;
    }
    public void AddStepValue(float delta)
    {
        stepValue += delta;
        if (stepValue > stepTargetValue)
        {
            stepValue = 0;
        }
    }
    public void Step(PlayerSoundType type, Transform player)
    {
        if(stepValue == 0)
        {
            SoundItem soundItem = GetSoundItem(type);
            AudioPack.audioSystem.PlaySound(soundItem.clip);
            AudioPack.audioSystem.InvokeSoundEvent(player.position, soundItem.range);
            stepMarker.transform.localScale = Vector3.one * soundItem.range / 2;
        }
    }

    private SoundItem GetSoundItem(PlayerSoundType stepSoundType)
    {
        return stepSounds.Find(s => s.type == stepSoundType);
    }
}

[Serializable]
public class SoundItem
{
    public PlayerSoundType type;
    public AudioClip clip;
    public float range;
}

public enum PlayerSoundType
{
    simpleMove,
    runMove,
    crouchMove
}
