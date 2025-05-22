using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NamedAudioClip
{
    public string name;
    public AudioClip clip;
}

public class SoundLibrary : MonoBehaviour
{
    public static SoundLibrary Instance;

    [Header("Sound Clips")]
    public List<NamedAudioClip> clips;

    private Dictionary<string, AudioClip> clipDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        clipDictionary = new Dictionary<string, AudioClip>();
        foreach (var entry in clips)
        {
            if (!clipDictionary.ContainsKey(entry.name))
                clipDictionary.Add(entry.name, entry.clip);
        }
    }

    public AudioClip GetClip(string name)
    {
        if (clipDictionary.ContainsKey(name))
        {
            return clipDictionary[name];
        }
        else
        {
            Debug.LogWarning("Sound '" + name + "' not found in SoundLibrary!");
            return null;
        }
    }

}