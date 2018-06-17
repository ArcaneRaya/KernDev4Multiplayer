using UnityEngine;

[System.Serializable]
public class FeedItem
{
    public int ID
    {
        get
        {
            return id;
        }
    }

    public string Message
    {
        get
        {
            return message;
        }
    }

    public Texture2D Image
    {
        get
        {
            return image;
        }
    }

    private int id;
    private string message;
    private Texture2D image;

    public FeedItem(int id, string message, Texture2D image)
    {
        this.id = id;
        this.message = message;
        this.image = image;
    }
}