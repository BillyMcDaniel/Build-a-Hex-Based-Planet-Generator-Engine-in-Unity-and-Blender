using UnityEngine;

[System.Serializable]
public class CellData {
	public TerrainType type;
	public Transform root;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public CellData(TerrainType type, Transform root, MeshFilter meshFilter, MeshRenderer meshRenderer) {
		this.type = type;
		this.root = root;
		this.meshFilter = meshFilter;
		this.meshRenderer = meshRenderer;
	}
}
