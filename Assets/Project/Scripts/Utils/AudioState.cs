using System;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class AudioState
    {
        public AudioClip[] audios;
        public AudioClip Random() => audios[UnityEngine.Random.Range(0, audios.Length)];
    }
}