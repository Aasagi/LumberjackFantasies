using System.Collections.Generic;
using System.Linq;
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
        PickUpLog,
        WoodChop
    }

    public class AudioSingleton : MonoBehaviour
    {
        #region Fields
        private List<AudioSource> AudioSources;
        public List<AudioClip> CriterFoundSounds;
        public List<AudioClip> EnemySpottedSounds;
        public List<AudioClip> GruntSounds;
        public List<AudioClip> LevelUpSounds;
        public List<AudioClip> LogPickUpSounds;
        public int MaxiumumSimultaneousSounds = 10;
        public List<AudioClip> PainSounds;
        public List<AudioClip> WoodChopSounds;
        private readonly Dictionary<SoundType, List<AudioClip>> audioClips =
            new Dictionary<SoundType, List<AudioClip>>();
        #endregion

        #region Public Properties
        public static AudioSingleton Instance { get; private set; }
        #endregion

        #region Public Methods and Operators
        public void PlaySound(SoundType type)
        {
            var player = GetAvailiblePlayer();
            if (player == null)
            {
                return;
            }

            player.clip = GetRandomSound(type);
            player.Play();
        }
        #endregion

        #region Methods
        private AudioSource GetAvailiblePlayer()
        {
            return AudioSources.FirstOrDefault(s => s.isPlaying == false);
        }

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
            audioClips.Add(SoundType.WoodChop, WoodChopSounds);

            AudioSources = new List<AudioSource>();
            for (var i = 0; i < MaxiumumSimultaneousSounds; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                AudioSources.Add(source);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            //var toRemove = audioSources.Where(audio => !audio.isPlaying);
            //foreach (var removedAudio in toRemove)
            //{
            //    audioSources.Remove(removedAudio);
            //}
        }
        #endregion
    }
}