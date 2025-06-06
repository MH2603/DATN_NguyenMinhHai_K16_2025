using UnityEngine;

namespace MH
{
    public static class ViewFinderExtension
    {
        public static Texture2D ConvertToStaticTexture2D(RenderTexture renderTexture)
        {
            // Step 1: Create a new Texture2D with the same dimensions as the RenderTexture
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

            // Step 2: Save the current active RenderTexture
            RenderTexture currentRT = RenderTexture.active;

            // Step 3: Create a temporary RenderTexture
            RenderTexture tempRT = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 0, renderTexture.format);

            // Step 4: Blit the RenderTexture to the temporary RenderTexture
            Graphics.Blit(renderTexture, tempRT);

            // Step 5: Set the temporary RenderTexture as active
            RenderTexture.active = tempRT;

            // Step 6: Read the pixels from the temporary RenderTexture into the Texture2D
            texture.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);

            // Step 7: Apply changes to the Texture2D
            texture.Apply();

            // Step 8: Restore the previous active RenderTexture
            RenderTexture.active = currentRT;

            // Step 9: Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tempRT);

            return texture;
        }
        
        public static Plane[] GetCameraViewPlanes(Camera camera)
        {
            
            Plane[] planes = new Plane[4];
            Vector3[] frustumCorners = new Vector3[4];

            // Get the frustum corners in world space
            camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

            Vector3 bottomLeft = camera.transform.TransformPoint(frustumCorners[0]);
            Vector3 topLeft = camera.transform.TransformPoint(frustumCorners[1]);
            Vector3 topRight = camera.transform.TransformPoint(frustumCorners[2]);
            Vector3 bottomRight = camera.transform.TransformPoint(frustumCorners[3]);

            // Create planes from the frustum corners
            planes[0] = new Plane(camera.transform.position, bottomLeft, topLeft); // Left plane
            planes[1] = new Plane(camera.transform.position, topLeft, topRight); // Top plane
            planes[2] = new Plane(camera.transform.position, topRight, bottomRight); // Right plane
            planes[3] = new Plane(camera.transform.position, bottomRight, bottomLeft); // Bottom plane

            return planes;
        }
    }
}