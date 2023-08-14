using UnityEngine;
using Services.SaveSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    public abstract class AbstractDependencyTest : MonoBehaviour
    {
        [Inject] private ISaveService _saveService;
        [MonoInject] private Transform _transform;
    }

    public class DependencyTest : AbstractDependencyTest
    {
        #region FIELDS PRIVATE
        [MonoInject] private Rigidbody _rigidbody;
        [MonoInject] private BoxCollider _boxCollider;
        #endregion

        #region UNITY CALLBACKS
        #endregion
    }
}
