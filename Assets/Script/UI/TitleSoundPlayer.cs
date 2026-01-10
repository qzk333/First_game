using UnityEngine;

public class TitleSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sideSwordClip;  // 侧边剑的声音
    public AudioClip mainSwordClip;  // 中间剑的声音
    public AudioClip fireClip;       // 火焰声音

    // 这些函数会被动画事件调用
    public void PlaySideSwordSound()
    {
        audioSource.PlayOneShot(sideSwordClip);
    }

    public void PlayMainSwordSound()
    {
        audioSource.PlayOneShot(mainSwordClip);
    }

    public void PlayFireSound()
    {
        audioSource.PlayOneShot(fireClip);
    }
}