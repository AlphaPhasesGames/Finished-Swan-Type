using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PaintMask : MonoBehaviour
{
    public int textureSize = 512;

    public RenderTexture Mask { get; private set; }

    void Awake()
    {
        Mask = new RenderTexture(
            textureSize,
            textureSize,
            0,
            RenderTextureFormat.ARGB32
        );

        Mask.wrapMode = TextureWrapMode.Clamp;
        Mask.filterMode = FilterMode.Bilinear;
        Mask.Create();

        // Assign to material
        Renderer r = GetComponent<Renderer>();
        r.material.SetTexture("_PaintMask", Mask);
    }

    //  PUBLIC so DecalShooter can call it
    public static void PaintCircle(
        RenderTexture target,
        Vector2 uv,
        float radius,
        Color color
    )
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = target;

        GL.PushMatrix();
        GL.LoadPixelMatrix(0, target.width, target.height, 0);

        Vector2 pixel = new Vector2(
            uv.x * target.width,
            (1f - uv.y) * target.height
        );

        float r = radius * target.width;

        Color prevColor = GUI.color;
        GUI.color = color;

        GUI.DrawTexture(
            new Rect(pixel.x - r, pixel.y - r, r * 2f, r * 2f),
            Texture2D.whiteTexture
        );

        GUI.color = prevColor;
        GL.PopMatrix();

        RenderTexture.active = prev;
    }
}
