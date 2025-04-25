namespace Platformer.Core
{
    public class SimulationScheduler : ISimulationScheduler
    {
        public T Schedule<T>(float tick = 0) where T : Simulation.Event, new() => Simulation.Schedule<T>(tick);
    }
}