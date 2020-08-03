using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject.Destroy(collider.gameObject);
    }
}
