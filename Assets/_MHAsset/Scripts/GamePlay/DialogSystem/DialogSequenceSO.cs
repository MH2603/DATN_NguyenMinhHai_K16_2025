using UnityEngine;

namespace MH.DialogSystem
{
    [System.Serializable]
    public class Sentence
    {
        [HideInInspector]public float Duration;
        [TextArea]
        public string Content;
    }
        
    [CreateAssetMenu(fileName = "Dialog Sequence", menuName = "MH_SO/Dialog/Dialog Sequence")]
    public class DialogSequenceSO : ScriptableObject
    {
        public string Speaker;
        public int CharacterDur = 100;
        
        public Sentence[] Sentences;
    }
}