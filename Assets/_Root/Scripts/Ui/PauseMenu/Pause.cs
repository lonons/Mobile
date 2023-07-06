using System;
using UnityEngine;

namespace Ui
{
    internal class Pause
    {
        private const float PauseTimeScale = 0f;

        public event Action Enabled;

        public event Action Disabled;

        private readonly float _initialTimeScale;

        private bool _isDisposed;
        
        public bool IsEnabled { get; private set; }

        public Pause() => _initialTimeScale = Time.timeScale;

        public void Enable()
        {
            if (IsEnabled)
                throw new InvalidOperationException("It's already enabled");

            Time.timeScale = PauseTimeScale;
            IsEnabled = true;
            Enabled?.Invoke();
        }

        public void Disable()
        {
            if (!IsEnabled)
                throw new InvalidOperationException("It's already disabled");

            Time.timeScale = _initialTimeScale;
            IsEnabled = false;
            Disabled?.Invoke();
        }
    }
}