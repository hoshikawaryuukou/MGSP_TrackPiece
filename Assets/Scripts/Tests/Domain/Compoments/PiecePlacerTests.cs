using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Domain.Compoments;
using NUnit.Framework;
using System;

namespace MGSP.TrackPiece.Tests.Domain
{
    [TestFixture]
    public class PiecePlacerTests
    {
        private const int BoardSize = 16;
        private PiecePlacer piecePlacer;
        private PlayerId[] board;

        [SetUp]
        public void SetUp()
        {
            piecePlacer = new PiecePlacer();
            board = new PlayerId[BoardSize];
        }

        [Test]
        public void Place_WithValidParameters_PlacesPiece()
        {
            // Arrange
            int positionIndex = 0;
            PlayerId playerId = PlayerId.Player1;

            // Act
            piecePlacer.Place(board, positionIndex, playerId);

            // Assert
            Assert.AreEqual(playerId, board[positionIndex]);
        }

        [Test]
        public void Place_WithNullBoard_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                piecePlacer.Place(null, 0, PlayerId.Player1));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(BoardSize)]
        [TestCase(BoardSize + 1)]
        public void Place_WithOutOfRangePositionIndex_ThrowsArgumentOutOfRangeException(int positionIndex)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                piecePlacer.Place(board, positionIndex, PlayerId.Player1));
        }

        [Test]
        public void Place_WithOccupiedPosition_ThrowsInvalidOperationException()
        {
            // Arrange
            int positionIndex = 0;
            board[positionIndex] = PlayerId.Player1;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                piecePlacer.Place(board, positionIndex, PlayerId.Player2));
        }

        [Test]
        public void Place_WithNonePlayerId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                piecePlacer.Place(board, 0, PlayerId.None));
        }

        [Test]
        public void Place_WithValidPositionIndices_PlacesPieces()
        {
            // Test all valid positions
            for (int i = 0; i < BoardSize; i++)
            {
                // Arrange
                var testBoard = new PlayerId[BoardSize];
                PlayerId playerId = (i % 2 == 0) ? PlayerId.Player1 : PlayerId.Player2;

                // Act
                piecePlacer.Place(testBoard, i, playerId);

                // Assert
                Assert.AreEqual(playerId, testBoard[i]);

                // Verify other positions remain empty
                for (int j = 0; j < BoardSize; j++)
                {
                    if (j != i)
                        Assert.AreEqual(PlayerId.None, testBoard[j]);
                }
            }
        }

        [Test]
        public void Place_MultipleValidPlacements_AllSucceed()
        {
            // Arrange
            int[] positions = { 0, 5, 10, 15 }; // Different positions on 4x4 board
            PlayerId[] players = { PlayerId.Player1, PlayerId.Player2, PlayerId.Player1, PlayerId.Player2 };

            // Act
            for (int i = 0; i < positions.Length; i++)
            {
                piecePlacer.Place(board, positions[i], players[i]);
            }

            // Assert
            for (int i = 0; i < positions.Length; i++)
            {
                Assert.AreEqual(players[i], board[positions[i]]);
            }
        }

        [Test]
        public void Place_WithBoundaryPositions_Succeeds()
        {
            // Test first position
            piecePlacer.Place(board, 0, PlayerId.Player1);
            Assert.AreEqual(PlayerId.Player1, board[0]);

            // Reset board and test last position
            board = new PlayerId[BoardSize];
            piecePlacer.Place(board, BoardSize - 1, PlayerId.Player2);
            Assert.AreEqual(PlayerId.Player2, board[BoardSize - 1]);
        }

        [Test]
        public void Place_WithPlayer1AndPlayer2_BothWork()
        {
            // Test placing Player1
            piecePlacer.Place(board, 0, PlayerId.Player1);
            Assert.AreEqual(PlayerId.Player1, board[0]);

            // Test placing Player2 in different position
            piecePlacer.Place(board, 1, PlayerId.Player2);
            Assert.AreEqual(PlayerId.Player2, board[1]);
        }
    }
}
