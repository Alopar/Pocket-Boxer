using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

namespace Gameplay
{
    public class Blinker : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(0, 10)] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        #endregion

        #region FIELDS PRIVATE
        private List<Renderer> _skins = new();
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _skins = GetComponentsInChildren<Renderer>().ToList();
        }
        #endregion

        #region METHODS PUBLIC
        [Button("BLINK")]
        public void Blink()
        {
            StopAllCoroutines();
            StartCoroutine(Blinking(_duration));
        }
        #endregion

        #region COROUTINES
        IEnumerator Blinking(float duration)
        {
            var timer = 0f;
            while (timer <= duration)
            {
                var time = timer / duration;
                var value = _curve.Evaluate(time);
                var color = new Color(value, value, value, 1f);
                foreach (var skin in _skins)
                {
                    var materials = new List<Material>();
                    skin.GetMaterials(materials);
                    foreach (var material in materials)
                    {
                        material.SetColor("_EmissionColor", color);
                    }
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }
        #endregion
    }
}
