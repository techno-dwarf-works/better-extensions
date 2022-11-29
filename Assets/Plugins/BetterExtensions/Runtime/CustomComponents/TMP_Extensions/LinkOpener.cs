#if TEXTMESHPRO
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    /// <summary>
    /// Component to open URL inside TMPro.TMP_Text
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class LinkOpener : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _tmp;

        private void Awake()
        {
            _tmp = GetComponent<TMP_Text>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var linkIndex =
                TMP_TextUtilities.FindIntersectingLink(_tmp, eventData.position,
                    eventData
                        .pressEventCamera); // If you are not in a Canvas using Screen Overlay, put your camera instead of null
            if (linkIndex == -1) return; // was a link clicked?
            var linkInfo = _tmp.textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
#endif