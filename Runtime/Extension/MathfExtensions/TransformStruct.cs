using UnityEngine;

namespace Better.Extensions.Runtime.Extension.MathfExtensions
{
    public readonly struct LineIntersection
    {
        public LineIntersection(bool intersectsOnLines, bool intersectsOnSegments)
        {
            IntersectsOnLines = intersectsOnLines;
            IntersectsOnSegments = intersectsOnSegments;
        }

        public bool IntersectsOnLines { get; }
        public bool IntersectsOnSegments { get; }
    }
    
    public readonly struct TransformStruct
    {
        public Vector3 Position { get; }
        public Vector3 LocalPosition { get; }
        public Quaternion Rotation { get; }
        public Vector3 LossyScale { get; }
        public Vector3 LocalScale { get; }
        public Matrix4x4 LocalToWorld { get; }
        public Matrix4x4 WorldToLocal { get; }

        public TransformStruct(Transform transform)
        {
            Position = transform.position;
            LocalPosition = transform.localPosition;
            Rotation = transform.rotation;
            LossyScale = transform.lossyScale;
            LocalScale = transform.localScale;
            LocalToWorld = transform.localToWorldMatrix;
            WorldToLocal = transform.worldToLocalMatrix;
        }

        public TransformStruct(Vector3 position, Vector3 localPosition, Quaternion rotation, Vector3 lossyScale,
            Vector3 localScale)
        {
            Position = position;
            LocalPosition = localPosition;
            Rotation = rotation;
            LossyScale = lossyScale;
            LocalScale = localScale;
            LocalToWorld = default;
            WorldToLocal = default;
        }
    }
}