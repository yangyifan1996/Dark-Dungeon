using System.Collections.Generic;
using UnityEngine;

public class BuildResArgs
{
    public string appVersion
    {
        get;
        private set;
    }

    public ulong resVersion
    {
        get;
        private set;
    }

    public string platform
    {
        get;
        private set;
    }

    public bool buildNew
    {
        get;
        private set;
    }

    public string outputPath
    {
        get;
        private set;
    }

    public bool retainIntermediate
    {
        get;
        private set;
    }

    public bool luaLogEnable
    {
        get;
        private set;
    }

    public BuildResArgs()
    {
        resVersion = 0;
        buildNew = true;
        outputPath = string.Empty;
        appVersion = string.Empty;
        retainIntermediate = false;
        luaLogEnable = true;
    }

    public static BuildResArgs Parse(string[] args)
    {
        List<string> listArgs = new List<string>(args);

        BuildResArgs buildResArgs = new BuildResArgs();
        
        for(int index = 0; index < listArgs.Count; index++)
        {
            string cmd = listArgs[index];

            switch (cmd)
            {
                case "-appVersion":
                    buildResArgs.appVersion = listArgs[index + 1];
                    break;
                case "-resVersion":
                    buildResArgs.resVersion = ulong.Parse(listArgs[index + 1]);
                    break;
                case "-platform":
                    buildResArgs.platform = listArgs[index + 1];
                    break;
                case "-buildNew":
                    buildResArgs.buildNew = bool.Parse(listArgs[index + 1]);
                    break;
                case "-outputPath":
                    buildResArgs.outputPath = listArgs[index + 1];
                    break;
                case "-retainIntermediate":
                    buildResArgs.retainIntermediate = bool.Parse(listArgs[index + 1]);
                    break;
                case "-luaLogEnable":
                    buildResArgs.luaLogEnable = bool.Parse(listArgs[index + 1]);
                    break;
                default:
                    continue;
            }
        }
        return buildResArgs;
    }
}
