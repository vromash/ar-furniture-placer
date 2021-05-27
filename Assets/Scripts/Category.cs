using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    [SerializeField]
    GameObject hideIcon;
    [SerializeField]
    GameObject showIcon;
    [SerializeField]
    GameObject modelWrapper;

    GameObject categoryContrainer;
    RectTransform rectTransform;

    private void Awake()
    {
        categoryContrainer = GameObject.Find("Category Container");
        rectTransform = GetComponent<RectTransform>();
    }

    public void ToggleModelList()
    {
        modelWrapper.SetActive(!modelWrapper.activeSelf);
        hideIcon.SetActive(!hideIcon.activeSelf);
        showIcon.SetActive(!showIcon.activeSelf);

        float height = modelWrapper.GetComponent<RectTransform>().sizeDelta.y;
        if (!modelWrapper.activeSelf)
        {
            categoryContrainer.gameObject.transform.position = new Vector2(categoryContrainer.gameObject.transform.position.x, categoryContrainer.gameObject.transform.position.y + height * 0.28f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - height);
            categoryContrainer.GetComponent<RectTransform>().sizeDelta = new Vector2(categoryContrainer.GetComponent<RectTransform>().sizeDelta.x, categoryContrainer.GetComponent<RectTransform>().sizeDelta.y - height);
        }

        if (modelWrapper.activeSelf)
        {
            categoryContrainer.gameObject.transform.position = new Vector2(categoryContrainer.gameObject.transform.position.x, categoryContrainer.gameObject.transform.position.y - height * 0.28f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + height);
            categoryContrainer.GetComponent<RectTransform>().sizeDelta = new Vector2(categoryContrainer.GetComponent<RectTransform>().sizeDelta.x, categoryContrainer.GetComponent<RectTransform>().sizeDelta.y + height);
        }
    }
}
