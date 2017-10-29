﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainPlayer")
        {
            Online aux = other.GetComponent<Online>();
            aux.Respawn();
        }
    }
}
