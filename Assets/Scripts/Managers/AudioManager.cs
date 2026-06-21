// AudioManager.cs
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Настройки громкости")]
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 0.8f;
    [Range(0f, 1f)]
    [SerializeField] private float _uiVolume = 0.8f;

    [Header("Аудио клипы")]
    [SerializeField] private AudioClipData[] _musicClips;
    [SerializeField] private AudioClipData[] _sfxClips;
    [SerializeField] private AudioClipData[] _uiClips;

    private AudioSource _musicSource;
    private List<AudioSource> _sfxSources = new List<AudioSource>();
    private List<AudioSource> _uiSources = new List<AudioSource>();

    private Dictionary<string, AudioClipData> _musicDict = new Dictionary<string, AudioClipData>();
    private Dictionary<string, AudioClipData> _sfxDict = new Dictionary<string, AudioClipData>();
    private Dictionary<string, AudioClipData> _uiDict = new Dictionary<string, AudioClipData>();

    private const int MAX_SFX_SOURCES = 10;
    private const int MAX_UI_SOURCES = 5;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeAudioSources();
        InitializeAudioDictionaries();
    }

    private void InitializeAudioSources()
    {
        // Создаём источник для музыки
        GameObject musicObj = new GameObject("MusicSource");
        musicObj.transform.parent = transform;
        _musicSource = musicObj.AddComponent<AudioSource>();
        _musicSource.volume = _musicVolume;

        // Создаём источники для SFX
        for (int i = 0; i < MAX_SFX_SOURCES; i++)
        {
            GameObject sfxObj = new GameObject($"SFXSource_{i}");
            sfxObj.transform.parent = transform;
            AudioSource sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.volume = _sfxVolume;
            _sfxSources.Add(sfxSource);
        }

        // Создаём источники для UI звуков
        for (int i = 0; i < MAX_UI_SOURCES; i++)
        {
            GameObject uiObj = new GameObject($"UISource_{i}");
            uiObj.transform.parent = transform;
            AudioSource uiSource = uiObj.AddComponent<AudioSource>();
            uiSource.volume = _uiVolume;
            _uiSources.Add(uiSource);
        }

        Debug.Log("AudioManager инициализирован");
    }

    private void InitializeAudioDictionaries()
    {
        foreach (var clip in _musicClips)
        {
            _musicDict[clip.clipName] = clip;
        }

        foreach (var clip in _sfxClips)
        {
            _sfxDict[clip.clipName] = clip;
        }

        foreach (var clip in _uiClips)
        {
            _uiDict[clip.clipName] = clip;
        }
    }

    /// <summary>
    /// Проигрывает музыку с затуханием предыдущей
    /// </summary>
    public void PlayMusic(string clipName, float fadeDuration = 0.5f)
    {
        if (!_musicDict.ContainsKey(clipName))
        {
            Debug.LogWarning($"Музыка '{clipName}' не найдена!");
            return;
        }

        AudioClipData clipData = _musicDict[clipName];
        
        if (_musicSource.isPlaying)
        {
            StartCoroutine(FadeOutAndPlayMusic(_musicSource, clipData, fadeDuration));
        }
        else
        {
            _musicSource.clip = clipData.audioClip;
            _musicSource.loop = clipData.loop;
            _musicSource.volume = clipData.volume * _musicVolume;
            _musicSource.Play();
            Debug.Log($"Проигрывается музыка: {clipName}");
        }
    }

    /// <summary>
    /// Останавливает музыку с затуханием
    /// </summary>
    public void StopMusic(float fadeDuration = 0.5f)
    {
        if (_musicSource.isPlaying)
        {
            StartCoroutine(FadeOutMusic(_musicSource, fadeDuration));
        }
    }

    /// <summary>
    /// Проигрывает звуковой эффект
    /// </summary>
    public void PlaySFX(string clipName)
    {
        if (!_sfxDict.ContainsKey(clipName))
        {
            Debug.LogWarning($"SFX '{clipName}' не найден!");
            return;
        }

        AudioClipData clipData = _sfxDict[clipName];
        AudioSource availableSource = GetAvailableAudioSource(_sfxSources);

        if (availableSource != null)
        {
            availableSource.clip = clipData.audioClip;
            availableSource.loop = clipData.loop;
            availableSource.volume = clipData.volume * _sfxVolume;
            availableSource.PlayOneShot(clipData.audioClip, clipData.volume * _sfxVolume);
            Debug.Log($"Проигрывается SFX: {clipName}");
        }
        else
        {
            Debug.LogWarning("Нет доступных источников звука для SFX!");
        }
    }

    /// <summary>
    /// Проигрывает UI звук
    /// </summary>
    public void PlayUISound(string clipName)
    {
        if (!_uiDict.ContainsKey(clipName))
        {
            Debug.LogWarning($"UI звук '{clipName}' не найден!");
            return;
        }

        AudioClipData clipData = _uiDict[clipName];
        AudioSource availableSource = GetAvailableAudioSource(_uiSources);

        if (availableSource != null)
        {
            availableSource.clip = clipData.audioClip;
            availableSource.loop = clipData.loop;
            availableSource.volume = clipData.volume * _uiVolume;
            availableSource.PlayOneShot(clipData.audioClip, clipData.volume * _uiVolume);
            Debug.Log($"Проигрывается UI звук: {clipName}");
        }
        else
        {
            Debug.LogWarning("Нет доступных источников звука для UI!");
        }
    }

    /// <summary>
    /// Получает доступный источник звука
    /// </summary>
    private AudioSource GetAvailableAudioSource(List<AudioSource> sources)
    {
        foreach (var source in sources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // Если нет свободного, переиспользуем первый
        return sources.Count > 0 ? sources[0] : null;
    }

    /// <summary>
    /// Корутина для затухания и проигрывания новой музыки
    /// </summary>
    private System.Collections.IEnumerator FadeOutAndPlayMusic(AudioSource source, AudioClipData clipData, float duration)
    {
        float startVolume = source.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        source.Stop();
        source.clip = clipData.audioClip;
        source.loop = clipData.loop;
        source.volume = clipData.volume * _musicVolume;
        source.Play();

        Debug.Log($"Включена музыка: {clipData.clipName}");
    }

    /// <summary>
    /// Корутина для затухания музыки
    /// </summary>
    private System.Collections.IEnumerator FadeOutMusic(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        source.Stop();
    }

    /// <summary>
    /// Установить громкость музыки
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        _musicSource.volume = _musicVolume;
    }

    /// <summary>
    /// Установить громкость SFX
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        foreach (var source in _sfxSources)
        {
            source.volume = _sfxVolume;
        }
    }

    /// <summary>
    /// Установить громкость UI
    /// </summary>
    public void SetUIVolume(float volume)
    {
        _uiVolume = Mathf.Clamp01(volume);
        foreach (var source in _uiSources)
        {
            source.volume = _uiVolume;
        }
    }

    public float GetMusicVolume() => _musicVolume;
    public float GetSFXVolume() => _sfxVolume;
    public float GetUIVolume() => _uiVolume;

    /// <summary>
    /// Остановить всю музыку и звуки
    /// </summary>
    public void StopAll()
    {
        _musicSource.Stop();
        foreach (var source in _sfxSources)
        {
            source.Stop();
        }
        foreach (var source in _uiSources)
        {
            source.Stop();
        }
    }
}
