using System.Collections.Generic;
using UnityEngine;

public class StickyConnectionManager : MonoBehaviour
{
    public static StickyConnectionManager Instance;

    public GameObject connectionPrefab;

    StickyNote first;

    Dictionary<(StickyNote, StickyNote), StickyConnection> connections = new();

    void Awake()
    {
        Instance = this;
    }

    public void SelectSticky(StickyNote note)
    {
        if (first == null)
        {
            first = note;
            return;
        }

        if (first == note)
        {
            first = null;
            return;
        }

        var key = (first, note);
        var rev = (note, first);

        if (connections.ContainsKey(key) || connections.ContainsKey(rev))
        {
            var conn = connections.ContainsKey(key) ? connections[key] : connections[rev];
            Destroy(conn.gameObject);
            connections.Remove(key);
            connections.Remove(rev);
        }
        else
        {
            var go = Instantiate(connectionPrefab);
            var conn = go.GetComponent<StickyConnection>();
            conn.a = first;
            conn.b = note;
            connections.Add(key, conn);
        }

        first = null;
    }
}
