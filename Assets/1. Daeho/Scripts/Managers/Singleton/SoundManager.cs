using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } = null;

    [SerializeField] AudioClip[] audio_clip;

    public enum SoundType
    {
        SoundEffect,
        BGM
    }
    private float _bgm_volume;

    public float bgm_volume
    {
        get
        {
            _bgm_volume = PlayerPrefs.GetFloat("bgm_volume", 0);
            return _bgm_volume;
        }
        set
        {
            _bgm_volume = value;
            PlayerPrefs.SetFloat("bgm_volume", _bgm_volume);
            foreach (var source in bgm_sources)
            {
                source.volume = _bgm_volume;
            }
        }
    }

    private float _sound_effect_volume;

    public float sound_effect_volume
    {
        get
        {
            _sound_effect_volume = PlayerPrefs.GetFloat("sound_effect_volume", 0);
            return _sound_effect_volume;
        }
        set
        {
            _sound_effect_volume = value;
            PlayerPrefs.SetFloat("sound_effect_volume", _sound_effect_volume);
            foreach (var source in sound_effect_sources)
            {
                source.volume = _sound_effect_volume;
            }
        }
    }

    [SerializeField] UnityEngine.UI.Slider sound_effect_slider;
    [SerializeField] UnityEngine.UI.Slider bgm_slider;

    List<AudioSource> sound_effect_sources = new List<AudioSource>();
    List<AudioSource> bgm_sources = new List<AudioSource>();

    Dictionary<string, AudioClip> audio_clips;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < audio_clip.Length; i++)
        {
            audio_clips.Add(audio_clip[i].name, audio_clip[i]);
        }

        sound_effect_slider.onValueChanged.AddListener((f) => { sound_effect_volume = f; });
        bgm_slider.onValueChanged.AddListener((f) => { bgm_volume = f; });
    }

    void Update()
    {
    }

    public void PlaySound(SoundType sound_type, string clip_name, bool loop = false)
    {
        List<AudioSource> sources = (sound_type == SoundType.SoundEffect ? sound_effect_sources : bgm_sources);

        foreach (var source in sources)
        {
            if (!source.isPlaying)
            {
                source.clip = audio_clips[clip_name];
                source.loop = loop;
                source.Play();
                return;
            }
        }

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        sources.Add(audioSource);

        audioSource.loop = loop;
        audioSource.volume = (sound_type == SoundType.SoundEffect ? sound_effect_volume : bgm_volume);
        audioSource.clip = audio_clips[clip_name];
        audioSource.Play();
    }
}
