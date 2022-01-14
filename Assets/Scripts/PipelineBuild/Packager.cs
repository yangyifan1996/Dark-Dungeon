using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEngine.Build.Pipeline;

public class Packager
{

    //获取打包平台
    public static string GetPlatformName(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebGL:
                return "WebGL";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
            default:
                return null;
        }
    }

    public static void Exclude()
    {
        for (int i = 0; i < BundlePackSetting.ExcludeDirectory.Count; i++)
        {
            string excludeDirectory = BundlePackSetting.ExcludeDirectory[i];

            if (!string.IsNullOrEmpty(excludeDirectory))
            {
                DeleteDirectory(BundlePackSetting.ExcludeDirectory[i], true);
            }
        }
    }

    public static void DeleteDirectory(string path, bool recursive)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
            }
        }
        catch
        {
            DeleteDirectory(path, recursive);
        }
    }

    [MenuItem("Project/Bundle Packer/Build New", false, 0)]
    public static void New()
    {
        string appVersion = Application.version;
        ulong resVersion = GenerateResVersion();

        New(BundlePackSetting.OutputPath, appVersion, resVersion);

        AssetDatabase.Refresh();
    }

    public static void New(string outputPath, string appVersion, ulong resVersion, bool retainIntermediate = false, bool luaLogEnable = false)
    {
        outputPath = Path.Combine(outputPath, "App" + appVersion);

        string assetBundleBuildPath = Path.Combine(outputPath, "AssetBundles/");

        //打包AssetBundle
        CompatibilityAssetBundleManifest assetBundleManifest = BuildAssetBundles(resVersion, assetBundleBuildPath, true, luaLogEnable);

        if (assetBundleManifest == null)
        {
            Debug.LogError("Build assetbundles failed");
            return;
        }

        string soundBankBuildPath = Path.Combine(outputPath, "SoundBanks/");

        EncryptKeys encryptKeys = new EncryptKeys();

        ////生成资源包文件清单
        //IPackageManifest resourceBundleManifest = BuildResourceBundleManifest(outputPath, assetBundleManifest, soundBankManifest, encryptKeys, appVersion, resVersion);

        ////清单文件写入磁盘
        //using (FileStream fs = new FileStream(Path.Combine(outputPath, "manifest.xml"), FileMode.Create))
        //{
        //    PackageManifest.Serializer.WriteToStream(resourceBundleManifest, fs);
        //}

        //using (FileStream fs = new FileStream(Path.Combine(outputPath, "origin.xml"), FileMode.Create))
        //{
        //    PackageManifest.Serializer.WriteToStream(resourceBundleManifest, fs);
        //}

        ////把路径对应的密钥保存到keys.xml文件
        //using (FileStream fs = new FileStream(Path.Combine(outputPath, "keys.xml"), FileMode.Create))
        //{
        //    encryptKeys.WriteToSteam(fs);
        //}

        ////部署本地资源包，生成上传服务器的更新包

        //if (BundlePackSetting.AutoDeploy)
        //{
        //    Deploy(outputPath, true, true);
        //}

        ////清除目录
        //if (!retainIntermediate)
        //{
        //    if (Directory.Exists(assetBundleBuildPath))
        //    {
        //        DeleteDirectory(assetBundleBuildPath, true);
        //    }

        //    if (Directory.Exists(soundBankBuildPath))
        //    {
        //        DeleteDirectory(soundBankBuildPath, true);
        //    }
        //}
    }

    public static ulong GenerateResVersion()
    {
        string resVersion = string.Format(
                "{0:yy}{1:MM}{2:dd}{3:HH}{4:mm}",
                System.DateTime.Now,
                System.DateTime.Now,
                System.DateTime.Now,
                System.DateTime.Now,
                System.DateTime.Now);

        ulong version;

        if (ulong.TryParse(resVersion, out version))
        {
            return version;
        }
        return 0;
    }

    static CompatibilityAssetBundleManifest BuildAssetBundles(ulong version, string outputPath, bool clear, bool luaLogEnable = false)
    {
        //清除输出
        if (clear)
        {
            if (Directory.Exists(outputPath))
            {
                DeleteDirectory(outputPath, true);
            }
        }
        //创建输出目录
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        //开始打包
        CompatibilityAssetBundleManifest manifest = MyBuildPipeline.BuildAssetBundles(outputPath, EditorUserBuildSettings.activeBuildTarget);

        return manifest;
    }
}