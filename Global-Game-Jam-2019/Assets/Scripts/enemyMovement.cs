using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    private float speed = 4.0f;



    //declare global variable
    public float rand1, rand2, rand3, timer;
    public Vector2 position;
 

    // Start is called before the first frame update
    void Start()
    {

        position = gameObject.transform.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        Randomize();
    }

    void Update()
    {
        position = gameObject.transform.position;

        if (position.x >= -6 && position.x <= 6 && position.y >= -4.5 && position.y <= 4.5)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rand1, rand2);
        }
        else if ((position.x >= -6 && position.x <= 6) && ( position.y < -4.5 || position.y > 4.5))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rand1, -rand2);
        }
        else if ((position.x < -6 || position.x > 6) && (position.y >= -4.5 || position.y <= 4.5))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-rand1, rand2);
        }



        if (timer > 0)
        {

            timer--;
        }
        else if(timer <= 0)
        {
            Randomize();
        }
    }
    void DecideDestination()
    {

    }


    void Randomize()
    {
        rand1 = Random.Range(-5.0f, 5.0f);
        rand2 = Random.Range(-5.0f, 5.0f);
        rand3 = Random.Range(5.0f, 10.0f);
        timer = rand3;
        
    }

}
   


