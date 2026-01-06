using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume = 0.5f;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmHighPass;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume = 0.5f;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelsIndex;
    //Toutes les SFX possibles
    public enum SFX
    {
        Dead,
        Hit,
        LevelUp=3,
        Lose,
        Melee,
        Range=7,
        Select,
        Win
    }

    void Awake()
    {
        instance = this;
        Init();
    }


    void Init()
    {
        //init BGM player
        GameObject bgmObj = new GameObject("BGM Player");
        bgmObj.transform.parent = transform;
        bgmPlayer = bgmObj.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClip;
        bgmHighPass = Camera.main.GetComponent<AudioHighPassFilter>();

        //init SFX players
        GameObject sfxObj = new GameObject("SFX Player");
        sfxObj.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            
            sfxPlayers[i] = sfxObj.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;

        }
    }

    public void PlaySFX(SFX sfx) //Joue une SFX si celui-ci n'est pas déjà en cours de lecture
    {
        for (int i = 0; i < sfxPlayers.Length; i++) 
        {
            int loopIndex = (channelsIndex + i) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            int ranIndex = 0;
            if (sfx == SFX.Melee || sfx == SFX.Hit)
            {
                ranIndex = Random.Range(0, 2);
            }
           

            channelsIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayBGM(bool isPlay) //Démarre ou arrête la BGM
    {
        if (!isPlay)
        {
            bgmPlayer.Stop();
            
        }
        else
        {
            bgmPlayer.Play();
        }
    }
    public void HighPassBGM(bool isPlay) //Active ou désactive le filtre sur la BGM (étouffe)
    {
        bgmHighPass.enabled = isPlay;
    }

}
