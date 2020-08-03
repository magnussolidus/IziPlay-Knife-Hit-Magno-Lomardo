using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    public Sprite mySprite;
    public float yAxisOffset;

    void Start()
    {
        if (!mySprite)
            Debug.LogError($"No sprite for {this.gameObject.name}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Knife"))
        {
            GameplayController.AddPoint();
            DestroyCoin();
        }
    }

    void DestroyCoin()
    {
        // TODO plays animation
        Destroy(this.gameObject);
    }
    public void AdjustPositionOnDisk()
    {
        this.gameObject.transform.SetPositionAndRotation(new Vector3(0, yAxisOffset, 0), Quaternion.identity);
    }

}
