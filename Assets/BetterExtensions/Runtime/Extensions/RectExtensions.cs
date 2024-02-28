using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class RectExtensions
    {
        public static float GetRatio(this Rect self)
        {
            return self.size.y / self.size.y;
        }
    }
}