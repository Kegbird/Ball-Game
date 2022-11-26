using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlayerAudio : MonoBehaviour, IPlayerAudioInterface
{
    private static IPlayerAudioInterface instance;
    private AudioSource m_audio_source;
    private Dictionary<SoundEnum, AudioClip> m_audio_clips;

    private void Awake()
    {
        instance = this;
        m_audio_source = GetComponent<AudioSource>();
        m_audio_clips = new Dictionary<SoundEnum, AudioClip>();
        m_audio_clips[SoundEnum.JUMP] = Resources.Load<AudioClip>(ResourceNames.JUMP_SFX_FILENAME);
        m_audio_clips[SoundEnum.WIN] = Resources.Load<AudioClip>(ResourceNames.WIN_SFX_FILENAME);
        m_audio_clips[SoundEnum.INVALID_MOVE] = Resources.Load<AudioClip>(ResourceNames.INVALID_MOVE_SFX_FILENAME);
        m_audio_clips[SoundEnum.VICTORY] = Resources.Load<AudioClip>(ResourceNames.VICTORY_SFX_FILENAME);
    }

    public static IPlayerAudioInterface GetInstance()
    {
        return instance;
    }

    public void PlaySound(SoundEnum sound_enum)
    {
        m_audio_source.clip = m_audio_clips[sound_enum];
        m_audio_source.Play();
    }
}
