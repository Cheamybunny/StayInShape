using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotPreviewer : MonoBehaviour
{
    [SerializeField] ScreenshotDisplayEvents screenshotDisplayEvents;
    public static string recentScreenshotPath;
    private string[] files = null;
    int whichScreenShotIsShown = 0;

    // Start is called before the first frame update
    void Start()
    {
        files = Directory.GetFiles(Application.persistentDataPath, "*.png");
        print(files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            string fileName = files[i];
            Debug.Log("Comparing " + fileName + " to " + recentScreenshotPath);
            if (fileName == recentScreenshotPath)
            {
                Debug.Log("Set Index to " + i.ToString());
                whichScreenShotIsShown = i;
            }
            Debug.Log(fileName);
        }

        screenshotDisplayEvents.setScreenshot(GetPictureAndShowIt());
    }

    public Sprite getMostRecentScreenshot()
    {
        // This clause only occurs when the scene is first instantiated
        string pathToFile = recentScreenshotPath; // Assume that there is a recent screenshot path, since the only way to navigate to this scene is by a successful screenshot capture
        Debug.Log("Most recent screenshot is " + pathToFile);
        if (pathToFile != null)
        {
            Texture2D texture = GetScreenshotImage(pathToFile);
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sp;
        }
        else
        {
            Debug.Log("Error occurred! There is no recent screenshot path. The screenshot feature or file directory may have errors");
            return null;
        }
    }

    public Sprite GetPictureAndShowIt()
    {
        if (files == null)
        {
            Debug.Log("Getting most recent screenshot because files are null!");
            return getMostRecentScreenshot();
        }
        
        if (files.Length >= 0)
        {
            Debug.Log("Showing screenshot with index " + whichScreenShotIsShown.ToString());
            string pathToFile = files[whichScreenShotIsShown];
            Texture2D texture = GetScreenshotImage(pathToFile);
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sp;
        } else
        {
            Debug.Log("Files.length is not 0");
            return null;
        }
    }

    Texture2D GetScreenshotImage(string filePath)
    {
        Texture2D texture = null;
        byte[] buffer;
        if (File.Exists(filePath))
        {
            buffer = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(buffer);
        }
        return texture;
    }

    public Sprite NextPicture() // TODO: Implement UI button
    {
        if (files.Length > 0)
        {
            whichScreenShotIsShown += 1;
            if (whichScreenShotIsShown > files.Length - 1)
            {
                whichScreenShotIsShown = 0; // Cycle back
            }
            return GetPictureAndShowIt();
        }

        return null;
    }

    public Sprite PreviousPicture() // TODO: Implement UI button
    {
        if (files.Length > 0)
        {
            whichScreenShotIsShown -= 1;
            if (whichScreenShotIsShown < 0)
            {
                whichScreenShotIsShown = files.Length - 1; // Cycle back
            }
            return GetPictureAndShowIt();
        }

        return null;
    }

    public string GetPictureName() // TODO: Implement text to show what picture is being displayed
    {
        return files[whichScreenShotIsShown];
    }

    public void ClearPictures() // TODO: Implement UI button
    {
        if (files.Length >= 0)
        {
            foreach (string filename in files)
            {
                File.Delete(filename);
                Debug.Log("Deleted " + filename);
            }
        }
    }
}
