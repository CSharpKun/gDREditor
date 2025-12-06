//Author: Benjamin "Sweden" Jillson : Sweden#6386 For Project Eden's Garden
/// <summary>
/// A behaviour for changing the Chapter UI by index of the chapter
/// </summary>
public class ChapterUI : MonoBehaviour
{
    public Sprite defaultSprite = null;
    public Image image = null;
    [Header("Sprite by Chapter Index")]
    public Sprite[] sprites = null;
    private void Start()
    {
        if (!defaultSprite || !image)
            Debug.LogError("Chapter UI Requires a Default Sprite to be used! \n Or, the image component hasn't been referenced!");
        ProgressionManager.OnChapterChange += ChangeUI;
    }

    void ChangeUI(int i) => image.sprite = sprites[i] != null ? sprites[i] : defaultSprite;
}