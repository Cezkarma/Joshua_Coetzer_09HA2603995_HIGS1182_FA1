using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Tooltip("Length of each arm of the cross, in pixels.")]
    [SerializeField] private float size = 18f;
    [SerializeField] private float thickness = 2f;
    [SerializeField] private Color color = Color.white;

    private void OnGUI()
    {
        Vector2 centre = new Vector2(Screen.width, Screen.height) * 0.5f;

        GUI.color = color;
        GUI.DrawTexture(new Rect(centre.x - size * 0.5f, centre.y - thickness * 0.5f, size, thickness), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(centre.x - thickness * 0.5f, centre.y - size * 0.5f, thickness, size), Texture2D.whiteTexture);
    }
}
