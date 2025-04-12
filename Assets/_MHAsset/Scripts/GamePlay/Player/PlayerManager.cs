using UnityEngine;

namespace MH.Player
{
    public interface IPlayerManager
    {
        PlayerEntity SpawnPlayer();
        PlayerEntity GetPlayer { get; } 
        void Teleport(Vector3 newPos, Quaternion newRotation);

        void SetCharacterVisible(bool on);

        void SetLockControl(bool on);
    }

    public class PlayerManager : IPlayerManager
    {
        private PlayerEntity _playerPrefab;
        private PlayerEntity _player; // the current player in scene

        public PlayerManager(PlayerEntity playerPrefab)
        {
            _playerPrefab = playerPrefab;
        }

        public PlayerEntity GetPlayer => _player;

        public void SetCharacterVisible(bool on)
        {
            _player.gameObject.SetActive(on);
        }

        public void SetLockControl(bool on)
        {
            
        }

        public PlayerEntity SpawnPlayer()
        {
            _player = GameObject.Instantiate(_playerPrefab);
            return _player;
        }

        public void Teleport(Vector3 newPos, Quaternion newRotation)
        {
            if(!_player) SpawnPlayer();

            _player.transform.position = newPos;
            _player.transform.rotation = newRotation;
        }
    }
}
