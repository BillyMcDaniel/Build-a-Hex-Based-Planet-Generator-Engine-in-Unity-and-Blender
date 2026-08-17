using UnityEngine;

public class CellFix : MonoBehaviour {

    [SerializeField] Transform[] children;

    void Start() {
        foreach(Transform child in children) {
            CreateCell(child);
        }
    }

    void CreateCell(Transform meshTransform) {
        MeshFilter mf = meshTransform.GetComponent<MeshFilter>();
        if(mf != null) {
            Mesh mesh = mf.sharedMesh;
            // Calculate the average normal
            Vector3 normal = Vector3.zero;
            foreach(Vector3 n in mesh.normals) {
                normal += n;
            }
            normal.Normalize();
            normal = meshTransform.TransformDirection(normal);
            // Create parent
            GameObject goCell = new GameObject(meshTransform.name);
            goCell.transform.SetParent(transform);
            goCell.transform.position = meshTransform.position;
            goCell.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
            // Parent mesh while preserving world transform
            meshTransform.SetParent(goCell.transform, true);
            meshTransform.name = "Mesh";
        }
        
    }

}
