using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image hideIcon;
    [SerializeField] Image showIcon;
    [SerializeField] GameObject restartButtonObject;
    [SerializeField] GameObject clearButtonObject;
    [SerializeField] GameObject hideButtonObject;
    [SerializeField] GameObject shopButtonObject;
    [SerializeField] GameObject galleryButtonObject;
    [SerializeField] GameObject screenshotButtonObject;
    [SerializeField] GameObject deleteButtonObject;
    [SerializeField] ModelManager modelManager;
    [SerializeField] ARPlacementInteractableUIBlock aRPlacementInteractable;

    [Header("Prefabs")]
    [SerializeField] GameObject modelPrefab;
    [SerializeField] GameObject categoryPrefab;

    [Header("Shop")]
    [SerializeField] GameObject shopTab;
    [SerializeField] GameObject categoryContainer;
    [SerializeField] GameObject categoryPanel;
    [SerializeField] GameObject modelPanel;

    [Header("Screenshots")]
    [SerializeField] Screenshots screenshots;
    [SerializeField] GameObject galleryTab;
    [SerializeField] GameObject blink;
    [SerializeField] Image galleryCurrentImage;
    [SerializeField] RectTransform currentImageWrapper;
    [SerializeField] GameObject galleryNoImageText;
    [SerializeField] Sprite noImagePlaceholder;

    List<CategoryMap> categoriesInScene = new List<CategoryMap>();

    [Header("Chosen Model")]
    GameObject chosenModel;

    public struct CategoryMap
    {
        public string name;
        public GameObject objectInScene;
    }

    public IEnumerator Blink()
    {
        blink.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        blink.SetActive(false);
    }

    public void UpdateGalleryImage(Sprite image)
    {
        galleryCurrentImage.sprite = image;
        galleryNoImageText.SetActive(false);
        SetNativeImageSize();
    }

    public void SetDefaultGalleryImage()
    {
        galleryCurrentImage.sprite = noImagePlaceholder;
        galleryNoImageText.SetActive(true);
        galleryCurrentImage.SetNativeSize();
    }

    private void SetNativeImageSize()
    {
        galleryCurrentImage.SetNativeSize();

        float newWidth = 0;
        float newHeight = 0;

        if (currentImageWrapper.sizeDelta.x < galleryCurrentImage.rectTransform.sizeDelta.x)
        {
            float percentageX = currentImageWrapper.sizeDelta.x / galleryCurrentImage.sprite.texture.width;
            newWidth = galleryCurrentImage.sprite.texture.width * percentageX;
            newHeight = galleryCurrentImage.sprite.texture.height * percentageX;
        }

        if (currentImageWrapper.sizeDelta.y < newHeight)
        {
            float percentageY = currentImageWrapper.sizeDelta.y / newHeight;
            newWidth = newWidth * percentageY;
            newHeight = newHeight * percentageY;
        }

        galleryCurrentImage.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }

    public void UseModel()
    {
        aRPlacementInteractable.placementPrefab = modelManager.currentModel.prefab;
        StartCoroutine(UseModelPrivate());
    }

    private IEnumerator UseModelPrivate()
    {
        yield return new WaitForSeconds(0.05f);
        ToggleShopTab();
    }

    public void OpenModelWindow(Model newModel)
    {
        UpdateModelPanel(newModel);
        categoryPanel.SetActive(false);
        modelPanel.SetActive(true);
    }

    public void CloseModelWindow()
    {
        modelPanel.SetActive(false);
        categoryPanel.SetActive(true);
    }

    private void UpdateModelPanel(Model newModel)
    {
        modelPanel.transform.GetChild(0).GetComponent<Text>().text = newModel.name;
        modelPanel.transform.GetChild(1).GetComponent<Image>().sprite = newModel.image;
        modelPanel.transform.GetChild(2).GetComponent<Text>().text = newModel.description;
        modelManager.currentModel = newModel;
    }

    public void ToggleShopTab()
    {
        shopTab.SetActive(!shopTab.activeSelf);
        CloseModelWindow();
    }

    public void ToggleGalleryTab()
    {
        galleryTab.SetActive(!galleryTab.activeSelf);
        screenshots.UpdateGalleryImage();
    }

    public void TogglePlaneHideButton()
    {
        showIcon.gameObject.SetActive(!showIcon.gameObject.activeSelf);
        hideIcon.gameObject.SetActive(!hideIcon.gameObject.activeSelf);
    }

    public void ToggleDeleteObjectButton()
    {
        StartCoroutine(ToggleDeleteObjectButtonPrivate());
    }

    private IEnumerator ToggleDeleteObjectButtonPrivate()
    {
        yield return new WaitForSeconds(0.05f);
        deleteButtonObject.SetActive(!deleteButtonObject.activeSelf);
    }

    public void ToggleMainMenu()
    {
        restartButtonObject.gameObject.SetActive(!restartButtonObject.gameObject.activeSelf);
        clearButtonObject.gameObject.SetActive(!clearButtonObject.gameObject.activeSelf);
        hideButtonObject.gameObject.SetActive(!hideButtonObject.gameObject.activeSelf);
        shopButtonObject.gameObject.SetActive(!shopButtonObject.gameObject.activeSelf);
        galleryButtonObject.gameObject.SetActive(!galleryButtonObject.gameObject.activeSelf);
        screenshotButtonObject.gameObject.SetActive(!screenshotButtonObject.gameObject.activeSelf);
    }

    public void GenerateShop(List<string> categories, List<Model> models)
    {
        GenerateCategories(categories);
        GenerateModels(models);
    }

    private void GenerateCategories(List<string> categories)
    {
        categories.ForEach((category) =>
        {
            GameObject createdCategory = Instantiate(
                categoryPrefab,
                categoryContainer.gameObject.transform.position,
                categoryContainer.gameObject.transform.rotation,
                categoryContainer.transform
            );
            // category -> header dropdown -> title
            createdCategory.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = category;

            CategoryMap cm = new CategoryMap();
            cm.name = category;
            cm.objectInScene = createdCategory;
            categoriesInScene.Add(cm);

            createdCategory.GetComponent<RectTransform>().sizeDelta = new Vector2(
                categoryContainer.GetComponent<RectTransform>().sizeDelta.x,
                createdCategory.GetComponent<RectTransform>().sizeDelta.y
            );

            categoryContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(
                categoryContainer.GetComponent<RectTransform>().sizeDelta.x,
                categoryContainer.GetComponent<RectTransform>().sizeDelta.y + createdCategory.GetComponent<RectTransform>().sizeDelta.y + 10f
            );
        });
        categoryContainer.transform.position = new Vector2(categoryContainer.transform.position.x, categoryContainer.GetComponent<RectTransform>().sizeDelta.y * -0.28f);
    }

    private void GenerateModels(List<Model> models)
    {
        models.ForEach((model) =>
        {
            // category -> model wrapper -> model container
            Transform modelContainer = categoriesInScene.Find((category) => category.name == model.category).objectInScene.transform.GetChild(1).GetChild(0).GetChild(0).transform;
            GameObject createdModel = Instantiate(
                modelPrefab,
                modelContainer.gameObject.transform.position,
                modelContainer.gameObject.transform.rotation,
                modelContainer
            );
            createdModel.GetComponent<Image>().sprite = model.image;
            createdModel.GetComponent<ModelPrefab>().model = model;

            modelContainer.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                modelContainer.gameObject.GetComponent<RectTransform>().sizeDelta.x + createdModel.GetComponent<RectTransform>().sizeDelta.x + 10f,
                modelContainer.gameObject.GetComponent<RectTransform>().sizeDelta.y
            );
        });

        categoriesInScene.ForEach((cm) =>
        {
            Transform modelContainer = cm.objectInScene.transform.GetChild(1).GetChild(0).GetChild(0).transform;
            modelContainer.position = new Vector2(modelContainer.GetComponent<RectTransform>().sizeDelta.x * 0.28f, modelContainer.transform.position.y);
        });
    }
}
