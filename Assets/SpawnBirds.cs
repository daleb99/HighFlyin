using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBirds : MonoBehaviour
{

    public GameObject birdPrefab;
    public float respawnTime = 5.0f;
    private Vector2 screenBoundary;

    // Start is called before the first frame update
    void Start()
    {
        screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(birdFlock());
    }
    private void spawnBird()
    {
        GameObject a = Instantiate(birdPrefab) as GameObject;
        a.transform.position = new Vector2(-screenBoundary.x * -2, Random.Range(-screenBoundary.y, screenBoundary.y + 5));
    }

    IEnumerator birdFlock()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            spawnBird();
        }
    }
}
