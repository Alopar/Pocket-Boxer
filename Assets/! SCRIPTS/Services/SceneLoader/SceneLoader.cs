using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.CoroutineRunner;
using Utility.DependencyInjection;

namespace Services.SceneLoader
{
    public class SceneLoader : ISceneLoaderService
    {
        #region FIELDS PRIVATE
        [Inject] private readonly ICoroutineRunner _coroutineRunner;
        #endregion

        #region METHODS PRIVATE
        private IEnumerator LoadScene(AsyncOperation operation, Action callback)
        {
            while (!operation.isDone)
            {
                yield return null;
            }

            callback?.Invoke();
        }
        #endregion

        #region METHODS PUBLIC
        public void Load(string name, Action callback = null)
        {
            var operation = SceneManager.LoadSceneAsync(name);
            _coroutineRunner.StartCoroutine(LoadScene(operation, callback));
        }

        public void Load(int index, Action callback = null)
        {
            var operation = SceneManager.LoadSceneAsync(index);
            _coroutineRunner.StartCoroutine(LoadScene(operation, callback));
        }
        #endregion
    }
}
