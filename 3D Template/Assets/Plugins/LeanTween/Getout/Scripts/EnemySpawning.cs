using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] Freelook freelook;

    [Header("Spawning")]

    [SerializeField] Transform points;
    [SerializeField] GameObject enemy;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            ProduceEnemy();
    }

    void ProduceEnemy()
    {
        //produce
        Transform spawnPoint = points.GetChild(Random.Range(0, points.childCount - 1));

        GameObject newEnemy = Instantiate(enemy);
        newEnemy.SetActive(false);

        //position
        newEnemy.transform.position = spawnPoint.position;

        //release
        StartCoroutine(WaitFor(() => newEnemy.SetActive(true), spawnPoint));
    }

    bool NotFacing(Transform target)
    {
        Vector3 normalizedDist = (freelook.cam.transform.position - target.position).normalized;
        float dotProduct = Vector3.Dot(normalizedDist, freelook.cam.transform.forward);

        return (dotProduct > 0);
    }

    IEnumerator WaitFor(System.Action action, Transform point)
    {
        while (!NotFacing(point)) {
            yield return null;
        }

        action.Invoke();
    }
}
