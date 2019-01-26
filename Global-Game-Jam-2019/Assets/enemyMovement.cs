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


    Vector2[] gridPoints =
    {
        //room 1
        new Vector2(middleX+2, middleY+2),
        new Vector2(middleX-2, middleY-2),
        new Vector2(middleX+2, middleY+2),
        new Vector2(middleX+2, middleY-2),
        new Vector2(middleX, middleY-2),
        //room2
        new Vector2(middle2X+2, middle2Y+2),
        new Vector2(middle2X-2, middle2Y-2),
        new Vector2(middle2X+2, middle2Y+2),
        new Vector2(middle2X+2, middle2X-2),
        new Vector2(middle2X, middle2Y-2),

    };
    

    public Vector2 target;
    public Vector2  targetPoint;
    private Vector2 position;
    private bool onGridPoint;
    private bool onTargetPoint;
    public int arrayPosition = 0;
    public int rand;
    public int targetArray;
    public int targetRoomNumber;
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

        
        position = gameObject.transform.position;
        
        float step = speed * Time.deltaTime;
        
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

    

   
        
    


    void DecideTargetPoint()
    {
        
        rand = Random.Range(0, 8);
        targetArray = rand;
        targetPoint = gridPoints[rand];
    }

    void DecideGridPoint()
    {
        DecideTargetPoint();


        
        if ((targetPoint == gridPoints[5] ||  targetPoint == gridPoints[6] || targetPoint == gridPoints[7] || targetPoint == gridPoints[8] || targetPoint == gridPoints[9]) && (position==gridPoints[0] || position == gridPoints[1] || position == gridPoints[2] || position == gridPoints[3]))
        {
            target = gridPoints[4];
        }
        else if ((targetPoint == gridPoints[0] || targetPoint == gridPoints[1] || targetPoint == gridPoints[2] || targetPoint == gridPoints[3] || targetPoint == gridPoints[4]) && (position == gridPoints[5] || position == gridPoints[6] || position == gridPoints[7] || position == gridPoints[8]))
        {
            target = gridPoints[9];
        }
        else
        {
            target = targetPoint;
        }

    }

    public bool targetOutsideRoom()
    {

        if (gridPointRoomNumber != targetRoomNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
