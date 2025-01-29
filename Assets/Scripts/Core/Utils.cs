using System;
using System.Collections.Generic;
using UnityEngine;

namespace ELY
{
    public static class Utils {
        
        /// <summary>
        /// Searches for a component of type T in the parent hierarchy of the given transform.
        /// Starts from the immediate parent and goes up the hierarchy.
        /// </summary>
        /// <typeparam name="T">The type of component to find.</typeparam>
        /// <param name="childTransform">The starting transform.</param>
        /// <param name="logError">If true, logs an error if the component is not found.</param>
        /// <returns>The component of type T if found; otherwise, null.</returns>
        public static T FindComponentInParents<T>(Transform childTransform, bool logError = true) where T : Component
        {
            Transform currentParent = childTransform.parent;

            while (currentParent != null)
            {
                if (currentParent.TryGetComponent(out T component))
                {
                    return component;
                }
                currentParent = currentParent.parent;
            }

            if (logError)
            {
                Debug.LogError($"Did not find a component of type {typeof(T).Name} in parents of {childTransform.name}.");
            }

            return null;
        }

        /// <summary>
        /// Adds a specified component to all child GameObjects of the given parent and its descendants, 
        /// if those GameObjects already have a component of type T.
        /// </summary>
        /// <typeparam name="T">The type of the component to check for on the GameObjects.</typeparam>
        /// <param name="componentTypeToAdd">The type of the component to add to the GameObjects.</param>
        /// <param name="parent">The parent Transform whose children and descendants will be processed.</param>
        /// <param name="onComponentAdded">The delegate that runs after the component is added to a GameObject.</param>
        public static void AddComponentToAllParentsChildsIfHasComponent<T>(Type componentTypeToAdd, Transform parent, Action<Transform> onComponentAdded = default) where T : Component
        {
            // Loop through all the children of the parent Transform (direct children)
            foreach (Transform childTransform in parent)
            {
                // Check if this child has the specified component T
                if (childTransform.GetComponent<T>() != null)
                {
                    Debug.Log($"Child {childTransform.name} has component {typeof(T).Name}");

                    // Ensure the type to add is a subclass of Component
                    if (componentTypeToAdd.IsSubclassOf(typeof(Component)))
                    {
                        // Add the component to the child GameObject
                        Component newComponent = childTransform.gameObject.AddComponent(componentTypeToAdd);
                        Debug.Log($"Added {componentTypeToAdd.Name} to {childTransform.name}");

                        // After the component is added, invoke the delegate
                        onComponentAdded?.Invoke(childTransform);
                    }
                    else
                    {
                        Debug.LogError($"The type {componentTypeToAdd} is not a subclass of Component.");
                    }
                }

                // Recursively call this method to check for children and grandchildren (and so on)
                AddComponentToAllParentsChildsIfHasComponent<T>(componentTypeToAdd, childTransform, onComponentAdded);
            }
        }

        public static LayerMask GetGameObjectLayerMask(GameObject gameObject)
        {
            return 1 << gameObject.layer;
        }

        private static Dictionary<Action, float> cooldowns = new Dictionary<Action, float>();
        public static void CallFunctionWithCooldown(Action function, float cooldownInSeconds)
        {
            float currentTime = Time.time;

            // Cleanup old entries
            List<Action> keysToRemove = new List<Action>();
            foreach (var entry in cooldowns)
            {
                if (currentTime - entry.Value > cooldownInSeconds * 2) // Arbitrary threshold for cleanup
                {
                    keysToRemove.Add(entry.Key);
                }
            }
            foreach (var key in keysToRemove)
            {
                cooldowns.Remove(key);
            }

            // Check cooldown and execute function
            if (cooldowns.TryGetValue(function, out float lastCallTime))
            {
                if (currentTime - lastCallTime < cooldownInSeconds)
                {
                    return;
                }
            }

            function.Invoke();
            cooldowns[function] = currentTime;
        }        

        public static void LookTowardsHorizontal(GameObject gameObject, Vector3 targetLookPosition)
        {
            targetLookPosition.y = gameObject.transform.position.y; // Keep the current object's Y position
            gameObject.transform.LookAt(targetLookPosition); // Rotate the object to face the target
        }

        public static void DrawSphereGizmos(Vector3 position, float radius, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(position, radius);
        }

    } // End Of Utils

}
