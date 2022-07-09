using System.Collections;
using UnityEngine;

namespace BetterExtension.Runtime
{
    /// <summary>
    /// UI state tool for canvas groups
    /// </summary>
    public static class UIExtension
    {
        /// <summary>
        /// Changing canvas visibility and interactivity
        /// </summary>
        /// <param name="group"></param>
        /// <param name="isVisible"></param>
        public static void SetActive(this CanvasGroup group, bool isVisible)
        {
            group.alpha = isVisible ? 1 : 0;
            group.interactable = isVisible;
            group.blocksRaycasts = isVisible;
        }

        /// <summary>
        /// Changing state of mouse cursor
        /// </summary>
        /// <param name="state"></param>
        public static void SetCursorActive(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = state;
        }

        /// <summary>
        /// Changing canvas visibility and interactivity after delay
        /// </summary>
        /// <param name="group"></param>
        /// <param name="isVisible"></param>
        /// <param name="delay"></param>
        public static IEnumerator SetActive(CanvasGroup group, bool isVisible, float delay)
        {
            group.alpha = isVisible ? 1 : 0;
            group.blocksRaycasts = isVisible;
            yield return new WaitForSeconds(delay);
            group.interactable = isVisible;
        }
    }
}