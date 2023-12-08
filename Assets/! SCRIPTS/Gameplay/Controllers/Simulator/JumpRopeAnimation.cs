using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class JumpRopeAnimation : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(0, 10)] private float _speed;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Animator _animator;
        #endregion

        #region METHODS PUBLIC
        private void Start()
        {
            StartCoroutine(RollRope());
        }
        #endregion

        #region COROUTINES
        private IEnumerator RollRope()
        {
            var material = _meshRenderer.materials[0];
            material.SetTextureOffset("_BaseMap", Vector2.zero);
            while (true)
            {
                var offset = material.GetTextureOffset("_BaseMap");
                offset.x += (Time.deltaTime * _speed) * _animator.speed;
                material.SetTextureOffset("_BaseMap", offset);

                yield return null;
            }
        }
        #endregion
    }
}
