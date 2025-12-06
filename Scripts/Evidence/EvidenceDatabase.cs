using System.Collections.Generic;
[System.Serializable]
[CreateAssetMenu(menuName = "DREditor/Trials/EvidenceDB", fileName = "EvidenceDatabase")]
public class EvidenceDatabase : ScriptableObject
{
    public List<Evidence> Evidences = new List<Evidence>();
}
