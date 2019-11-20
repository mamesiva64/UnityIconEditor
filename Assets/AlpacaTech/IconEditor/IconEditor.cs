using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif


[ExecuteInEditMode]
public class IconEditor : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject root;
    [SerializeField] private int width = 256;
    [SerializeField] private int height = 256;
    [SerializeField] private string prefix = "icon";
    [SerializeField] private string folder = "icons";
    [SerializeField] private bool activeSwitch = true;

#if UNITY_EDITOR
    [ContextMenu("SaveIcons")]
    private void SaveIcons()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        //  enum root childlens
        var objects = new List<GameObject>();
        var objectsActive = new List<bool>();
        foreach (Transform transform in root.transform)
        {
            var obj = transform.gameObject;
            objectsActive.Add(obj.activeSelf);
            objects.Add(obj);
            if (activeSwitch)
            {
                obj.SetActive(false);
            }
        }


        //  
        var dir = Application.dataPath + "/../IconEditor";
        Directory.CreateDirectory(dir);
        dir += "/" + folder;
        Directory.CreateDirectory(dir);

        //  render loop
        for (int i = 0; i < objects.Count; ++i)
        {
            var obj = objects[i];
            var renderTexture = new RenderTexture(width, height, 32);

            //  描画
            camera.targetTexture = renderTexture;
            if (activeSwitch)
            {
                obj.SetActive(true);
            }
            camera.Render();
            if (activeSwitch)
            {
                obj.SetActive(false);
            }

            //  保存
            RenderTexture.active = renderTexture;
            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            // Encode texture into PNG
            var bytes = tex.EncodeToPNG();
            //          Object.Destroy(tex);

            //  save png
            File.WriteAllBytes(dir + "/" + prefix + i.ToString("D2") + ".png", bytes);
        }

        for (int i = 0; i < objects.Count; ++i)
        {
            objects[i].SetActive(objectsActive[i]);
        }

        camera.targetTexture = null;

        OpenFolder(dir);
    }

    public static void OpenFolder(string path)
    {
        switch(Application.platform)
        {
            case RuntimePlatform.OSXEditor: 
                System.Diagnostics.Process.Start(path);
                break;
            case RuntimePlatform.WindowsEditor:
                EditorUtility.RevealInFinder(path);
                break;
            case RuntimePlatform.LinuxEditor:
                break;
        }
    }
#endif


}
