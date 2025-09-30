using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D harvestCursor, harvestCursorAnim;
    void Start()
    {

    }

    void Update()
    {

    }

    public void SetHarvestCursor()
    {
        Cursor.SetCursor(harvestCursor, Vector2.zero, CursorMode.Auto);
        //  print("Cursor changed to Harvest");
    }

    public void SetCursorAnim()
    {
        Cursor.SetCursor(harvestCursorAnim, Vector2.zero, CursorMode.Auto);
        Invoke("SetHarvestCursor", .5f);
        print("Cursor changed to Harvest Animation");

    }



    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
