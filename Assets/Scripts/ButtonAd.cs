using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAd : MonoBehaviour
{
    public int myResult = -1;
    private Button _self;

    void Start()
    {
        if(myResult <= 0)
        {
            Debug.LogError($"Botão {gameObject.name} não configurado!\nAjuste o valor do resultado e tente novamente!");
        }
        _self = GetComponent<Button>();
        
    }

}
