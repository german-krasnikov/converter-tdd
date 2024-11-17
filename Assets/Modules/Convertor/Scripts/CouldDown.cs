using System;

namespace Modules.Converter
{
    public class CouldDown
    {
        public event Action OnComplete;

        private float _time;
        private float _interval;

        public CouldDown(float interval)
        {
            if (interval <= 0) throw new ArgumentException();
            _interval = interval;
        }

        public void Start()
        {
            _time = 0;
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0) throw new ArgumentException();
            _time += deltaTime;

            while (_time >= _interval)
            {
                OnComplete?.Invoke();
                _time -= _interval;
            }
        }

        public void Stop()
        {
            _time = 0;
        }
    }
}