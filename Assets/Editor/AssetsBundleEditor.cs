using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AssetBundleEditor
{
    static string root_path = "AssetBundles";
    static string variant = "assetbundle";

    [MenuItem("アセットバンドル / ビルド(iPhone向け)", false)]
    static private void BuildAssetBundlesForWindows64()
    {
        UnityEditor.BuildTarget target_platform = UnityEditor.BuildTarget.iOS;

        var output_path = System.IO.Path.Combine(root_path, target_platform.ToString());

        if (System.IO.Directory.Exists(output_path) == false)
        {
            System.IO.Directory.CreateDirectory(output_path);
        }

        var asset_bundle_build_list = new List<UnityEditor.AssetBundleBuild>();
        foreach (string asset_bundle_name in UnityEditor.AssetDatabase.GetAllAssetBundleNames())
        {
            var builder = new AssetBundleBuild();
            builder.assetBundleName = asset_bundle_name;
            builder.assetNames = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(builder.assetBundleName);
            builder.assetBundleVariant = variant;
            asset_bundle_build_list.Add(builder);
        }
        if (asset_bundle_build_list.Count > 0)
        {
            UnityEditor.BuildPipeline.BuildAssetBundles(
                output_path,
                asset_bundle_build_list.ToArray(),
                UnityEditor.BuildAssetBundleOptions.ChunkBasedCompression,
                target_platform
            );
        }
    }
}