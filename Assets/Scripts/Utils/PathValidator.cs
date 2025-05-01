using System;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class PathValidator
    {
        public static bool IsObstructed(PatrolPath path, TilemapCollider2D collider, out List<Vector2> obstructionPoints)
        {
            var start = path.transform.TransformPoint(path.startPosition);
            var end = path.transform.TransformPoint(path.endPosition);

            obstructionPoints = new List<Vector2>();

            var samples = Mathf.CeilToInt(Vector2.Distance(start, end));
            for (var i = 0; i <= samples; i++)
            {
                var point = Vector2.Lerp(start, end, i / (float) samples);
                if (collider.OverlapPoint(point))
                {
                    Debug.DrawLine(point + Vector2.down * 0.2f, point + Vector2.up * 0.2f, Color.red, 60f);
                    obstructionPoints.Add(point);
                }
            }
            return obstructionPoints.Count > 0;
        }

        public static bool IsGroundValid(PatrolPath path, TilemapCollider2D collider, out List<Vector2> missingGroundPoints, float enemyHeight)
        {
            var start = path.transform.TransformPoint(path.startPosition);
            var end = path.transform.TransformPoint(path.endPosition);

            missingGroundPoints = new List<Vector2>();
            
            var samples = Mathf.CeilToInt(Vector2.Distance(start, end));
            var rayLength = enemyHeight + 0.01f;

            for (var i = 0; i <= samples; i++)
            {
                var point = Vector2.Lerp(start, end, i / (float) samples);
                var origin = point + Vector2.up * 0.01f;

                var hit = Physics2D.Raycast(origin, Vector2.down, rayLength);
                
                if (hit.collider == null || !hit.collider == collider)
                {
                    Debug.DrawLine(point, origin + Vector2.down * rayLength, Color.yellow, 60f);
                    missingGroundPoints.Add(point);
                }
            }

            return missingGroundPoints.Count == 0;
        }

        public static bool ValidatePath(PatrolPath path, TilemapCollider2D collider,  Action<PathValidationError, IReadOnlyList<Vector2>> onError, float enemyHeight = 1f)
        {
            var obstructed = IsObstructed(path, collider, out var obstructedPoints);
            var missingGround = !IsGroundValid(path, collider, out var missingGroundPoints, enemyHeight);

            if (obstructed)
            {
                onError?.Invoke(PathValidationError.Obstructed, obstructedPoints);
            }
            if (missingGround)
            {
                onError?.Invoke(PathValidationError.MissingGround, missingGroundPoints);
            }
            
            return obstructed || missingGround;
        }

        public enum PathValidationError
        {
            Obstructed, 
            MissingGround
        }
    }
}