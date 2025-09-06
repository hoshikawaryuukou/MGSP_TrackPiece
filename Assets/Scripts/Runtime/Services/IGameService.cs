using System.Collections.Generic;

namespace MGSP.TrackPiece.Services
{
    public enum GameLevel { _4x4, _6x6 }

    public enum GamePlayer { White, Black }

    public interface IGameStageEvent { }

    public interface IGameService
    {
        IList<IGameStageEvent> CreateNewGame(GameLevel level);
        IList<IGameStageEvent> Place(int positionIndex);
    }
}
