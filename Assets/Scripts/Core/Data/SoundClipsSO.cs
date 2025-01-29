using UnityEngine;

namespace ELY.Core
{
    [CreateAssetMenu(menuName = "Create SoundsData/AllSoundClips")]
    public class SoundClipsSO : ScriptableObject
    {
        public SoundManager.SoundAudioClip[] soundAudioClips;
    }
}
