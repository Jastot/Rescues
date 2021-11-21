using System;


namespace Rescues
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAudioInterfacesAttribute : Attribute
    {
        public readonly string AssetName;
        public InjectAudioInterfacesAttribute(string assetName = null) =>
            AssetName = assetName;
    }
}