using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="BiomeMusicLibray", menuName = "Biome music Library")]
public class BiomeMusicLibrary : ScriptableObject
{

    [System.Serializable]
    public class BiomeMusicEntry
    {
        public string biomeName;
        public string musicKey;
    }

    public List<BiomeMusicEntry> biomeMusics = new List<BiomeMusicEntry>();
}
