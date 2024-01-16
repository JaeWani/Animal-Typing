using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundInfo
    {
        public string Name;
        public AudioClip Clip;
    }

    public static SoundManager instance { get; private set; }

    public List<SoundInfo> soundInfos = new List<SoundInfo>();
    public Dictionary<string, AudioClip> soundInfoDictionary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        foreach (var item in soundInfos) soundInfoDictionary.Add(item.Name, item.Clip);
    }

    private void _PlaySound(string key, float volume, bool isLoop)
    {
        var obj = new GameObject(name = key);

        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.parent = transform;

        var soundObj = obj.AddComponent<AudioSource>();

        soundObj.clip = soundInfoDictionary[key];
        soundObj.volume = volume;
        soundObj.loop = isLoop;
        soundObj.Play();
    }

    public static void PlaySound(string key, float volume, bool isLoop) => instance._PlaySound(key, volume, isLoop);
}
