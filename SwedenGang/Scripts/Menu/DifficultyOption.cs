//Author: Benjamin "Sweden" Jillson : Sweden#6386 For Project Eden's Garden
public class DifficultyOption : Selectable
{
    public Image imageOption = null;
    public Sprite normal = null;
    public Sprite selected = null;
    public GameManager.Difficulty difficulty;
    public UnityEvent OnSelection;
    public UnityEvent OnDeselection;
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnSelection?.Invoke();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        OnDeselection?.Invoke();
    }
    public void KeepImage(bool check)
    {
        if (!check)
        {
            imageOption.sprite = selected;
        }
        else
        {
            imageOption.sprite = normal;
        }
    }
}
