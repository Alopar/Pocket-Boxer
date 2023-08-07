using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    [DefaultExecutionOrder(-5)]
    public class AudioManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private AudioListener _listener;
        #endregion

        #region FIELDS PRIVATE
        private static AudioManager _instance;

        private AudioSource _uiSource;
        private AudioSource _gameSource;
        private AudioSource _backgroundMusicSource;
        #endregion

        #region PROPERTIES
        public static AudioManager Instance => _instance;
        #endregion

        #region HANDLERS
        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "MoonSDKScene") return;

            if (_listener == null)
            {
                _listener = FindObjectOfType<AudioListener>();

                if (_listener == null)
                {
                    Debug.LogError("listener not found, audio will not be played!");
                    return;
                }
            }

            _uiSource = _listener.gameObject.AddComponent<AudioSource>();
            _uiSource.ignoreListenerPause = true;

            _gameSource = _listener.gameObject.AddComponent<AudioSource>();

            _backgroundMusicSource = _listener.gameObject.AddComponent<AudioSource>();
            _backgroundMusicSource.loop = true;
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneLoadedHandler;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion

        #region METHODS PUBLIC
        public void PlaySound(string name, SoundType type)
        {
            var clip = Resources.Load<AudioClip>(name);
            if(clip == null)
            {
                Debug.LogError("sound not found!");
                return;
            }

            EmitSound(clip, type);
        }

        public void PlaySound(AudioClip clip, SoundType type)
        {
            if (clip == null)
            {
                Debug.LogError("sound not defined!");
                return;
            }

            EmitSound(clip, type);
        }

        public void PlayMusic(string name)
        {
            var clip = Resources.Load<AudioClip>(name);
            if (clip == null)
            {
                Debug.LogError("music not found!");
                return;
            }

            EmitMusic(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogError("music not defined!");
                return;
            }

            EmitMusic(clip);
        }
        #endregion

        #region METHODS PRIVATE
        private void EmitSound(AudioClip clip, SoundType type)
        {
            switch (type)
            {
                case SoundType.Game:
                    _gameSource.PlayOneShot(clip);
                    break;
                case SoundType.UI:
                    _uiSource.PlayOneShot(clip);
                    break;
            }
        }

        private void EmitMusic(AudioClip clip)
        {
            _backgroundMusicSource.clip = clip;
            _backgroundMusicSource.Play();
        }
        #endregion
    }

    public enum SoundType : byte
    {
        UI = 0,
        Game = 1
    }
}
