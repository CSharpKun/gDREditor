using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Tutorial_Asset", menuName = "DREditor/TrialTutorialAsset")]
[Serializable]
public class TrialTutorialAsset : ScriptableObject
{
    public Texture2D title = null;
    public List<Texture2D> pages = new List<Texture2D>();
}
