using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Gameplay
{
    [SelectionBase]
    public class RelaxerController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _id;

        [Space(10)]
        [SerializeField, Range(0, 60)] private float _duration;
        [SerializeField, Range(0, 1000)] private uint _energy;

        [Space(10)]
        [SerializeField] private CinemachineVirtualCamera _camera;

        [Space(10)]
        [SerializeField] private Transform _dollPoint;
        [SerializeField] private CharacterAnimation _dollAnimation;

        [Space(10)]
        [SerializeField] private ParticleSystem _sleepVFX;
        #endregion

        #region FIELDS PRIVATE
        private float _progress;
        private int _energonCounter = 0;
        private Manikin _manikin;
        private BatteryComponent _userBattery;
        #endregion

        #region EVENTS
        public event Action<float> OnTimerChange;
        public event Action<float> OnProgressChange;
        public event Action OnExploitationEnd;
        #endregion

        #region METHODS PRIVATE
        private float NextProgressPoint(int points, int pointCounter)
        {
            return (100f / points) * pointCounter + 1;
        }
        #endregion

        #region METHODS PUBLIC
        public void TurnOn()
        {
            _progress = 0;
            _energonCounter = 0;
            _camera.Priority = 10;

            _manikin?.Activate(_dollAnimation);
            StartCoroutine(Exploitation(_duration));

            _sleepVFX.Play();
        }

        public void TurnOff()
        {
            _camera.Priority = 0;

            RemoveDoll();
            StopAllCoroutines();

            _sleepVFX.Stop();
        }

        public void AddProgress(float value)
        {
            _progress += value;
            _progress = Mathf.Clamp(_progress, 0, 100f);
            OnProgressChange?.Invoke(_progress / 100f);

            if(_progress >= NextProgressPoint(100, _energonCounter))
            {
                _energonCounter++;
                _userBattery.TrySetEnergy(_energy / 100);
            }

            if(_progress == 100f)
            {
                OnExploitationEnd?.Invoke();
            }
        }

        public void SetDoll(GameObject doll)
        {
            _manikin = new Manikin(doll, _dollPoint);
        }

        public void RemoveDoll()
        {
            _manikin?.Dispose();
            _manikin = null;
        }

        public void SetUserBattety(BatteryComponent battery)
        {
            _userBattery = battery;
        }
        #endregion

        #region COROUTINES
        private IEnumerator Exploitation(float duration)
        {
            var timer = duration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                OnTimerChange?.Invoke(timer);

                var delta = Time.deltaTime / duration;
                AddProgress(100f * delta);

                yield return null;
            }

            OnExploitationEnd?.Invoke();
        }
        #endregion
    }
}
