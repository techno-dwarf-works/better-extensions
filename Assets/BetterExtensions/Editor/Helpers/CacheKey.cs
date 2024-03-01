using System;
using Better.Extensions.Runtime;

namespace Better.Extensions.EditorAddons
{
    public struct CacheKey : IEquatable<CacheKey>
    {
        private readonly Type _type;
        private readonly string _propertyPath;

        public CacheKey(Type type, string propertyPath)
        {
            _type = type;
            _propertyPath = propertyPath;
        }

        public bool Equals(CacheKey other)
        {
            return _type == other._type && _propertyPath.CompareOrdinal(other._propertyPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CacheKey key && Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_type != null ? _type.GetHashCode() : 0) * 397) ^ (_propertyPath != null ? _propertyPath.GetHashCode() : 0);
            }
        }
    }
}