using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace UnityEngine.XR.Interaction.Toolkit.AR
{
    public class Screenshots : MonoBehaviour
    {
        [SerializeField] ARInteractions arInteractions;
        [SerializeField] UIManager uiManager;
        [SerializeField] Text counterText;
        [SerializeField] Animator deleteAnimator;
        [SerializeField] Animator saveAnimator;
        bool takeScreenshotOnNextFrame;
        string[] files = null;
        int currentScreenshot = 0;

        public void TakeScreenshotPublic()
        {
            arInteractions.TogglePlaneDetection();
            StartCoroutine(TakeScreenshot());
        }

        private IEnumerator TakeScreenshot()
        {
            yield return new WaitForSeconds(0.05f);
            uiManager.ToggleMainMenu();
            yield return new WaitForSeconds(0.3f);
            ScreenCapture.CaptureScreenshot("bachelor-" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png");
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(uiManager.Blink());
            yield return new WaitForSeconds(0.2f);
            uiManager.ToggleMainMenu();
            arInteractions.TogglePlaneDetection();
        }

        public void UpdateGalleryImage()
        {
            GetFiles();
            if (files.Length > 0)
            {
                uiManager.UpdateGalleryImage(GetCurrentPicture());
            }
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            if (files.Length > 0)
            {
                counterText.text = (currentScreenshot + 1) + "/" + files.Length;
            }
            else
            {
                counterText.text = "0/0";
            }
        }

        public void DeleteImage()
        {
            if (files.Length > 0)
            {
                string pathToFile = files[currentScreenshot];
                if (File.Exists(pathToFile))
                {
                    File.Delete(pathToFile);
                    StartCoroutine(PlayDeleteAnimation());
                }

                GetFiles();

                if (files.Length > 0)
                {
                    NextPicture();
                }
                else
                {
                    uiManager.SetDefaultGalleryImage();
                    UpdateCounter();
                }
            }
        }

        private IEnumerator PlayDeleteAnimation()
        {
            yield return new WaitForSeconds(0.1f);
            deleteAnimator.SetBool("delete", true);
        }

        private IEnumerator PlaySaveAnimation()
        {
            saveAnimator.SetBool("save", true);
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator StopSaveAnimation()
        {
            yield return new WaitForSeconds(0.4f);
            saveAnimator.SetBool("save", false);
        }

        public void NextPicture()
        {
            if (files.Length > 0)
            {
                currentScreenshot += 1;
                if (currentScreenshot > files.Length - 1)
                    currentScreenshot = 0;
                uiManager.UpdateGalleryImage(GetCurrentPicture());
            }
            UpdateCounter();
        }

        public void PreviousPicture()
        {
            if (files.Length > 0)
            {
                currentScreenshot -= 1;
                if (currentScreenshot < 0)
                    currentScreenshot = files.Length - 1;
                uiManager.UpdateGalleryImage(GetCurrentPicture());
            }
            UpdateCounter();
        }

        private void GetFiles()
        {
            files = Directory.GetFiles(Application.persistentDataPath + "/", "*.png");
        }

        private Sprite GetCurrentPicture()
        {
            string pathToFile = files[currentScreenshot];
            Texture2D texture = GetScreenshotImage(pathToFile);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        private Texture2D GetScreenshotImage(string filePath)
        {
            Texture2D texture = null;
            byte[] fileBytes;
            if (File.Exists(filePath))
            {
                fileBytes = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
                texture.LoadImage(fileBytes);
            }
            return texture;
        }

        public void SaveScreenshot()
        {
            if (files.Length > 0)
            {
                StartCoroutine(PlaySaveAnimation());

                // Get picture
                string pathToFile = files[currentScreenshot];
                Texture2D texture = GetScreenshotImage(pathToFile);

                // Save the screenshot to Gallery/Photos
                string name = string.Format("{0}_screenshot_{1}.png", Application.productName, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                NativeGallery.SaveImageToGallery(texture, "Bachelor captures", name);

                StartCoroutine(StopSaveAnimation());
            }
        }
    }
}