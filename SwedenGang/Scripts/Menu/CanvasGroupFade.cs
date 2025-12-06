using DG.Tweening;
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFade : MonoBehaviour
{
    CanvasGroup group;
    private void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float time)
    {
        group.DOKill();
        group.DOFade(1, time).SetUpdate(true);
    }
    public void FadeOut(float time)
    {
        group.DOKill();
        group.DOFade(0, time).SetUpdate(true);
    }
}
