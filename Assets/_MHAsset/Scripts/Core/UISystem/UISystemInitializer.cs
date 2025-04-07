using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MH.UISystem
{
    [CreateAssetMenu(fileName = "UISystem", menuName = "MH_SO/GameSystem/UISystem")]
    public class UISystemInitializer : GameSystemInitializer
    {
        [SerializeField] private UIManager _uiManagerPrefab;

        public override async UniTask Initialize()
        {
            var UIManager = GameObject.Instantiate(_uiManagerPrefab);

            await UniTask.DelayFrame(1);
        }
    }
}
