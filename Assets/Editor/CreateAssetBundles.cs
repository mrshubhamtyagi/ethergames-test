using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("AssetBundles/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}
