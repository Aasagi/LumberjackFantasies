using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class AxeStats : MonoBehaviour
    {
        public Attack Attack;
        public float SwingSpeedMultiplayer = 1.0f;
        private int _downedTrees;
        public EventHandler DownedTreesChanged;

        public int DownedTrees
        {
            get { return _downedTrees; }
            set
            {
                _downedTrees = value;
                if (DownedTreesChanged != null) DownedTreesChanged(_downedTrees, null);
            }
        }
    }
}