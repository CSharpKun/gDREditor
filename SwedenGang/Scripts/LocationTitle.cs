//Author: Benjamin "Sweden" Jillson : Sweden#6386 For Project Eden's Garden
public class LocationTitle : MonoBehaviour
{
    [SerializeField] string title;
    private void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.SetLocation(title);
        }
    }
}
