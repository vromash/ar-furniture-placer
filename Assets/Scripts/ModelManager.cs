using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
public class ModelManager : MonoBehaviour
{
    [SerializeField]
    UIManager UIManager;
    [SerializeField]
    List<string> categories = new List<string>();
    [SerializeField]
    List<Model> models = new List<Model>();
    [SerializeField]
    public Model currentModel;

    void Start()
    {
        UIManager.GenerateShop(categories, models);
    }

    public void ChoseModel(Model newModel)
    {
        currentModel = newModel;
    }
}

[System.Serializable]
public struct Model
{
    public string name;
    public string category;
    public string description;
    public Sprite image;
    public GameObject prefab;
}