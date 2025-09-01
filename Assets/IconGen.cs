using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class IconGen : MonoBehaviour
{
    [SerializeField] protected GameObject[] objs;
    [SerializeField] protected Canvas cnvs;
    [SerializeField] protected Camera cam;

    [SerializeField]
    protected GameObject[] objectsToActivateOnGameStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < objectsToActivateOnGameStart.Length; i++)
        {
            objectsToActivateOnGameStart[i].SetActive(false);
        }
        
        for (int i = 0; i < objs.Length; i++)
        {
            DoSnapshot(objs[i], cnvs, cam);
        }

        for (int i = 0; i < objectsToActivateOnGameStart.Length; i++)
        {
            objectsToActivateOnGameStart[i].SetActive(true);
        }
        
        // Deactivate self
        gameObject.SetActive(false);
    }

    private static void DoSnapshot(GameObject go, Canvas canvas, Camera cam)
    {
        var ins = GameObject.Instantiate(go, canvas.transform, false);
        // var ins = GameObject.Instantiate(go, Transform.Instantiate() Vector3.zero, false);
        // ins.transform.position = Vector3.zero;
        ins.transform.localScale = new Vector3(100, 100, 1);
        ins.transform.localPosition = new Vector3(0, -100, 0);
        ins.SetActive(true);

        // string fileName = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(go)) + ".png";
        string fileName = go.name + ".png";
        string astPath = "Assets/UI/Images/" + fileName;
        fileName = Application.dataPath + "/UI/Images/" + fileName;
        FileInfo info = new FileInfo(fileName);
        if (info.Exists)
            File.Delete(fileName);
        else if (!info.Directory.Exists)
            info.Directory.Create();

        var renderTarget = RenderTexture.GetTemporary(1920, 1080, 10);
        cam.aspect = 1920.0f / 1080f;
        cam.orthographic = true;
        cam.targetTexture = renderTarget;
        cam.Render();

        RenderTexture.active = renderTarget;
        Texture2D tex = new Texture2D(renderTarget.width, renderTarget.height);
        tex.ReadPixels(new Rect(0, 0, renderTarget.width, renderTarget.height), 0, 0);

        File.WriteAllBytes(fileName, tex.EncodeToPNG());

        cam.targetTexture = null;
        Object.DestroyImmediate(ins);
    }

    private void OnGUI()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            // Debug.Log("Updating sprite renderers");
            // sprite_rndrs[i].sprite = sprites[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}