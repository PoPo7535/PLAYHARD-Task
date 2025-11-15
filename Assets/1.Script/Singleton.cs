using Sirenix.OdinInspector;
using UnityEngine;

namespace Utility
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _I;
		private static readonly object _lock = new object();

		public static T I
		{
			get
			{
				if (applicationIsQuitting)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
					return null;
				}

				lock (_lock)
				{
					if (_I == null)
					{
						var all = FindObjectsOfType<T>();
						_I = all != null && all.Length > 0 ? all[0] : null;

						if (all != null && all.Length > 1)
							Debug.LogWarning("[Singleton] There are " + all.Length + " instances of " + typeof(T) + "... This may happen if your singleton is also a prefab, in which case there is nothing to worry about.");

						if (_I == null)
						{
							GameObject singleton = new GameObject();
							_I = singleton.AddComponent<T>();
							singleton.name = "(singleton) " + typeof(T).ToString();

							Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
						}
						else
							Debug.Log("[Singleton] Using instance already created: " + _I.gameObject.name);

						if (Application.isPlaying && _I.gameObject.transform.parent == null)
							DontDestroyOnLoad(_I.gameObject);
					}

					return _I;
				}
			}
		}

		private static bool applicationIsQuitting = false;

		public void OnDestroy()
		{
			_I = null;
		}
	}
	
	public class LocalSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _I;
		private static readonly object _lock = new object();

		public static T I
		{
			get
			{
				if (applicationIsQuitting)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
					return null;
				}

				lock (_lock)
				{
					if (_I == null)
					{
						var all = FindObjectsOfType<T>();
						_I = all != null && all.Length > 0 ? all[0] : null;

						if (all != null && all.Length > 1)
							Debug.LogWarning("[Singleton] There are " + all.Length + " instances of " + typeof(T) + "... This may happen if your singleton is also a prefab, in which case there is nothing to worry about.");

						if (_I == null)
						{
							GameObject singleton = new GameObject();
							_I = singleton.AddComponent<T>();
							singleton.name = "(singleton) " + typeof(T).ToString();

							Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
						}
						else
							Debug.Log("[Singleton] Using instance already created: " + _I.gameObject.name);
					}

					return _I;
				}
			}
		}

		private static bool applicationIsQuitting = false;

		public void OnDestroy()
		{
			_I = null;
		}
	}
	public class SerializeSingleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
	{
		private static T _I;
		private static readonly object _lock = new object();

		public static T I
		{
			get
			{
				if (applicationIsQuitting)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
					return null;
				}

				lock (_lock)
				{
					if (_I == null)
					{
						var all = FindObjectsOfType<T>();
						_I = all != null && all.Length > 0 ? all[0] : null;

						if (all != null && all.Length > 1)
							Debug.LogWarning("[Singleton] There are " + all.Length + " instances of " + typeof(T) + "... This may happen if your singleton is also a prefab, in which case there is nothing to worry about.");

						if (_I == null)
						{
							GameObject singleton = new GameObject();
							_I = singleton.AddComponent<T>();
							singleton.name = "(singleton) " + typeof(T).ToString();

							Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
						}
						else
							Debug.Log("[Singleton] Using instance already created: " + _I.gameObject.name);
					}

					return _I;
				}
			}
		}

		private static bool applicationIsQuitting = false;

		public void OnDestroy()
		{
			_I = null;
		}
	}
}