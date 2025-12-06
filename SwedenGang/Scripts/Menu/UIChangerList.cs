//Author: Benjamin "Sweden" Jillson : Sweden#6386 For Project Eden's Garden
using System.Collections.Generic;

public class UIChangerList : MonoBehaviour
{
    [SerializeField] List<UIDisplayChanger> changers = new List<UIDisplayChanger>();

    public void Select()
    {
        foreach (UIDisplayChanger c in changers)
        {
            c.Select();
        }
    }

    public void Deselect()
    {
        foreach (UIDisplayChanger c in changers)
        {
            c.Deselect();
        }
    }
}
