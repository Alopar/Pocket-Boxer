using UnityEngine;

namespace Services.AudioSystem
{
    public interface IAudioService
    {
        void PlayMusic(AudioClip clip, float volume = 1);
        void PlayMusic(string name, float volume = 1);
        void PlaySound(AudioClip clip, SoundType type, float volume = 1);
        void PlaySound(string name, SoundType type, float volume = 1);
    }
}
