

using UnityEngine;

namespace MH
{
    public class GameBootstrap : MonoBehaviour
    {

        [SerializeField] private GameSystemInitializer[] _gameSystemInitializers;

        void Awake()
        {
            InitializeAllGameSystem();
        }

        private async void InitializeAllGameSystem()
        {
            for (int i=0; i < _gameSystemInitializers.Length; i++)
            {
                await _gameSystemInitializers[i].Initialize();
                Debug.Log($" [GameBootstrap] [{i + 1}] {_gameSystemInitializers[i].name} is initialized ");
            }
        }
    }
}
