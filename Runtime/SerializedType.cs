using System;
using UnityEngine;

namespace Better.Extensions.Runtime
{
    /// <summary>
    /// Reference to a class <see cref="System.Type"/> with Unity serialization.
    /// </summary>
    [Serializable]
    public class SerializedType : ISerializationCallbackReceiver
    {
        [SerializeField] private protected string fullQualifiedName;

        private Type _type;

        public SerializedType()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializedType"/> class.
        /// </summary>
        /// <param name="qualifiedTypeName">Assembly qualified class name.</param>
        public SerializedType(string qualifiedTypeName)
        {
            ValidateStringType(qualifiedTypeName);
            fullQualifiedName = qualifiedTypeName;
        }

        private protected void ValidateStringType(string qualifiedTypeName)
        {
            if (!TryGetReferenceType(qualifiedTypeName, out _type))
            {
                throw new Exception($"{qualifiedTypeName} not found");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializedType"/> class.
        /// </summary>
        public SerializedType(Type type)
        {
            _type = type;
            fullQualifiedName = type.AssemblyQualifiedName;
        }

        public static string GetReferenceValue(Type type)
        {
            return type != null
                ? type.AssemblyQualifiedName
                : string.Empty;
        }

        public static bool TryGetReferenceType(string value, out Type type)
        {
            type = !string.IsNullOrEmpty(value)
                ? Type.GetType(value)
                : null;

            return type != null;
        }

        public override string ToString()
        {
            return Type != null ? Type.FullName : $"(None)";
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(fullQualifiedName))
            {
                _type = Type.GetType(fullQualifiedName);
                if (_type == null)
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"'{fullQualifiedName}' class type not found.");
#endif
                }
            }
            else
            {
                _type = null;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        public Type Type
        {
            get
            {
                if (_type == null && !string.IsNullOrEmpty(fullQualifiedName))
                {
                    if (!TryGetReferenceType(fullQualifiedName, out _type))
                    {
                        fullQualifiedName = string.Empty;
                    }
                }
                return _type;
            }
        }


        public static implicit operator string(SerializedType typeReference) => typeReference.fullQualifiedName;

        public static implicit operator Type(SerializedType typeReference) => typeReference.Type;

        public static implicit operator SerializedType(Type type) => new SerializedType(type);
    }
}