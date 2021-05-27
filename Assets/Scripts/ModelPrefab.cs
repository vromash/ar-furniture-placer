using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelPrefab : MonoBehaviour
{
    UIManager UIManager;

    [SerializeField]
    Button button;
    public Model model;

    void Awake()
    {
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        button.onClick.AddListener(() => UIManager.OpenModelWindow(model));
    }
}
