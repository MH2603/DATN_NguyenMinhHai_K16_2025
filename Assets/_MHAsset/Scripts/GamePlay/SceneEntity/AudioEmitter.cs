using System;
using UnityEngine;

namespace MH
{
    public class AudioEmitter : MonoBehaviour
    {
        [SerializeField] private bool playOnAwake = true;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            if(playOnAwake) 
            {
                Play();
            }
        }


        public void Play()
        {
            _audioSource.Play();
        }
    }
}