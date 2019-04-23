using UnityEngine;
using System.Collections;
using System.IO;

//setting up run system front and back both
public class runSetup : MonoBehaviour
{
    private bool takescreenshotonNextFrame = false;
    private Camera gameCamera;

    private static runSetup instance;
    private static int x_, y_, w_, h_;

    private void Awake()
    {
        gameCamera = gameObject.GetComponent<Camera>();
        instance = this;
        string log_directory = Application.persistentDataPath + "/run_images";
        if (!Directory.Exists(log_directory))
        {
            Directory.CreateDirectory(log_directory);
        }
    }

    private void OnPostRender()
    {
        string run_image = Application.persistentDataPath + "/run_images" + "/run_" + Manager.Instance.run + ".png";

        if(takescreenshotonNextFrame)
        {
            takescreenshotonNextFrame = false;
            RenderTexture renderTex = gameCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGBAFloat, false);
            Rect rect = new Rect(x_, y_, w_, h_);

            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArr = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(run_image,byteArr);

            Debug.Log("image saved for run:" + Manager.Instance.run);
            Debug.Log(run_image);

            RenderTexture.ReleaseTemporary(renderTex);
            gameCamera.targetTexture = null;
        }
    }

    private void takeScreenShot(int width, int height)
    {
        gameCamera.targetTexture = RenderTexture.GetTemporary(width,height,16);
        takescreenshotonNextFrame = true;
    }

    public static void takeScreenShot_static(int width, int height, int x, int y, int w, int h)
    {
        instance.takeScreenShot(width , height);

        w_ = w;
        h_ = h;
        x_ = x;
        y_ = y;
    }
}
