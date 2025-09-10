using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GLTFast;
using GLTFast.Materials;
using UnityEngine;
using UnityEngine.UI;

public class ModelLoader : MonoBehaviour {
    public Text loadingTime;
    public string[] allModels;

    private int _nbFilesLoaded = 0;

    void Start () {
        _nbFilesLoaded = 0;
        foreach (string model in allModels) {
            Load (Application.streamingAssetsPath + "/" + model);
        }
    }

    async void Load (string url) {
        Stopwatch stopWatch = new Stopwatch ();
        stopWatch.Start ();
        loadingTime.text = "";
        await Load3DFile (url);
        loadingTime.text = stopWatch.Elapsed.ToString ("mm\\:ss\\.ff");
    }

    public static async Task<GameObject> Load3DFile (string filePath) {
        string fileExt = Path.GetExtension (filePath);
        try {
            GameObject go = new GameObject ("GltFastLoader");
            bool isLoaded = false;

            GltfAsset gltfAsset = go.AddComponent<GltfAsset> ();
            gltfAsset.LoadOnStartup = false;

            isLoaded = await gltfAsset.Load ("file://" + filePath);

            while (!isLoaded) {
                await Task.Delay (10);
            }
            // Apply 180° on y to keep rotation as original
            foreach (Transform child in go.transform) {
                child.Rotate (0, 180, 0);
            }
            return go;
        } catch (Exception ex) {
            UnityEngine.Debug.LogError ("Exception: " + ex);
        }
        return null;
    }
}