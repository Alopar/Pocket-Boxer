using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class TrademillAnimation : AbstaractSimulatorAnimation
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(0, 10)] private float _speed;
        [SerializeField] private MeshRenderer _meshRenderer;
        #endregion

        #region METHODS PUBLIC
        public override void TurnOn()
        {
            StartCoroutine(MoveRoad());
        }

        public override void TurnOff()
        {
            StopAllCoroutines();
        }
        #endregion

        #region COROUTINES
        private IEnumerator MoveRoad()
        {
            var material = _meshRenderer.materials[4];
            material.SetTextureOffset("_BaseMap", Vector2.zero);
            while (true)
            {
                var offset = material.GetTextureOffset("_BaseMap");
                offset.y -= Time.deltaTime * _speed;
                material.SetTextureOffset("_BaseMap", offset);

                yield return null;
            }
        }
        #endregion
    }
}
