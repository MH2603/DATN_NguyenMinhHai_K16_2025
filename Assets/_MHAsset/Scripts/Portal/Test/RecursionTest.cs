using System.Collections.Generic;
using UnityEngine;

public class RecursionTest : MonoBehaviour
{
    public Camera camera;
    public MeshRenderer screen;
    public int RecursionCount;

    public RenderTexture _viewTexture;
    public List<Sprite> _sprites = new();

    // Manually render the camera attached to this portal
    // Called after PrePortalRender, and before PostPortalRender
    public void Render() {

        TryCreateViewTexture ();

        for (int i=0; i < RecursionCount; i++)
        {
            camera.Render ();
            if(i ==0 ) Debug.Break();
            
            _sprites.Add(ConvertRenderTextureToSprite(_viewTexture));
        }
        
        
    }
    
    
    void TryCreateViewTexture()
    {
        if (_viewTexture == null || _viewTexture.width != Screen.width || _viewTexture.height != Screen.height)
        {
            if(_viewTexture != null) _viewTexture.Release();

            //creates a new RenderTexture object with the current screen width, height, and a depth buffer of 24 bits.
            //This RenderTexture is used to render the view from the portal camera.
            _viewTexture = new RenderTexture(Screen.width, Screen.height, 24);
                
            // set the texture to the camera
            camera.targetTexture = _viewTexture;
            
            // set the texture to the screen of linked portal
            screen.material.SetTexture("_BaseMap", _viewTexture);
            screen.material.SetTexture("_MainTex", _viewTexture);
        }
        
    }
    
    public Sprite ConvertRenderTextureToSprite(RenderTexture renderTexture)
    {
        // Create a new Texture2D with the same dimensions as the RenderTexture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Copy the RenderTexture to the Texture2D
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Create a new Sprite from the Texture2D
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
}
