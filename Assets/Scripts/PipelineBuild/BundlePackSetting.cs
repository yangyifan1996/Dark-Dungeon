using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BundlePackSetting : ScriptableObject
{
    //#if UNITY_ANDROID
    public const string BUNDLE_PACK_SETTING_PATH = "Assets/ScriptableObject/BundlePackSetting_Android.asset";
    //#elif UNITY_IOS
    //        public const string BUNDLE_PACK_SETTING_PATH = "Assets/ScriptableObject/BundlePackSetting_iOS.asset";
    //#else
    //    public const string BUNDLE_PACK_SETTING_PATH = "Assets/ScriptableObject/BundlePackSetting.asset";
    //#endif

    private static BundlePackSetting Current
    {
        get
        {
            if (m_Current == null)
            {
                m_Current = AssetDatabase.LoadAssetAtPath<BundlePackSetting>(BUNDLE_PACK_SETTING_PATH);
            }
            return m_Current;
        }
    }
    private static BundlePackSetting m_Current;

    /// <summary>
    /// 自动部署(仅本地)
    /// </summary>
    public static bool AutoDeploy
    {
        get
        {
            return Current.m_AutoDeploy;
        }
    }
    [SerializeField]
    private bool m_AutoDeploy = true;

    /// <summary>
    /// 本地输出路径
    /// </summary>
    public static string OutputPath
    {
        get
        {
            return Current.m_OutputPath;
        }
    }
    [SerializeField]
    private string m_OutputPath = string.Empty;


    /// <summary>
    /// 本地部署路径
    /// </summary>
    public static string DeployPath
    {
        get
        {
            return Current.m_DeployPath;
        }
    }
    [SerializeField]
    private string m_DeployPath = string.Empty;

    /// <summary>
    /// 打包选项
    /// </summary>
    public static BuildAssetBundleOptions Options
    {
        get
        {
            return Current.m_Options;
        }
    }
    [SerializeField]
    private BuildAssetBundleOptions m_Options = BuildAssetBundleOptions.None;

    /// <summary>
    /// 打包前删除的目录
    /// </summary>
    public static List<string> ExcludeDirectory
    {
        get
        {
            return Current.m_ExcludeDirectory;
        }
    }
    [SerializeField]
    private List<string> m_ExcludeDirectory = new List<string>();

    /// <summary>
    /// 打包排除
    /// </summary>
    public static List<string> ExcludeExtension
    {
        get
        {
            return Current.m_ExcludeExtension;
        }
    }
    [SerializeField]
    private List<string> m_ExcludeExtension = new List<string>();

    /// <summary>
    /// 指定打包目录
    /// </summary>
    public static List<BundlePackDirectory> PackToAssetBundle
    {
        get
        {
            return Current.m_PackToAssetBundle;
        }
    }
    [SerializeField]
    private List<BundlePackDirectory> m_PackToAssetBundle = new List<BundlePackDirectory>();

    /// <summary>
    /// 音效bank目录
    /// </summary>
    public static List<PackDirectory> PackToSoundBank
    {
        get
        {
            return Current.m_PackToSoundBank;
        }
    }
    [SerializeField]
    List<PackDirectory> m_PackToSoundBank = new List<PackDirectory>();

    public static bool NeedBundleEncrypt(string assetBundleName)
    {
        bool encryption = false;

        for (int i = 0; i < PackToAssetBundle.Count; i++)
        {
            BundlePackDirectory bundlePackDirectory = PackToAssetBundle[i];

            string dir = bundlePackDirectory.path.ToLower();

            if (assetBundleName.StartsWith(dir))
            {
                encryption = bundlePackDirectory.bundleEncryption;
            }
        }
        return encryption;
    }
}
