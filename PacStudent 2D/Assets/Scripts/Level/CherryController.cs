using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherry; // Drag and drop your cherry prefab here in the inspector
    private GameObject cherryInstance;
    private float spawnInterval = 10f; // Spawns every 10 seconds
    private float nextSpawnTime;
    private Camera mainCamera;
    private Tweener tweener;

    private void Start()
    {
        mainCamera = Camera.main;
        nextSpawnTime = Time.time + spawnInterval;
        cherry = GameObject.Find("Bonus Cherry");
        tweener = cherry.GetComponent<Tweener>();
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCherry();
            nextSpawnTime = Time.time + spawnInterval;
        }

        if (cherryInstance && isOutOfCameraView(cherryInstance.transform.position))
        {
            Destroy(cherryInstance);
        }
    }

    private void SpawnCherry()
    {
        // Cherry needs to spawn at a random position just outside of the camera view:
        Vector3 spawnPosition = GetRandomPositionOutsideCameraView();

        // Target position directly opposite from spawn point - this also will ensure it passes through centre of map:
        Vector3 targetPosition = GetOppositePosition(spawnPosition);

        // Instantiate cherry:
        cherryInstance = Instantiate(cherry, spawnPosition, Quaternion.identity);
        Debug.Log("Spawning cherry...");

        tweener.AddTween(cherry.transform, spawnPosition, targetPosition, 7.0f);
    }

    private Vector3 GetRandomPositionOutsideCameraView()
    {
        Vector3 randomPosition = new Vector3();

        float randomSide = Random.Range(0, 4);
        float randomOffset = Random.Range(-0.5f, 0.5f);

        if (randomSide < 1) // Top
        {
            randomPosition = mainCamera.ViewportToWorldPoint(new Vector3(0.5f + randomOffset, 1.1f, 10f));
        }
        else if (randomSide < 2) // Bottom
        {
            randomPosition = mainCamera.ViewportToWorldPoint(new Vector3(0.5f + randomOffset, -0.1f, 10f));
        }
        else if (randomSide < 3) // Left
        {
            randomPosition = mainCamera.ViewportToWorldPoint(new Vector3(-0.1f, 0.5f + randomOffset, 10f));
        }
        else // Right
        {
            randomPosition = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, 0.5f + randomOffset, 10f));
        }

        return randomPosition;
    }

    private Vector3 GetOppositePosition(Vector3 startPosition)
    {
        Vector3 screenCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
        Vector3 direction = (screenCenter - startPosition).normalized;
        return startPosition + direction * 100f; // Assuming the screen's diagonal is less than 100 units
    }

    private bool isOutOfCameraView(Vector3 position)
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(position);
        return viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1;
    }
}
