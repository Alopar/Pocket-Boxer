using System.Collections;
using UnityEngine;

namespace Utility.CoroutineRunner
{

    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
