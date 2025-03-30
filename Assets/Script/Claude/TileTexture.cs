using UnityEngine;

public class TileTextureGenerator : MonoBehaviour
{
    public int textureSize = 64;
    public int tileSize = 8;
    public Color color1 = Color.white;
    public Color color2 = Color.gray;

    void Start()
    {
        Texture2D texture = GenerateTileTexture();
        ApplyTextureToFloor(texture);
    }

    Texture2D GenerateTileTexture()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                Color color = ((x / tileSize) + (y / tileSize)) % 2 == 0 ? color1 : color2;
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        texture.filterMode = FilterMode.Point; // This makes the texture pixelated
        texture.wrapMode = TextureWrapMode.Repeat;
        return texture;
    }

    void ApplyTextureToFloor(Texture2D texture)
    {
        GameObject floor = GameObject.Find("Floor");
        if (floor != null)
        {
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                floorRenderer.material.mainTexture = texture;
                floorRenderer.material.mainTextureScale = new Vector2(10, 10); // Adjust this to change the tiling
            }
        }
    }
}