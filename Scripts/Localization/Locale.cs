namespace DREditor.Localization
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Localization/Locale", fileName = "New Locale")]
    public class Locale : ScriptableObject
    {
        public Texture2D icon;
        public string langCode;
        public string langName;
        public string langLocalizedName;
    }
}
