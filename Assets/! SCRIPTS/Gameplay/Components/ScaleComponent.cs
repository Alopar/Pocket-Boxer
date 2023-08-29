using System.Collections;
using UnityEngine;
using NaughtyAttributes;

namespace Gameplay
{
    public class ScaleComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _view;
        [SerializeField, Range(0, 3)] private float _duration;
        [SerializeField, Range(0, 1)] private float _magnitude;
        [SerializeField] private AnimationCurve _curve;
        #endregion

        #region FIELDS PRIVATE
        #endregion

        #region UNITY CALLBACKS
        #endregion

        #region METHODS PUBLIC
        [Button("SCALE")]
        public void Scale()
        {
            StopAllCoroutines();
            StartCoroutine(Scaling(_duration, _magnitude));
        }
        #endregion

        #region COROUTINES
        IEnumerator Scaling(float duration, float magnitude)
        {
            var timer = 0f;
            while (timer <= duration)
            {
                var time = timer / duration;
                var value = _curve.Evaluate(time);
                var scale = magnitude * value;

                _view.localScale = Vector3.one + (Vector3.one * scale);

                timer += Time.deltaTime;
                yield return null;
            }
        }
        #endregion
    }
}
