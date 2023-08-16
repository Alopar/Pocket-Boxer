using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventHolder;

namespace Gameplay.Managers
{
    [DefaultExecutionOrder(-5)]
    public class GameManager : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private const string LEVEL_COUNTER = "LEVEL-COUNTER";
        private const string LAST_LOCATION_NUMBER = "LAST-LOCATION-NUMBER";

        private static GameManager _instance;
        #endregion

        #region PROPERTIES
        public static GameManager Instance => _instance;
        #endregion

        #region HANDLERS
        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
        {

        }

        [EventHolder]
        private void LevelStart(LevelStartInfo info)
        {
            SendLocationStartInfo(info.Number + 1);
        }

        [EventHolder]
        private void LevelEnd(LevelEndInfo info)
        {
            SendLocationEndInfo(info.Number + 1);
        }
        #endregion

        #region UNITY CALLBACKS
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

        private void OnEnable()
        {
            SubscribeService.SubscribeListener(this);
            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        private void OnDisable()
        {
            SubscribeService.UnsubscribeListener(this);
            SceneManager.sceneLoaded -= SceneLoadedHandler;
        }

        private void Start()
        {
            Init();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            StartCoroutine(LevelTracker());
        }

        private void SendLocationStartInfo(int index)
        {
            var currentLocationNumber = index;

            var lastLocationNumber = 0;
            if (PlayerPrefs.HasKey(LAST_LOCATION_NUMBER))
            {
                lastLocationNumber = PlayerPrefs.GetInt(LAST_LOCATION_NUMBER);
            }

            if(currentLocationNumber > lastLocationNumber)
            {
                //HoopslyIntegration.LocationStartEvent(currentLocationNumber.ToString());
                PlayerPrefs.SetInt(LAST_LOCATION_NUMBER, currentLocationNumber);
                PlayerPrefs.Save();
            }
        }

        private void SendLocationEndInfo(int index)
        {
            //HoopslyIntegration.LocationEndEvent(index.ToString(), LevelFinishedResult.win);
        }
        #endregion

        #region COROUTINES
        IEnumerator LevelTracker()
        {
            var levelCounter = 1;
            if (PlayerPrefs.HasKey(LEVEL_COUNTER))
            {
                levelCounter = PlayerPrefs.GetInt(LEVEL_COUNTER);
            }
            
            while (true)
            {
                //HoopslyIntegration.RaiseLevelStartEvent(levelCounter.ToString());

                yield return new WaitForSeconds(60f);

                //HoopslyIntegration.RaiseLevelFinishedEvent(levelCounter.ToString(), LevelFinishedResult.win);

                levelCounter++;
                PlayerPrefs.SetInt(LEVEL_COUNTER, levelCounter);
                PlayerPrefs.Save();
            }
        }
        #endregion
    }
}
