using System;
using UnityEngine;
[Serializable]
public class BundlePackDirectory : PackDirectory
{
    public CompressionType compressionType;
    public bool bundleEncryption;
}