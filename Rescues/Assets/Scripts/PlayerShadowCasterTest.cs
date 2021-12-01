using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerShadowCasterTest : MonoBehaviour
{
    [SerializeField] ShadowCaster2D _shadowCaster;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] MeshFilter _meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //var vertices = _meshRenderer.additionalVertexStreams.vertices;
        var vertices = _meshFilter.mesh.vertices;
        _shadowCaster.SetPath(vertices);
        _shadowCaster.SetPathHash(Random.Range(int.MinValue, int.MaxValue));
        _shadowCaster.Update();
    }
}

public static class ShadowCaster2DExtensions
{
    /// <summary>
    /// Replaces the path that defines the shape of the shadow caster.
    /// </summary>
    /// <remarks>
    /// Calling this method will change the shape but not the mesh of the shadow caster. Call SetPathHash afterwards.
    /// </remarks>
    /// <param name="shadowCaster">The object to modify.</param>
    /// <param name="path">The new path to define the shape of the shadow caster.</param>
    public static void SetPath(this ShadowCaster2D shadowCaster, Vector3[] path)
    {
        FieldInfo shapeField = typeof(ShadowCaster2D).GetField("m_ShapePath",
                                                               BindingFlags.NonPublic |
                                                               BindingFlags.Instance);
        shapeField.SetValue(shadowCaster, path);
    }

    /// <summary>
    /// Replaces the hash key of the shadow caster, which produces an internal data rebuild.
    /// </summary>
    /// <remarks>
    /// A change in the shape of the shadow caster will not block the light, it has to be rebuilt using this function.
    /// </remarks>
    /// <param name="shadowCaster">The object to modify.</param>
    /// <param name="hash">The new hash key to store. It must be different from the previous key to produce the rebuild. You can use a random number.</param>
    public static void SetPathHash(this ShadowCaster2D shadowCaster, int hash)
    {
        FieldInfo hashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash",
                                                              BindingFlags.NonPublic |
                                                              BindingFlags.Instance);
        hashField.SetValue(shadowCaster, hash);
    }
}
