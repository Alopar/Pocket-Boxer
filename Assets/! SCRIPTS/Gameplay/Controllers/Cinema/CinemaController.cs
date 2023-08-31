using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Services.SignalSystem;
using UnityEngine.Events;
using Manager;

namespace Gameplay
{
    public class CinemaController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<CinemaSequence> _cinemaSequences;
        #endregion

        #region FIELDS PRIVATE
        private List<CinemachineVirtualCamera> _virtualCameras;
        #endregion

        #region HANDLERS
        private void h_CinemaStart(CinemaStartInfo info)
        {
            StartCoroutine(PlayCinemaSequence(_cinemaSequences.Find(e => e.ID == info.ID), info.Callback));
        }
        #endregion

        #region UNITY CALLBACKS
        #endregion

        #region METHODS PRIVATE
        #endregion

        #region METHODS PUBLIC
        public void Init()
        {
            _virtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>().ToList();
        }

        public void TurnOn()
        {
            SignalSystem<CinemaStartInfo>.AddListener(h_CinemaStart, false);
        }

        public void TurnOff()
        {
            SignalSystem<CinemaStartInfo>.RemoveListener(h_CinemaStart);
        }
        #endregion

        #region COROUTINES
        IEnumerator PlayCinemaSequence(CinemaSequence sequence, UnityAction callback)
        {
            yield return new WaitForSeconds(0.1f);

            SignalSystem<InputControlInfo>.Send(new InputControlInfo(false));

            for (int i = 0; i < sequence.Steps.Count; i++)
            {
                var step = sequence.Steps[i];

                switch (step.Type)
                {
                    case CinemaStepType.Await:
                        break;
                    case CinemaStepType.Observation:
                        _virtualCameras.ForEach(e => e.Priority = 0);
                        step.Camera.Priority = 99;
                        break;
                    case CinemaStepType.Movement:
                        SignalSystem<CinemaActorMoveInfo>.Send(new(step.Actor, step.Point));
                        break;
                    case CinemaStepType.Emotion:
                        SignalSystem<CinemaActorEmotionInfo>.Send(new(step.Actor, step.Emotion));
                        break;
                }

                yield return new WaitForSeconds(step.Duration);
            }

            callback?.Invoke();

            _virtualCameras.ForEach(e => e.Priority = 0);
            SignalSystem<CinemaFinishInfo>.Send(new());
            SignalSystem<InputControlInfo>.Send(new InputControlInfo(true));
        }
        #endregion
    }

    public enum CinemaStepType
    {
        Await,
        Observation,
        Movement,
        Emotion,
    }

    [Serializable]
    public struct CinemaStep
    {
        public CinemaStepType Type;

        [Space(10)]
        public float Duration;
        public CinemachineVirtualCamera Camera;
        public ActorComponent Actor;
        public Transform Point;
        public string Emotion;
    }

    [Serializable]
    public struct CinemaSequence
    {
        public string ID;
        public List<CinemaStep> Steps;
    }
}