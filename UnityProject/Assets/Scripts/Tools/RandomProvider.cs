using System.Collections.Generic;

namespace Clovers.Tools
{
    public class RandomProvider : IRandomProvider
    {
        private readonly int[] _randomIntegers =
        {
            142342, 21347, 1234123, 213467, 123489, 123434, 213456, 321421, 132490, 123412,
            123477, 12343, 123499, 123445, 123466, 123423, 123419, 3214281, 412349, 312410, 0
        };
        private int _index;

        private int NextInt()
        {
            var value = _randomIntegers[_index];
            _index = (_index + 1) % _randomIntegers.Length;
            return value;
        }
        
        public int GetRandomInt(int min, int max)
        {
            var range = max - min + 1;
            return min + NextInt() % range;
        }

        public float GetRandomFloat(float min, float max)
        {
            var randomValue = NextInt() / (float)_randomIntegers.Length;
            return min + randomValue * (max - min);
        }

        public T GetRandom<T>(T[] array)
        {
            return array[GetRandomInt(0, array.Length - 1)];
        }

        public T GetRandom<T>(List<T> list)
        {
            return list[GetRandomInt(0, list.Count - 1)];
        }
    }
}