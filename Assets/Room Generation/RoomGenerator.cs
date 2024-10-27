using UnityEngine;

public abstract class ResizableWall : MonoBehaviour
{
	public abstract Vector2 Size { get; set; }
}

[System.Serializable]
public class Walls
{
	public Transform Room { get; private set; }

	[SerializeField]
	private Transform ceiling;
	public Transform Ceiling => GetWall(ref ceiling, nameof(ceiling));

	[SerializeField]
	private Transform front;
	public Transform Front => GetWall(ref front, nameof(front));

	[SerializeField]
	private Transform back;
	public Transform Back => GetWall(ref back, nameof(back));

	[SerializeField]
	private Transform left;
	public Transform Left => GetWall(ref left, nameof(left));

	[SerializeField]
	private Transform right;
	public Transform Right => GetWall(ref right, nameof(right));

	[SerializeField]
	private Transform floor;
	public Transform Floor => GetWall(ref floor, nameof(floor));

	public Walls(Walls walls, Transform room) : this(room)
	{
		ceiling = walls.ceiling;
		front = walls.front;
		back = walls.back;
		left = walls.left;
		right = walls.right;
		floor = walls.floor;
	}

	public Walls(Transform room)
	{
		Room = room;
	}

	private Transform GetWall(ref Transform wall, string name)
	{
		if (wall == null)
			wall = GetMissingWall(name);

		return wall;
	}

	private Transform GetMissingWall(string lowercaseName)
	{
		if (TryGetWallInChildren(lowercaseName, out var existingWall))
			return existingWall;

		return CreateMissingWall(lowercaseName);
	}

	private Transform CreateMissingWall(string lowercaseName)
	{
		var wall = GameObject.CreatePrimitive(PrimitiveType.Quad);
		wall.name = $"{char.ToUpper(lowercaseName[0])}{lowercaseName.Substring(1)}";
		wall.transform.parent = Room;
		return wall.transform;
	}

	private bool TryGetWallInChildren(string lowercaseName, out Transform wall)
	{
		for (int i = 0; i < Room.childCount; i++)
		{
			wall = Room.GetChild(i);
			if (wall.name.ToLower().Contains(lowercaseName))
				return true;
		}

		wall = null;
		return false;
	}
}

public class RoomGenerator : MonoBehaviour
{
	[Header("Room parts")]
	[SerializeField]
	private Walls walls;
	public Walls Walls
	{
		get
		{
			walls ??= new Walls(transform);
			if (walls.Room == null)
				walls = new Walls(walls, transform);
			return walls;
		}
	}

	[Header("Room size")]
	[SerializeField, Min(0)]
	private float width = 1;
	[SerializeField, Min(0)]
	private float height = 1;
	[SerializeField, Min(0)]
	private float depth = 1;

	public void Generate()
	{
		ScaleRoom();
		ChangeWallsPositions();
	}

	private void ChangeWallsPositions()
	{
		Vector3 position = Walls.Front.localPosition;
		position.z = depth * -0.5f;
		position.y = height * 0.5f;
		Walls.Front.localPosition = position;

		position = Walls.Back.localPosition;
		position.z = depth * 0.5f;
		position.y = height * 0.5f;
		Walls.Back.localPosition = position;

		position = Walls.Left.localPosition;
		position.x = width * -0.5f;
		position.y = height * 0.5f;
		Walls.Left.localPosition = position;

		position = Walls.Right.localPosition;
		position.x = width * 0.5f;
		position.y = height * 0.5f;
		Walls.Right.localPosition = position;

		position = Walls.Ceiling.localPosition;
		position.y = height;
		Walls.Ceiling.localPosition = position;
	}

	private void ScaleRoom()
	{
		SetSize(Walls.Front, width, height);
		SetSize(Walls.Back, width, height);

		SetSize(Walls.Left, depth, height);
		SetSize(Walls.Right, depth, height);

		SetSize(Walls.Floor, width, depth);
		SetSize(Walls.Ceiling, width, depth);
	}

	private void SetSize(Transform wall, float x, float y)
	{
		if (wall.TryGetComponent<ResizableWall>(out var resizableWall))
			resizableWall.Size = new Vector2(x, y);
		else
			wall.localScale = new Vector3(x, y, 1);
	}

	private void OnValidate()
	{
		Generate();
	}
}