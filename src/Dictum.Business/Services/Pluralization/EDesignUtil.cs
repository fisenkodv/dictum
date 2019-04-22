using System;

namespace Dictum.Business.Services.Pluralization
{
    internal static class EDesignUtil
    {
        static internal T CheckArgumentNull<T>(T value, string parameterName) where T : class
        {
            if (null == value)
            {
                throw new ArgumentException(parameterName);
            }
            return value;
        }
    }
}
