using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Jenkis
{
    static void DryRunBuild()
    {
        //nothing to do
    }

    static void BuildResources()
    {
        string[] cmdArgs = System.Environment.GetCommandLineArgs();
        //解析参数
        BuildResArgs buildResArgs = BuildResArgs.Parse(cmdArgs);
        //设置版本号
        PlayerSettings.bundleVersion = buildResArgs.appVersion;
        //删除打包排除的目录
        Packager.Exclude();
        //如果打的是新包
        if (buildResArgs.buildNew)
        {
            if (!string.IsNullOrEmpty(buildResArgs.outputPath) && !string.IsNullOrEmpty(buildResArgs.appVersion) && buildResArgs.resVersion > 0)
            {
                Packager.New(buildResArgs.outputPath, buildResArgs.appVersion, buildResArgs.resVersion, buildResArgs.retainIntermediate, buildResArgs.luaLogEnable);
            }
            else
            {
                Debug.LogError("missing command line arguments!");
            }
        }
        else
        {
            //if (!string.IsNullOrEmpty(buildResArgs.outputPath) && !string.IsNullOrEmpty(buildResArgs.appVersion) && buildResArgs.resVersion > 0)
            //{
            //    Packager.Append(buildResArgs.outputPath, buildResArgs.appVersion, buildResArgs.resVersion, buildResArgs.luaLogEnable);
            //}
            //else
            //{
            //    Debug.LogError("missing command line arguments!");
            //}
        }
    }
}
