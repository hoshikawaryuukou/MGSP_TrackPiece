using MGSP.TrackPiece.Domain;
using NUnit.Framework;
using System;

namespace MGSP.TrackPiece.Tests.Domain
{
    public class GameTests
    {
        private Game game;

        [SetUp]
        public void SetUp()
        {
            game = GameBuilder.Create4x4Game();
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Assert
            Assert.AreEqual(PlayerId.Player1, game.GetActivePlayerId());
            Assert.AreEqual(GameResult.None, game.GetResult());

            var board = game.GetBoard();
            Assert.AreEqual(16, board.Length);
            for (int i = 0; i < board.Length; i++)
            {
                Assert.AreEqual(PlayerId.None, board[i]);
            }
        }

        [Test]
        public void Constructor_WithInitialBoard_ShouldInitializeCorrectly()
        {
            // Arrange
            var initialBoard = new PlayerId[16];
            initialBoard[0] = PlayerId.Player1;
            initialBoard[5] = PlayerId.Player2;
            initialBoard[10] = PlayerId.Player1;

            // Act
            var customGame = GameBuilder.CreateCustomGame(GameConfigTable.GetConfig(GameLevel._4x4), initialBoard, PlayerId.Player2);

            // Assert
            Assert.AreEqual(PlayerId.Player2, customGame.GetActivePlayerId());
            Assert.AreEqual(GameResult.None, customGame.GetResult());

            var board = customGame.GetBoard();
            Assert.AreEqual(PlayerId.Player1, board[0]);
            Assert.AreEqual(PlayerId.Player2, board[5]);
            Assert.AreEqual(PlayerId.Player1, board[10]);
            Assert.AreEqual(PlayerId.None, board[1]); // 確保其他位置為空
        }

        [Test]
        public void Constructor_WithNullInitialBoard_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Game.CreateCustom(GameConfigTable.GetConfig(GameLevel._4x4), null, PlayerId.Player1));
        }

        [Test]
        public void Constructor_WithInvalidBoardSize_ShouldThrowException()
        {
            // Arrange
            var invalidBoard = new PlayerId[15]; // 錯誤的大小

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Game.CreateCustom(GameConfigTable.GetConfig(GameLevel._4x4), invalidBoard, PlayerId.Player1));
        }

        [Test]
        public void Constructor_WithNoneAsStartingPlayer_ShouldThrowException()
        {
            // Arrange
            var validBoard = new PlayerId[16];

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Game.CreateCustom(GameConfigTable.GetConfig(GameLevel._4x4), validBoard, PlayerId.None));
        }

        [Test]
        public void CreateNew_WithNullConfig_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Game.CreateNew(null));
        }

        [Test]
        public void Constructor_WithNullConfigInFullConstructor_ShouldThrowException()
        {
            // Arrange
            var validBoard = new PlayerId[16];

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Game.CreateCustom(null, validBoard, PlayerId.Player1));
        }

        [Test]
        public void CreateNew_ShouldInitializeCorrectly()
        {
            // Act - 測試 4x4
            var game4x4 = Game.CreateNew(GameConfigTable.GetConfig(GameLevel._4x4));
            Assert.AreEqual(PlayerId.Player1, game4x4.GetActivePlayerId());
            Assert.AreEqual(GameResult.None, game4x4.GetResult());
            Assert.AreEqual(16, game4x4.GetBoard().Length);

            // Act - 測試 6x6
            var game6x6 = Game.CreateNew(GameConfigTable.GetConfig(GameLevel._6x6));
            Assert.AreEqual(PlayerId.Player1, game6x6.GetActivePlayerId());
            Assert.AreEqual(GameResult.None, game6x6.GetResult());
            Assert.AreEqual(36, game6x6.GetBoard().Length);
        }

        [Test]
        public void GetBoard_ShouldReturnCopy()
        {
            // Arrange
            game.Place(0);
            var board1 = game.GetBoard();
            var board2 = game.GetBoard();

            // Act - 修改第一個返回的陣列
            board1[1] = PlayerId.Player2;

            // Assert - 確保返回的是不同的物件，且互不影響
            Assert.AreNotSame(board1, board2);
            Assert.AreEqual(PlayerId.None, board2[1]);
        }

        [Test]
        public void Place_ShouldDelegateToPiecePlacer()
        {
            // Act
            game.Place(5);

            // Assert - 僅驗證 Place 方法有正確呼叫，具體邏輯由 PiecePlacer 負責測試
            var board = game.GetBoard();
            Assert.AreEqual(PlayerId.Player1, board[5]);
        }

        [Test]
        public void SwitchPlayer_ShouldAlternatePlayer()
        {
            // Arrange
            Assert.AreEqual(PlayerId.Player1, game.GetActivePlayerId());

            // Act & Assert
            game.SwitchPlayer();
            Assert.AreEqual(PlayerId.Player2, game.GetActivePlayerId());

            game.SwitchPlayer();
            Assert.AreEqual(PlayerId.Player1, game.GetActivePlayerId());
        }

        [Test]
        public void Shift_ShouldDelegateToBoardShifter()
        {
            // Arrange - 直接創建包含棋子的初始盤面
            var initialBoard = new PlayerId[16];
            initialBoard[0] = PlayerId.Player1;
            initialBoard[1] = PlayerId.Player2;

            game = GameBuilder.CreateCustomGame(GameConfigTable.GetConfig(GameLevel._4x4), initialBoard, PlayerId.Player1);

            // Act
            game.Shift();

            // Assert - 僅驗證 Shift 方法有正確呼叫，具體移動邏輯由 BoardShifter 負責測試
            var board = game.GetBoard();
            Assert.AreNotEqual(PlayerId.Player1, board[0]); // 確保位置有變化
            Assert.AreNotEqual(PlayerId.Player2, board[1]); // 確保位置有變化
        }

        [Test]
        public void Evaluate_ShouldDelegateToWinChecker()
        {
            // Arrange - 創建一個簡單的獲勝情況
            var winningBoard = new PlayerId[16];
            winningBoard[0] = PlayerId.Player1;
            winningBoard[1] = PlayerId.Player1;
            winningBoard[2] = PlayerId.Player1;
            winningBoard[3] = PlayerId.Player1;

            game = GameBuilder.CreateCustomGame(GameConfigTable.GetConfig(GameLevel._4x4), winningBoard, PlayerId.Player1);

            // Act
            game.Evaluate();

            // Assert - 僅驗證 Evaluate 方法有正確呼叫並更新結果，具體判斷邏輯由 WinChecker 負責測試
            Assert.AreNotEqual(GameResult.None, game.GetResult());
        }

        [Test]
        public void FullGameFlow_ShouldWorkCorrectly()
        {
            // Arrange & Act - 模擬完整的遊戲流程
            Assert.AreEqual(PlayerId.Player1, game.GetActivePlayerId());
            Assert.AreEqual(GameResult.None, game.GetResult());

            // Player1 放置棋子
            game.Place(0);
            var boardAfterPlace = game.GetBoard();
            Assert.AreEqual(PlayerId.Player1, boardAfterPlace[0]);

            // 移動棋盤
            game.Shift();
            var boardAfterShift = game.GetBoard();
            Assert.AreNotEqual(PlayerId.Player1, boardAfterShift[0]); // 確保位置有變化

            // 評估遊戲狀態
            game.Evaluate();
            // 在這個簡單例子中應該還沒有結果
            Assert.AreEqual(GameResult.None, game.GetResult());

            // 切換玩家
            game.SwitchPlayer();
            Assert.AreEqual(PlayerId.Player2, game.GetActivePlayerId());
        }
    }
}
