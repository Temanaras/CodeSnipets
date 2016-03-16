using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProceduralLevelGeneration {
    /// <summary>
    /// Room classs is deisgned to represent a room in level generation
    /// </summary>
    public class Room {
        MyVector2 location;
        Room[] adjacencyList;
        public bool[] AvaliableAdjacency;
        public int AdjacencyNumber;
        Random randomNumber;

        /// <summary>
        /// Constructor for Room, called by the generator and is passed the number od adjacencies and the direction is was created from.
        /// </summary>
        /// <param name="position">Position of the Room</param>
        /// <param name="numOfAdjacencies">Number of Adjacencies the room will have</param>
        /// <param name="incommingDirection">The direction the room that this room was made from.</param>
        public Room(MyVector2 position, int numOfAdjacencies, eDirection incommingDirection) {
            randomNumber = new Random();
            location = position;
            adjacencyList = new Room[4];
            AvaliableAdjacency = new bool[4];
            int tempAdjacencyCounter = numOfAdjacencies-1;//reduced by 1 since you force the adjacency in the incomming direction
            int tempRandom;
            //marks the incomming room and places an adjacency in the proper spot for it.
            if (incommingDirection==eDirection.NORTH) {
                AvaliableAdjacency[(int)eDirection.SOUTH] = true;
            }
            if (incommingDirection == eDirection.SOUTH) {
                AvaliableAdjacency[(int)eDirection.NORTH] = true;
            }
            if (incommingDirection == eDirection.EAST) {
                AvaliableAdjacency[(int)eDirection.WEST] = true;
            }
            if (incommingDirection == eDirection.WEST) {
                AvaliableAdjacency[(int)eDirection.EAST] = true;
            }
            while (tempAdjacencyCounter > 0) {
                tempRandom = randomNumber.Next(0, 4);
                if (AvaliableAdjacency[tempRandom] == false) {
                    AvaliableAdjacency[tempRandom] = true;
                    --tempAdjacencyCounter;
                }
            }
            AdjacencyNumber = numOfAdjacencies;
        }
        /// <summary>
        /// Adds and adjacency to the room.
        /// </summary>
        /// <param name="adjacentRoom">The Room you are setting as the adjacent room</param>
        /// <param name="direction">The Direction of the incomming room</param>
        /// <returns></returns>
        public bool AddAdjacency(Room adjacentRoom, eDirection direction) {
            if (AvaliableAdjacency[(int)direction] && adjacencyList[(int)direction] == null) {
                adjacencyList[(int)direction] = adjacentRoom;
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Used to force adjacencies for error handling
        /// </summary>
        /// <param name="adjacentRoom">The Room you are setting as the adjacent room</param>
        /// <param name="direction">The Direction of the incomming room</param>
        public void ForceAdjacency(Room adjacentRoom, eDirection direction) {
            if (adjacentRoom == null) {
                AvaliableAdjacency[(int)direction] = false;
                adjacencyList[(int)direction] = adjacentRoom;
            }
            else {
                AvaliableAdjacency[(int)direction] = true;
                adjacencyList[(int)direction] = adjacentRoom;
            }
        }
        /// <summary>
        /// Checks to see that the room does not already have an adjacency at the specified location.
        /// </summary>
        /// <param name="direction">The direction to check for an adjacency</param>
        /// <returns>Bool indicating if you can add an adjacency</returns>
        public bool CanAddAdjacency(eDirection direction) {
            return AvaliableAdjacency[(int)direction];
        }


        /// <summary>
        /// Get the number of empty adjacencies
        /// </summary>
        /// <returns>number of empty adjacencies</returns>
        public int GetNumOfEmptyAdjacencies() {
            int emptyAdjacencies = 0;
            for (int i = 0; i < adjacencyList.Length; i++) {
                if (AvaliableAdjacency[i] == true) {
                    ++emptyAdjacencies;
                }
            }
            return emptyAdjacencies;
        }
        /// <summary>
        /// Returns if there are ANY empty adjacencies
        /// </summary>
        /// <returns>True if there are any empty adjacencies</returns>
        public bool areEmptyAdjacencies() {
            for (int i = 0; i <= adjacencyList.Length; i++) {
                if (adjacencyList[i] == null) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Used for debugging, returns the avaliable adjacencies
        /// </summary>
        /// <returns>Formatted string bool,bool,bool,bool </returns>
        public string AdjacenciesAsString() {
            return string.Format(" {0},{1},{2},{3}",AvaliableAdjacency[0], AvaliableAdjacency[1], AvaliableAdjacency[2], AvaliableAdjacency[3]);
        }
    }
    /// <summary>
    /// My own Vector 2 class to easily retrive the directions around the point
    /// </summary>
    public class MyVector2 {//wrote my own vector to make tracking relative directions simpler
        public int x, y;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inX">X component of vector</param>
        /// <param name="inY">Y component of vector</param>
        public MyVector2(int inX, int inY) {
            x = inX;
            y = inY;
        }
        /// <summary>
        /// Gets the adjacent spot of the vector based on the current postion
        /// </summary>
        /// <param name="direction">The direction you want to get the position of</param>
        /// <returns>The relitave vector position</returns>
        public MyVector2 GetDirection(eDirection direction) {
            MyVector2 returnVector = null;
            switch (direction) {
                case eDirection.NORTH:
                    returnVector = new MyVector2(x, y + 1);
                    break;
                case eDirection.SOUTH:
                    returnVector = new MyVector2(x, y - 1);
                    break;
                case eDirection.EAST:
                    returnVector = new MyVector2(x + 1, y);
                    break;
                case eDirection.WEST:
                    returnVector = new MyVector2(x - 1, y);
                    break;
            }
            return returnVector;
        }
    }
}
