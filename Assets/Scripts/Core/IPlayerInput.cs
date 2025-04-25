namespace Platformer.Core
{
    public interface IPlayerInput
    {
        float Horizontal();
        bool JumpPressed();
        bool JumpReleased();
    }
}