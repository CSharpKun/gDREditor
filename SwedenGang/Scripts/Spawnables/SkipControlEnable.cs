//Author: Benjamin "Sweden" Jillson : Sweden#6386 For Project Eden's Garden
public class SkipControlEnable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!GameSaver.LoadingFile)
        {
            RoomLoader.skipActivateControls = true;
            RoomLoader.PreEndLoad += EndLoad;
        }

    }

    void EndLoad()
    {
        RoomLoader.skipActivateControls = false;
    }
    private void OnDisable()
    {
        RoomLoader.PreEndLoad -= EndLoad;
        EndLoad();
    }
}
