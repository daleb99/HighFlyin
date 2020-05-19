using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Rigidbody2D))]
public class PlaneControl : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerCrashed;

    public float tapForce;
    public float tiltSmooth;
    public Vector3 startPos;

    Rigidbody2D rbody;
    Quaternion downRot;
    Quaternion forwardRot;

    GameController controller;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        downRot = Quaternion.Euler(0, 0, -90);
        forwardRot = Quaternion.Euler(0, 0, 2);
        controller = GameController.Instance;
        rbody.simulated = false;
    }

    // When we are flying, listen for the game being started and ended
    void OnEnable()
    {
        GameController.OnGameStarted += OnGameStarted;
        GameController.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    
    // We're not flying, stop checking for start and end game
    void OnDisable()
    {
        GameController.OnGameStarted -= OnGameStarted;
        GameController.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    // Allow the plane to move and to detect crashes with other objects
    void OnGameStarted()
    {
        rbody.velocity = Vector3.zero;
        rbody.simulated = true;

        // Start the score system
        StartCoroutine("ScoreSystem");
    }

    // Give the user 1 point for every second survived
    IEnumerator ScoreSystem()
    {
        while (!controller.gameIsOver)
        {
            controller.UpdateScore();
            yield return new WaitForSeconds(1);
        }
    }

    // When the game is over, reset objects
    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    // Called once per frame
    void Update()
    {
        if (controller.GameOver) return;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.rotation = forwardRot;
            rbody.velocity = Vector3.zero;
            rbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRot, tiltSmooth * Time.deltaTime);
        
    }

    // Method for when a crash is detected
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "CrashZone")
        {
            rbody.simulated = false;
            OnPlayerCrashed(); // Event sent to GameController
        }
    }
}
