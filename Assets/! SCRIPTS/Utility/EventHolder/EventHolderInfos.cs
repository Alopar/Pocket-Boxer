using UnityEngine;
using UnityEngine.Events;
using Gameplay;
using Services.TutorialSystem;
using PointerType = Gameplay.PointerType;

namespace EventHolder
{
    #region INPUT
    public class InputInfo
    {
        public Vector2 Direction { get; private set; }
        public bool PointerDown { get; private set; }
        public bool PointerUp { get; private set; }
        public bool IsDeathZone { get; private set; }
        public float Distance { get; private set; }

        public InputInfo(Vector2 direction, bool pointerDown, bool pointerUp, float distance, bool isDeathZone)
        {
            Direction = direction;
            PointerDown = pointerDown;
            PointerUp = pointerUp;
            IsDeathZone = isDeathZone;
            Distance = distance;
        }

        public override string ToString()
        {
            return $"Down: {PointerDown}, Up: {PointerUp}, Activated: {IsDeathZone}";
        }
    }

    public class InputControlInfo
    {
        public bool Enable { get; private set; }

        public InputControlInfo(bool enable)
        {
            Enable = enable;
        }
    }
    #endregion

    #region CAMERA
    public class CameraOffsetInfo
    {
        public Vector3 Offset { get; private set; }

        public CameraOffsetInfo(Vector3 offset)
        {
            Offset = offset;
        }
    }

    public class CameraLookAtInfo
    {
        public Transform LookAtPoint { get; private set; }

        public CameraLookAtInfo(Transform lookAtPoint)
        {
            LookAtPoint = lookAtPoint;
        }
    }

    public class CameraLookPlayerInfo
    {
        // N/A
    }

    public class CameraChangeFOVInfo
    {
        public float FOV { get; private set; }

        public CameraChangeFOVInfo(float fov)
        {
            FOV = fov;
        }
    }
    #endregion

    #region POINTERS
    public class TrackTargetInfo
    {
        public Transform Target { get; private set; }
        public PointerType PointerType { get; private set; }

        public TrackTargetInfo(Transform target, PointerType pointerType)
        {
            Target = target;
            PointerType = pointerType;
        }
    }

    public class UntrackTargetInfo
    {
        public Transform Target { get; private set; }

        public UntrackTargetInfo(Transform target)
        {
            Target = target;
        }
    }
    #endregion

    #region RESOURCES
    public class MoneyChangeInfo
    {
        public uint Value { get; private set; }

        public MoneyChangeInfo(uint value)
        {
            Value = value;
        }
    }

    public class DiamondChangeInfo
    {
        public uint Value { get; private set; }

        public DiamondChangeInfo(uint value)
        {
            Value = value;
        }
    }

    public class ExperiencePointsChangeInfo
    {
        public uint Value { get; private set; }

        public ExperiencePointsChangeInfo(uint value)
        {
            Value = value;
        }
    }

    public class StrengthPointsChangeInfo
    {
        public uint Value { get; private set; }

        public StrengthPointsChangeInfo(uint value)
        {
            Value = value;
        }
    }

    public class DexterityPointsChangeInfo
    {
        public uint Value { get; private set; }

        public DexterityPointsChangeInfo(uint value)
        {
            Value = value;
        }
    }

    public class EndurancePointsChangeInfo
    {
        public uint Value { get; private set; }

        public EndurancePointsChangeInfo(uint value)
        {
            Value = value;
        }
    }
    #endregion

    #region PLAYER
    public class PlayerSpawnInfo
    {
        public PlayerController PlayerController { get; private set; }

        public PlayerSpawnInfo(PlayerController playerController)
        {
            PlayerController = playerController;
        }
    }

    public class BatteryOccupiedInfo
    {
        public int Capacity { get; private set; }
        public int Occupied { get; private set; }

        public BatteryOccupiedInfo(int capacity, int occupied)
        {
            Capacity = capacity;
            Occupied = occupied;
        }
    }
    #endregion

    #region STATS
    public class StrengthChangeInfo
    {
        public uint Level { get; private set; }
        public float Delta { get; private set; }

        public StrengthChangeInfo(uint level, float delta)
        {
            Level = level;
            Delta = delta;
        }
    }

    public class DexterityChangeInfo
    {
        public uint Level { get; private set; }
        public float Delta { get; private set; }

        public DexterityChangeInfo(uint level, float delta)
        {
            Level = level;
            Delta = delta;
        }
    }

    public class EnduranceChangeInfo
    {
        public uint Level { get; private set; }
        public float Delta { get; private set; }

        public EnduranceChangeInfo(uint level, float delta)
        {
            Level = level;
            Delta = delta;
        }
    }
    #endregion

    #region LEVEL
    public class LevelStartInfo
    {
        public int Number { get; private set; }

        public LevelStartInfo(int number)
        {
            Number = number;
        }
    }

    public class LevelEndInfo
    {
        public int Number { get; private set; }

        public LevelEndInfo(int number)
        {
            Number = number;
        }
    }
    #endregion

    #region CINEMA
    public class CinemaStartInfo
    {
        public string ID { get; private set; }
        public UnityAction Callback { get; private set; }

        public CinemaStartInfo(string iD, UnityAction callback)
        {
            ID = iD;
            Callback = callback;
        }
    }

    public class CinemaFinishInfo
    {
        // N/A
    }

    public class CinemaActorMoveInfo
    {
        public ActorComponent Actor { get; private set; }
        public Transform TargetPoint { get; private set; }

        public CinemaActorMoveInfo(ActorComponent actor, Transform targetPoint)
        {
            Actor = actor;
            TargetPoint = targetPoint;
        }
    }

    public class CinemaActorEmotionInfo
    {
        public ActorComponent Actor { get; private set; }
        public string EmotionName { get; private set; }

        public CinemaActorEmotionInfo(ActorComponent actor, string emotionName)
        {
            Actor = actor;
            EmotionName = emotionName;
        }
    }
    #endregion

    #region SCREENS
    public class ShowScreenInfo
    {
        public ScreenType ScreenType { get; private set; }

        public ShowScreenInfo(ScreenType screenType)
        {
            ScreenType = screenType;
        }
    }

    public class CloseScreenInfo
    {
        public ScreenType ScreenType { get; private set; }

        public CloseScreenInfo(ScreenType screenType)
        {
            ScreenType = screenType;
        }
    }

    public class ScreenOpenedInfo
    {
        public ScreenType ScreenType { get; private set; }

        public ScreenOpenedInfo(ScreenType screenType)
        {
            ScreenType = screenType;
        }
    }

    public class ScreenClosedInfo
    {
        public ScreenType ScreenType { get; private set; }

        public ScreenClosedInfo(ScreenType screenType)
        {
            ScreenType = screenType;
        }
    }
    #endregion

    #region TUTORIAL
    public class GameplayEventInfo
    {
        public GameplayEvent GameplayEvent { get; private set; }

        public GameplayEventInfo(GameplayEvent gameplayEvent)
        {
            GameplayEvent = gameplayEvent;
        }
    }

    public class TutorialStepInfo
    {
        public TutorialStep TutorialStep { get; private set; }

        public TutorialStepInfo(TutorialStep tutorialStep)
        {
            TutorialStep = tutorialStep;
        }
    }

    public class TutorialObservingInfo
    {
        public GameObject GameObject { get; private set; }

        public TutorialObservingInfo(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
    #endregion
}