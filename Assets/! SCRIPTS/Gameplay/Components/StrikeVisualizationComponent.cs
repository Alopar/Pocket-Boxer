using UnityEngine;
using Tools;

namespace Gameplay
{
    public class StrikeVisualizationComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _headPoint;
        [SerializeField] private Transform _rightHandPoint;
        [SerializeField] private Transform _leftHandPoint;
        [SerializeField] private Transform _rightFootPoint;
        [SerializeField] private Transform _leftFootPoint;

        [Space(10)]
        [SerializeField] private ParticleSystem _hitVfxPrefab;

        [Space(10)]
        [SerializeField] private AnimationEventTransmitter _animationTransmitter;
        #endregion

        #region HANDLERS
        private void AnimationStrike(byte index, string target)
        {
            Transform strikePoint = null;
            switch(target)
            {
                case "HE":
                    strikePoint = _headPoint;
                    break;
                case "RH":
                    strikePoint = _rightHandPoint;
                    break;
                case "LH":
                    strikePoint = _leftHandPoint;
                    break;
                case "RF":
                    strikePoint = _rightFootPoint;
                    break;
                case "LF":
                    strikePoint = _leftFootPoint;
                    break;
            }

            Instantiate(_hitVfxPrefab, strikePoint.position, Quaternion.identity);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            _animationTransmitter.AnimationStringEvent01 += AnimationStrike;
        }

        private void OnDisable()
        {
            _animationTransmitter.AnimationStringEvent01 -= AnimationStrike; 
        }
        #endregion
    }
}
