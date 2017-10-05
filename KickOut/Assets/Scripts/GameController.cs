using SocketIO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject bombPower;

    private void Start()
    {
        StartCoroutine(PowersSpawner());
    }

    private IEnumerator PowersSpawner()
    {
        Vector3 position1;
        Vector3 position2;

        while (true)
        {
            float x1 = Random.Range(-17.1f, -25.1f);
            float z1 = Random.Range(-14.1f, 14.1f);
            position1 = new Vector3(x1, 0.5f, z1);

            float x2 = Random.Range(17.1f, 25.1f);
            float z2 = Random.Range(-14.1f, 14.1f);
            position2 = new Vector3(x2, 0.5f, z2);

            Destroy(Instantiate(bombPower, position1, Quaternion.identity), 10f);
            Destroy(Instantiate(bombPower, position2, Quaternion.identity), 10f);

            yield return new WaitForSeconds(20f);
        }
    }
}