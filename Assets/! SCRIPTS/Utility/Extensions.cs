using System;

namespace UnityEngine
{
    public static class ParticleSystemExtensions
    {
        ///<summary>
        ///ATTENTION: this method will stop particle system!
        ///</summary>
        public static void SetDuration(this ParticleSystem particleSystem, float duration)
        {
            particleSystem.Stop();
            var main = particleSystem.main;
            main.duration = duration;
        }
    }

    public static class ComponentExtension
    {
        /// <summary>
        /// The auto generated enum tag of this game object.
        /// </summary>
        public static Tag? Tag(this Component component)
        {
            return Utility.Tags.Tags.GetTagByString(component.tag);
        }
    }

    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns true if the object is null or the unity object is considered as "null" (was destroyed)
        /// </summary>
        public static bool IsNull<T>(this T obj) where T : class
        {
            if (obj is UnityEngine.Object unityObject)
                return unityObject == null;

            return obj == null;
        }

        /// <summary>
        /// returns the given object or a real null-pointer if the object is null or considered as "null" (destroyed unity object).
        /// Use this in combination with a coalescing operator.
        /// Example: `MyObject.Get()?.DoSomething();`
        /// </summary>
        public static T Get<T>(this T obj) where T : class
        {
            if (IsNull(obj))
                return null;

            return obj;
        }

        /// <summary>
        /// Invokes the given action with the element as parameter,
        /// if the object is not null / the unity-object is not considered as "null" (destroyed).
        /// Otherwise, nothing happens.
        /// </summary>
        public static void Do<T>(this T obj, Action<T> action) where T : class
        {
            if (IsNull(obj))
                return;

            action?.Invoke(obj);
        }
    }
}