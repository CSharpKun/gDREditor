public class MenuHolder : MonoBehaviour
{
    //public static MenuHolder instance = null;
    private void Awake()
    {

        DontDestroyOnLoad(this);
    }
}
