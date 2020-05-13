/*
 * AssetLabels Class
 * 
 * A c# class to represent a AssetLabels object
 * with a given string and getLenght method.
 * 
 * @Author Lars Pastoor
 * @Version: 1.0
 * @Date: 10/05/2020
 * 
 * 
 */
[System.Serializable]
public class AssetLabels
{
    public string[] label;
    /*
     * GetLenght() method
     * 
     * returns the length of the imported
     * JSON string.
     * 
     */
    public int GetLabelLenght()
    {
        return label.Length;
    }
}
