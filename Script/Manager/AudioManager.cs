using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource[] sfx; //Sound effects
    [SerializeField] private AudioSource[] bgm; //background music

    public bool playBgm;
    private int bgmIndex;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    public void PlaySFX(int _sfxIndex)
    {
        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;
        StopAllBGM();
        bgm[bgmIndex].Play();
    }


    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

}
