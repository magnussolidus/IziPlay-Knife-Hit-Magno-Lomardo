using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{

    public static GameplayController instance;

    public static int curScore;
    public static int curStage;
    public static int highScore;
    public static int maxStage;
    public static int coins;
    public static bool canThrow;
    public static bool stageFinished;
    public static bool gameOver;

    public Transform spawnPosition;
    public List<GameObject> knivesPool;
    
    public WoodenDisk myDisk;
    public GameObject knifePrefab;
    
    public Text score;
    public Text finalScore;
    public Canvas continueCanvas;
    public int minKnifes;

    private GameObject _curKnife;
    private int kniveHeader;

    public GameplayController()
    {
        curScore = 0;
        curStage = 0;
        highScore = 0;
        maxStage = 0;
        coins = 0;
        minKnifes = 5;
        canThrow = true;
        stageFinished = false;
    }

    // lista de moedas

    private void Awake()
    {
        if (instance != null && Controller.instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //DontDestroyOnLoad(this.gameObject); // foi necessário remover para deixar de ser singleton por falta de tempo
            instance = this;
            coins = PlayerPrefs.GetInt("coins");
            LoadHighScore();
            StartGame();
        }
    }

    private void Start()
    {
        if (!spawnPosition)
        {
            this.spawnPosition = this.transform;
            spawnPosition.position = new Vector3(
                spawnPosition.position.x,
                spawnPosition.position.y - 3.5f,
                spawnPosition.position.z);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && canThrow)
        {
            ThrowNextKnife();
            canThrow = false;
        }

        if(kniveHeader == 0 && GameplayController.stageFinished)
        {
            // todas as facas lançadas, vitória
            NextStage();
        }

        if(gameOver)
        {
            EndStage();
        }

    }

    private void LateUpdate()
    {
        score.text = $"Placar: {curScore}";
        finalScore.text = $"Placar Final: {GameplayController.curScore}\nEstágio {GameplayController.curStage}";
    }

    private void NextStage()
    {
        ++curStage;
        if (curStage > maxStage)
            maxStage = curStage;
        Save();
        FillKnivesPool(minKnifes + ((curStage/4) * 2));
        myDisk.ResetDisk();
        canThrow = true;
        GameplayController.stageFinished = false;
    }


    public int GetStageKnives()
    {
        return knivesPool.Count;
    }

    private void EndStage()
    {
        continueCanvas.enabled = true;
        myDisk.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        myDisk.gameObject.SetActive(true);
        coins += curScore;
        Save();
        GameplayController.gameOver = false;
        continueCanvas.enabled = false;
        curScore = 0;
        curStage = 0;
        FillKnivesPool(minKnifes);
        canThrow = true;
    }


    public void Save()
    {
        PlayerPrefs.SetInt("coins", coins);
        SaveHighScore();
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("highScore");
        maxStage = PlayerPrefs.GetInt("maxstage");
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("highScore", curScore);
        PlayerPrefs.SetInt("maxStage", curStage);
    }

    public void FillKnivesPool(int amt)
    {
        for(int i=0; i<amt; i++)
        {
            var go = GameObject.Instantiate(knifePrefab);
            go.transform.position = spawnPosition.position;
            go.SetActive(true);
            this.knivesPool.Add(go);
            go.GetComponent<Knife>().DisableBeforeThrow();
        }

    }

    public void ThrowNextKnife()
    {
        if (!canThrow || knivesPool.Count <= 0)
            return;
        
        kniveHeader = knivesPool.Count - 1;
        _curKnife = knivesPool.ToArray()[kniveHeader];
        knivesPool.RemoveAt(kniveHeader);

        var knifeComp = _curKnife.GetComponent<Knife>();

        knifeComp.ActivateForThrow();
        knifeComp._throw = true;
        _curKnife.GetComponent<Rigidbody2D>().isKinematic = false;
        _curKnife.GetComponent<Rigidbody2D>().velocity = new Vector2(knifeComp.speed, 0);
            //.AddForce(Vector2.up, ForceMode2D.Impulse);

    }

    public void ResetKnifePoll()
    {
        knivesPool.Clear();
    }

    public static void AddPoint()
    {
        ++curScore;
    }

    public static void AddPoints(int ammount)
    {
        curScore += ammount;
    }

    public static void StageComplete()
    {
        GameplayController.stageFinished = true;
    }

    public static void AdFeedback(AdController.AdResult code)
    {
        switch(code)
        {
            case AdController.AdResult.fail: 
                Debug.LogWarning("Anúncio fechado antes");
                break;
            case AdController.AdResult.suscess: // caso de sucesso
                Debug.Log("Anúncio assistido com sucesso!");
                break;
            case AdController.AdResult.skip: // caso de skip | deve ser removido antes de publicado
                Debug.LogWarning("Anúncio pulado, retornado o código de sucesso");
                break;
            default:
                Debug.LogError($"Código Desconhecido: {code}");
                break;
        }
    }
}
