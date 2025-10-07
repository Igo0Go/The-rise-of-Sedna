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
