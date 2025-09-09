using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace MGSP.TrackPiece.Services
{
    public enum GameLevel { _4x4, _6x6 }
    public enum GamePlayer { White, Black }

    public interface IGameStageEvent { }

    public interface IGameService
    {
        UniTask<IReadOnlyList<IGameStageEvent>> CreateNewGame(GameLevel level);
        UniTask<IReadOnlyList<IGameStageEvent>> PlacePiece(int positionIndex);
    }
}
