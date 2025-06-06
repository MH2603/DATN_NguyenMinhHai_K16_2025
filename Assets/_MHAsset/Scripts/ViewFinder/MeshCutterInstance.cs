using System;
using UnityEngine;
using System.Collections.Generic;

public class MeshCutterInstance : MonoBehaviour
{
    // Reference to the plane used for cutting
    public Transform cuttingPlane;
    // Reference to the target object to be cut
    [SerializeField] private GameObject targetObject;

    private Mesh _positiveMesh;
    private Mesh _negativeMesh;
    
    private GameObject _positiveObject, _negativeObject;
    private List<Vector3> _intersections = new();

    // Unity's Start method, called before the first frame update
    void Start()
    {
        // Initialization code can be added here if needed
    }

    // Method to perform the cut operation
    public void Cut()
    {
        
        // Create a plane using the cuttingPlane's up direction and position
        Plane plane = new Plane(cuttingPlane.up, cuttingPlane.transform.position);
            
        // Call the static Cut method to perform the cut
        MeshCutter.Cut(targetObject, plane, out _positiveObject, out _negativeObject,_intersections);
        
        // _positiveMesh = _positiveObject.GetComponent<MeshFilter>().mesh;
        // _negativeMesh = _negativeObject.GetComponent<MeshFilter>().mesh;
        //
        // Vector2[] uvArray = targetObject.GetComponent<MeshFilter>().mesh.uv;
        // Vector3[] vertices = targetObject.GetComponent<MeshFilter>().mesh.vertices;
        // var content = " Target UVs: ";
        // foreach (var uv in uvArray)
        // {
        //    content += uv + "| ";
        // }
        //
        // content += "\n Vertices: ";
        // foreach (var vertex in vertices)
        // {
        //     content += vertex + " | ";
        // }
        // Debug.Log(content);
    }

    private void OnDrawGizmosSelected()
    {
        // if (_positiveMesh) GizmosDrawer.DrawSpheres(MeshCutter.ConvertVerticesToWorldSpace(_positiveMesh, _positiveObject.transform), 0.1f);
        // if (_negativeMesh) GizmosDrawer.DrawSpheres( MeshCutter.ConvertVerticesToWorldSpace(_negativeMesh, _negativeObject.transform), 0.1f);

        for (int i=0; i < _intersections.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_intersections[i], 0.05f);
            
        }
   
    }
}