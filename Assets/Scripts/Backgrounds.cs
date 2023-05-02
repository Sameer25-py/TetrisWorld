using System.Collections.Generic;
using UnityEngine;

namespace TetrisWorld
{
    [CreateAssetMenu(fileName = "Backgrounds", menuName = "Backgrounds", order = 0)]
    public class Backgrounds : ScriptableObject
    {
        public List<Sprite> Sprites;
    }
}