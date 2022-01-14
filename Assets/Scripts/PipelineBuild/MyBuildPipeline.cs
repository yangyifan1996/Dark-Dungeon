using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.Build.Pipeline;

public class MyBuildPipeline
{
    public static CompatibilityAssetBundleManifest BuildAssetBundles(string outputPath, BuildTarget buildTarget)
    {
        Dictionary<string, ResourceBundleBuild> builds = GenerateAssetBundleBuilds();
        BundleBuildContent content = new BundleBuildContent(builds.Values.Select(build => build.assetBundleBuild).ToArray());
        BuildTargetGroup group = UnityEditor.BuildPipeline.GetBuildTargetGroup(buildTarget);
        BuildParameters parameters = new BuildParameters(buildTarget, group, outputPath, builds);

        if ((BundlePackSetting.Options & BuildAssetBundleOptions.ForceRebuildAssetBundle) != 0)
            parameters.UseCache = false;

        if ((BundlePackSetting.Options & BuildAssetBundleOptions.AppendHashToAssetBundleName) != 0)
            parameters.AppendHash = true;

        if ((BundlePackSetting.Options & BuildAssetBundleOptions.ChunkBasedCompression) != 0)
            parameters.BundleCompression = UnityEngine.BuildCompression.LZ4;
        else if ((BundlePackSetting.Options & BuildAssetBundleOptions.UncompressedAssetBundle) != 0)
            parameters.BundleCompression = UnityEngine.BuildCompression.Uncompressed;
        else
            parameters.BundleCompression = UnityEngine.BuildCompression.LZMA;
        if ((BundlePackSetting.Options & BuildAssetBundleOptions.DisableWriteTypeTree) != 0)
            parameters.ContentBuildFlags |= ContentBuildFlags.DisableWriteTypeTree;

        IBundleBuildResults results;
        //IList<IBuildTask> buildTask = AssetBundleCompatible(); //用于修复老版本sbp会打包preview asset的bug
        ReturnCode exitCode = ContentPipeline.BuildAssetBundles(parameters, content, out results);
        if (exitCode < ReturnCode.Success)
        {
            Debug.LogError(exitCode.ToString());
            return null;
        }
        CompatibilityAssetBundleManifest manifest = ScriptableObject.CreateInstance<CompatibilityAssetBundleManifest>();
        manifest.SetResults(results.BundleInfos);
        File.WriteAllText(parameters.GetOutputFilePathForIdentifier(Path.GetFileName(outputPath) + ".manifest"), manifest.ToString());
        //Write Json For Assetbunble Reporter
        File.WriteAllText(parameters.GetOutputFilePathForIdentifier(Path.GetFileName(outputPath) + "manifest.json"), JsonUtility.ToJson(manifest).ToString());
        return manifest;
    }

    static Dictionary<string, ResourceBundleBuild> GenerateAssetBundleBuilds()
    {
        Dictionary<string, ResourceBundleBuild> assetBundleBuildes = new Dictionary<string, ResourceBundleBuild>();

        for (int i = 0; i < BundlePackSetting.PackToAssetBundle.Count; i++)
        {
            BundlePackDirectory bundlePackDirectory = BundlePackSetting.PackToAssetBundle[i];

            if (bundlePackDirectory != null)
            {
                GenerateAssetBundleBuilds(bundlePackDirectory.path, bundlePackDirectory.compressionType, BundlePackSetting.ExcludeExtension, assetBundleBuildes);
            }
        }
        return assetBundleBuildes;
    }

    static void GenerateAssetBundleBuilds(string path, UnityEngine.CompressionType compressionType, List<string> excludeExtenstions, Dictionary<string, ResourceBundleBuild> assetBundleBuildes)
    {
        string assetBundleName = path.Replace("\\", "/");
        assetBundleName = Path.ChangeExtension(assetBundleName, ".bdl");
        assetBundleName = assetBundleName.ToLower();

        ResourceBundleBuild resourceBundleBuild;
        if (assetBundleBuildes.TryGetValue(assetBundleName, out resourceBundleBuild))
        {
            resourceBundleBuild.compressionType = compressionType;
        }
        else
        {
            //collect all files
            string[] assetNames = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);

            assetNames = assetNames.Select(name => name.Replace("\\", "/")).ToArray();

            //filter asset
            assetNames = assetNames.Where(
                filePath =>
                {
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        string extension = Path.GetExtension(filePath);
                        if (!string.IsNullOrEmpty(extension))
                        {
                            return !excludeExtenstions.Contains(extension);
                        }
                        return true;
                    }
                    return false;

                }).ToArray();

            if (assetNames.Length > 0)
            {
                AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
                assetBundleBuild.assetBundleName = assetBundleName;
                assetBundleBuild.assetNames = assetNames;
                assetBundleBuild.addressableNames = assetNames.Select(name => { return Path.GetFileNameWithoutExtension(name).ToLower(); }).ToArray();

                resourceBundleBuild = new ResourceBundleBuild();
                resourceBundleBuild.assetBundleBuild = assetBundleBuild;
                resourceBundleBuild.compressionType = compressionType;

                assetBundleBuildes.Add(assetBundleName, resourceBundleBuild);
            }
        }

        string[] subDirectories = AssetDatabase.GetSubFolders(path);
        foreach (string subPath in subDirectories)
        {
            GenerateAssetBundleBuilds(subPath, compressionType, excludeExtenstions, assetBundleBuildes);
        }
    }
}

