using UnityEngine;

namespace TetrisWorld
{
    public class Letter : MonoBehaviour
    {
        public Vector2Int Index = new Vector2Int(-1,1);
        public string     PrefabLetter = "";
    }
}