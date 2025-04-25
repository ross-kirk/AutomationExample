namespace Platformer.Core
{
    public interface ISimulationScheduler
    {
        T Schedule<T>(float tick = 0) where T : Simulation.Event, new();
    }
}