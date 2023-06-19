using System;
using System.Diagnostics.CodeAnalysis;
using JoostenProductions;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Features.AbilitySystem.Abilities
{
    internal class JumpAbility : IAbility
    {
        private const float StartTime = 0f;

        private readonly IAbilityItem _abilityItem;

        private float _time;
        private bool _isActive;
        private Transform _transformCache;
        private float _startHeight;
        private float _targetHeight;
        private float _jumpDuration;
        
        private enum JumpPhase {Up,Down}
        private JumpPhase _jumpPhase;

        public JumpAbility([NotNull] IAbilityItem abilityItem) =>
            _abilityItem = abilityItem ?? throw new ArgumentNullException();
        public void Apply(IAbilityActivator activator)
        {
            if (_isActive)
                return;
            StartAbility(activator);
        }

        private void StartAbility(IAbilityActivator activator)
        {
            _isActive = true;
            _time = StartTime;
            _transformCache = activator.ViewGameObject.transform;
            _startHeight = _transformCache.position.y;
            _targetHeight = _startHeight + activator.JumpHeight;
            _jumpDuration = _abilityItem.Value;
            _jumpPhase = JumpPhase.Up;
            
            UpdateManager.SubscribeToUpdate(Update);
        }

        private void FinishAbility()
        {
            _isActive = false;
            _time = default;
            _transformCache =default;
            _startHeight = default;
            _targetHeight = default;
            _jumpDuration = default;
            _jumpPhase = default;
            
            UpdateManager.UnsubscribeFromUpdate(Update);
        }

        private void Update()
        {
            UpdateTime(); 
            UpdatePosition();
            UpdateState();
        }

        private void UpdateState()
        {
            if (IsJumpPeak())
                _jumpPhase = JumpPhase.Down;
            if (IsJumpFinished())
                FinishAbility();
        }

        private bool IsJumpFinished() =>
            _jumpPhase == JumpPhase.Down &&
            _time <= StartTime;

        private bool IsJumpPeak() =>
            _jumpPhase == JumpPhase.Up &&
            _time >= _jumpDuration;

        private void UpdatePosition()
        {
            float curentHeight = CalculateHeight();
            _transformCache.position = CalculatePosition(curentHeight);
        }

        private Vector3 CalculatePosition(float curentHeight)
        {
            Vector3 position = _transformCache.position;
            position.y = curentHeight;
            return position;
        }

        private float CalculateHeight()
        {
            float ratio = _time / _jumpDuration;
            return Mathf.Lerp(_startHeight, _targetHeight, ratio);
        }

        private void UpdateTime()
        {
            switch (_jumpPhase)
            {
                case JumpPhase.Up : IncreaseTime();
                    break;
                case JumpPhase.Down : DecreaseTime();
                    break;
            }
        }

        private void DecreaseTime()
        {
            _time -= Time.deltaTime;
        }

        private void IncreaseTime()
        {
            _time += Time.deltaTime;
        }
    }
}