using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPower : MonoBehaviour
{
    public int bombs = 3;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MainPlayer")
        {
            other.GetComponent<Player>().AddBomb(bombs);
            Destroy(gameObject);
        }

        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
