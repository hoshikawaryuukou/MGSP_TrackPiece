using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Domain.Compoments;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MGSP.TrackPiece.Tests.Domain
{
    public class BoardShifterTests
    {
        private const int BoardSize = 16;
        private BoardShifter boardShifter;
        private PlayerId[] board;

        [SetUp]
        public void SetUp()
        {
            var track = new int[] { 1, 2, 3, 0, 5, 6, 7, 4, 9, 10, 11, 8, 13, 14, 15, 12 };
            boardShifter = new BoardShifter(track);
            board = new PlayerId[BoardSize];
        }

        [Test]
        public void Constructor_WithNullTrack_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new BoardShifter(null));
        }

        [Test]
        public void Constructor_WithValidTrack_DoesNotThrow()
        {
            var track = new int[] { 0, 1, 2, 3 };
            Assert.DoesNotThrow(() => new BoardShifter(track));
        }

        [Test]
        public void Constructor_WithIReadOnlyListTrack_WorksCorrectly()
        {
            var track = new List<int> { 1, 0, 3, 2 };
            var shifter = new BoardShifter(track);

            var testBoard = new PlayerId[] { PlayerId.Player1, PlayerId.Player2, PlayerId.None, PlayerId.None };
            var result = shifter.Shift(testBoard);

            Assert.AreEqual(PlayerId.Player2, result[0]); // 原本位置 1 的棋子移到位置 0
            Assert.AreEqual(PlayerId.Player1, result[1]); // 原本位置 0 的棋子移到位置 1
        }

        [Test]
        public void Shift_WithNullBoard_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => boardShifter.Shift(null));
        }

        [Test]
        public void Shift_WithWrongBoardSize_ThrowsArgumentException()
        {
            var wrongSizeBoard = new PlayerId[8]; // 期望 16，但給了 8

            var exception = Assert.Throws<ArgumentException>(() => boardShifter.Shift(wrongSizeBoard));
            Assert.That(exception.Message, Does.Contain($"Board must have exactly {BoardSize} positions"));
        }

        [Test]
        public void Shift_WithEmptyBoard_ReturnsEmptyBoard()
        {
            var result = boardShifter.Shift(board);

            Assert.AreEqual(BoardSize, result.Length);
            for (int i = 0; i < BoardSize; i++)
            {
                Assert.AreEqual(PlayerId.None, result[i]);
            }
        }

        [Test]
        public void Shift_WithSinglePiece_MovesCorrectly()
        {
            // 在位置 0 放置 Player1 的棋子
            board[0] = PlayerId.Player1;

            var result = boardShifter.Shift(board);

            // 根據軌道，位置 0 的棋子應該移動到位置 1
            Assert.AreEqual(PlayerId.Player1, result[1]);
            Assert.AreEqual(PlayerId.None, result[0]);
        }

        [Test]
        public void Shift_WithMultiplePieces_MovesAllCorrectly()
        {
            // 設置初始棋盤狀態
            board[0] = PlayerId.Player1;  // 會移動到位置 1
            board[1] = PlayerId.Player2;  // 會移動到位置 2  
            board[4] = PlayerId.Player1;  // 會移動到位置 5
            board[8] = PlayerId.Player2;  // 會移動到位置 9

            var result = boardShifter.Shift(board);

            Assert.AreEqual(PlayerId.Player1, result[1]);  // 0->1
            Assert.AreEqual(PlayerId.Player2, result[2]);  // 1->2
            Assert.AreEqual(PlayerId.Player1, result[5]);  // 4->5
            Assert.AreEqual(PlayerId.Player2, result[9]);  // 8->9

            // 確認原來的位置已經清空或被其他棋子占據
            Assert.AreEqual(PlayerId.None, result[0]);
            Assert.AreEqual(PlayerId.None, result[4]);
            Assert.AreEqual(PlayerId.None, result[8]);
        }

        [Test]
        public void Shift_WithFullBoard_MovesAllPiecesCorrectly()
        {
            // 填滿整個棋盤
            for (int i = 0; i < BoardSize; i++)
            {
                board[i] = (i % 2 == 0) ? PlayerId.Player1 : PlayerId.Player2;
            }

            var result = boardShifter.Shift(board);

            // 驗證每個位置都有棋子移動到
            for (int i = 0; i < BoardSize; i++)
            {
                Assert.AreNotEqual(PlayerId.None, result[i], $"Position {i} should not be empty");
            }

            // 驗證棋子總數保持不變
            int player1Count = 0, player2Count = 0;
            for (int i = 0; i < BoardSize; i++)
            {
                if (result[i] == PlayerId.Player1) player1Count++;
                else if (result[i] == PlayerId.Player2) player2Count++;
            }
            var expectedPlayerCount = BoardSize / 2;
            Assert.AreEqual(expectedPlayerCount, player1Count);
            Assert.AreEqual(expectedPlayerCount, player2Count);
        }

        [Test]
        public void Shift_WithCyclicTrack_HandlesCorrectly()
        {
            // 測試循環軌道：0->1->2->3->0
            var cyclicTrack = new int[] { 1, 2, 3, 0 };
            var shifter = new BoardShifter(cyclicTrack);
            var testBoard = new PlayerId[] { PlayerId.Player1, PlayerId.Player2, PlayerId.None, PlayerId.None };

            var result = shifter.Shift(testBoard);

            Assert.AreEqual(PlayerId.None, result[0]); // 原本位置 3 是空的，移動到位置 0
            Assert.AreEqual(PlayerId.Player1, result[1]); // 原本位置 0 的 Player1 移動到位置 1
            Assert.AreEqual(PlayerId.Player2, result[2]); // 原本位置 1 的 Player2 移動到位置 2
            Assert.AreEqual(PlayerId.None, result[3]); // 原本位置 2 是空的，移動到位置 3
        }

        [Test]
        public void Shift_DoesNotModifyOriginalBoard()
        {
            board[0] = PlayerId.Player1;
            board[5] = PlayerId.Player2;

            var originalBoard = (PlayerId[])board.Clone();

            boardShifter.Shift(board);

            // 驗證原始棋盤沒有被修改
            for (int i = 0; i < board.Length; i++)
            {
                Assert.AreEqual(originalBoard[i], board[i], $"Original board was modified at position {i}");
            }
        }

        [Test]
        public void Shift_ReturnsNewBoardInstance()
        {
            var result = boardShifter.Shift(board);

            Assert.AreNotSame(board, result, "Should return a new board instance");
        }
    }
}
