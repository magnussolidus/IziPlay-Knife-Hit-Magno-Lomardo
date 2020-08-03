using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class AdController : MonoBehaviour
{
    public enum AdType
    {
        mainCanvas = 0,
        overlay = 1,
        video = 2,
        inline = 3
    }

    public enum AdResult
    {
        fail = 0,
        suscess,
        skip,
        defaultResult,
        other
    }


    public static AdController instance;
    public static AdResult adResult;
    public static bool displayingAd = false;

    public static Canvas[] adList;

    private static int _curCanvas;

    private void Awake()
    {
        if (instance != null && Controller.instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            adList = gameObject.GetComponentsInChildren<Canvas>();
            adResult = AdResult.defaultResult;
        }

        foreach(Canvas c in adList)
        {
            c.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if(displayingAd && adResult != AdResult.defaultResult)
        {
            GameplayController.AdFeedback(adResult);
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            DisplayOverlayAd();
        }
    }

    public static void DisplayAd(AdType type)
    {
        adList[0].enabled = true;
        switch(type)
        {
            case AdType.video:
                DisplayVideoAd();
                break;
            case AdType.overlay:
                DisplayOverlayAd();
                break;
            case AdType.inline:
                StartInlineAd();
                break;
            default:
                Debug.LogError($"Tipo desconhecido! AdType: {type}");
                break;
        }
    }

    public static void DisplayOverlayAd()
    {
        AdController.displayingAd = true;
        EnableAdCanvas();
        EnableCanvasById((int)AdType.overlay);

    }

    private static void EnableAdCanvas()
    {
        AdController.adList[0].enabled = true;
    }

    private static void DisableAdCanvas()
    {
        AdController.adList[_curCanvas].enabled = false;
        AdController.adList[0].enabled = false;
    }

    public static void DisableOverlayAd(int result)
    {
        Debug.LogError("Disabled Overlay Ad");
        AdController.displayingAd = false;
        SetAdResult(result);
        DisableCanvasById((int)AdType.overlay);
        DisableAdCanvas();
    }

    private static void EnableCanvasById(int id)
    {
        AdController.adList[id].enabled = true;
    }

    private static void DisableCanvasById(int id)
    {
        AdController.adList[id].enabled = false;
        adResult = AdResult.defaultResult;
    }

    public static void DisableCurrentAd(int result)
    {
        AdController.displayingAd = false;
        SetAdResult(result);
        adResult = AdResult.defaultResult;
        DisableAdCanvas();
    }

    public static void StartInlineAd()
    {

    }

    public static void StopInlineAd()
    {

    }

    public static void DisplayVideoAd()
    {

    }

    public static void SetAdResult(int value)
    {
        switch(value)
        {
            case 0:
                GameplayController.AdFeedback(AdResult.fail);
                break;
            case 1:
                GameplayController.AdFeedback(AdResult.suscess);
                break;
            case 2:
                GameplayController.AdFeedback(AdResult.skip);
                break;
            default:
                GameplayController.AdFeedback(AdResult.other);
                break;

        }
    }

}
