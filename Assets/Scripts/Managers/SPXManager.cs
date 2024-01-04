using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EffectClips
{
    Clear = 0,
    Failed,
    Watering,
    WaterWall,
    PlayerDamage,
    MonsterDamaged1,
    Sword1,
    Sword2,
    No
}

public class SPXManager : MonoBehaviour
{
    public static SPXManager instance = null;
    public AudioSource audioSource;
    public AudioClip[] effectSounds;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(int i)
    {
        audioSource.Stop();
        audioSource.clip = effectSounds[i];
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void PlayOneShot(int i)
    {
        audioSource.PlayOneShot(effectSounds[i]);
    }
}
