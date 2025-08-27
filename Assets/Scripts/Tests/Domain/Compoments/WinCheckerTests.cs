using NUnit.Framework;
using MGSP.TrackPiece.Domain;
using System;

namespace MGSP.TrackPiece.Tests.Domain
{
    [TestFixture]
    public class WinCheckerTest
    {
        private WinChecker winChecker4x4;
        private WinChecker winChecker6x6;

        [SetUp]
        public void Setup()
        {
            var config4x4 = GameConfigTable.GetConfig(GameLevel._4x4);
            var config6x6 = GameConfigTable.GetConfig(GameLevel._6x6);

            winChecker4x4 = new WinChecker(config4x4.WinningLines, 4);
            winChecker6x6 = new WinChecker(config6x6.WinningLines, 6);
        }

        [Test]
        public void Constructor_WithNullWinningLines_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WinChecker(null, 4));
        }

        [Test]
        public void CheckWin_WithNullBoard_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => winChecker4x4.CheckWin(null));
        }

        [Test]
        public void CheckWin_EmptyBoard_ReturnsNone()
        {
            var board = new PlayerId[16];
            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.None, result);
        }

        [Test]
        public void CheckWin_4x4_Player1HorizontalWin_ReturnsPlayer1Win()
        {
            // 第一行：0, 1, 2, 3
            var board = new PlayerId[16];
            board[0] = PlayerId.Player1;
            board[1] = PlayerId.Player1;
            board[2] = PlayerId.Player1;
            board[3] = PlayerId.Player1;

            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.Player1Win, result);
        }

        [Test]
        public void CheckWin_4x4_Player2VerticalWin_ReturnsPlayer2Win()
        {
            // 第一列：0, 4, 8, 12
            var board = new PlayerId[16];
            board[0] = PlayerId.Player2;
            board[4] = PlayerId.Player2;
            board[8] = PlayerId.Player2;
            board[12] = PlayerId.Player2;

            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.Player2Win, result);
        }

        [Test]
        public void CheckWin_4x4_Player1DiagonalWin_ReturnsPlayer1Win()
        {
            // 主對角線：0, 5, 10, 15
            var board = new PlayerId[16];
            board[0] = PlayerId.Player1;
            board[5] = PlayerId.Player1;
            board[10] = PlayerId.Player1;
            board[15] = PlayerId.Player1;

            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.Player1Win, result);
        }

        [Test]
        public void CheckWin_4x4_Player2AntiDiagonalWin_ReturnsPlayer2Win()
        {
            // 反對角線：3, 6, 9, 12
            var board = new PlayerId[16];
            board[3] = PlayerId.Player2;
            board[6] = PlayerId.Player2;
            board[9] = PlayerId.Player2;
            board[12] = PlayerId.Player2;

            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.Player2Win, result);
        }

        [Test]
        public void CheckWin_4x4_FullBoardNoDraw_ReturnsDraw()
        {
            // Arrange - 直接創建平局情況的棋盤（棋盤滿但無獲勝線）
            var drawBoard = new PlayerId[16];
            // 創建一個巧妙的平局棋盤配置
            drawBoard[0] = PlayerId.Player1; drawBoard[1] = PlayerId.Player2;
            drawBoard[2] = PlayerId.Player1; drawBoard[3] = PlayerId.Player2;
            drawBoard[4] = PlayerId.Player1; drawBoard[5] = PlayerId.Player2;
            drawBoard[6] = PlayerId.Player1; drawBoard[7] = PlayerId.Player2;
            drawBoard[8] = PlayerId.Player1; drawBoard[9] = PlayerId.Player2;
            drawBoard[10] = PlayerId.Player1; drawBoard[11] = PlayerId.Player2;
            drawBoard[12] = PlayerId.Player2; drawBoard[13] = PlayerId.Player1;
            drawBoard[14] = PlayerId.Player2; drawBoard[15] = PlayerId.Player1;

            var result = winChecker4x4.CheckWin(drawBoard);
            Assert.AreEqual(GameResult.Draw, result);
        }

        [Test]
        public void CheckWin_4x4_BothPlayersWin_ReturnsDoubleWin()
        {
            var board = new PlayerId[16];
            // Player1 第一行獲勝
            board[0] = PlayerId.Player1;
            board[1] = PlayerId.Player1;
            board[2] = PlayerId.Player1;
            board[3] = PlayerId.Player1;

            // Player2 第一列獲勝
            board[4] = PlayerId.Player2;
            board[5] = PlayerId.Player2;
            board[6] = PlayerId.Player2;
            board[7] = PlayerId.Player2;

            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.DoubleWin, result);
        }

        [Test]
        public void CheckWin_6x6_Player1HorizontalWin_ReturnsPlayer1Win()
        {
            // 第一行：0, 1, 2, 3, 4, 5
            var board = new PlayerId[36];
            for (int i = 0; i < 6; i++)
            {
                board[i] = PlayerId.Player1;
            }

            var result = winChecker6x6.CheckWin(board);
            Assert.AreEqual(GameResult.Player1Win, result);
        }

        [Test]
        public void CheckWin_6x6_Player2VerticalWin_ReturnsPlayer2Win()
        {
            // 第一列：0, 6, 12, 18, 24, 30
            var board = new PlayerId[36];
            for (int i = 0; i < 6; i++)
            {
                board[i * 6] = PlayerId.Player2;
            }

            var result = winChecker6x6.CheckWin(board);
            Assert.AreEqual(GameResult.Player2Win, result);
        }

        [Test]
        public void CheckWin_6x6_Player1DiagonalWin_ReturnsPlayer1Win()
        {
            // 主對角線：0, 7, 14, 21, 28, 35
            var board = new PlayerId[36];
            for (int i = 0; i < 6; i++)
            {
                board[i * 7] = PlayerId.Player1;
            }

            var result = winChecker6x6.CheckWin(board);
            Assert.AreEqual(GameResult.Player1Win, result);
        }

        [Test]
        public void CheckWin_PartialLine_ReturnsNone()
        {
            var board = new PlayerId[16];
            // 只有3個連線，不足4個
            board[0] = PlayerId.Player1;
            board[1] = PlayerId.Player1;
            board[2] = PlayerId.Player1;
            // board[3] 留空

            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.None, result);
        }

        [Test]
        public void CheckWin_MixedLine_ReturnsNone()
        {
            var board = new PlayerId[16];
            // 混合了兩個玩家的棋子
            board[0] = PlayerId.Player1;
            board[1] = PlayerId.Player2;
            board[2] = PlayerId.Player1;
            board[3] = PlayerId.Player1;

            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.None, result);
        }

        [Test]
        public void CheckWin_LineStartsWithEmpty_ReturnsNone()
        {
            var board = new PlayerId[16];
            // 第一個位置是空的
            board[0] = PlayerId.None;
            board[1] = PlayerId.Player1;
            board[2] = PlayerId.Player1;
            board[3] = PlayerId.Player1;

            var result = winChecker4x4.CheckWin(board);
            Assert.AreEqual(GameResult.None, result);
        }
    }
}
