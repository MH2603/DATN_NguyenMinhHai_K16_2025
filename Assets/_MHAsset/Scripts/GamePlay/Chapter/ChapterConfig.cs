using UnityEngine;

namespace MH.ChapterSystem
{
    [CreateAssetMenu(fileName = "Chapter", menuName = "MH_SO/Chapter/Chapter Config")]
    public class ChapterConfig : ScriptableObject
    {
        public int Id;
        public Chapter ChapterPrefab;
        public string SceneName;
    }
}