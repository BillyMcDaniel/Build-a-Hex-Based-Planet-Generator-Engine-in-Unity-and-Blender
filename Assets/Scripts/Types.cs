using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellData {
	public TerrainType type;
	public Transform root;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public List<int> neighbors = new List<int>();
	public CellData(TerrainType type, Transform root, MeshFilter meshFilter, MeshRenderer meshRenderer) {
		this.type = type;
		this.root = root;
		this.meshFilter = meshFilter;
		this.meshRenderer = meshRenderer;
	}
}

[System.Serializable]
public class Region {
	public List<CellData> frontier = new List<CellData>();
}

[System.Serializable]
public class TerrainRule {
	public string displayName;
	public TerrainType from;
	public TerrainType to;
	[Range(0.01f, 0.99f)] public float coverage;
	public Vector2Int seedRange;
}
