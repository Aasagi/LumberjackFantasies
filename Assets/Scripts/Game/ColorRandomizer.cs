using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ColorRandomizer : MonoBehaviour
{

    public List<Color> Colors;
    private readonly Dictionary<Color, Material> _materials = new Dictionary<Color, Material>(); 
	// Use this for initialization
    private void Start()
    {
        var color = Colors[Random.Range(0, Colors.Count)];
        var material = GetMaterial(color);
        renderer.material = material;
    }

    private Material GetMaterial(Color color)
    {
        if (!_materials.ContainsKey(color))
        {
            var newMaterial = new Material(renderer.material) {color = color};
            _materials.Add(color, newMaterial);
        }

        return _materials[color];
    }

    // Update is called once per frame
	void Update () {
	
	}
}
