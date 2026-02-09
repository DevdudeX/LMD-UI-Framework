using UnityEngine;

namespace LMDModMenu.Core;

/// <summary>
/// Contains useful helper methods.
/// </summary>
internal static class ModHelpers
{
	// Credit: https://onewheelstudio.com/blog/2020/12/27/c-generics-and-unity
	/// <summary>
	/// Generic method to remove all child components of a certain type.
	/// </summary>
	/// <param name="parent">The parent GameObject.</param>
	/// <typeparam name="T">The component type to remove.</typeparam>
	public static void RemoveChildComponents<T>(this GameObject parent) where T : Component
	{
		T[] components = parent.GetComponentsInChildren<T>();

		foreach (var c in components)
		{
			GameObject.Destroy(c);
		}
	}
}
