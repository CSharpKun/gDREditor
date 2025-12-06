//Author: Benjamin "Sweden" Jillson : Sweden#6386 For Project Eden's Garden
public class CGEvents : MonoBehaviour
{
    [SerializeField] Image CG = null;
    public void Shake()
    {
        CG.rectTransform.DOShakePosition(1, 7);
        DialogueAssetReader.instance.trigger = true;
    }
}
