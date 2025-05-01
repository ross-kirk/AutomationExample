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
        public static bool IsObstructed(PatrolPath path, TilemapCollider2D collider)
        {
            var start = path.transform.TransformPoint(path.startPosition);
            var end = path.transform.TransformPoint(path.endPosition);

            var samples = Mathf.CeilToInt(Vector2.Distance(start, end));

            for (var i = 0; i <= samples; i++)
            {
                var point = Vector2.Lerp(start, end, i / (float) samples);
                if (collider.OverlapPoint(point))
                {
                    Debug.DrawLine(point + Vector2.down * 0.2f, point + Vector2.up * 0.2f, Color.red, 60f);
                    return true;
                }
            }
            return false;
        }

        public static bool IsGroundValid(PatrolPath path, TilemapCollider2D collider, float enemyHeight = 1f)
        {
            var start = path.transform.TransformPoint(path.startPosition);
            var end = path.transform.TransformPoint(path.endPosition);

            var samples = Mathf.CeilToInt(Vector2.Distance(start, end));

            for (var i = 0; i <= samples; i++)
            {
                var point = Vector2.Lerp(start, end, i / (float) samples);
                var groundCheck = point + Vector2.down * enemyHeight;
                if (!collider.OverlapPoint(groundCheck))
                {
                    Debug.DrawLine(point, groundCheck, Color.yellow, 60f);
                    return false;
                }
            }

            return true;
        }

        public static bool ValidatePath(PatrolPath path, TilemapCollider2D collider,  Action<PathValidationError> onError, float enemyHeight = 1f)
        {
            if (IsObstructed(path, collider))
            {
                onError?.Invoke(PathValidationError.Obstructed);
                return false;
            }

            if (!IsGroundValid(path, collider, enemyHeight))
            {
                onError?.Invoke(PathValidationError.MissingGround);
                return false;
            }

            return true;
        }

        public enum PathValidationError
        {
            Obstructed, 
            MissingGround
        }
    }
}