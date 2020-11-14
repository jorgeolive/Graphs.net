using System;

namespace Graphs.Core
{
    public class Weight
    {
        public float Value => _initialized ? _value : Init();
        private float _value;
        private bool _initialized = false;
        private Func<float> _weightProviderFunction;

        private float Init() {
            _value = _weightProviderFunction.Invoke();
            _initialized = true;
            return _value;
         }

        public Weight(float value) => (_value, _initialized) = (value, true);
        public Weight(Func<float> weightProviderFunction)
        {
            _weightProviderFunction = weightProviderFunction;
        }
    }

}