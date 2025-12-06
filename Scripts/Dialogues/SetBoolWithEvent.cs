using DREditor.EventObjects;

public class SetBoolWithEvent : MonoBehaviour
{
    public BoolWithEvent BoolWithEvent;

    public void SetBool()
    {
        if (BoolWithEvent != null) BoolWithEvent.Value = true;
    }
}
