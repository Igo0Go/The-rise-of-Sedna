using UnityEngine;

public class Decal : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float lifeTime = 1;
    [SerializeField]
    private AudioClip sound;

    void Awake()
    {
        AudioPack.audioSystem.PlaySoundInPoint(sound, transform.position);
        Destroy(gameObject, lifeTime);
    }
}
