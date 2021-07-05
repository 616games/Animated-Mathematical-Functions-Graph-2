using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    #region --Fields / Properties
    
    /// <summary>
    /// Prefab of the point game object used to populate the graph.
    /// </summary>
    [SerializeField]
    private GameObject _pointPrefab;

    /// <summary>
    /// How many points will be plotted.
    /// </summary>
    [SerializeField, Range(10, 100)]
    private int _resolution;

    /// <summary>
    /// The type of graph to be created.
    /// </summary>
    [SerializeField]
    private GraphFunctionType _graphFunctionType;

    /// <summary>
    /// A List of all the points instantiated to create the graph.
    /// </summary>
    private readonly List<GameObject> _points = new List<GameObject>();

    #endregion
    
    #region --Unity Specific Methods--
    
    private void Start()
    {
        Graph3D();
    }

    private void Update()
    {
        AnimateGraph();
    }
    
    #endregion
    
    #region --Custom Methods--
    
    
    
    /// <summary>
    /// Creates a three dimensional representation of the selected _graphFunctionType.
    /// </summary>
    private void Graph3D()
    {
        //Scale is adjusted to make sure all points fit within our domain of -1 to 1 based on how many points are set for the resolution.
        GraphFunction _function = GraphFunctionLibrary.GetGraphFunction(_graphFunctionType);
        float _step = 2f / _resolution;
        Vector3 _scale = Vector3.one * _step;
        
        Vector3 _position = Vector3.zero;
        
        //To create a 3D grid of points we need to square resolution and track the x and z position indices.
        //x represents a row along the X axis and we need to reset its value at the end of each row (once x reaches the number of points (resolution)).
        //z represents each row along the Z axis and we need to increase its value when x creates a new row.
        for (int i = 0, x = 0, z = 0; i < _resolution * _resolution; i++, x++)
        {
            if (x == _resolution)
            {
                x = 0;
                z++;
            }
            
            GameObject _point = Instantiate(_pointPrefab, transform, true);
            _points.Add(_point);
            
            //Position each point for each row along the X axis from the left shifted right .5 units (radius) so they aren't overlapping.
            //Factor in the point's adjusted scale.
            _position.x = (x + .5f) * _step - 1f;
            _position.z = (z + .5f) * _step - 1f;
            
            _position.y = _function(_position.x, _position.z, Time.time);
            _point.transform.position = _position;
            _point.transform.localScale = _scale;
        }
    }

    /// <summary>
    /// Animates the points of the graph based on the currently selected _graphFunctionType.
    /// </summary>
    private void AnimateGraph()
    {
        for (int i = 0; i < _resolution * _resolution; i++)
        {
            GraphFunction _function = GraphFunctionLibrary.GetGraphFunction(_graphFunctionType);
            GameObject _point = _points[i];
            Vector3 _position = _point.transform.position;
            _position.y = _function(_position.x, _position.z, Time.time);
            _point.transform.position = _position;
        }
    }

    #endregion
    
}
