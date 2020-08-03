using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{

    public SpriteRenderer mySprite;
    public int myId;
    public int speed = 5;
    public float yAxisOffset;

    public Transform spawn;
    public Collider2D throwCollider;
    public Collider2D stuckCollider;
    
    public bool _throw = false;
    public bool hit = false;

    private Vector3 _newPos;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        if(!mySprite)
        {
            Debug.LogWarning($"No Sprite for knife id {myId}");
            mySprite = GetComponent<SpriteRenderer>();
        }
        throwCollider = gameObject.GetComponent<EdgeCollider2D>();
        stuckCollider = gameObject.GetComponent<PolygonCollider2D>();
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _newPos = gameObject.transform.position;

        _rigidbody2D.isKinematic = true;

        if (gameObject.transform.root.GetComponent<WoodenDisk>())
        {
            KnifeOnDisk();
        }
        else
        {
            stuckCollider.enabled = false;
        }

    }

    public int GetSpeed()
    {
        return this.speed;
    }

    public void KnifeOnDisk()
    {
        _rigidbody2D.isKinematic = true;
        throwCollider.enabled = false;
        stuckCollider.enabled = true;
        AdjustPositionOnDisk();
    }

    private void AdjustPositionOnDisk()
    {
        this.gameObject.transform.SetPositionAndRotation(new Vector3(0, yAxisOffset, 0), Quaternion.identity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Boss":
                break;
            case "WoodenDisk":
                KnifeOnDisk();
                break;
            case "Knife":
                GameplayController.gameOver = true;
                break;
            default:
                Debug.Log($"{gameObject.name}({gameObject.tag}) colidiu com {collision.gameObject.name}({collision.gameObject.tag})");
                break;
        }
    }

    public void DisableBeforeThrow()
    {
        throwCollider.enabled = false;
        stuckCollider.enabled = false;
        mySprite.enabled = false;
    }

    public void ActivateForThrow()
    {
        throwCollider.enabled = true;
        stuckCollider.enabled = true;
        mySprite.enabled = true;
    }
}
