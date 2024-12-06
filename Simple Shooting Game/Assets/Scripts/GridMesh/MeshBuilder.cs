using PatchworkGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

[ExecuteAlways]
//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshBuilder : MonoBehaviour
{
    //private GenericGrid<BuildingObject> grid;
    [Header("Source Grid")]
    //[SerializeField] private BuildSystem _buildSystem;
    [SerializeField] private bool _useBuildSystem;

    [Header("References")]
    [SerializeField] private MeshRenderer _gridMeshRenderer;
    [SerializeField] private MeshFilter _gridMeshFilter;
    [Space]
    [SerializeField] private MeshRenderer _selectMeshRenderer;
    [SerializeField] private MeshFilter _selectMeshFilter;
    [Space]
    [SerializeField] private MeshRenderer _hoverMeshRenderer;
    [SerializeField] private MeshFilter _hoverMeshFilter;

    [Header("Grid Properties")]
    [SerializeField] private int _numRows;
    private int _savedNumRows;
    [SerializeField] private int _numCols;
    private int _savedNumCols;
    [SerializeField] private float _gridElevation = 0f;
    private float _savedGridElevation;
    [Space]
    [SerializeField] private float _tileSize;
    private float _savedTileSize;
    [SerializeField] private float _lineThickness;
    private float _savedLineThickness;
    [Space]
    [SerializeField] private Color _lineColor;
    private Color _savedLineColor;
    [SerializeField] private Color _hoverColor;
    private Color _savedHoverColor;
    [SerializeField] private Color _selectionColor;
    private Color _savedSelectionColor;
    [Space]
    [SerializeField] private Material _sourceMaterial;

    public Transform selectionTransform { get; private set; }
    public Transform hoverTransform { get; private set; }


    //events
    //public Action<BuildSystem.DualCoords> OnSelectionChanged;

    private void Awake()
    {
        if (_selectMeshRenderer != null) selectionTransform = _selectMeshRenderer.transform;
        else AddSelectionComponents();

        if (_hoverMeshRenderer != null) hoverTransform = _hoverMeshRenderer.transform;
        else AddHoverComponents();
    }
    private void AddSelectionComponents()
    {
        GameObject selectionObject = new GameObject("Selection");
        MeshFilter meshFilter = selectionObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = selectionObject.AddComponent<MeshRenderer>();

        _selectMeshFilter = meshFilter;
        _selectMeshRenderer = meshRenderer;
    }
    private void AddHoverComponents()
    {
        GameObject hoverObject = new GameObject("Hover");
        MeshFilter meshFilter = hoverObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = hoverObject.AddComponent<MeshRenderer>();

        _hoverMeshFilter = meshFilter;
        _hoverMeshRenderer = meshRenderer;
    }
    private void OnEnable()
    {
        //OnSelectionChanged += ConstructSelectionMesh;
    }
    private void OnDisable()
    {
        //OnSelectionChanged -= ConstructSelectionMesh;
    }
    private void Start()
    {
        HideMeshes();

        TriggerUpdate();

        CheckForUpdates();
    }
    private void Update()
    {
        if (Application.isEditor)
        {
            CheckForUpdates();
        }
    }

    #region Mesh Updates
    private void CheckForUpdates()
    {
        if (_gridMeshRenderer != null && _gridMeshFilter != null &&
            _selectMeshRenderer != null && _selectMeshFilter != null &&
            _hoverMeshRenderer != null && _hoverMeshFilter)
        {
            #region Trigger Updates
            if (_useBuildSystem)
            {
                /*//check if build system has been assigned
                if (_buildSystem == null) return;

                //Debug.Log("[BS] Checking for updates");

                //update based on build system properties
                if (_numRows != _buildSystem.numRows)
                {
                    _numRows = _buildSystem.numRows;
                    TriggerUpdate();
                }
                if (_numCols != _buildSystem.numCols)
                {
                    _numCols = _buildSystem.numCols;
                    TriggerUpdate();
                }
                if (_gridElevation != _buildSystem.gridElevation)
                {
                    _gridElevation = _buildSystem.gridElevation;
                    TriggerUpdate();
                }
                if (_tileSize != _buildSystem.tileSize)
                {
                    _tileSize = _buildSystem.tileSize;
                    TriggerUpdate();
                }*/

            }
            else
            {
                //Debug.Log("[NBS] Checking for updates");

                //if no build system can be changed on its own
                if (_savedNumRows != _numRows)
                {
                    TriggerUpdate();
                    _savedNumRows = _numRows;
                }
                if (_savedNumCols != _numCols)
                {
                    TriggerUpdate();
                    _savedNumCols = _numCols;
                }
                if (_savedGridElevation != _gridElevation)
                {
                    TriggerUpdate();
                    _savedGridElevation = _gridElevation;
                }
                if (_savedTileSize != _tileSize)
                {
                    TriggerUpdate();
                    _savedTileSize = _tileSize;
                }
            }

            //the same regarless of if using a build system
            if (_savedLineThickness != _lineThickness)
            {
                TriggerUpdate();
                _savedLineThickness = _lineThickness;
            }
            if (_savedLineColor != _lineColor)
            {
                TriggerUpdate();
                _savedLineColor = _lineColor;
            }
            if (_savedHoverColor != _hoverColor)
            {
                TriggerUpdate();
                _savedHoverColor = _hoverColor;
            }
            if (_savedSelectionColor != _selectionColor)
            {
                TriggerUpdate();
                _savedSelectionColor = _selectionColor;
            }

            #endregion
        }
    }
    private void TriggerUpdate()
    {
        Debug.Log("TriggerUpdate()");

        ConstructGridMesh();
        ConstructHoverMesh();
        //ConstructSelectionMesh();
    }
    #endregion

    #region Construction Methods
    public void ConstructGridMesh()
    {
        //create materials
        Material lineMaterial = new Material(_sourceMaterial);
        lineMaterial.color = _lineColor;

        //create point data

        List<Vector3> lineVertices = new List<Vector3>();
        List<int> lineTriangles = new List<int>();
        List<Vector3> lineNormals = new List<Vector3>();

        int triCount = 0;

        #region Working loop
        //rows
        for (int x = 0; x < _numRows + 1; x++)
        {
            float lineStart = x * _tileSize;
            float lineEnd = GridWidth();

            CreateLine(triCount, new Vector3(lineStart, _gridElevation, 0), new Vector3(lineStart, _gridElevation, lineEnd), _lineThickness, lineVertices, lineTriangles, lineNormals);
            triCount++;
        }

        //Debug.LogWarningFormat("[x]verts({0}), tris({1})", lineVertices.Count, lineTriangles.Count);

        //cols
        for (int z = 0; z < _numCols + 1; z++)
        {
            float lineStart = z * _tileSize;
            float lineEnd = GridHeight();

            CreateLine(triCount, new Vector3(0, _gridElevation, lineStart), new Vector3(lineEnd, _gridElevation, lineStart), _lineThickness, lineVertices, lineTriangles, lineNormals);
            triCount++;
        }

        //Debug.LogWarningFormat("[y]verts({0}), tris({1})", lineVertices.Count, lineTriangles.Count);
        #endregion

        //create mesh
        if (_gridMeshRenderer == null || _gridMeshFilter == null)
        {
            Debug.LogError("Missing grid mesh components.");
            return;
        }

        Mesh lineMesh = new Mesh();

        lineMesh.vertices = lineVertices.ToArray();
        lineMesh.triangles = lineTriangles.ToArray();
        lineMesh.RecalculateNormals();

        _gridMeshFilter.mesh = lineMesh;
        _gridMeshRenderer.material = lineMaterial;

    }
    private void ConstructHoverMesh()
    {
        //check for mesh components
        if (_hoverMeshRenderer == null || _hoverMeshFilter == null)
        {
            Debug.LogError("Missing hover mesh components.");
            return;
        }

        Material hoveredMaterial = new Material(_sourceMaterial);
        hoveredMaterial.color = _hoverColor;

        List<Vector3> hoverVertices = new List<Vector3>();
        List<int> hoverTriangles = new List<int>();

        float hoverStart = _tileSize * 0.5f;
        float hoverEnd = _tileSize;

        CreateLine(0, new Vector3(hoverStart, _gridElevation, 0), new Vector3(hoverStart, _gridElevation, hoverEnd), _tileSize, hoverVertices, hoverTriangles);

        Mesh hoverMesh = new Mesh();
        hoverMesh.vertices = hoverVertices.ToArray();
        hoverMesh.triangles = hoverTriangles.ToArray();
        hoverMesh.RecalculateNormals();

        _hoverMeshFilter.mesh = hoverMesh;
        _hoverMeshRenderer.material = hoveredMaterial;
    }
    private void ConstructSelectionMesh()
    {
        Debug.Log("ConstructSelectionMesh(single)");

        //check for mesh components
        if (_selectMeshRenderer == null || _selectMeshFilter == null)
        {
            Debug.LogError("Missing selection mesh components.");
            return;
        }

        Material selectionMaterial = new Material(_sourceMaterial);
        selectionMaterial.color = _selectionColor;

        List<Vector3> selectionVertices = new List<Vector3>();
        List<int> selectionTriangles = new List<int>();

        float halfX = _tileSize * 0.5f;
        float fullY = _tileSize;

        CreateLine(0, new Vector3(halfX, _gridElevation, 0), new Vector3(halfX, _gridElevation, fullY), _tileSize, selectionVertices, selectionTriangles);

        Mesh selectionMesh = new Mesh();
        selectionMesh.name = "SelectionMesh";
        selectionMesh.vertices = selectionVertices.ToArray();
        selectionMesh.triangles = selectionTriangles.ToArray();
        selectionMesh.RecalculateNormals();

        _selectMeshFilter.mesh = selectionMesh;
        _selectMeshRenderer.material = selectionMaterial;

    }
    private void ConstructSelectionMesh(Vector2Int startCoords, Vector2Int endCoords)
    {
        Debug.Log("ConstructSelectionMesh(range)");

        //check for mesh components
        if (_selectMeshRenderer == null || _selectMeshFilter == null)
        {
            Debug.LogError("Missing selection mesh components.");
            return;
        }

        Material selectionMaterial = new Material(_sourceMaterial);
        selectionMaterial.color = _selectionColor;

        List<Vector3> selectionVertices = new List<Vector3>();
        List<int> selectionTriangles = new List<int>();

        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.zero;

        //world positions
        if (_useBuildSystem)
        {
            //startPos = _buildSystem.grid.GetWorldPosition(startCoords.x, startCoords.y);
            //endPos = _buildSystem.grid.GetWorldPosition(endCoords.x, endCoords.y);
        }
        else
        {
            startPos = GridCube.GetWorldPosition3D(new Vector3Int(startCoords.x, 0, startCoords.y));
            endPos = GridCube.GetWorldPosition3D(new Vector3Int(endCoords.x, 0, endCoords.y));
        }

        //calculate size
        float halfTile = _tileSize * 0.5f;
        float xDiffPos = Mathf.Abs(startPos.x - endPos.x);
        float adjHalfX = xDiffPos % 2 == 0 ? xDiffPos - _tileSize * 0.5f : xDiffPos + _tileSize * 0.5f; //Mathf.Abs(startPos.x - endPos.x) + _tileSize * 0.5f;

        //float otherX = xDiffPos % 2 == 0 ? xDiffPos * _tileSize - halfTile : xDiffPos;
        float halfx = xDiffPos * halfTile + 0.5f;

        Debug.Log("adjHalfX = " + adjHalfX);

        if (adjHalfX == 0)
        {
            adjHalfX += _tileSize; // * 0.5f;
            Debug.Log("adjHalfX = " + adjHalfX);
        }
        float adjYpos = Mathf.Abs(startPos.z - endPos.z) + _tileSize;

        //calculate thickness
        float xDiffCoords = Mathf.Abs(startCoords.x - endCoords.x); //+ _tileSize;
        //if (xDiffCoords == 0) xDiffCoords += _tileSize;
        //float adjThickness = xDiffCoords % 2 == 0 ? xDiffCoords + _tileSize + 0.5f : xDiffCoords + _tileSize;
        float adjThickness = xDiffCoords + _tileSize;

        //Debug.LogFormat("adjx: {0} adjY: {1}", adjHalfX, adjYpos);

        //does all the math to size the mesh correctly
        CreateLine(0, new Vector3(halfx, _gridElevation, 0), new Vector3(halfx, _gridElevation, adjYpos), _tileSize * adjThickness, selectionVertices, selectionTriangles);

        //Debug.Log("selectionTriangles.Count: " + selectionTriangles.Count);

        Mesh selectionMesh = new Mesh();
        selectionMesh.name = "SelectionMesh";
        selectionMesh.vertices = selectionVertices.ToArray();
        selectionMesh.triangles = selectionTriangles.ToArray();
        selectionMesh.RecalculateNormals();

        _selectMeshFilter.mesh = selectionMesh;
        _selectMeshRenderer.material = selectionMaterial;
    }
    #endregion

    #region Visibility
    private void HideMeshes()
    {
        SelectionMeshVisibility(false);
        HoverMeshVisibility(false);
    }
    public void SelectionMeshVisibility(bool visible)
    {
        _selectMeshRenderer.enabled = visible;
    }
    public void HoverMeshVisibility(bool visible)
    {
        _hoverMeshRenderer.enabled = visible;
    }
    #endregion

    #region Public Functions
    private KeyValuePair<Vector2Int, bool> LocationToTile(Vector3 position)
    {
        KeyValuePair<Vector2Int, bool> debugPair = new KeyValuePair<Vector2Int, bool>(Vector2Int.zero, false);
        return debugPair;
    }
    private KeyValuePair<Vector3, bool> TileToLocation(Vector2Int xy, bool center)
    {
        KeyValuePair<Vector3, bool> debugPair = new KeyValuePair<Vector3, bool>(Vector3.zero, false);
        return debugPair;
    }

    private void SetSelectedTile(Vector2Int xy)
    {

    }
    public bool IsTileValid(Vector2Int xy)
    {
        bool debugBool = false;
        return debugBool;
    }
    #endregion

    #region Private Construction Functions
    private void CreateLine(int index, Vector3 start, Vector3 end, float thickness, List<Vector3> vertices, List<int> triangles, List<Vector3> normals)
    {
        //Debug.Log("start: " + start);
        //Debug.Log("end: " + end);
        float halfThickness = thickness * 0.5f;

        var lineDirection = end - start;
        lineDirection.Normalize();
        var thicknessDirection = Vector3.Cross(lineDirection, Vector3.up);

        /// Clockwise order
        /// 0, 1, 2     1, 2, 0
        /// 2, 1, 3     1, 3, 2
        /// 

        //adding ints to triangles
        int adjustment = index * 4;

        triangles.Add(adjustment + 0);
        triangles.Add(adjustment + 1);
        triangles.Add(adjustment + 2);
        triangles.Add(adjustment + 2);
        triangles.Add(adjustment + 1);
        triangles.Add(adjustment + 3);

        //add vector3s to vectices
        Vector3 vert0 = start + thicknessDirection * halfThickness;
        Vector3 vert1 = end + thicknessDirection * halfThickness;
        Vector3 vert2 = start + -thicknessDirection * halfThickness;
        Vector3 vert3 = end + -thicknessDirection * halfThickness;

        vertices.Add(vert0);
        vertices.Add(vert1);
        vertices.Add(vert2);
        vertices.Add(vert3);

        //add normals to list
        normals.Add(-Vector3.up);
        normals.Add(-Vector3.up);
        normals.Add(-Vector3.up);
        normals.Add(-Vector3.up);

    }
    private void CreateLine(int index, Vector3 start, Vector3 end, float thickness, List<Vector3> vertices, List<int> triangles)
    {
        //Debug.Log("start: " + start);
        //Debug.Log("end: " + end);
        float halfThickness = thickness * 0.5f;

        var lineDirection = end - start;
        lineDirection.Normalize();
        var thicknessDirection = Vector3.Cross(lineDirection, Vector3.up);

        /// Clockwise order
        /// 0, 1, 2     1, 2, 0
        /// 2, 1, 3     1, 3, 2
        /// 

        //adding ints to triangles
        int adjustment = index * 4;

        triangles.Add(adjustment + 0);
        triangles.Add(adjustment + 1);
        triangles.Add(adjustment + 2);
        triangles.Add(adjustment + 2);
        triangles.Add(adjustment + 1);
        triangles.Add(adjustment + 3);

        //add vector3s to vectices
        Vector3 vert0 = start + thicknessDirection * halfThickness;
        Vector3 vert1 = end + thicknessDirection * halfThickness;
        Vector3 vert2 = start + -thicknessDirection * halfThickness;
        Vector3 vert3 = end + -thicknessDirection * halfThickness;

        vertices.Add(vert0);
        vertices.Add(vert1);
        vertices.Add(vert2);
        vertices.Add(vert3);

    }
    private float GridWidth()
    {
        return _numCols * _tileSize;
    }
    private float GridHeight()
    {
        return _numRows * _tileSize;
    }
    #endregion

}
