using System;
using UnityEngine;

namespace MH.GameState
{
    public class GameStateContainer : MonoBehaviour
    {
        private GameStateSystem _gameStateSystem;

        public void Init(GameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
        }

        private void Update()
        {
            _gameStateSystem.Update();
        }
    }
}