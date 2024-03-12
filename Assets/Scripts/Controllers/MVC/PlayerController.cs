using System;
using platformer_2d.demo;
using UnityEngine;
using UnityEngine.InputSystem;

namespace platformer_2d.Demo
{
    public class PlayerController : IDisposable, InputActions.IPlayerActions
    {
        private PlayerModel _playerModel;
        private PlayerView _playerData;
        private PlayerConfig _playerConfig;

        private InputActions _playerActions;

        private PlayerController(PlayerModel playerModel, PlayerView playerData)
        {
            _playerModel = playerModel;
            _playerData = playerData;
            _playerActions = new InputActions();
            _playerActions.Player.SetCallbacks(this);
        }

        public static PlayerController Build(PlayerModel playerModel, PlayerView playerData)
        {
            return new PlayerController(playerModel, playerData);
        }

        public PlayerController EnableInput()
        {
            _playerActions.Player.Enable();
            return this;
        }
        
        public PlayerController DisableInput()
        {
            _playerActions.Player.Disable();
            return this;
        }

        public PlayerController WithConfig(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
            return this;
        }
        
        public void SetDefaultPosition(Vector3 position)
        {
            _playerData.transform.position = position;
        }

        public void Update()
        {
            // move player
            _playerData.UpdatePlayer();
        }

        public void Dispose()
        {
            _playerModel = null;
            _playerData = null;
            _playerActions.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _playerData.Move(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }

        public void OnFire(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _playerData.Jump();
        }
    }
}