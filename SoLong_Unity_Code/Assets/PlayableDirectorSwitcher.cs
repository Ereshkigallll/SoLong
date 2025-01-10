using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayableDirectorSwitcher : MonoBehaviour
{
    [SerializeField] private TimelineAsset timeline_en, timeline_zh;

    private Dictionary<Object, Object> cachedBindings;  // Dictionary for caching track bindings

    private PlayableDirector playableDirector;

    void Start()
    {
        // Retrieve the PlayableDirector component
        playableDirector = GetComponent<PlayableDirector>();

        // Initialize the bindings cache
        cachedBindings = new Dictionary<Object, Object>();

        // Cache current bindings
        CacheCurrentBindings();

#if LANGUAGE_ENGLISH
        // Set the English timeline
        SwitchPlayableAsset(timeline_en);
#elif LANGUAGE_CHINESE
        // Set the Chinese timeline
        SwitchPlayableAsset(timeline_zh);
#endif
    }

    // Method to cache current bindings
    private void CacheCurrentBindings()
    {
        cachedBindings.Clear();  // Clear previous cache

        // Use foreach instead of while for iterating over bindings
        foreach (var binding in playableDirector.playableAsset.outputs)
        {
            Object sourceObject = binding.sourceObject;
            Object boundObject = playableDirector.GetGenericBinding(sourceObject);

            if (sourceObject != null && boundObject != null)
            {
                cachedBindings[sourceObject] = boundObject;  // Cache the track and its bound object
                Debug.Log("Cached binding: " + sourceObject.name + " -> " + boundObject.name);
            }
        }
    }

    // Method to switch PlayableAssets
    public void SwitchPlayableAsset(PlayableAsset newPlayableAsset)
    {
        playableDirector.playableAsset = newPlayableAsset;  // Set new PlayableAsset
        /*
        RebindCachedBindings();  // Rebind cached objects
        */
        if (playableDirector.playOnAwake)
            playableDirector.Play();  // Play the new timeline if applicable       
    }

    // Method to rebind cached bindings
    private void RebindCachedBindings()
    {
        // Iterate over the cached bindings to reapply them
        foreach (KeyValuePair<Object, Object> cachedBinding in cachedBindings)
        {
            Object sourceObject = cachedBinding.Key;
            Object boundObject = cachedBinding.Value;

            // Log the rebinding action
            if (sourceObject != null && boundObject != null)
            {
                Debug.Log("Rebinding: " + sourceObject.name + " -> " + boundObject.name);
                playableDirector.SetGenericBinding(sourceObject, boundObject);
            }
            else
            {
                if (sourceObject == null)
                    Debug.LogError("Cached source object is null, cannot rebind.");
                if (boundObject == null)
                    Debug.LogWarning("Bound object for " + sourceObject?.name + " is null, check the integrity of cached bindings.");
            }
        }
    }
}
