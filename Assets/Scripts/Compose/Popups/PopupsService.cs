using ArcCreate.Gameplay.Data;
using UnityEngine;

namespace ArcCreate.Compose.Popups
{
    public class PopupsService : MonoBehaviour, IPopupsService
    {
        [SerializeField] private GameObject textDialogPrefab;
        [SerializeField] private Transform textDialogParent;
        [SerializeField] private Notification notification;
        [SerializeField] private ColorPickerWindow colorPickerWindow;
        [SerializeField] private ArcTypePickerWindow arcTypePickerWindow;
        [SerializeField] private ArcColorPickerWindow arcColorPickerWindow;
        [SerializeField] private TimingGroupPicker timingGroupPickerWindow;
        [SerializeField] private Hint hint;
        [SerializeField] private AudioClip vineboom;

        public void CreateTextDialog(string title, string content, params ButtonSetting[] buttonSettings)
        {
            GameObject go = Instantiate(textDialogPrefab, textDialogParent);
            TextDialog dialog = go.GetComponent<TextDialog>();
            dialog.Setup(title, content, buttonSettings);
        }

        public void Notify(Severity severity, string content)
        {
            notification.SetContent(severity, content);
        }

        public ArcTypePickerWindow OpenArcTypePicker(Vector2 screenPosition, ArcLineType? defaultType, object caller)
        {
            arcTypePickerWindow.OpenAt(screenPosition, defaultType, caller);
            return arcTypePickerWindow;
        }

        public ArcColorPickerWindow OpenArcColorPicker(Vector2 screenPosition, int? defaultColor, object caller)
        {
            arcColorPickerWindow.OpenAt(screenPosition, defaultColor, caller);
            return arcColorPickerWindow;
        }

        public ColorPickerWindow OpenColorPicker(Vector2 screenPosition, Color defaultColor)
        {
            colorPickerWindow.OpenAt(screenPosition, defaultColor);
            return colorPickerWindow;
        }

        public TimingGroupPicker OpenTimingGroupPicker(Vector2 screenPosition, int? defaultTg, object caller)
        {
            timingGroupPickerWindow.OpenAt(screenPosition, defaultTg, caller);
            return timingGroupPickerWindow;
        }

        public void PlayVineBoom()
        {
            AudioSource.PlayClipAtPoint(vineboom, Vector3.zero);
        }

        public void ShowHint(Vector2 screenPosition, Severity severity, string text, RectTransform rect, Camera camera)
        {
            hint.SetContent(screenPosition, severity, text, rect, camera);
        }
    }
}