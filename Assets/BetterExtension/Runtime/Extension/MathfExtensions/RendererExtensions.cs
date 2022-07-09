using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterExtension.Runtime.MathfExtensions
{
    public static class RendererExtensions
    {
        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            var screenBounds =
                new Rect(0f, 0f, Screen.width,
                    Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            var objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            return objectCorners.Select(camera.WorldToScreenPoint)
                .Count(tempScreenSpaceCorner => screenBounds.Contains(tempScreenSpaceCorner));
        }

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static int CountCornersNotVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            var screenBounds =
                new Rect(0f, 0f, Screen.width,
                    Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            var objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            return objectCorners.Select(camera.WorldToScreenPoint)
                .Count(tempScreenSpaceCorner => !screenBounds.Contains(tempScreenSpaceCorner));
        }

        public static void KeepFullyOnScreen(this RectTransform movable, RectTransform container)
        {
            var cornersCache = new Vector3[4];
            container.GetWorldCorners(cornersCache);

            // BL = Bottom Left, TR = Top Right (corners)
            Vector3 containerBL = cornersCache[0], containerTR = cornersCache[2];
            var containerSize = containerTR - containerBL; // NEW
            movable.GetWorldCorners(cornersCache);
            Vector3 movableBL = cornersCache[0], movableTR = cornersCache[2];
            var movableSize = movableTR - movableBL; // NEW
            var position = movable.position;
            Vector3 deltaBL = position - movableBL, deltaTR = movableTR - position;

            position.x = movableSize.x < containerSize.x // NEW
                ? Mathf.Clamp(position.x, containerBL.x + deltaBL.x, containerTR.x - deltaTR.x)
                : Mathf.Clamp(position.x, containerTR.x - deltaTR.x, containerBL.x + deltaBL.x); // NEW

            position.y = movableSize.y < containerSize.y // NEW
                ? Mathf.Clamp(position.y, containerBL.y + deltaBL.y, containerTR.y - deltaTR.y)
                : Mathf.Clamp(position.y, containerTR.y - deltaTR.y, containerBL.y + deltaBL.y); // NEW
            movable.position = position;
        }

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static IEnumerable<Vector3> CornersNotVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            var screenBounds =
                new Rect(0f, 0f, Screen.width,
                    Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            var objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            return objectCorners.Select(camera.WorldToScreenPoint)
                .Where(tempScreenSpaceCorner => !screenBounds.Contains(tempScreenSpaceCorner));
        }

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static IEnumerable<Vector3> CornersVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            var screenBounds =
                new Rect(0f, 0f, Screen.width,
                    Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            var objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            return objectCorners.Select(camera.WorldToScreenPoint)
                .Where(tempScreenSpaceCorner => screenBounds.Contains(tempScreenSpaceCorner));
        }

        /// <summary>
        /// Determines if this RectTransform is fully visible from the specified camera.
        /// Works by checking if each bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is fully visible from the specified camera; otherwise, <c>false</c>.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            if (!rectTransform.gameObject.activeInHierarchy) return false;
            return CountCornersVisibleFrom(rectTransform, camera) == 4; // True if all 4 corners are visible
        }

        /// <summary>
        /// Determines if this RectTransform is at least partially visible from the specified camera.
        /// Works by checking if any bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is at least partially visible from the specified camera; otherwise, <c>false</c>.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            if (!rectTransform.gameObject.activeInHierarchy) return false;
            return CountCornersVisibleFrom(rectTransform, camera) > 0; // True if any corners are visible
        }
    }
}