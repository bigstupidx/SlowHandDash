using UnityEngine;
using System.Collections;

namespace SlowHand
{
    [ExecuteInEditMode]
    public class BlockConnection : MonoBehaviour
    {
        public Color32 leftClr;
        public Color32 rightClr;
        MeshFilter meshFilter;
        public float maxHeight;
        public float minHeight;
        public float maxDist;
        // Use this for initialization
        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Color32[] clr = mesh.colors32;
            clr[0] = clr[3] = leftClr;
            clr[2] = clr[1] = rightClr;
            mesh.colors32 = clr;
        }

        public void UpdateConnection(Vector3 left, Vector3 right)
        {
            float dist = (right - left).magnitude - 1f;
            float p = maxHeight - (dist / maxDist) * (maxHeight - minHeight);

            Mesh mesh = meshFilter.mesh;
            Vector3[] vtr = mesh.vertices;
            vtr[0] = new Vector3(left.x, -p);
            vtr[3] = new Vector3(left.x, p);
            vtr[2] = new Vector3(right.x, -p);
            vtr[1] = new Vector3(right.x, p);
            mesh.vertices = vtr;
        }
    }
}