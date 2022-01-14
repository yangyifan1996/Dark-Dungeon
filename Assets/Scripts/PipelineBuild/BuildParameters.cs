using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Pipeline;

public class BuildParameters : BundleBuildParameters
{
    public Dictionary<string, ResourceBundleBuild> BundleCompressions { get; set; }

    public BuildParameters(BuildTarget target, BuildTargetGroup group, string outputFolder, Dictionary<string, ResourceBundleBuild> resourceBundleBuilds) : base(target, group, outputFolder)
    {
        BundleCompressions = resourceBundleBuilds;
    }

    public override BuildCompression GetCompressionForIdentifier(string identifier)
    {
        ResourceBundleBuild resourceBundleBuild;

        if (BundleCompressions.TryGetValue(identifier, out resourceBundleBuild))
        {
            switch (resourceBundleBuild.compressionType)
            {
                case CompressionType.Lz4:
                case CompressionType.Lz4HC:
                    return BuildCompression.LZ4;
                case CompressionType.Lzma:
                    return BuildCompression.LZMA;
                case CompressionType.None:
                    return BuildCompression.Uncompressed;
            }
        }
        return base.GetCompressionForIdentifier(identifier);
    }
}

