using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// DeathZone components mark a collider which will schedule a
    /// PlayerEnteredDeathZone event when the player enters the trigger.
    /// </summary>
    public class DeathZone : MonoBehaviour
    {
        internal ISimulationScheduler scheduler = new SimulationScheduler();
        
        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = scheduler.Schedule<PlayerEnteredDeathZone>();
                ev.deathzone = this;
            }
        }
    }
}