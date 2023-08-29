using System;
using System.Threading.Tasks;
using UnityEngine;
using EventHolder;

namespace Utility
{
    public class GameTracker
    {
        #region FIELDS PRIVATE
        private const float LEVEL_DURATION = 10f;
        private const string LEVEL_COUNTER = "LEVEL-COUNTER";
        private const string LAST_LOCATION_NUMBER = "LAST-LOCATION-NUMBER";
        #endregion

        #region CONSTRUCTORS
        public GameTracker()
        {
            //LevelTimer(LEVEL_DURATION);
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
        private async void LevelTimer(float delay)
        {
            var levelCounter = 1;
            if (PlayerPrefs.HasKey(LEVEL_COUNTER))
            {
                levelCounter = PlayerPrefs.GetInt(LEVEL_COUNTER);
            }
            
            while (true)
            {
                Debug.Log(levelCounter);
                //HoopslyIntegration.RaiseLevelStartEvent(levelCounter.ToString());

                await Task.Delay((int)(delay * 1000f));

                //HoopslyIntegration.RaiseLevelFinishedEvent(levelCounter.ToString(), LevelFinishedResult.win);

                levelCounter++;
                
                PlayerPrefs.SetInt(LEVEL_COUNTER, levelCounter);
                PlayerPrefs.Save();
            }
        }
        #endregion
    }
}
