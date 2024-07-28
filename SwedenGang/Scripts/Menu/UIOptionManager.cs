using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIOptionManager : MonoBehaviour
{
    [SerializeField] List<UIOptionGroup> groups = new List<UIOptionGroup>();
    private void Start()
    {
        groups = GetComponentsInChildren<UIOptionGroup>().ToList();
    }
    public void SetUpGroups()
    {
        groups.ForEach(n => n.SetupGroup());
    }
    public void UnSetUpGroups()
    {
        groups.ForEach(n => n.UnSetupGroup());
    }
}
