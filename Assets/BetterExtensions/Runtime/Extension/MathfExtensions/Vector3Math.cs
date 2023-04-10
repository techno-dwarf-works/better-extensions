using Unity.Collections;
using UnityEngine;

namespace Better.Extensions.Runtime.MathfExtensions
{
    public struct Vector3Math
    {
        /// <summary>
        /// Take projection of point on Plane
        /// </summary>
        /// <param name="planeOffset"></param>
        /// <param name="planeNormal"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 ClosestPointOnPlane(Vector3 planeOffset, Vector3 planeNormal, Vector3 point)
        {
            return point + DistanceFromPlane(planeOffset, planeNormal, point) * planeNormal;
        }

        /// <summary>
        /// Take Distance from plane to point
        /// </summary>
        /// <param name="planeOffset"></param>
        /// <param name="planeNormal"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static float DistanceFromPlane(Vector3 planeOffset, Vector3 planeNormal, Vector3 point)
        {
            return Vector3.Dot(planeOffset - point, planeNormal);
        }


        /// <summary>
        ///   <para>Transforms position from world space to local space.</para>
        /// </summary>
        public static Vector3 InverseTransformPoint(Vector3 point, Vector3 position, Quaternion rotation,
            Vector3 lossyScale)
        {
            return Vector3.Scale(new Vector3(1 / lossyScale.x, 1 / lossyScale.y, 1 / lossyScale.z),
                Quaternion.Inverse(rotation) * (point - position));
        }

        /// <summary>
        ///   <para>Transforms vector from world space to local space.</para>
        /// </summary>
        public static Vector3 InverseTransformVector(TransformStruct transformStruct, Vector3 vector3)
        {
            return InverseTransformVector(transformStruct.Position, transformStruct.Rotation,
                transformStruct.LocalScale, vector3);
        }

        /// <summary>
        ///   <para>Transforms vector from world space to local space.</para>
        /// </summary>
        public static Vector3 InverseTransformVector(Vector3 position, Quaternion rotation, Vector3 localScale,
            Vector3 vector3)
        {
            var initialMatrix = Matrix4x4.TRS(position, rotation, localScale).inverse;
            var scaleMatrix = Matrix4x4.Scale(vector3);

            var transformed = initialMatrix * scaleMatrix;
            return transformed.lossyScale;
        }

        /// <summary>
        ///   <para>Transforms vector from local space to world space.</para>
        /// </summary>
        public static Vector3 TransformVector(TransformStruct transformStruct, Vector3 vector3)
        {
            var initialMatrix = Matrix4x4.TRS(transformStruct.Position, transformStruct.Rotation,
                transformStruct.LocalScale);
            var scaleMatrix = Matrix4x4.Scale(transformStruct.LocalScale);

            var transformed = initialMatrix * scaleMatrix;
            return transformed.lossyScale;
        }

        /// <summary>
        ///   <para>Transforms position from world space to local space.</para>
        /// </summary>
        public static Vector3 InverseTransformPoint(TransformStruct transformStruct, Vector3 point)
        {
            return Vector3.Scale(
                new Vector3(1 / transformStruct.LossyScale.x, 1 / transformStruct.LossyScale.y,
                    1 / transformStruct.LossyScale.z),
                Quaternion.Inverse(transformStruct.Rotation) * (point - transformStruct.Position));
        }

        /// <summary>
        ///   <para>Transforms position from local space to world space.</para>
        /// </summary>
        public static Vector3 TransformPoint(TransformStruct transformStruct, Vector3 point)
        {
            return TransformPoint(point, transformStruct.Position, transformStruct.Rotation,
                transformStruct.LossyScale);
        }

        /// <summary>
        ///   <para>Transforms position from local space to world space.</para>
        /// </summary>
        public static Vector3 TransformPoint(Vector3 point, Vector3 position, Quaternion rotation, Vector3 lossyScale)
        {
            return position + rotation * Vector3.Scale(lossyScale, point);
        }

        public static Vector3 Parabola(Vector3 start, Vector3 end, Vector3 height, float step)
        {
            var middlePoint = MiddlePoint(start, end, -height);
            var m1 = Vector3.Lerp(start, middlePoint, step);
            var m2 = Vector3.Lerp(middlePoint, end, step);
            return Vector3.Lerp(m1, m2, step);
        }

        private static Vector3 MiddlePoint(Vector3 start, Vector3 end, Vector3 height)
        {
            var result = start + (end - start) / 2 + height;
            return result;
        }

        /// <summary>
        /// Multiplies Vector3 and Quaternion
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Vector3 Multiply(Vector3 vector, Quaternion quaternion)
        {
            // Vector
            var x = vector.x;
            var y = vector.y;
            var z = vector.z;
            // Quaternion
            var qx = quaternion.x;
            var qy = quaternion.y;
            var qz = quaternion.z;
            var qw = quaternion.w;
            // Quaternion * Vector
            var ix = qw * x + qy * z - qz * y;
            var iy = qw * y + qz * x - qx * z;
            var iz = qw * z + qx * y - qy * x;
            var iw = -qx * x - qy * y - qz * z;
            var result = Vector3.zero;
            // Final Quaternion * Vector = Result
            result.x = ix * qw + iw * -qx + iy * -qz - iz * -qy;
            result.y = iy * qw + iw * -qy + iz * -qx - ix * -qz;
            result.z = iz * qw + iw * -qz + ix * -qy - iy * -qx;
            return result;
        }

        /// <summary>
        /// Check absolute min and max speed limits
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static Vector3 CheckMinMaxSpeed(Vector3 speed, float min, float max)
        {
            var bufferSpeed = speed;
            bufferSpeed = CheckMinAbsolute(speed, min);
            bufferSpeed = CheckMaxAbsolute(bufferSpeed, max);
            return bufferSpeed;
        }

        /// <summary>
        /// Check absolute min limits for Vector3
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="min"></param>
        public static Vector3 CheckMinAbsolute(Vector3 speed, float min)
        {
            var bufferSpeed = speed;
            bufferSpeed.x = CheckMinW(speed.x, min);
            bufferSpeed.y = CheckMinW(speed.y, min);
            bufferSpeed.z = CheckMinW(speed.z, min);
            return bufferSpeed;
        }

        /// <summary>
        /// Check absolute min limits for float
        /// </summary>
        /// <param name="w"></param>
        /// <param name="min"></param>
        public static float CheckMinW(float w, float min)
        {
            if (Mathf.Abs(w) < min)
                w = 0f;
            return w;
        }

        /// <summary>
        /// Check absolute max limits for float
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="max"></param>
        public static Vector3 CheckMaxAbsolute(Vector3 speed, float max)
        {
            var clampX = Mathf.Clamp(speed.x, -max, max);
            var clampY = Mathf.Clamp(speed.y, -max, max);
            var clampZ = Mathf.Clamp(speed.z, -max, max);
            return new Vector3(clampX, clampY, clampZ);
        }

        /// <summary>
        /// Check that point in two other points
        /// </summary>
        /// <param name="point"></param>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        private static bool IsInTwoPoints(float point, float a1, float a2)
        {
            return a1 >= point && point >= a2;
        }

        /// <summary>
        /// Check two segments for intersects like segments and like lines
        /// Return bool = IntersectsOnSegments
        /// </summary>
        /// <param name="firstLinePoint1"></param>
        /// <param name="firstLinePoint2"></param>
        /// <param name="secondLinePoint1"></param>
        /// <param name="secondLinePoint2"></param>
        /// <param name="firstLineClosestPoint"></param>
        /// <param name="secondLineClosestPoint"></param>
        /// <returns></returns>
        public static LineIntersection LineLineIntersection(Vector3 firstLinePoint1, Vector3 firstLinePoint2,
            Vector3 secondLinePoint1, Vector3 secondLinePoint2, out Vector3 firstLineClosestPoint,
            out Vector3 secondLineClosestPoint)
        {
            var marginOfError = 0.01f;

            var r = firstLinePoint2 - firstLinePoint1;
            var s = secondLinePoint2 - secondLinePoint1;
            var q = firstLinePoint1 - secondLinePoint1;

            var dotqr = Vector3.Dot(q, r);
            var dotqs = Vector3.Dot(q, s);
            var dotrs = Vector3.Dot(r, s);
            var dotrr = Vector3.Dot(r, r);
            var dotss = Vector3.Dot(s, s);

            var denom = dotrr * dotss - dotrs * dotrs;
            var numer = dotqs * dotrs - dotqr * dotss;

            var t = numer / denom;
            var u = (dotqs + t * dotrs) / dotss;

            // The two points of intersection
            firstLineClosestPoint = firstLinePoint1 + t * r;
            secondLineClosestPoint = secondLinePoint1 + u * s;

            // Is the intersection occuring along both line segments and does it intersect
            var intersectsOnSegments = false;
            var intersectsOnLines = false;

            if (0 <= t && t <= 1 && 0 <= u && u <= 1) intersectsOnSegments = true;
            if ((firstLineClosestPoint - secondLineClosestPoint).magnitude <= marginOfError) intersectsOnLines = true;

            return new LineIntersection(intersectsOnLines, intersectsOnSegments);
        }

        public static Vector3 Average(NativeArray<Vector3> vectors)
        {
            var t = Vector3.zero;
            for (var index = 0; index < vectors.Length; index++)
            {
                var vector = vectors[index];
                t += vector;
            }

            return t / vectors.Length;
        }

        public static Vector3 Average(params Vector3[] vectors)
        {
            var t = Vector3.zero;
            foreach (var vector in vectors) t += vector;
            return t / vectors.Length;
        }

        public static Vector3 MiddlePoint(Vector3 start, Vector3 end)
        {
            var t = start + end;
            return t / 2;
        }

        public static Vector2 MiddlePoint(Vector2 start, Vector2 end)
        {
            var t = start + end;
            return t / 2;
        }
    }
}