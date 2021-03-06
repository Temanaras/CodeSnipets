﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralLevelGeneration {
    public class Level {
        int levelWidth;
        int levelHeight;
        public int NumberOfRooms;
        Room[,] level;//level is represented by a 2D array of rooms
        MyVector2 startingRoom; //This is a a holding point for the beginning of the map.
        Random randomNumber;
        public int MaxX, MaxY, MinX, MinY; //hold the maximum and minimum of the x and y to track the edges of the map to ensure no empty doors. 

        /// <summary>
        /// COnstructor for level
        /// </summary>
        /// <param name="incommingHeight">Height for the level</param>
        /// <param name="incommingWidth">Width fort he level</param>
        public Level(int incommingHeight, int incommingWidth) {
            level = new Room[incommingHeight, incommingWidth];
            levelHeight = incommingHeight;
            levelWidth = incommingWidth;
            MaxX = -1;
            MaxY = -1;
            MinX = 1000;
            MinY = 1000;
            startingRoom = new MyVector2(levelWidth / 2, levelHeight / 2);
        }
        /// <summary>
        /// Generates a level and returns the 2D array for the level 
        /// </summary>
        /// <returns>2D array of rooms represneting the level</returns>
        public Room[,] GenerateLevel() {
            NumberOfRooms = 0;
            randomNumber = new Random();
            level[startingRoom.x, startingRoom.y] = new Room(startingRoom, 4, eDirection.NORTH);

            DrunkWalk(new MyVector2(startingRoom.x, startingRoom.y), null, eDirection.NORTH);
            findMaxAndMin();
            cleanupEdges();
            return level;
        }
        /// <summary>
        /// Recursive method to generate a level
        /// </summary>
        /// <param name="startingPoint">The room to be started from</param>
        /// <param name="previousPoint">The previous room for seting proper adjacencies</param>
        /// <param name="commingFrom">The direction it came from</param>
        /// <returns></returns>
        public Room DrunkWalk(MyVector2 startingPoint, MyVector2 previousPoint, eDirection commingFrom) {

            if (startingPoint.x >= levelWidth - 1 || startingPoint.y >= levelHeight - 1) {//return previsou room if new room is out of bounds
                return level[previousPoint.x, previousPoint.y];
            }
            if (startingPoint.x < 0 || startingPoint.y < 0) {//return previsou room if new room is out of bounds
                return level[previousPoint.x, previousPoint.y];
            }

            if (previousPoint != null && level[startingPoint.x, startingPoint.y] != null) {//if the room is already made and it is not the first iteration, collapse chain
                return level[startingPoint.x, startingPoint.y];
            }
            if (level[startingPoint.x, startingPoint.y] == null) {//if room at location is null, make a new room there
                int tempNumber = randomNumber.Next(1, 101);
                int numOfAdjacents = 0;//probability control on what rooms get generated
                if (tempNumber < 10) {
                    numOfAdjacents = 4;
                }
                else if (tempNumber < 31) {
                    numOfAdjacents = 3;
                }
                else if (tempNumber < 61) {
                    numOfAdjacents = 2;
                }
                else if (tempNumber < 101) {
                    numOfAdjacents = 1;
                }
                level[startingPoint.x, startingPoint.y] = new Room(new MyVector2(startingPoint.x, startingPoint.y), numOfAdjacents, commingFrom);
                ++NumberOfRooms;
            }
            if (level[startingPoint.x, startingPoint.y].AdjacencyNumber == 1) {//if the room you are in only has 1 adjacency, collapse
                return level[startingPoint.x, startingPoint.y];
            }
            if (previousPoint != null) {//if the previous room is not null, set the adjacency for the new room to be the previous room
                level[startingPoint.x, startingPoint.y].AddAdjacency(level[previousPoint.x, previousPoint.y], commingFrom);
            }

            for (int i = 0; i < 4; i++) {//walks through each possible connection position
                if (level[startingPoint.x, startingPoint.y].CanAddAdjacency((eDirection)i)) {//is there is a connection at that direction
                    level[startingPoint.x, startingPoint.y].AddAdjacency(DrunkWalk(startingPoint.GetDirection((eDirection)i), startingPoint, (eDirection)i), (eDirection)i);
                }
            }
            return level[startingPoint.x, startingPoint.y];//deafault point

        }
        /// <summary>
        /// Finds the min and max, used to assist in ensuring no open doors
        /// </summary>
        private void findMaxAndMin() {
            for (int i = 0; i < levelWidth; i++) {
                for (int j = 0; j < levelHeight; j++) {
                    if (level[i, j] != null) {
                        if (j > MaxX) {
                            MaxX = j;
                        }
                        if (i > MaxY) {

                            MaxY = i;
                        }
                        if (i < MinY) {
                            MinY = i;
                        }
                        if (j < MinX) {
                            MinX = j;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Cleans up the outer edges
        /// </summary>
        private void cleanupEdges() {
            for (int i = 0; i < levelWidth; i++) {
                for (int j = 0; j < levelHeight; j++) {
                    if (level[i, j] == null) {
                        continue;
                    }
                    if (i == MinY) {
                        level[i, j].ForceAdjacency(null, eDirection.WEST);
                    }
                    if (i == MaxY) {
                        level[i, j].ForceAdjacency(null, eDirection.EAST);
                    }
                    if (j == MinX) {
                        level[i, j].ForceAdjacency(null, eDirection.SOUTH);
                    }
                    if (j == MaxX) {
                        level[i, j].ForceAdjacency(null, eDirection.NORTH);
                    }
                }
            }
        }
        /// <summary>
        /// Function to draw the level to strings, used for debugging
        /// </summary>
        public void DrawLevel() {
            for (int i = 0; i < levelWidth; i++) {
                Console.Write("_");
                for (int j = 0; j < levelHeight; j++) {
                    if (level[i, j] != null) {
                        Console.Write("|{0}", level[i, j].AdjacencyNumber);
                    }
                    else {
                        Console.Write("| ");
                    }
                }
                Console.WriteLine("|");
            }
            for (int i = 0; i < levelWidth; i++) {
                Console.Write("_");
            }
            Console.WriteLine();
        }


    }
}
