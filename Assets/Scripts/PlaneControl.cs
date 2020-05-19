using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlaneControl : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerCrashed;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    Rigidbody2D rbody;
    Quaternion downRot;
    Quaternion forwardRot;

    GameController controller;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        downRot = Quaternion.Euler(0, 0, -90);
        forwardRot = Quaternion.Euler(0, 0, 15);
        controller = GameController.Instance;
        rbody.simulated = false;
    }

    void OnEnable()
    {
        GameController.OnGameStarted += OnGameStarted;
        GameController.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameController.OnGameStarted -= OnGameStarted;
        GameController.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rbody.velocity = Vector3.zero;
        rbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        controller.incScore();

        if (controller.GameOver) return;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.rotation = forwardRot;
            rbody.velocity = Vector3.zero;
            rbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRot, tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "CrashZone")
        {
            rbody.simulated = false;
            OnPlayerCrashed(); // Event sent to GameController
        }
    }
}
