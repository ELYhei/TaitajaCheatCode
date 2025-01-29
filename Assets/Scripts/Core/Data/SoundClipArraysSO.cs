using UnityEngine;

namespace ELY.Core
{
    [CreateAssetMenu(menuName = "Create SoundsData/AllSoundClipArrays")]
    public class SoundClipArraysSO : ScriptableObject
    {
        public SoundManager.SoundAudioClipArray[] soundAudioClipArrays;
    }
}