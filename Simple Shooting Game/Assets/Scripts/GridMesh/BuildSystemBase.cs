using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

public class BuildSystemBase : MonoBehaviour
{
    /*//public GenericGrid<BuildObject> grid { get; private set; }

    [Header("Grid Properties")]
    [Min(1)] public int numRows;
    [Min(1)] public int numCols;
    public float gridElevation = 0.01f;
    [Min(0.001f)] public float tileSize;

    [Header("Game Object References")]
    [SerializeField] private TextMeshPro _debugText;

    //references
    private MeshBuilder _meshBuilder;

    //saved properties
    private Vector2Int _currentCoords;
    //remove serialized field after finished with debug stuff
    [SerializeField] private bool _isSelecting = false;
    [SerializeField] private List<BuildObject> _selectedTiles;

    //events
    #region Public Events
    public static Action<Vector2Int> OnHoverTileChanged;
    public static Action<Vector2Int> OnSelectionDown;
    public static Action<Vector2Int> OnSelectionUp;
    #endregion

    private void OnEnable()
    {
        OnHoverTileChanged += HoverTileChanged;
        OnHoverTileChanged += MoveDebugDisplay;
        OnSelectionDown += SelectionButtonDown;
        OnSelectionUp += SelectionButtonUp;
    }
    private void OnDisable()
    {
        OnHoverTileChanged -= HoverTileChanged;
        OnHoverTileChanged -= MoveDebugDisplay;
        OnSelectionDown -= SelectionButtonDown;
        OnSelectionUp -= SelectionButtonUp;
    }
    private void Awake()
    {
        //get references
        _meshBuilder = GetComponentInChildren<MeshBuilder>();
        if (_meshBuilder == null) Debug.LogError("Missing Mesh Building");

        //create grid
        grid = new GenericGrid<BuildObject>(numCols, numRows, tileSize, transform.position, (GenericGrid<BuildObject> grid, int x, int y) => new BuildObject(grid, x, y));
    }

    #region Private Grid Functions
    private void HoverTileChanged(Vector2Int coords)
    {
        //update saved coords
        _currentCoords = coords;

        //update hover mesh
        MoveHoverMesh();

        //update selection while selecting
        if (_isSelecting == true)
        {
            SelectionMoved(coords);
        }
    }
    private void MoveHoverMesh()
    {
        if (_currentCoords != -Vector2Int.one)
        {
            _meshBuilder.HoverMeshVisibility(true);

            float halfSize = tileSize * 0.5f;
            Vector3 targetPosition = grid.GetWorldPosition(_currentCoords.x, _currentCoords.y);
            targetPosition = new Vector3(targetPosition.x, _meshBuilder.hoverTransform.position.y, targetPosition.z);

            _meshBuilder.hoverTransform.position = targetPosition;
        }
        else
        {
            _meshBuilder.HoverMeshVisibility(false);
        }

    }

    #region Selection Stuff

    [Header("Selection Stuff")]
    private Vector2Int _selectionDownCoords = new Vector2Int();
    [SerializeField]
    private List<BuildObject> _selectedBuildObjects;

    public struct DualCoords
    {
        public Vector2Int startCoords;
        public Vector2Int endCoords;
    }

    private void SelectionButtonDown(Vector2Int coords)
    {
        //Debug.LogWarning("down: " + coords);

        //if valid tile set isSelecting to true
        BuildObject bObject = grid.GetGridObject(coords.x, coords.y);
        if (bObject != null)
        {
            _isSelecting = true;

            //what to do if on valid tile
            //Debug.LogWarning("Selecting");

            //show mesh
            _meshBuilder.SelectionMeshVisibility(true);

            ///TODO probably store starting position

            //store first position
            _selectionDownCoords = coords;
            //add first position to buildobjectlist
            _selectedBuildObjects = new List<BuildObject>();
            _selectedBuildObjects.Clear();
            _selectedBuildObjects.Add(bObject);

            MoveSelectionMesh(coords);
        }
        else
        {
            _isSelecting = false;

            //hide mesh
            _meshBuilder.SelectionMeshVisibility(false);

            //TODO probably clear _selectedTiles list

            _selectionDownCoords = -Vector2Int.one;
            _selectedBuildObjects.Clear();
        }
    }
    private void SelectionMoved(Vector2Int coords)
    {
        //Debug.LogWarning("during: " + coords);
        //Debug.LogWarning("Selecting");

        MoveSelectionMesh(coords);
    }
    private void SelectionButtonUp(Vector2Int coords)
    {
        //Debug.LogWarning("up: " + coords);

        // set isSelecting to false even if not on valid tile
        _isSelecting = false;

        //Debug.LogWarning("Selecting");

        BuildObject bObject = grid.GetGridObject(coords.x, coords.y);
        if (bObject != null)
        {
            //what to do if on valid tile

            //TODO probably populate _selectedTiles list

            //quick math count number of selected tiles
            int x = Mathf.Abs(_selectionDownCoords.x - coords.x);
            x++;
            int y = Mathf.Abs(_selectionDownCoords.y - coords.y);
            y++;
            int prod = x * y;

            Debug.Log("Selected Tile Count: " + prod);

            *//*//store tiles in list
            int xDir = 1;
            int yDir = 1;

            //check if negative direction
            xDir = coords.x >= _selectionDownCoords.x ? 1 : -1;
            yDir = coords.y >= _selectionDownCoords.y ? 1 : -1;

            for (int xCoord = _selectionDownCoords.x; xDir > 0 ? xCoord < coords.x : xCoord >= coords.x; xCoord++)
            {
                for (int yCoord = _selectionDownCoords.y; yDir > 0 ? yCoord < coords.y : yCoord >= coords.y; yCoord++)
                {
                    if(!_selectedBuildObjects.Contains(grid.GetGridObject(xCoord, yCoord)))
                    {
                        BuildObject b = grid.GetGridObject(xCoord, yCoord);
                        _selectedBuildObjects.Add(b);
                    }
                }
            }

            Debug.Log("Selected Tile Count: " + _selectedBuildObjects.Count);*//*
        }
        else
        {
            //hide mesh
            _meshBuilder.SelectionMeshVisibility(false);

            //TODO probably clear _selectedTiles list
        }
    }
    private void MoveSelectionMesh(Vector2Int coords)
    {
        /// TODO update selection visual based on first and current coords
        /// this is going to be a long one

        /// if same as start tile
        /// if different from start tile
        /// 

        Debug.LogWarning("MoveSelectionMesh()");

        DualCoords dualCoords = new DualCoords();
        dualCoords.startCoords = _selectionDownCoords;
        dualCoords.endCoords = coords;

        if (coords == _selectionDownCoords)
        {
            //move on selection down

            //reconstruct mesh
            _meshBuilder.OnSelectionChanged?.Invoke(dualCoords);

            //move mesh
            float halfSize = tileSize * 0.5f;
            Vector3 targetPosition = grid.GetWorldPosition(_currentCoords.x, _currentCoords.y);
            targetPosition = new Vector3(targetPosition.x, _meshBuilder.selectionTransform.position.y, targetPosition.z);

            _meshBuilder.selectionTransform.position = targetPosition;
        }
        else
        {
            //move according to start and end tiles

            //reconstruct mesh
            _meshBuilder.OnSelectionChanged?.Invoke(dualCoords);
            //_meshBuilder.ConstructSelectionMesh(dualCoords); //_selectionDownCoords, coords

            float halfSize = tileSize * 0.5f;
            Vector3 targetPosition = grid.GetWorldPosition(_currentCoords.x, _currentCoords.y);
            targetPosition = new Vector3(targetPosition.x, _meshBuilder.selectionTransform.position.y, targetPosition.z);

            //_meshBuilder.selectionTransform.position = targetPosition;
        }
    }
    #endregion

    //debug
    private void MoveDebugDisplay(Vector2Int newCoords)
    {
        //Debug.Log("[BS]MoveDebugDisplay()");

        if (_debugText != null)
        {
            if (newCoords != -Vector2Int.one)
            {
                if (!_debugText.gameObject.activeInHierarchy) _debugText.gameObject.SetActive(true);

                float halfSize = tileSize * 0.5f;
                Vector3 targetPosition = grid.GetWorldPosition(newCoords.x, newCoords.y);

                _debugText.transform.position = new Vector3(targetPosition.x + halfSize, gridElevation, targetPosition.z + halfSize);
                _debugText.text = grid.GetGridObject(newCoords.x, newCoords.y).ToString();
            }
            else
            {
                if (_debugText.gameObject.activeInHierarchy) _debugText.gameObject.SetActive(false);
            }

        }
    }
    #endregion

    #region Public Grid Functions
    public Vector2Int GetGridXY(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return new Vector2Int(x, y);
    }

    #endregion*/
}

