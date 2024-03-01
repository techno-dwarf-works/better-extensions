using System;
using System.Reflection;

namespace Better.Extensions.EditorAddons
{
    public class CachedFieldInfo
    {
        public FieldInfo FieldInfo { get; }
        public Type Type { get; }
        
        public CachedFieldInfo(FieldInfo fieldInfo, Type type)
        {
            FieldInfo = fieldInfo;
            Type = type;
        }
    }
}