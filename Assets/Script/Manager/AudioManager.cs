using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [SerializeField] private float sfxMinimumDistance;
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

    /*
    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        if (sfx[_sfxIndex].isPlaying)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.15f);
            sfx[_sfxIndex].Play();
        }
    }*/
    public void PlaySFX(int _sfxIndex, Transform _source, bool _forcePlay = false)
    {
        if (_sfxIndex >= sfx.Length) return;

        // 距离检查逻辑保持不变
        if (_source != null && Vector2.Distance(PlayerManager.instance.transform.position, _source.position) > sfxMinimumDistance)
            return;

        AudioSource source = sfx[_sfxIndex];

        if (_forcePlay)
        {
            // 如果强制播放（用于攻击）：先停再播，确保瞬间响应
            source.Stop();
        }
        else
        {
            // 如果不是强制播放（用于走路、跳跃）：如果正在播，就跳过
            if (source.isPlaying) return;
        }

        if (_sfxIndex == 9)
        {
            source.pitch = 1f; // 强制回归正常音高
        }

        else
        {
            source.pitch = Random.Range(.85f, 1.15f); // 其他音效随机化
        }

        source.Play();
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
