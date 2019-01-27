using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    private float speed = 4.0f;
    public const int middleX = 0;
    public const int middleY =0;
    public const int middle2X = 5;
    public const int middle2Y = 5;
    public const int middle3X = -5;
    public const int middle3Y = -5;

    Vector2[] gridPoints =
    {
        //room 1
        new Vector2(middleX+2, middleY+2),
        new Vector2(middleX-2, middleY-2),
        new Vector2(middleX+2, middleY+2),
        new Vector2(middleX+2, middleY-2),
        new Vector2(middleX,  middleY-2),
        
        //room2
        new Vector2(middle2X+2, middle2Y+2),
        new Vector2(middle2X-2, middle2Y-2),
        new Vector2(middle2X+2, middle2Y+2),
        new Vector2(middle2X+2, middle2X-2),
        new Vector2(middle2X, middle2Y-2),

         //room3
        new Vector2(middle3X+2, middle3Y+2),
        new Vector2(middle3X-2, middle3Y-2),
        new Vector2(middle3X+2, middle3Y+2),
        new Vector2(middle3X+2, middle3X-2),
        new Vector2(middle3X, middle3Y-2)

    };
    
    //declare global variables
    public Vector2 target;
    public Vector2  targetPoint;
    private Vector2 position;
    private bool onGridPoint;
    private bool onTargetPoint;
    public int arrayPosition = 0;
    public int rand;
    public int targetArray;
    public int gridPointRoomNumber;
    public int playerLocation;

    // Start is called before the first frame update
    void Start()
    {       
        DecideGridPoint();
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //update game enemy position
        position = gameObject.transform.position;
        
        //enemy speed
        float step = speed * Time.deltaTime;

        //move to points on grid
        if (position != targetPoint && position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, step);
        }
        else if (position==target && position!=targetPoint)
        {
           target = targetPoint;
        }
        else if (position == targetPoint)
        {
            DecideGridPoint();
        }
   

    }

    //decide target point
    void DecideTargetPoint()
    { 
        rand = Random.Range(0, 15);
        targetArray = rand;
        targetPoint = gridPoints[rand];
    }

    //decide temporary target
    void DecideGridPoint()
    {
        DecideTargetPoint();
        
        int currentRoom = getCurrentRoomNumber();
        int targetRoom = getTargetRoomNumber();

        if (currentRoom != targetRoom && currentRoom == 1)
        {
            target = gridPoints[4];
        }
        else if (currentRoom != targetRoom && currentRoom == 2)
        {
            target = gridPoints[9];
        }
        else
        {
            target = targetPoint;
        }

    }

    public int getTargetRoomNumber()
    {
        int numberOfRooms = 3;
        int index = 0;
        int dataPoints = numberOfRooms * 2;

        while (index < dataPoints)
        {
            if (targetPoint == gridPoints[index] && index < 5)
            {
                return 1;
            }
            else if (targetPoint == gridPoints[index] && index >= 5 && index < 10)
            {
                return 2;
            }
            else if (targetPoint == gridPoints[index] && index >= 10 && index < 15)
            {
                return 3;
            }
            else if (targetPoint == gridPoints[index] && index >= 15 && index < 20)
            {
                return 4;
            }
            index++;

        }
        return 0; ///SHOULD NOT HIT THIS CODE
    }


    public int getCurrentRoomNumber()
    {
        int numberOfRooms = 3;
        int index = 0;
        int dataPoints = numberOfRooms * 2;

        while (index < dataPoints)
        {
            if (position == gridPoints[index] && index < 5)
            {
                return 1;
            }
            else if (position == gridPoints[index] && index >= 5 && index < 10)
            {
                return 2;
            }
            else if (position == gridPoints[index] && index >= 10 && index < 15)
            {
                return 3;
            }
            else if (position == gridPoints[index] && index >= 15 && index < 20)
            {
                return 4;
            }
            index++;
            
        }
        return 0; ///SHOULD NOT HIT THIS CODE
    }

}


