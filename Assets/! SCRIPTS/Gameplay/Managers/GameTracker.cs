using EventHolder;
using DG.Tweening;
using PP = UnityEngine.PlayerPrefs;

namespace Gameplay
{
    public class GameTracker
    {
        #region FIELDS PRIVATE
        private const float LEVEL_DURATION = 60f;
        private const string LEVEL_COUNTER = "LEVEL-COUNTER";
        private const string LAST_LOCATION_NUMBER = "LAST-LOCATION-NUMBER";
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
            SendLocationStartInfo(info.Number);
        }

        [EventHolder]
        private void LevelEnd(LevelEndInfo info)
        {
            SendLocationEndInfo(info.Number);
        }
        #endregion

        #region METHODS PRIVATE
        private void StartLevelTracker(float delay)
        {
            var counter = PP.HasKey(LEVEL_COUNTER) ? PP.GetInt(LEVEL_COUNTER) : 1;
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
            {
                //HoopslyIntegration.RaiseLevelStartEvent(counter.ToString());
            });
            sequence.AppendInterval(delay);
            sequence.AppendCallback(() =>
            {
                //HoopslyIntegration.RaiseLevelFinishedEvent(counter.ToString(), LevelFinishedResult.win);

                counter++;
                PP.SetInt(LEVEL_COUNTER, counter);
                PP.Save();
            });
            sequence.SetLoops(-1);
            sequence.Play();
        }

        private void SendLocationStartInfo(int number)
        {
            var currentNumber = number;
            var lastNumber = PP.HasKey(LAST_LOCATION_NUMBER) ? PP.GetInt(LAST_LOCATION_NUMBER) : 0;
            if(currentNumber > lastNumber)
            {
                //HoopslyIntegration.LocationStartEvent(currentNumber.ToString());
                PP.SetInt(LAST_LOCATION_NUMBER, currentNumber);
                PP.Save();
            }
        }

        private void SendLocationEndInfo(int number)
        {
            //HoopslyIntegration.LocationEndEvent(number.ToString(), LevelFinishedResult.win);
        }
        #endregion
    }
}
