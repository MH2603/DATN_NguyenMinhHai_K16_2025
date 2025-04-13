using MH.SaveSystem;
using MH.Player;
using UnityEngine;

namespace MH.ChapterSystem
{
    public interface IChapter 
    {
        string Id { get; }
        void Load();
        void UnLoad();
    }

    public class Chapter : MonoBehaviour
    {
        #region --------------- Fields -----------

        [SerializeField] private Transform[] _checkPoints;

        private int _currentCheckPointIndex;
        private Transform _checkPoint => _checkPoints[_currentCheckPointIndex]; 
  
        #endregion

        #region ------------ Unity Methods -----------

        void Start()
        {
            
        }

        #endregion


        #region -------------- Public Methods -------------

        public Transform GetCheckPoint(int index)
        {
            return _checkPoints[index];
        }

        #endregion

    }
}
