using UnityEngine;
using UnityEngine.Events;

namespace Tools
{
    public class AnimationEventTransmitter : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(1, 10)] private byte _index = 1;
        #endregion

        #region EVENTS
        public event UnityAction<byte> AnimationEvent01;
        public event UnityAction<byte> AnimationEvent02;
        public event UnityAction<byte> AnimationEvent03;
        public event UnityAction<byte> AnimationEvent04;
        public event UnityAction<byte> AnimationEvent05;

        public event UnityAction<byte, float> AnimationFloatEvent01;
        public event UnityAction<byte, float> AnimationFloatEvent02;
        public event UnityAction<byte, float> AnimationFloatEvent03;

        public event UnityAction<byte, string> AnimationStringEvent01;
        public event UnityAction<byte, string> AnimationStringEvent02;
        public event UnityAction<byte, string> AnimationStringEvent03;
        #endregion

        #region METHODS PUBLIC
        public void EmitEvent01()
        {
            AnimationEvent01?.Invoke(_index);
        }

        public void EmitEvent02()
        {
            AnimationEvent02?.Invoke(_index);
        }

        public void EmitEvent03()
        {
            AnimationEvent03?.Invoke(_index);
        }

        public void EmitEvent04()
        {
            AnimationEvent04?.Invoke(_index);
        }

        public void EmitEvent05()
        {
            AnimationEvent05?.Invoke(_index);
        }

        public void EmitFloatEvent01(float value)
        {
            AnimationFloatEvent01?.Invoke(_index, value);
        }

        public void EmitFloatEvent02(float value)
        {
            AnimationFloatEvent02?.Invoke(_index, value);
        }

        public void EmitFloatEvent03(float value)
        {
            AnimationFloatEvent03?.Invoke(_index, value);
        }

        public void EmitStringEvent01(string text)
        {
            AnimationStringEvent01?.Invoke(_index, text);
        }

        public void EmitStringEvent02(string text)
        {
            AnimationStringEvent02?.Invoke(_index, text);
        }

        public void EmitStringEvent03(string text)
        {
            AnimationStringEvent03?.Invoke(_index, text);
        }
        #endregion
    }
}
