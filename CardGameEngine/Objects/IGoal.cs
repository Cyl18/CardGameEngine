namespace CardGameEngine.Objects
{
    public interface IGoal
    {
        bool Achieved(Game game, out GameObject winner);
    }
}