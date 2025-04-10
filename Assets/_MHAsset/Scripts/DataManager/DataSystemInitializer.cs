using UnityEngine;

namespace MH.DataSystem
{
    [CreateAssetMenu(fileName = "DataSystem", menuName = "MH_SO/GameSystem/DataSystem")]
    public class DataSystemInitializer : GameSystemInitializer
    {
        [SerializeField] private TextAsset  _jsonFiles;
    }
}