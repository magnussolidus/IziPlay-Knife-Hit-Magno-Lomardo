using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Camera myCamera;
    // Start is called before the first frame update
    void Awake()
    {
        myCamera = gameObject.GetComponent<Camera>();
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
