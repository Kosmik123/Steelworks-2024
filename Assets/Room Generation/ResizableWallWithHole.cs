using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ResizableWallWithHole : ResizableWall
{
	public const int numberOfVertices = 16;
	public const int numberOfQuads = 8;

	private static readonly int[] verticesWithQuad = { 0, 1, 2, 4, 6, 8, 9, 10 };

	private MeshFilter _meshFilter;
	public MeshFilter MeshFilter
	{
		get
		{
			if (_meshFilter == null)
				_meshFilter = GetComponent<MeshFilter>();
			return _meshFilter;
		}
	}

	[SerializeField]
	private Vector2 wallSize;
	public override Vector2 Size
	{
		get => wallSize;
		set
		{
			transform.localScale = Vector3.one;
			wallSize = value;
			ValidateHoleDimensions();
			UpdateMesh();
		}
	}

	[field: SerializeField]
	public Vector2 HolePosition { get; set; }
	[field: SerializeField]
	public Vector2 HoleSize { get; set; }

	private void Reset()
	{
		var roomGenerator = GetComponentInParent<RoomGenerator>();
		if (roomGenerator)
			roomGenerator.Generate();
	}

	private void UpdateMesh()
	{
		const int trianglesPerQuad = 2;
		const int verticesPerTriangle = 3;

		var vertices = new Vector3[numberOfVertices];
		var triangles = new int[numberOfQuads * trianglesPerQuad * verticesPerTriangle];
		var uvs = new Vector2[numberOfVertices];

		PopulateMeshVerticesAndUVs(vertices, uvs);
		PopulateTriangles(triangles);

		var mesh = new Mesh();
		mesh.name = "Wall With Hole";
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		MeshFilter.sharedMesh = mesh;
	}

	private static void PopulateTriangles(int[] triangles)
	{
		int triangleIndex = 0;
		foreach (var vertex in verticesWithQuad)
		{
			triangles[triangleIndex++] = vertex;
			triangles[triangleIndex++] = vertex + 4;
			triangles[triangleIndex++] = vertex + 5;
			triangles[triangleIndex++] = vertex + 5;
			triangles[triangleIndex++] = vertex + 1;
			triangles[triangleIndex++] = vertex;
		}
	}

	private void PopulateMeshVerticesAndUVs(Vector3[] vertices, Vector2[] uvs)
	{
		Vector2 halfHoleSize = HoleSize / 2;
		Vector2 halfWallSize = Size / 2;
		vertices[0] = new Vector3(-halfWallSize.x, -halfWallSize.y);
		vertices[1] = new Vector3(HolePosition.x - halfHoleSize.x, -halfWallSize.y);
		vertices[2] = new Vector3(HolePosition.x + halfHoleSize.x, -halfWallSize.y);
		vertices[3] = new Vector3(halfWallSize.x, -halfWallSize.y);

		vertices[4] = new Vector3(-halfWallSize.x, HolePosition.y - halfHoleSize.y);
		vertices[5] = new Vector3(HolePosition.x - halfHoleSize.x, HolePosition.y - halfHoleSize.y);
		vertices[6] = new Vector3(HolePosition.x + halfHoleSize.x, HolePosition.y - halfHoleSize.y);
		vertices[7] = new Vector3(halfWallSize.x, HolePosition.y - halfHoleSize.y);

		vertices[8] = new Vector3(-halfWallSize.x, HolePosition.y + halfHoleSize.y);
		vertices[9] = new Vector3(HolePosition.x - halfHoleSize.x, HolePosition.y + halfHoleSize.y);
		vertices[10] = new Vector3(HolePosition.x + halfHoleSize.x, HolePosition.y + halfHoleSize.y);
		vertices[11] = new Vector3(halfWallSize.x, HolePosition.y + halfHoleSize.y);

		vertices[12] = new Vector3(-halfWallSize.x, halfWallSize.y);
		vertices[13] = new Vector3(HolePosition.x - halfHoleSize.x, halfWallSize.y);
		vertices[14] = new Vector3(HolePosition.x + halfHoleSize.x, halfWallSize.y);
		vertices[15] = new Vector3(halfWallSize.x, halfWallSize.y);

		for (int i = 0; i < numberOfVertices; i++)
		{
			uvs[i] = new Vector2(
				(vertices[i].x + halfHoleSize.x) / Size.x,
				(vertices[i].y + halfHoleSize.y) / Size.y);
		}
	}

	private void OnValidate()
	{
		ValidateHoleDimensions();
		UpdateMesh();
	}

	private void ValidateHoleDimensions()
	{
		ValidateHoleSize();
		ValidateHolePosition();
	}

	private void ValidateHolePosition()
	{
		Vector2 halfHoleSize = HoleSize / 2;
		Vector2 halfWallSize = Size / 2;
		var position = HolePosition;
		position.x = Mathf.Clamp(HolePosition.x, -halfWallSize.x + halfHoleSize.x, halfWallSize.x - halfHoleSize.x);
		position.y = Mathf.Clamp(HolePosition.y, -halfWallSize.y + halfHoleSize.y, halfWallSize.y - halfHoleSize.y);
		HolePosition = position;
	}

	private void ValidateHoleSize()
	{
		var size = HoleSize;
		size.x = Mathf.Clamp(HoleSize.x, 0, wallSize.x);
		size.y = Mathf.Clamp(HoleSize.y, 0, wallSize.y);
		HoleSize = size;
	}
}