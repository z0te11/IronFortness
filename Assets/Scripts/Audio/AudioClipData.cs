// AudioClipData.cs
using UnityEngine;

[System.Serializable]
public class AudioClipData
{
    public string clipName;
    public AudioClip audioClip;
    [Range(0f, 1f)]
    public float volume = 1f;
    public bool loop = false;
}
