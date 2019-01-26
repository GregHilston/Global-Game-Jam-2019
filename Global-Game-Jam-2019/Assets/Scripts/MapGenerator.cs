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
    private const int columns = 25;
    private const int rows = 25;
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
        Debug.Log("OnDrawGizmos()");

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

               Gizmos.DrawCube(new Vector3(baseXOffset * row, baseYOffset * column, 0.0f), new Vector3(size, size, size));
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

    private void EntireBoardAsFloor()
    {

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
        Debug.Log("seed: " + seed);
        Debug.Log("md5: " + this.CreateMD5(seed));

        int numberOfRooms = this.CalculateNumberOfRooms(seed);
        int numberOfPeopleHome = this.CalculateNumberOfPeopleHome(seed);
        int numberOfPetsHome = this.CalculateNumberOfPetsHome(seed);
        int numberOfObjectsToMove = this.CalculateNumberOfObjectsToMove(seed);
        int numberOfCameras = this.CalculateNumberOfCameras(seed);

        this.InitializeBoard();

        Debug.Log("board: " + this.board);
    }
}

