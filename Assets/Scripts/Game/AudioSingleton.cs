using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public enum SoundType
    {
        LevelUp,
        Criters,
        Enemy,
        Grunt,
        Pain,
        PickUpLog
    }

    public class AudioSingleton : MonoBehaviour
    {
        public static AudioSingleton Instance { get; private set; }

        #region Fields
        public List<AudioClip> CriterFoundSounds;
        public List<AudioClip> EnemySpottedSounds;
        public List<AudioClip> GruntSounds;
        public List<AudioClip> LevelUpSounds;
        public List<AudioClip> LogPickUpSounds;
        public List<AudioClip> PainSounds;
        public AudioSource Player;
        private readonly Dictionary<SoundType, List<AudioClip>> audioClips =
            new Dictionary<SoundType, List<AudioClip>>();
        #endregion

        #region Public Methods and Operators
        public void PlaySound(SoundType type)
        {
            Player.clip = GetRandomSound(type);
            Player.Play();
        }
        #endregion

        #region Methods
        private AudioClip GetRandomSound(SoundType type)
        {
            var container = audioClips[type];
            var index = Random.Range(0, container.Count - 1);

            return container[index];
        }

        private void Start()
        {
            Instance = this;

            audioClips.Add(SoundType.LevelUp, LevelUpSounds);
            audioClips.Add(SoundType.Criters, CriterFoundSounds);
            audioClips.Add(SoundType.Enemy, EnemySpottedSounds);
            audioClips.Add(SoundType.Grunt, GruntSounds);
            audioClips.Add(SoundType.Pain, PainSounds);
            audioClips.Add(SoundType.PickUpLog, LogPickUpSounds);
        }

        // Update is called once per frame
        private void Update()
        {
        }
        #endregion
    }
}