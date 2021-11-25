using System;
using System.Reflection;


namespace Rescues
{
    public static class AudioControllerInjector
    {
        #region Fields

        private static readonly Type _injectAudioInterfacesAttribute = typeof(InjectAudioInterfacesAttribute);

        #endregion


        #region Methods

        public static T Inject<T>(this AudioControllerContext context, T target)
        {
            var targetType = target.GetType();
            var allFields = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < allFields.Length; i++)
            {
                var fieldInfo = allFields[i];
                var injectAssetAttribute = fieldInfo.GetCustomAttribute(_injectAudioInterfacesAttribute) as InjectAudioInterfacesAttribute;
                if (injectAssetAttribute == null)
                {
                    continue;
                }
                var objectToInject = context.GetObjectOfType(fieldInfo.FieldType);
                fieldInfo.SetValue(target, objectToInject);
            }
            return target;
        } 

        #endregion
    }
}