[System.Serializable]
[CreateAssetMenu(menuName = "DREditor/Regulations/Regulation", fileName = "Regulation")]
public class Regulation : ScriptableObject
{ // Code by Willy Bee

    public int regulationNum;
    public Sprite regulationImg;
    public Sprite regulationDesc;

    public Regulation(int index, Sprite img, Sprite desc)
    {
        regulationNum = index;
        regulationImg = img;
        regulationDesc = desc;
    }
}