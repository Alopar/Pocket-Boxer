using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Utility
{
    public static class WebRequestInEditor
    {
        #region FIELDS PRIVATE
        private static List<TaskWebRequest> _tasks = new();
        #endregion

        #region METHODS PRIVATE
        private static void EditorUpdate()
        {
            var tasksToRemove = new List<TaskWebRequest>();
            foreach (var task in _tasks)
            {
                if (task.CheckWebRequestResult())
                {
                    tasksToRemove.Add(task);
                }
            }
            tasksToRemove.ForEach(e => _tasks.Remove(e));

            if (_tasks.Count == 0)
            {
                EditorApplication.update -= EditorUpdate;
            }
        }

        private static void RegistrationTask(TaskWebRequest task)
        {
            if (_tasks.Count == 0)
            {
                _tasks.Add(task);
                EditorApplication.update += EditorUpdate;
            }
            else
            {
                _tasks.Add(task);
            }
        }
        #endregion

        #region METHODS PUBLIC
        public static TaskWebRequest Request(string url)
        {
            var request = UnityWebRequest.Get(url);
            request.SendWebRequest();

            var task = new TaskWebRequest(request);
            RegistrationTask(task);
            return task;
        }
        #endregion
    }

    public class TaskWebRequest
    {
        #region FIELDS PRIVATE
        private UnityWebRequest _request;
        #endregion

        #region CONSTRUCTORS
        public TaskWebRequest(UnityWebRequest request)
        {
            _request = request;
        }
        #endregion

        #region EVENTS
        public event Action<string> OnRequestSuccess;
        #endregion

        #region METHODS PUBLIC
        public bool CheckWebRequestResult()
        {
            var result = false;
            switch (_request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogWarning($"WebRequest error: {_request.error}");
                    result = true;
                    break;
                case UnityWebRequest.Result.Success:
                    var requestString = _request.downloadHandler.text;
                    OnRequestSuccess?.Invoke(requestString);
                    result = true;
                    break;
            }

            if (result)
            {
                _request.Dispose();
            }

            return result;
        }
        #endregion
    }
}