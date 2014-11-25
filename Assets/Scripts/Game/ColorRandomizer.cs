using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ColorRandomizer : MonoBehaviour
    {
        #region Fields
        public List<Color> Colors;
        private readonly Dictionary<Color, Material> _materials = new Dictionary<Color, Material>();
        #endregion

        // Use this for initialization

        #region Methods
        private Material GetMaterial(Color color)
        {
            if (!_materials.ContainsKey(color))
            {
                var newMaterial = new Material(renderer.material) { color = color };
                _materials.Add(color, newMaterial);
            }

            return _materials[color];
        }

        private void Start()
        {
            var color = Colors[Random.Range(0, Colors.Count)];
            var material = GetMaterial(color);
            renderer.material = material;
        }

        // Update is called once per frame
        private void Update()
        {
        }
        #endregion
    }
}