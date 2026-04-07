using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameObject ball;
    public float minAngle = -10;
    public float maxAngle = 70;

    void Start()
    {
        for(int i = 0;i < 30; i++) {
            SpawnObject(5);
        }
    }

    public void SpawnObject(float distance)
    {
        float angle  = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        float angleY = Random.Range(0f, Mathf.PI * 2f);

        Vector3 direction = new Vector3(
            Mathf.Cos(angleY) * Mathf.Cos(angle),
            Mathf.Sin(angle),
            Mathf.Sin(angleY) * Mathf.Cos(angle)
        );

        Vector3 spawnPos = transform.position + direction * distance;
        Instantiate(ball, spawnPos, Quaternion.identity);
    }
}
