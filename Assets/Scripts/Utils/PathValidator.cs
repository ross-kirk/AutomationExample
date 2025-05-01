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

            var hitBuffer = new List<RaycastHit2D>();
            var filter = new ContactFilter2D().NoFilter();
            
            for (var i = 0; i <= samples; i++)
            {
                var point = Vector2.Lerp(start, end, i / (float) samples);
                var origin = point + Vector2.up * 0.01f;
                var hitCount = Physics2D.Raycast(origin, Vector2.down, filter, hitBuffer, rayLength);

                var hitTilemap = false;

                for (var h = 0; h < hitCount; h++)
                {
                    if (hitBuffer[h].collider != collider) continue;
                    hitTilemap = true;
                    break;
                }

                if (!hitTilemap)
                {
                    Debug.DrawLine(point, origin + Vector2.down * rayLength, Color.yellow, 60f);
                    missingGroundPoints.Add(point);
                }
            }

            return missingGroundPoints.Count == 0;
        }
        
        /// <summary>
        /// Validates an enemy PatrolPath for obstructions & missing ground collision below
        /// </summary>
        /// <param name="path">Path to validate</param>
        /// <param name="collider">Collider to check for</param>
        /// <param name="onError">Error callback, raised as (path error type, list of affected points in path).</param>
        /// <param name="enemyHeight">Height of the enemy used for the path (to easily check the ground is close enough to path)</param>
        /// <returns></returns>
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
            
            return !(obstructed || missingGround);
        }

        public enum PathValidationError
        {
            Obstructed, 
            MissingGround
        }
    }
}