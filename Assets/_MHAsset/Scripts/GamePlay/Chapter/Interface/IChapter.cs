using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [SerializeField] public string Id { get; private set; }

        #endregion

        #region ------------ Unity Methods -----------

        private void Start()
        {
            
        }

        #endregion


        #region -------------- Public Methods -------------


        #endregion

    }
}
