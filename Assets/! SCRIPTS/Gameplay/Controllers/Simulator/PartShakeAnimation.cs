using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class PartShakeAnimation : AbstaractSimulatorAnimation
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(0, 10)] private float _shakeDuration;
        [SerializeField, Range(0, 10)] private float _shakeStrength;
        [SerializeField] private GameObject _animationPart;
        #endregion

        #region METHODS PUBLIC
        public override void TurnOn()
        {
            // N/A
        }

        public override void TurnOff()
        {
            // N/A
        }

        public override void PlayShot()
        {
            _animationPart.transform.DOShakePosition(_shakeDuration, _shakeStrength);
        }
        #endregion
    }
}
