using System;
using UnityEngine;
public enum ePackageType
{
    local = 0,
    remote = 1,
    localpreload = 2,
    remotepreload = 3,
    localoptional = 4,
    remoteoptional = 5
}

[Serializable]
public class PackDirectory
{
    public string path;
    public ePackageType type;
}

