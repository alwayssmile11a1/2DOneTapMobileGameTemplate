using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gamekit2D
{

    //It is common to create a class to contain all of your
    //extension methods. This class must be static.
    public static class Vector2Extension
    {

        //Even though they are used like normal methods, extension
        //methods must be declared static. Notice that the first
        //parameter has the 'this' keyword followed by a Transform
        //variable. This variable denotes which class the extension
        //method becomes a part of.
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }

    public static class CameraExtension
    {
        /// <summary>
        /// Half width of camera when in orthographic mode
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static float GetHalfCameraWidth(this Camera cam)
        {
            return cam.orthographicSize * cam.aspect;
        }
    }

    public static class QuaternionExtension
    {
        /// <summary>
        /// Create a quaternion so that the red axis points towards the specified direction
        /// the rotationOffset can be used to offset the rotation
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="rotationOffset"></param>
        public static Quaternion RotateToDirection(Vector2 direction, float rotationOffset = 0)
        {
            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            return Quaternion.Euler(0, 0, rotationZ + rotationOffset);
        }
    }

    public static class TransformExtension
    {
        public static Bounds TransformBounds(this Transform transform, Bounds localBounds)
        {
            var center = transform.TransformPoint(localBounds.center);

            // transform the local extents' axes
            var extents = localBounds.extents;
            var axisX = transform.TransformVector(extents.x, 0, 0);
            var axisY = transform.TransformVector(0, extents.y, 0);
            var axisZ = transform.TransformVector(0, 0, extents.z);

            // sum their absolute value to get the world extents
            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds {center = center, extents = extents};
        }

        public static Bounds InverseTransformBounds(this Transform transform, Bounds worldBounds)
        {
            var center = transform.InverseTransformPoint(worldBounds.center);

            // transform the local extents' axes
            var extents = worldBounds.extents;
            var axisX = transform.InverseTransformVector(extents.x, 0, 0);
            var axisY = transform.InverseTransformVector(0, extents.y, 0);
            var axisZ = transform.InverseTransformVector(0, 0, extents.z);

            // sum their absolute value to get the world extents
            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds {center = center, extents = extents};
        }

        /// <summary>
        /// Rotate the transform so that the red axis points towards the specified direction
        /// the rotationOffset can be used to offset the rotation
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="direction"></param>
        /// <param name="rotationOffset"></param>
        public static void RotateToDirection(this Transform transform, Vector2 direction, float rotationOffset = 0)
        {
            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ + rotationOffset);

        }

        /// <summary>
        /// Rotate the transform so that the red axis points towards the specified position
        /// the rotationOffset can be used to offset the rotation
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="direction"></param>
        /// <param name="rotationOffset"></param>
        public static void RotateTo(this Transform transform, Vector3 targetPosition, float rotationOffset = 0)
        {
            Vector3 direction = targetPosition - transform.position;

            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ + rotationOffset);

        }

        /// <summary>
        /// Change offset of this transform based on sprite facing. 
        /// If spriteRenderer is not being flipped, transform.position = target.position + originalOffset; and vice versa
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="target"></param>
        /// <param name="spriteRenderer"></param>
        /// <param name="originalOffset"></param>
        public static void ChangeOffsetBasedOnSpriteFacing(this Transform transform, Transform target, SpriteRenderer spriteRenderer, Vector3 originalOffset)
        {
            if (!spriteRenderer.flipX)
            {
                transform.position = target.position + originalOffset;
            }
            else
            {
                transform.position = target.position + new Vector3(-originalOffset.x, originalOffset.y, 0);
            }
        }

    }

    public static class PlatformerEffector2DExtension
    {
        public static bool ValidCollision(this PlatformEffector2D effector, Vector2 velocity)
        {
            float dot = Vector2.Dot(effector.transform.up, -velocity.normalized);
            float cos = Mathf.Cos(effector.surfaceArc * 0.5f * Mathf.Deg2Rad);

            //we round both the dot & cos to 1/1000 precision to avoid undefined behaviour on edge case (e.g. side of a paltform with 180 side arc)
            dot = Mathf.Round(dot * 1000.0f) / 1000.0f;
            cos = Mathf.Round(cos * 1000.0f) / 1000.0f;

            if (dot > cos)
            {
                return true;
            }

            return false;
        }
    }

    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask layers, GameObject gameObject)
        {
            return 0 != (layers.value & 1 << gameObject.layer);
        }
    }

}