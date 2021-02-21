using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public enum FX
{
    bang1,
    glassbreak,
    factoryAmbient,
    forestAmbient,
    nightCricketsAmbient,
    rollingStone,
    woosh,
    stepGrass,
    stepFactory
}

public enum MUSIC
{
    Menu,
    Game,
}

public class SoundController : MonoBehaviour
{
    [Serializable]
    public class FXClip
    {
        public FX name;
        public List<AudioClip> clip = new List<AudioClip>();
    }

    [Serializable]
    public class MusicClip {
        public MUSIC name;
        public List<AudioClip> clip = new List<AudioClip>();
    }

    public static SoundController instance;
    [SerializeField] private int fxSourceCount;
    [SerializeField] private int musicSourceCount;

    [SerializeField] private List<FXClip> fxClips = new List<FXClip>();
    [SerializeField] private List<MusicClip> musicClips = new List<MusicClip>();

    [SerializeField] private GameObject sourceObjFX;
    [SerializeField] private GameObject sourceObjMusic;

    private List<AudioSource> sourcesFX = new List<AudioSource>();
    private List<AudioSource> sourcesMusic = new List<AudioSource>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Spawn audio sources
        for (int i = 0; i < fxSourceCount; i++)
        {
            GameObject source = Instantiate(sourceObjFX, transform);
            source.name = "FXSource";
            sourcesFX.Add(source.GetComponent<AudioSource>());
        }

        //Spawn audio sources
        for (int j = 0; j < musicSourceCount; j++) {
            GameObject source = Instantiate(sourceObjMusic, transform);
            source.name = "MusicSource";
            sourcesMusic.Add(source.GetComponent<AudioSource>());
        }
    }


    public void PlayFX(FX fx)
    {
        foreach (AudioSource s in sourcesFX)
        {
            if (!s.isPlaying)
            {
                s.PlayOneShot(FindClip(fx));
                return;
            }
        }
        Debug.LogError("NO AUDIOSOURCE AVAILABLE!");
    }

    public void PlayMusic(MUSIC music) {
        foreach (AudioSource s in sourcesMusic) {
            if (!s.isPlaying) {
                s.PlayOneShot(FindClip(music));
                return;
            }
        }
        Debug.LogError("NO AUDIOSOURCE AVAILABLE!");
    }

    private AudioClip FindClip(FX fx)
    {
        foreach (FXClip f in fxClips)
        {
            if (f.name == fx)
            {
                return f.clip[Random.Range(0, f.clip.Count-1)];
            }
        }
        Debug.LogError("NO CLIP FOUND");
        return null;
    }

    private AudioClip FindClip(MUSIC music) {
        foreach (MusicClip m in musicClips) {
            if (m.name == music) {
                return m.clip[Random.Range(0, m.clip.Count - 1)];
            }
        }
        Debug.LogError("NO CLIP FOUND");
        return null;
    }

}
