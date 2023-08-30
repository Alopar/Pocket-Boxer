using UnityEngine;
using EventHolder;
using DG.Tweening;
using PP = UnityEngine.PlayerPrefs;

namespace Utility
{
    public class GameTracker
    {
        #region FIELDS PRIVATE
        private const float LEVEL_DURATION = 10f;
        private const string LEVEL_COUNTER = "LEVEL-COUNTER";
        private const string LAST_LOCATION_NUMBER = "LAST-LOCATION-NUMBER";

        private int _levelCounter;
        #endregion

        #region CONSTRUCTORS
        public GameTracker()
        {
            StartLevelTracker(LEVEL_DURATION);
            SubscribeService.SubscribeListener(this);
        }
        #endregion

        #region HANDLERS
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

        #region METHODS PRIVATE
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
        private void StartLevelTracker(float delay)
        {
            _levelCounter = PP.HasKey(LEVEL_COUNTER) ? PP.GetInt(LEVEL_COUNTER) : 1;

            var before = new TweenCallback(() => {
                Debug.Log(_levelCounter);
                //HoopslyIntegration.RaiseLevelStartEvent(levelCounter.ToString());
            });

            var after = new TweenCallback(() => {
                //HoopslyIntegration.RaiseLevelFinishedEvent(levelCounter.ToString(), LevelFinishedResult.win);

                _levelCounter++;
                PP.SetInt(LEVEL_COUNTER, _levelCounter);
                PP.Save();
            });

            DOVirtual.DelayedCall(delay, before).OnComplete(after).SetLoops(-1);
        }
        #endregion
    }
}
