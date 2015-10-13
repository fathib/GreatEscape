using System;
using System.Drawing;
using GreatEscape;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreatEscapeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void No_Wall_straightforward()
        {
            Board game = new Board(3, 3, 0,2);

            game.LoadPlayersPositions(0, 0, 0);

            var path = game.FindPath(game.MyPosition);
            Assert.AreEqual(new Point(1,0), path[0]);
            Assert.AreEqual(new Point(2,0), path[1]);

        }

        [TestMethod]
        public void One_Wall_Should_Go_Down()
        {
            Board game = new Board(3, 3, 0, 2);

            game.LoadPlayersPositions(0, 0, 0);
            game.LoadWall(1, 0,"V");

            var path = game.FindPath(game.MyPosition);
            Assert.AreEqual(new Point(0, 1), path[0]);
            Assert.AreEqual(new Point(0, 2), path[1]);
            Assert.AreEqual(new Point(1, 2), path[2]);
            Assert.AreEqual(new Point(2, 2), path[3]);

        }

        [TestMethod]
        public void One_Wall_Should_Go_Up()
        {
            Board game = new Board(3, 3, 0, 2);

            game.LoadPlayersPositions(0, 2, 0);
            game.LoadWall(1, 1, "V");

            var path = game.FindPath(game.MyPosition);
            Assert.AreEqual(new Point(0, 1), path[0]);
            Assert.AreEqual(new Point(0, 0), path[1]);
            Assert.AreEqual(new Point(1, 0), path[2]);
            Assert.AreEqual(new Point(2, 0), path[3]);

        }


        [TestMethod]
        public void One_Wall_2Possible_Way_Should_Go_Up()
        {
            Board game = new Board(4, 4, 0, 2);

            game.LoadPlayersPositions(0,1, 0);
            game.LoadWall(1, 1, "V");

            var path = game.FindPath(game.MyPosition);
            
            Assert.AreEqual(new Point(0, 0), path[0]);
            Assert.AreEqual(new Point(1, 0), path[1]);
            Assert.AreEqual(new Point(2, 0), path[2]);
            Assert.AreEqual(new Point(3, 0), path[3]);
            
        }

        [TestMethod]
        public void One_Wall_not_near_2Possible_Way_Should_Go_Up()
        {
            Board game = new Board(4, 4, 0, 2);

            game.LoadPlayersPositions(0, 1, 0);
            game.LoadWall(2, 1, "V");

            var path = game.FindPath(game.MyPosition);

            Assert.AreEqual(new Point(1, 1), path[0]);
            Assert.AreEqual(new Point(1, 0), path[1]);
            Assert.AreEqual(new Point(2, 0), path[2]);
            Assert.AreEqual(new Point(3, 0), path[3]);

        }


        [TestMethod]
        public void One_Wall_not_connected_2Possible_Way_Should_Go_Down()
        {
            Board game = new Board(4, 4, 0, 2);

            game.LoadPlayersPositions(0, 2, 0);
            game.LoadWall(2, 1, "V");

            var path = game.FindPath(game.MyPosition);

            Assert.AreEqual(new Point(1, 2), path[0]);
            Assert.AreEqual(new Point(1, 3), path[1]);
            Assert.AreEqual(new Point(2, 3), path[2]);
            Assert.AreEqual(new Point(3, 3), path[3]);

        }

        [TestMethod]
        public void One_Wall_2Possible_Way_Should_Go_Down()
        {
            Board game = new Board(4, 4, 0, 2);

            game.LoadPlayersPositions(0, 2, 0);
            game.LoadWall(1, 1, "V");

            var path = game.FindPath(game.MyPosition);

            Assert.AreEqual(new Point(0, 3), path[0]);
            Assert.AreEqual(new Point(1, 3), path[1]);
            Assert.AreEqual(new Point(2, 3), path[2]);
            Assert.AreEqual(new Point(3, 3), path[3]);

        }

    }
}
