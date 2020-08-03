using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class WoodenDisk : MonoBehaviour
{
    public bool clockwise = true;
    public bool paused = false;
    public int minSpeed;
    
    public List<Sprite> mySprites;
    public GameObject coinPrefab;
    public GameObject knifePrefab;
    public GameplayController gcontroller;

    public List<GameObject> throwedKnifes;
    public List<GameObject> coinPool;

    private float rotationSpeed = 33.0f;
    private float rotationAngle;
    private int _spawnCoins;

    private Knife _lastKnife;
    private SpriteRenderer _mySprite;

    void Start()
    {
        if(!coinPrefab)
        {
            Debug.LogError("No Coin Prefab!!!");
            Application.Quit();
        }
     
        _mySprite = this.gameObject.GetComponent<SpriteRenderer>();
        _mySprite.sprite = GetRandomWoodenSprite();

        UpdateRotationAngle();
        FillCoinPool(GameplayController.curStage);
        _spawnCoins = Random.Range(0, coinPool.Count);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            TogglePause();
    }
    void FixedUpdate()
    {
        RotateDisk();
        if(Time.timeSinceLevelLoad % 5 == 0)
        {
            clockwise = RandomDirection();
            rotationSpeed = RandomSpeed();
            UpdateRotationAngle();

        }
    }

    // verificar se é possível obter o transform de onde ocorreu a colisão e usá-lo para criar um novo gameObject para ancorar a adaga
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Knife":
                AddItemToThisDisk(collision.gameObject);
                if(gcontroller.GetStageKnives() <= throwedKnifes.Count)
                {
                    GameplayController.StageComplete();
                }
                GameplayController.AddPoint();
                GameplayController.canThrow = true;
                break;
            default:
                Debug.Log($"{gameObject.name}({gameObject.tag}) colidiu com {collision.gameObject.name}({collision.gameObject.tag})");
                break;
        }
    }

    public void ResetDisk()
    {
        foreach(GameObject c in throwedKnifes)
        {
            GameObject.Destroy(c);
        }
        ClearKnivesPool();
        FillCoinPool(Random.Range(0, 6));
        _mySprite.sprite = GetRandomWoodenSprite();
        SpawnCoins();
    }
    
    private void SpawnCoins()
    {
        foreach(GameObject g in coinPool)
        {
            g.transform.parent = this.gameObject.transform;
            g.GetComponent<Coin>().AdjustPositionOnDisk();
        }
    }

    private bool RandomDirection()
    {
        var r = Random.Range(0.0f, 1.0f);
        
        if (r > 0.5f)
            return true;
        
        return false;
    }

    private int RandomSpeed()
    {
        var r = Random.Range(minSpeed/2, minSpeed * 3);
        return r;
    }

    private Sprite GetRandomWoodenSprite()
    {
        return mySprites.ToArray()[Random.Range(0, mySprites.Count)];
        
    }

    private void FillCoinPool(int amt)
    {
        if (amt <= 0)
            amt = 2;
        
        coinPool.Clear();
        amt /= 2;
        
        for(int i=0; i<amt; i++)
        {
            coinPool.Add(coinPrefab);
        }
    }

    private void ClearKnivesPool()
    {
        throwedKnifes.Clear();
    }

    public bool DiskIsPaused()
    {
        return paused;
    }

    public void TogglePause()
    {
        if (!paused)
        {
            PauseDisk();
            return;
        }
        UnpauseDisk();
    }

    private void PauseDisk()
    {
        this.paused = true;
    }

    private void UnpauseDisk()
    {
        this.paused = false;
        if (this.rotationSpeed <= 0)
            rotationSpeed = minSpeed;
        UpdateRotationAngle();
    }

    private void RotateDisk()
    {
        if (paused)
            return;

        if (this.clockwise)
            transform.Rotate(Vector3.forward, rotationAngle * Time.fixedDeltaTime);
        else
            transform.Rotate(Vector3.forward, rotationAngle * Time.fixedDeltaTime * -1);
    }

    private void UpdateRotationAngle()
    {
        rotationAngle = (rotationSpeed / 100) * 360;
    }

    public void AddItemToThisDisk(GameObject other)
    {
        if(other.name == "Jelly")
        {
            // add coin
            other.transform.SetParent(this.transform);
        }
        else if(other.CompareTag("Knife"))
        {
            // add knife
            other.transform.SetParent(this.transform);
            throwedKnifes.Add(other.gameObject);
            other.GetComponent<Knife>().KnifeOnDisk();
        }
    }
}
