using Cysharp.Threading.Tasks;
using MH.SceneLoader;
using UnityEngine;

namespace MH.GraphicQuality
{
    [CreateAssetMenu(fileName = "GraphicSetting System", menuName = "MH_SO/GameSystem/GraphicSetting")]
    public class GraphicSettingRegister : GameSystemInitializer
    {
        public override async UniTask Initialize()
        {
            var graphicSettingManager = new GraphicQualityManager();
            
            ServiceLocator.Register<IGraphicQualityManager>(graphicSettingManager);
        }
    }
}