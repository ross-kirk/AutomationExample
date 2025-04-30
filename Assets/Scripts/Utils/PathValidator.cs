using System;
using System.Collections.Generic;
using System.Linq;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class PathValidator
    {
        public static bool ValidatePathSampled(PatrolPath path, int sampleCount, Action<PathValidationError> onPathError)
        {
            var pathStart = path.transform.TransformPoint(path.startPosition);
            var pathEnd = path.transform.TransformPoint(path.endPosition);
            var direction = (pathEnd - pathStart).normalized;
            var distance = Vector2.Distance(pathStart, pathEnd);

            var hit = Physics2D.Raycast(pathStart, direction, distance);
            if (hit.collider is TilemapCollider2D)
            {
                onPathError?.Invoke(PathValidationError.Blocked);
                return false;
            }
            
            for (var i = 0; i <= sampleCount; i++)
            {
                var t = i / (float)sampleCount;
                var pos = Vector2.Lerp(pathStart, pathEnd, t);
                var rayStart = pos + Vector2.up * 0.1f;
                var groundHit = Physics2D.Raycast(rayStart, Vector2.down, 1.5f);
                var isOverGround = groundHit.collider is TilemapCollider2D;
                Debug.DrawRay(pos, Vector2.down * 1.5f, isOverGround ? Color.yellow : Color.blue, 60f);

                if (!isOverGround)
                {
                    onPathError?.Invoke(PathValidationError.MissingGround);
                    return false;
                }
            }
            return true;
        }

        public static bool ValidatePathByTile(PatrolPath path, Action<PathValidationError> onPathError)
        {
            var start = path.transform.TransformPoint(path.startPosition);
            var end = path.transform.TransformPoint(path.endPosition);
            var distance = Vector2.Distance(start, end);
            var sampleCount = Mathf.CeilToInt(distance);
            return ValidatePathSampled(path, sampleCount, onPathError);
        }

        public enum PathValidationError
        {
            Blocked, 
            MissingGround
        }
    }
}