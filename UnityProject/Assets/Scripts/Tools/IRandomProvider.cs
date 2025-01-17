using System.Collections.Generic;

namespace Clovers.Tools
{
    public interface IRandomProvider
    {
        int GetRandomInt(int min, int max);
        float GetRandomFloat(float min, float max);
        T GetRandom<T>(T[] array);
        T GetRandom<T>(List<T> list);
    }
}