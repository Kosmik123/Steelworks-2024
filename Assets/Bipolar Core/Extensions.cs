using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar
{
	public static class CollectionExtensions
	{
		public static T GetRandom<T>(this IReadOnlyList<T> collection) => collection[Random.Range(0, collection.Count)];

		public static T GetRandom<T>(this IReadOnlyCollection<T> collection)
		{
			int randomIndex = Random.Range(0, collection.Count);
			var elem = collection.GetEnumerator();
			for (int i = 0; i < randomIndex; i++)
				elem.MoveNext();

			return elem.Current;
		}

		public static bool AddDistinct<T>(this IList<T> list, T element)
		{
			if (list.Contains(element))
				return false;

			list.Add(element);
			return true;
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			for (int n = 0; n < list.Count; n++)
			{
				int k = Random.Range(0, n + 1);
				(list[n], list[k]) = (list[k], list[n]);
			}
		}
	}

	public static class ComponentExtensions
	{
		public static T GetRequired<T>(this MonoBehaviour owner, ref T component) where T : Component
		{
			if (component == null)
				component = owner.GetComponent<T>();
			return component;
		}
	}

	public static class TransformExtensions
	{
		public static void LookHorizontallyAt(this Transform transform, Transform target) => LookHorizontallyAt(transform, target.position);

		public static void LookHorizontallyAt(this Transform transform, Vector3 target)
		{
			target.y = transform.position.y;
			transform.LookAt(target);
		}
	}

#if UNITY_2022_1_OR_NEWER

	public interface IRequiresRigidbody : IRequires<Rigidbody>
	{
		public Rigidbody Rigidbody => RequiredComponent;
	}

	public interface IRequires<T> where T : Component
	{
		private static Dictionary<Component, T> cachedRequiredComponentsByOwner;

		private void EnsureInitialized()
		{
			if (cachedRequiredComponentsByOwner == null)
			{
				cachedRequiredComponentsByOwner = new Dictionary<Component, T>();
				UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
			}
		}

		private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene arg0)
		{
			foreach (var (owner, _) in cachedRequiredComponentsByOwner)
				if (owner == null)
					cachedRequiredComponentsByOwner.Remove(owner);
		}

		protected T RequiredComponent
		{
			get
			{
				if (this is Component owner && owner.TryGetComponent<T>(out var required))
					return required;
				
				throw new System.TypeAccessException();
			}
		}
	}
#endif
}
