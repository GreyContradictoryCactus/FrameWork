using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager: MonoBehaviour
{
    #region 初始化

    private const int MAX_BACKGROUND_MUSIC_NUM = 1;
    private const int MAX_SOUNDEFFECT_NUM = 10;
    private const float PLAT_INTERVAL = 0.3f;

    public static Transform root = GameObject.Find("GlobalRoot").transform.Find("AudioRoot");

    private enum KindsOfSound
    {
        SoundEffect,
        BackgroundMusic,
        All
    }

    private class Channel
    {
        public AudioSource audioSource = null;
        public float lasePlayTime = -1;
    }
    private static List<Channel> backGroundChannels ;
    private static List<Channel> soundEffectChannels;

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        backGroundChannels = new List<Channel>();
        soundEffectChannels = new List<Channel>();

        for(int i=1;i<=MAX_BACKGROUND_MUSIC_NUM; i++)
        {
            backGroundChannels.Add(new Channel());
        }
        for(int i=1;i<=MAX_SOUNDEFFECT_NUM;i++)
        {
            soundEffectChannels.Add(new Channel());
        }

        foreach(Channel channel in backGroundChannels) 
        {
            channel.audioSource = root.gameObject.AddComponent<AudioSource>();
            channel.lasePlayTime = -1;
        }
        foreach (Channel channel in soundEffectChannels)
        {
            channel.audioSource = root.gameObject.AddComponent<AudioSource>();
            channel.lasePlayTime = -1;
        }
        LogManager.Info("[AudioManager]:Init","FrameworkManagerInit");
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 播放

    private static void PlaySound(Channel channel, AudioClip clip, float volume, float pan, float pitch, bool loop)
    {
        AudioSource audioSource = channel.audioSource;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.panStereo = pan;
        audioSource.loop = loop;
        audioSource.pitch = pitch;
        audioSource.Play();
        channel.lasePlayTime = Time.time;
    }
    public static void PlaySoundEffect(AudioClip clip,float volume = 1.0f,float pan = 0,float pitch = 1.0f,bool loop = false)
    {
        Channel findClip = FindChannel(clip, KindsOfSound.SoundEffect);

        //刚刚开始播放过
        if(findClip != null && findClip.audioSource.isPlaying && findClip.lasePlayTime > Time.time - PLAT_INTERVAL)
        {
            return;
        }

        Channel earliestChannel = soundEffectChannels[0];
        foreach(Channel channel in soundEffectChannels)
        {
            if(channel.audioSource.isPlaying && channel.lasePlayTime < earliestChannel.lasePlayTime)
            {
                earliestChannel = channel;
            }

            if(channel.audioSource.isPlaying == false) 
            {
                PlaySound(channel, clip, volume, pan, pitch, loop);
                return;
            }

        }
        //没有空闲的就顶替最早的
        PlaySound(earliestChannel, clip, volume, pan, pitch, loop);

    }
    public static void PlayBackgroundMusic(AudioClip clip, float volume = 1.0f, float pan = 0, float pitch = 1.0f, bool loop = true)
    {
        Channel findClip = FindChannel(clip, KindsOfSound.BackgroundMusic);

        //刚刚开始播放过
        if (findClip != null && findClip.audioSource.isPlaying && findClip.lasePlayTime > Time.time - PLAT_INTERVAL)
        {
            return;
        }

        Channel earliestChannel = backGroundChannels[0];
        foreach (Channel channel in backGroundChannels)
        {
            if (channel.audioSource.isPlaying && channel.lasePlayTime < earliestChannel.lasePlayTime)
            {
                earliestChannel = channel;
            }

            if (channel.audioSource.isPlaying == false)
            {
                PlaySound(channel, clip, volume, pan, pitch, loop);
                return;
            }

        }
        //没有空闲的就顶替最早的
        PlaySound(earliestChannel, clip, volume, pan, pitch, loop);

    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 停止播放
    public static void StopSoundEffect(AudioClip clip)
    {
        Channel channel = FindChannel(clip, KindsOfSound.SoundEffect);
        channel.audioSource.Stop();
    }
    public static void StopBackgroundMusic(AudioClip clip)
    {
        Channel channel = FindChannel(clip, KindsOfSound.BackgroundMusic);
        channel.audioSource.Stop();
    }

    public static void StopAllSoundEffect()
    {
        foreach(Channel channel in soundEffectChannels)
        {
            channel.audioSource.Stop();
        }
    }
    public static void StopAllBackgroundMusic()
    {
        foreach (Channel channel in backGroundChannels)
        {
            channel.audioSource.Stop();
        }
    }
    public static void StopAll()
    {
        StopAllBackgroundMusic();
        StopAllSoundEffect();
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 查找音频频道
    private static Channel FindChannel(AudioClip clip,KindsOfSound whichChannels)
    {
        if (whichChannels == KindsOfSound.BackgroundMusic)
        { 
            foreach (Channel channel in backGroundChannels)
            {
                if (channel.audioSource.clip == clip)
                {
                    return channel;
                }
            }
        }
        if(whichChannels == KindsOfSound.SoundEffect)
        {
            foreach (Channel channel in soundEffectChannels)
            {
                if (channel.audioSource.clip == clip)
                {
                    return channel;
                }
            }
        }
        if (whichChannels == KindsOfSound.All)
        {
            foreach (Channel channel in backGroundChannels)
            {
                if (channel.audioSource.clip == clip)
                {
                    return channel;
                }
            }
            foreach (Channel channel in soundEffectChannels)
            {
                if (channel.audioSource.clip == clip)
                {
                    return channel;
                }
            }
        }
        return null;

        
    }
    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
}
