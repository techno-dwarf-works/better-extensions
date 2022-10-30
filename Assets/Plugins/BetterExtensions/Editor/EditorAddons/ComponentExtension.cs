using UnityEditor;
using UnityEngine;

namespace BetterExtensions.EditorAddons
{
    public static class ComponentExtension
    {
        public static bool IsTargetComponent(this SerializedProperty property, out Component component)
        {
            component = null;
            if (property.serializedObject.targetObject is Component inner)
            {
                component = inner;
                return true;
            }

            return false;
        }
    }
}