using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBird : MonoBehaviour
{

    public float speed = 1.0f;
    private Rigidbody2D rb;
    private Vector2 screenBoundary;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed, 0);
        screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        // Is the bird to the left of the screen
        if(transform.position.x < -screenBoundary.x * 2)
        {
            // Delete the bird off the screen
            Destroy(this.gameObject);
        }
    }
}
