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
    private const int columns = 25;
    private const int lastColumn = columns - 1; // -1 to convert from 0 based to 1 based
    private const int rows = 25;
    private const int lastRow = rows - 1; // -1 to convert from 0 based to 1 based
    private const int roomArea = columns * rows;
    private const int minRoomSize = 2;
    private const int maxRoomSize = 6;
    private TileType[][] board;

    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor, Door,
    }

    // Start is called before the first frame update
    void Start()
    {
        this.GenerateMap("potato");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        // 2d has a zero z plane
        Vector3 startingPositionTopLeft = this.camera.ScreenToWorldPoint(new Vector3(0, this.camera.pixelHeight, this.camera.nearClipPlane));

        Color[] tileColors = { Color.black, Color.gray, Color.white };

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
                    case TileType.Door:
                        {
                            Gizmos.color = tileColors[2];
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
        const int indexOfInterest = 0;

        return 6;
    }

    private int CalculateNumberOfPeopleHome(string input)
    {
        const int indexOfInterest = 1;

        return 1;
    }

    private int CalculateNumberOfPetsHome(string input)
    {
        const int indexOfInterest = 2;

        return 1;
    }

    private int CalculateNumberOfObjectsToMove(string input)
    {
        const int indexOfInterest = 3;

        return 3;
    }

    private int CalculateNumberOfCameras(string input)
    {
        const int indexOfInterest = 4;

        return 1;
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
               if(row == firstRowOrColumn || row == lastRow)
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
        System.Random rnd = new System.Random();
        int randomInt = rnd.Next(0, 1);

        if(randomInt == 0)
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
        System.Random rnd = new System.Random();

        return rnd.Next(secondRow, secondToLastColumn);
    }

    private void AddExternalDoor()
    {
        int externalRow = this.GenerateRandomOuterRow();
        int externalColumn = this.GenerateRandomOuterColumn();

        board[externalRow][externalColumn] = TileType.Door;
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
    /// <param name="seed">Seed.</param>
    public void GenerateMap(string seed) 
    {
        int numberOfRooms = this.CalculateNumberOfRooms(seed);
        int numberOfPeopleHome = this.CalculateNumberOfPeopleHome(seed);
        int numberOfPetsHome = this.CalculateNumberOfPetsHome(seed);
        int numberOfObjectsToMove = this.CalculateNumberOfObjectsToMove(seed);
        int numberOfCameras = this.CalculateNumberOfCameras(seed);

        this.InitializeBoard();
        this.AddOuterWalls();
        this.AddExternalDoor();
    }
}

