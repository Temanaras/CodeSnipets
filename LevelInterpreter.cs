using UnityEngine;
using System.Collections;
using ProceduralLevelGeneration;

public class LevelInterpreter : MonoBehaviour {
    public GameObject Wall;
    public GameObject Door;
    public GameObject[] floors;
    GameObject FinishedLevel;
    Stack floorsInLevel;
    Level newLevel;
    Room[,] level;
    public int WallSizeOffset;
    public int LevelHeight, LevelWidth;
    // Use this for initialization


    public GameObject BuildLevel() {
        floorsInLevel = new Stack();
        newLevel = new Level(LevelHeight, LevelWidth);
        level = newLevel.GenerateLevel();
        placeFloors();
        FinishedLevel = new GameObject();
        FinishedLevel.name = "Level";
        for (int i = 0; i < LevelWidth; i++) {
            for (int j = 0; j < LevelHeight; j++) {
                if (level[i, j] != null) {
                    GameObject newRoom = new GameObject();
                    newRoom.name = string.Format("Room:{0}{1}: ", i, j);
                    buildRoom(level[i, j], newRoom, i, j);
                    GameObject newFloor = Instantiate(floorsInLevel.Pop() as GameObject);
                    newRoom.name += newFloor.name;
                    newFloor.transform.parent = newRoom.transform;
                    newRoom.transform.position = new Vector2(i * WallSizeOffset, j * WallSizeOffset);
                    newRoom.transform.parent = FinishedLevel.transform;
                }
            }
        }
        return FinishedLevel;
    }
    public GameObject GetEntryPoint() {
        foreach (Transform room in FinishedLevel.transform ) {
            if (room.name.Contains("Entry")) {
                return room.gameObject;
            }
        }
        return null;
    }
    public GameObject GetExitPoint() {
        foreach (Transform room in FinishedLevel.transform) {
            if (room.name.Contains("Exit")) {
                return room.gameObject;
            }
        }
        return null;
    }
    private void placeFloors() {
        GameObject[] floorsToPlace = new GameObject[newLevel.NumberOfRooms];
        floorsToPlace[0] = floors[(int)eFloors.ENTRY];
        floorsToPlace[1] = floors[(int)eFloors.EXIT];
        float floorNumToPLace;
        eFloors nextFloorPlaced;
        for (int i = 2; i < newLevel.NumberOfRooms; i++) {
            floorNumToPLace = Random.value;
            if (floorNumToPLace<.2f) {
                nextFloorPlaced = eFloors.TREASURE;
            }
            else {
                nextFloorPlaced = eFloors.NORMAL;
            }
            floorsToPlace[i] =floors[(int)nextFloorPlaced];
        }

        shuffle(ref floorsToPlace);
        floorsInLevel.Push(floors[(int)eFloors.NORMAL]);
        for (int i = 0;i< floorsToPlace.Length;i++) {
            floorsInLevel.Push(floorsToPlace[i]);
        }
    }

    public void buildRoom(Room incommingRoom, GameObject outgoingRoom, int incommingX, int incommingY) {
        //GameObject[] sides = new GameObject[4];

        for (int i = 0; i < 4; i++) {
            if (incommingRoom.IsAdjacencyAt((eDirection)i)) {
                GameObject tempDoor = Instantiate(Door); ;
                tempDoor.transform.parent = outgoingRoom.transform;
                adjustItemByDirection(tempDoor, (eDirection)i);
            }
            else {
                GameObject tempWall = Instantiate(Wall);
                tempWall.transform.parent = outgoingRoom.transform;
                adjustItemByDirection(tempWall, (eDirection)i);
            }
        }
    }
    private void adjustItemByDirection(GameObject wall, eDirection direction) {
        if (direction == eDirection.NORTH) {
            wall.transform.localPosition += new Vector3(0, WallSizeOffset / 2, 0);
            wall.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        if (direction == eDirection.SOUTH) {
            wall.transform.localPosition -= new Vector3(0, WallSizeOffset / 2, 0);
            wall.transform.localRotation = Quaternion.Euler(0, 0, 360);
        }
        if (direction == eDirection.EAST) {
            wall.transform.localPosition += new Vector3(WallSizeOffset / 2, 0, 0);
            wall.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        if (direction == eDirection.WEST) {
            wall.transform.localPosition -= new Vector3(WallSizeOffset / 2, 0, 0);
            wall.transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    private static void shuffle<T>(ref T[] array) {

        int n = array.Length;
        while(n>=1){
            n--;
            int k = Random.Range(0,n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }

    }

}
