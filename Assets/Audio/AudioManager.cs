using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundEffect
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource sfxSource;
    public List<SoundEffect> sounds;

    void Awake() => instance = this;

    public void PlaySound(string soundName)
    {
        SoundEffect s = sounds.Find(x => x.name == soundName);
        if (s != null)
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
}