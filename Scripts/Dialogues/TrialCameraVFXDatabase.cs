using System.Collections.Generic;

namespace DREditor.Dialogues
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/CameraVFX/CameraVFX Database", fileName = "CameraVFXDatabase")]
    public class TrialCameraVFXDatabase : ScriptableObject
    {
        public List<TrialCameraVFX> vfxs = new List<TrialCameraVFX>();

        public List<string> GetNames()
        {
            var names = new List<string>();
            vfxs.ForEach(vfx => names.Add(vfx.vfxName));
            return names;
        }
    }
}