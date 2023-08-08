using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.AudioSystem
{
    public class AudioSystem : IAudioService
    {
        #region FIELDS PRIVATE
        private AudioSource _uiSource;
        private AudioSource _gameSource;
        private AudioSource _backgroundMusicSource;
        #endregion

        #region CONSTRUCTORS
        public AudioSystem()
        {
            SceneManager.sceneLoaded += SceneLoadedHandler;
        }
        #endregion

        #region HANDLERS
        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
        {
            var listener = GameObject.FindObjectOfType<AudioListener>();
            if (listener == null)
            {
                Debug.LogError("listener not found, audio will not be played!");
                return;
            }

            _uiSource = listener.gameObject.AddComponent<AudioSource>();
            _uiSource.ignoreListenerPause = true;

            _gameSource = listener.gameObject.AddComponent<AudioSource>();

            _backgroundMusicSource = listener.gameObject.AddComponent<AudioSource>();
            _backgroundMusicSource.loop = true;
        }
        #endregion

        #region METHODS PUBLIC
        public void PlaySound(string name, SoundType type, float volume = 1f)
        {
            var clip = Resources.Load<AudioClip>(name);
            if (clip == null)
            {
                Debug.LogError("sound not found!");
                return;
            }

            EmitSound(clip, type, volume);
        }

        public void PlaySound(AudioClip clip, SoundType type, float volume = 1f)
        {
            if (clip == null)
            {
                Debug.LogError("sound not defined!");
                return;
            }

            EmitSound(clip, type, volume);
        }

        public void PlayMusic(string name, float volume = 1f)
        {
            var clip = Resources.Load<AudioClip>(name);
            if (clip == null)
            {
                Debug.LogError("music not found!");
                return;
            }

            EmitMusic(clip, volume);
        }

        public void PlayMusic(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
            {
                Debug.LogError("music not defined!");
                return;
            }

            EmitMusic(clip, volume);
        }
        #endregion

        #region METHODS PRIVATE
        private void EmitSound(AudioClip clip, SoundType type, float volume = 1f)
        {
            switch (type)
            {
                case SoundType.Game:
                    _gameSource.PlayOneShot(clip, volume);
                    break;
                case SoundType.UI:
                    _uiSource.PlayOneShot(clip, volume);
                    break;
            }
        }

        private void EmitMusic(AudioClip clip, float volume = 1f)
        {
            _backgroundMusicSource.clip = clip;
            _backgroundMusicSource.volume = volume;
            _backgroundMusicSource.Play();
        }
        #endregion
    }
}
