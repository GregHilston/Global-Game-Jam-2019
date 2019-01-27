using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map generator.
/// Referenced https://unity3d.com/learn/tutorials/topics/scripting/basic-2d-dungeon-generation
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public Camera camera;
    public string seed;
    private System.Random random;
    private const int rows = 25;
    private const int firstNonWallRow = 1; // skipping the 0th row, as its a wall
    private const int lastNonWallRow = rows - 2; // -2, 1 for o based and 1 for skipping last row, as its a wall
    private const int lastRow = rows - 1; // -1 to convert from 0 based to 1 based
    private const int firstNonWallColumn = 1; // skipping hthe 0th column, as its a wall
    private const int columns = 25;
    private const int lastNonWallColumn = columns - 2; // -2, 1 for o based and 1 for skipping last column, as its a wall
    private const int lastColumn = columns - 1; // -1 to convert from 0 based to 1 based
    private const int roomArea = columns * rows;
    private const int minRoomSize = 2;
    private const int maxRoomSize = 6;
    private TileType[][] board;

    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor, ExternalDoor, RoomPoint, InteralDoor, DoorWalkWay, GrabbableObject, SecurityCamera
        // RoomPoint will act as a spawn location for enemies
    }

    private enum CardinalDirection
    {
        North, South, East, West,
    }

	#region Wall Testing Stuff

	// This is all the stuff involved in testing walls

	List<(int, int)> Intersections;

	class Wall
	{
		public (int, int) Pt0 { get; set; } // This should be one intersection
		public (int, int) Pt1 { get; set; } // This should be second intersection
		public List<(int, int)> DissolvableWalls { get; private set; } // These are walls between
		public int ID { get; private set; }

		public Wall(int id)
		{
			ID = id;
			DissolvableWalls = new List<(int, int)>();
		}
	}

	class Tracker
	{
		private int thisInt;
		public int GetInt { get { int copyInt = thisInt; thisInt++; return copyInt; } private set { thisInt = value; } }

		public Tracker()
		{
			thisInt = 0;
		}
	}

	List<Wall> AllWalls;

	bool[][] hasThisSpaceBeenChecked;

	#endregion

	// Start is called before the first frame update
	void Start()
    {
        this.seed = "potatoesd";
        //this.GenerateMap();

		// All the wall stuff
		Intersections = new List<(int, int)>();
		AllWalls = new List<Wall>();
		hasThisSpaceBeenChecked = new bool[rows][];
		for (int i = 0; i < rows; i++)
		{
			hasThisSpaceBeenChecked[i] = new bool[columns];
			for (int j = 0; j < columns; j++)
			{
				hasThisSpaceBeenChecked[i][j] = false;
			}
		}

		GenerateMap();
	}

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (board != null)
        {
            // 2d has a zero z plane
            Vector3 startingPositionTopLeft = this.camera.ScreenToWorldPoint(new Vector3(0, this.camera.pixelHeight, this.camera.nearClipPlane));

            Color[] tileColors = { Color.black, Color.gray, Color.white, Color.cyan, new Color(255, 99, 71), Color.yellow };

            for (int row = 0; row < this.board.Length; row++)
            {
                for (int column = 0; column < this.board[row].Length; column++)
                {
                    switch (this.board[row][column])
                    {
                        case TileType.Wall:
                            {
                                Gizmos.color = tileColors[0];
                                break;
                            }
                        case TileType.Floor:
                            {
                                Gizmos.color = tileColors[1];
                                break;
                            }
                        case TileType.ExternalDoor:
                            {
                                Gizmos.color = tileColors[2];
                                break;
                            }
                        case TileType.RoomPoint:
                            {
                                Gizmos.color = tileColors[3];
                                break;
                            }
                    }

                    const float sharedUnit = 1.0f;
                    const float baseXOffset = sharedUnit;
                    const float baseYOffset = sharedUnit;
                    const float size = 0.8f;

                    Gizmos.DrawCube(new Vector3(startingPositionTopLeft.x + baseXOffset * row, startingPositionTopLeft.y + baseYOffset * -column, 0.0f), new Vector3(size, size, size));
                }
            }
        }
    }

    /// <summary>
    /// Creates the MD5 hash of a given input.
    /// From: https://stackoverflow.com/a/24031467/1983957
    /// </summary>
    /// <returns>The MD5 Hash</returns>
    /// <param name="input">Input stirng to calculate.</param>
    private string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    private int CalculateNumberOfRooms(string input)
    {
        // const int indexOfInterest = 0;

        return 6;
    }

    private int CalculateNumberOfPeopleHome(string input)
    {
        // const int indexOfInterest = 1;

        return 1;
    }

    private int CalculateNumberOfPetsHome(string input)
    {
        // const int indexOfInterest = 2;

        return 1;
    }

    private int CalculateNumberOfObjectsToMove(string input)
    {
        // const int indexOfInterest = 3;

        return 3;
    }

    private int CalculateNumberOfCameras(string input)
    {
        // const int indexOfInterest = 4;

        return 1;
    }

    private int CalculateUnityRandomSeed(string input)
    {
        const int indexOfInterestStart = 5;
        const int indexOfInterestEnd = 8;

        string unityRandomSeed = input.Substring(indexOfInterestStart, indexOfInterestEnd);
        int integerRepresentation = int.Parse(unityRandomSeed, System.Globalization.NumberStyles.HexNumber);

        return integerRepresentation;
    }

    private void InitializeBoard()
    {
        this.board = new TileType[rows][];

        for (int row = 0; row < this.board.Length; row++)
        {
            this.board[row] = new TileType[columns];

            for (int column = 0; column < this.board[row].Length; column++)
            {
                this.board[row][column] = TileType.Floor;
                
            }
        }
    }

    private void AddOuterWalls()
    {
        for (int row = 0; row < this.board.Length; row++)
        {
            for (int column = 0; column < this.board[row].Length; column++)
            {
                const int firstRowOrColumn = 0;
               if (row == firstRowOrColumn || row == lastRow)
                {
                    board[row][column] = TileType.Wall;
                } else if (column == firstRowOrColumn || column == lastColumn)
                {
                    board[row][column] = TileType.Wall;
                }

            }
        }
    }

    /// <summary>
    /// Generates the random outer row. Always using the first or the last row
    /// for simplicity and so we can use any column we want.
    /// </summary>
    /// <returns>The random outer row.</returns>
    private int GenerateRandomOuterRow()
    {
        int randomInt = this.random.Next(0, 1);

        if (randomInt == 0)
        {
            return 0;
        }
        else
        {
            return lastRow;
        }
    }

    /// <summary>
    /// Generates the random outer column. To be used in combination with a 
    /// first row or last row to ensure we're external.
    /// </summary>
    /// <returns>The random outer column.</returns>
    private int GenerateRandomOuterColumn()
    {
        int secondRow = 1; // ensuring we can't randomize to 0 and be in the corner
        int secondToLastColumn = lastColumn - 1;

        return this.random.Next(secondRow, secondToLastColumn);
    }

    private void AddExternalDoor()
    {
        int externalRow = this.GenerateRandomOuterRow();
        int externalColumn = this.GenerateRandomOuterColumn();

        board[externalRow][externalColumn] = TileType.ExternalDoor;
    }

    private (int, int) GenerateStep(int row, int column, CardinalDirection cardinalDirection)
    {
        switch (cardinalDirection)
        {
            case CardinalDirection.North:
                {
                    return (row - 1, column);
                }
            case CardinalDirection.East:
                {
                    return (row, column - 1);
                }
            case CardinalDirection.South:
                {
                    return(row + 1, column);
                }
            case CardinalDirection.West:
                {
                    return(row, column + 1);
                }
        }

        throw new System.Exception("An unexpected CardinalDirection was passed"); // THIS SHOULD NEVER HAPPEN
    }

    private int calculateShortestDistanceToOtherRoomPointsAndOuterWalls()
    {
        int shortestDistance = int.MaxValue;
        List<(int, int)> roomPoints = new List<(int, int)>();

        for (int row = 0; row < this.board.Length; row++)
        {
            for (int column = 0; column < this.board[row].Length; column++)
            {
                if (board[row][column] == TileType.RoomPoint)
                {
                    roomPoints.Add((row, column));
                }
            }
        }

        for (int i = 0; i < roomPoints.Count; i++)
        {
            for (int j = i + 1; j < roomPoints.Count; j++)
            {
                int distanceBetweenIAndJ = Mathf.Abs(roomPoints[i].Item1 - roomPoints[j].Item1) + Mathf.Abs(roomPoints[i].Item2 - roomPoints[j].Item2);
                if (distanceBetweenIAndJ < shortestDistance)
                {
                    shortestDistance = distanceBetweenIAndJ;
                }
            }
        }
        
        return shortestDistance;
    }

    private int ClosestRoomPointsByRowOrColumnOrExternalWall()
    {
        int shortestDistance = int.MaxValue;
        List<(int, int)> roomPoints = new List<(int, int)>();

        for (int row = 0; row < this.board.Length; row++)
        {
            for (int column = 0; column < this.board[row].Length; column++)
            {
                if (board[row][column] == TileType.RoomPoint)
                {
                    roomPoints.Add((row, column));
                }
            }
        }

        for (int i = 0; i < roomPoints.Count; i++)
        {
            for (int j = i + 1; j < roomPoints.Count; j++)
            {
                int rowDistanceBetweenIAndJ = Mathf.Abs(roomPoints[i].Item1 - roomPoints[j].Item1);

                if (rowDistanceBetweenIAndJ < shortestDistance)
                {
                    shortestDistance = rowDistanceBetweenIAndJ;
                }

                int columnDistanceBetweenIAndJ = Mathf.Abs(roomPoints[i].Item2 - roomPoints[j].Item2);

                if (columnDistanceBetweenIAndJ < shortestDistance)
                {
                    shortestDistance = columnDistanceBetweenIAndJ;
                }

                // Rows Walls
                int distanceToFirstRow = Mathf.Abs(roomPoints[i].Item1 - 0);
                if (distanceToFirstRow < shortestDistance)
                {
                    shortestDistance = distanceToFirstRow;
                }

                int distanceToLastRow = Mathf.Abs(roomPoints[i].Item1 - lastRow);
                if (distanceToLastRow < shortestDistance)
                {
                    shortestDistance = distanceToLastRow;
                }

                // Column Walls
                int distanceToFirstColumn = Mathf.Abs(roomPoints[i].Item2 - 0);
                if (distanceToFirstColumn < shortestDistance)
                {
                    shortestDistance = distanceToFirstColumn;
                }

                int distanceToLastColumn = Mathf.Abs(roomPoints[i].Item2 - lastColumn);
                if (distanceToLastColumn < shortestDistance)
                {
                    shortestDistance = distanceToLastColumn;
                }
            }
        }

        return shortestDistance;
    }

    private void RemoveRoomPoints()
    {
        for (int row = 0; row < this.board.Length; row++)
        {
            for (int column = 0; column < this.board[row].Length; column++)
            {
                if (board[row][column] == TileType.RoomPoint) {
                    board[row][column] = TileType.Floor;
                }
            }
        }
    }

    private void GenerateRoomPoints(int numberOfRoomPoints = 3)
    {
        const int minimumDistanceFromRoomPointToOuterWallsAndOtherRoomPoints = 8;
        const int minimumDistanceInRowOrColumn = 4; // this is two empty spaces between, for a total of three indexes away

        do
        {
            int numberOfRoomPointsPlaced = 0;

            this.RemoveRoomPoints();
            while (numberOfRoomPointsPlaced != numberOfRoomPoints)
            {
                int potentialRow = this.random.Next(firstNonWallRow, lastNonWallRow);
                int potentialColumn = this.random.Next(firstNonWallColumn, lastNonWallColumn);

                if (board[potentialRow][potentialColumn] == TileType.Floor)

                {
                    // we have found a room point!
                    board[potentialRow][potentialColumn] = TileType.RoomPoint;
                    numberOfRoomPointsPlaced += 1;
                }
            }
        } while (calculateShortestDistanceToOtherRoomPointsAndOuterWalls() < minimumDistanceFromRoomPointToOuterWallsAndOtherRoomPoints || ClosestRoomPointsByRowOrColumnOrExternalWall() < minimumDistanceInRowOrColumn);
    }

    private bool IsCoordinateExternal(int row, int column)
    {
        if(board[row][column] == TileType.Wall || board[row][column] == TileType.ExternalDoor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GenerateWallsFromRoomPoints()
    {
        List<(int, int)> roomPoints = new List<(int, int)>();

        for (int row = 0; row < this.board.Length; row++)
        {
            for (int column = 0; column < this.board[row].Length; column++)
            {
                if (board[row][column] == TileType.RoomPoint)
                {
                    roomPoints.Add((row, column));
                }
            }
        }

        CardinalDirection[] cardinalDirections = { CardinalDirection.North, CardinalDirection.East, CardinalDirection.South, CardinalDirection.West };

        foreach ((int, int) roomPoint in roomPoints) { 
            foreach (CardinalDirection cardinalDirection in cardinalDirections)
            {
                bool running = true;
                int currentRow = roomPoint.Item1;
                int currentColumn = roomPoint.Item2;

                do
                {
                    (int newRow, int newColumn) = GenerateStep(currentRow, currentColumn, cardinalDirection);
                    
                    if (IsCoordinateExternal(newRow, newColumn))
                    {
                        running = false;
                    }
                    else if (board[newRow][newColumn] == TileType.Floor || board[newRow][newColumn] == TileType.RoomPoint) {
                        board[newRow][newColumn] = TileType.Wall;
                        currentRow = newRow;
                        currentColumn = newColumn;
                    }
                } while (running);
            }
        }
    }

    private void ReplaceAllRoomPointsWithWalls()
    {
        for (int row = 0; row < this.board.Length; row++)
        {
            for (int column = 0; column < this.board[row].Length; column++)
            {
                if (board[row][column] == TileType.RoomPoint)
                {
                    board[row][column] = TileType.Wall;
                }
            }
        }
    }

    private void GenerateRooms() {
        this.GenerateRoomPoints();
        this.GenerateWallsFromRoomPoints();
        this.ReplaceAllRoomPointsWithWalls();
    }

	/// <summary>
	/// Determines all the walls connected in the generated map
	/// </summary>
	private void FindAllWalls()
	{
		// The logic behind this method is that every "wall" in the map is sequence
		// of wall tiles bookended by intersection points.
		// Intersection points are where walls meet.
		// Since the point of this method will eventually be to dissolve those wall
		// tiles in between those intersection points, every wall tile is considered
		// a "dissolvable" wall as long as it is not an intersection point.
		
		// Queue that checks every intersection point for walls they define.
		// Queues up the top left wall tile.
		// The dges are guaranteed to be wall tiles.
		Queue<(int, int)> tmpIntersections = new Queue<(int, int)>();
		tmpIntersections.Enqueue((0, 0));

		// Tracks which tiles we are on
		int thisRow = 0;
		int thisCol = 0;

		// Tracker object simply generates a unique integer ID for every Wall object created
		Tracker tmpTracker = new Tracker();

		// Since we need to check every direction at every intersection, this array contains
		// every direction enumeration. We will iterate through it to check all of them.
		CardinalDirection[] cardinalDirections = { CardinalDirection.North, CardinalDirection.East, CardinalDirection.South, CardinalDirection.West };

		// Not necessary, since intersection points are the one space that don't count as "checked"
		// (that's only needed to make sure we don't create two walls with switched starting and
		// ending points), but just in case...
		hasThisSpaceBeenChecked[0][0] = true;

		// Do this for every intersection you find
		while (tmpIntersections.Count > 0)
		{
			// Starts off by dequeueing and checking first item in the temporary intersection checker
			(thisRow, thisCol) = tmpIntersections.Dequeue();

			// Checks every direction from the intersection
			foreach (CardinalDirection dir in cardinalDirections)
			{
				// Creates a temporary wall to start off.
				// Dissolvable walls will be added as we find them.
				// Hitting an intersection will bookend that wall and finalize it.
				Wall tmpWall = new Wall(tmpTracker.GetInt);

				// Starts off in the direction being checked
				(int newRow, int newCol) = GenerateStep(thisRow, thisCol, dir);

				// If this tile being checked is inside the bounds of the array,
				// is either a wall or a door,
				// and has not been checked yet,
				// do further checking on it.
				while (newRow > -1 && newRow < rows && newCol > -1 && newCol < columns &&
					(board[newRow][newCol] == TileType.Wall || board[newRow][newCol] == TileType.ExternalDoor) &&
					!hasThisSpaceBeenChecked[newRow][newCol])
				{
					// We can create a unique number at the end by multiplying 1 by prime numbers,
					// based on what walls border this one being checked.
					int testVal = 1;

					// We will overwrite these temporary X and Y values a lot as we check directions
					int tmpX;
					int tmpY;

					// If the East square does not exist or is a floor, mult by 2
					(tmpX, tmpY) = GenerateStep(newRow, newCol, CardinalDirection.East);

					if (tmpX < 0 || tmpX >= rows || tmpY < 0 || tmpY >= columns ||
						board[tmpX][tmpY] == TileType.Floor)
					{ testVal *= 2; }

					// If the North square does not exist or is a floor, mult by 3
					(tmpX, tmpY) = GenerateStep(newRow, newCol, CardinalDirection.North);

					if (tmpX < 0 || tmpX >= rows || tmpY < 0 || tmpY >= columns ||
						board[tmpX][tmpY] == TileType.Floor)
					{ testVal *= 3; }

					// If the West square does not exist or is a floor, mult by 5
					(tmpX, tmpY) = GenerateStep(newRow, newCol, CardinalDirection.West);

					if (tmpX < 0 || tmpX >= rows || tmpY < 0 || tmpY >= columns ||
						board[tmpX][tmpY] == TileType.Floor)
					{ testVal *= 5; }

					// If the South square does not exist or is a floor, mult by 7
					(tmpX, tmpY) = GenerateStep(newRow, newCol, CardinalDirection.South);

					if (tmpX < 0 || tmpX >= rows || tmpY < 0 || tmpY >= columns ||
						board[tmpX][tmpY] == TileType.Floor)
					{ testVal *= 7; }

					// If the resultant value is 10 or 21, it's a regular wall,
					// because that means the only walls bordering it are on its
					// East and West, or North and South, meaning it's not an intersection.
					if (testVal == 10 || testVal == 21)
					{
						// Add it as a dissolvable wall and mark it as "checked"
						tmpWall.DissolvableWalls.Add((newRow, newCol));
						hasThisSpaceBeenChecked[newRow][newCol] = true;

						// Continue checking along this direction
						(newRow, newCol) = GenerateStep(newRow, newCol, dir);
					}
					// Otherwise, it's an intersection
					else
					{
						// Set the bookends of the Wall object and add it to the persitent
						// list of Walls.
						tmpWall.Pt0 = (thisRow, thisCol);
						tmpWall.Pt1 = (newRow, newCol);
						AllWalls.Add(tmpWall);

						// Prevents us from adding an intersection twice to ones
						// we've already checked.
						if (!Intersections.Contains((newRow, newCol)) &&
							!tmpIntersections.Contains((newRow, newCol)))
						{
							tmpIntersections.Enqueue((newRow, newCol));
						}

						// This is just to cancel the while loop,
						// since we just hit an intersection.
						newRow = -1;
					}
				}
			}

			// Once we're done checking this intersection, add it to the
			// persistent list of all unique intersections.
			Intersections.Add((thisRow, thisCol));

		}

		Debug.Log("Walls: " + AllWalls.Count.ToString() +
			"; Intsecs: " + Intersections.Count.ToString());
	}

    private void KnockDownSkinnyRooms()
    {
        int minimumNumberOfHeightOrWidth = 3;
    }

    /// <summary>
    /// Counts the rooms. We'll iterate over all the cells and find the first
    /// floor. We'll mark this floor with a number and grow outward, up against
    /// all walls and maybe an external door. We'll do this till every floor 
    /// is marked.
    /// 
    /// Once done, we'll know how many rooms there are. 
    /// </summary>
    /// <returns>The rooms.</returns>
    private int CountRooms()
    {
        List<int> rooms = new List<int>();
        int currentRoom;
        Tracker tracker = new Tracker();
        int[][] roomNumber = new int[rows][];
        const int notInARoom = -1;

        for (int i = 0; i < rows; i++)
        {
            roomNumber[i] = new int[columns];
            for (int j = 0; j < columns; j++)
            {
                roomNumber[i][j] = notInARoom;
            }
        }


        for (int row = 0; row < this.board.Length; row++)
        {
            for (int column = 0; column < this.board[row].Length; column++)
            {
                if (board[row][column] == TileType.Floor && roomNumber[row][column] == notInARoom) {
                    // We've found a tile in a room

                    (int northRow, int northColumn) = GenerateStep(row, column, CardinalDirection.North);
                    (int eastRow, int eastColumn) = GenerateStep(row, column, CardinalDirection.East);
                    (int southRow, int southColumn) = GenerateStep(row, column, CardinalDirection.South);
                    (int westRow, int westColumn) = GenerateStep(row, column, CardinalDirection.West); 

                    if (roomNumber[northRow][northColumn] != notInARoom)
                    {
                        currentRoom = roomNumber[northRow][northColumn];
                    }
                    else if (roomNumber[eastRow][eastColumn] != notInARoom)
                    {
                        currentRoom = roomNumber[eastRow][eastColumn];
                    }
                    else if (roomNumber[southRow][southColumn] != notInARoom)
                    {
                        currentRoom = roomNumber[southRow][southColumn];
                    }
                    else if (roomNumber[westRow][westColumn] != notInARoom)
                    {
                        currentRoom = roomNumber[westRow][westColumn];
                    }
                    else
                    {
                        currentRoom = tracker.GetInt;
                    }

                    if (!rooms.Contains(currentRoom))
                    {
                        rooms.Add(currentRoom);
                    }

                    roomNumber[row][column] = currentRoom;
                }
            }
        }

        return rooms.Count;
    }

    private void KnockDownWalls(int numberOfRooms)
    {
        // TODO loop until we have the correct number of rooms
        this.KnockDownSkinnyRooms();
    }

    /// <summary>
    /// Generates the a map from a given seed.
    /// We leverage hashing the seed to MD5, leveraging the 32 character string.
    /// We'll interpret each character as hex and each index as follows:
    /// 0: (int) Room Count =   
    /// 1: (int) People Home = 
    /// 2: (int) Number of Pets Home = 
    /// 3: (int) Number of Objects To Move =
    /// 4: (int) Number of cameras = 
    /// 5: (bool) is day time
    /// </summary>
    public void GenerateMap() 
    {
        string md5 = this.CreateMD5(this.seed);

        int numberOfRooms = this.CalculateNumberOfRooms(md5);
        int numberOfPeopleHome = this.CalculateNumberOfPeopleHome(md5);
        int numberOfPetsHome = this.CalculateNumberOfPetsHome(md5);
        int numberOfObjectsToMove = this.CalculateNumberOfObjectsToMove(md5);
        int numberOfCameras = this.CalculateNumberOfCameras(md5);
        int unityRandomSeed = this.CalculateUnityRandomSeed(md5);
        
        this.random =  new System.Random(unityRandomSeed);

        this.InitializeBoard();
        this.AddOuterWalls();
        this.AddExternalDoor();
        this.GenerateRooms();
        this.KnockDownWalls(numberOfRooms);

        Debug.Log("Number of rooms: " + this.CountRooms().ToString());

        FindAllWalls();
    }
}

