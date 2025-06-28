using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.ProBuilder.Shapes;

[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;

    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Piece[,] grid;
    private Piece selectedPiece;
    private ChessGameController chessController;
    private SquareSelectorCreator squareSelector;

    private PieceCreator pieceCreator;

    private GameObject chosenObject;
    public bool isPromoting = false;
    public Piece promotedPiece;

    [SerializeField] public Promote queen;
    [SerializeField] public Promote rook;
    [SerializeField] public Promote knight;
    [SerializeField] public Promote bishop;


    private void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    public void SetDependencies(ChessGameController chessController)
    {
        this.chessController = chessController;
    }



    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
        
    }

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / squareSize) + BOARD_SIZE / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / squareSize) + BOARD_SIZE / 2;
        return new Vector2Int(x, y);
    }

    public void OnGameRestarted() 
    {
        selectedPiece = null;
        CreateGrid();
    }

    public void OnSquareSelected(Vector3 inputPosition)
    {
        if (!chessController.IsGameInProgress()) return;
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        if (selectedPiece)
        {
            if (piece != null && selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && selectedPiece != piece && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
            else if (selectedPiece.CanMoveTo(coords))
            {
                if (selectedPiece.GetComponent<Pawn>() != null) 
                {
                    if (selectedPiece.occupiedSquare == new Vector2Int(coords.x-1,coords.y) && GetPieceOnSquare(new Vector2Int(coords.x - 1, coords.y + 1)) == null) 
                    {

                        TryToTakeOppositePiece(coords);
                       
                        if(selectedPiece.team == TeamColor.White) coords.y++;
                        else coords.y--;
                        UpdateBoardOnPieceMove(coords, selectedPiece.occupiedSquare, selectedPiece, null);
                        selectedPiece.MovePiece(coords);
                        selectedPiece.turnsSinceLastMove = 0;
                        foreach (var pcs in FindObjectsByType<Piece>(FindObjectsSortMode.None))
                        {
                            if (pcs != selectedPiece) pcs.turnsSinceLastMove++;
                        }
                        DeselectPiece();
                        EndTurn();

                    }
                    

                    else if (selectedPiece.occupiedSquare == new Vector2Int(coords.x + 1, coords.y) && GetPieceOnSquare(new Vector2Int(coords.x + 1, coords.y + 1)) == null) 
                    {
                        TryToTakeOppositePiece(coords);

                        if (selectedPiece.team == TeamColor.White) coords.y++;
                        else coords.y--;
                        UpdateBoardOnPieceMove(coords, selectedPiece.occupiedSquare, selectedPiece, null);
                        selectedPiece.MovePiece(coords);
                        selectedPiece.turnsSinceLastMove = 0;
                        foreach (var pcs in FindObjectsByType<Piece>(FindObjectsSortMode.None))
                        {
                            if (pcs != selectedPiece) pcs.turnsSinceLastMove++;
                        }
                        DeselectPiece();
                        EndTurn();
                    }
                    else OnSelectedPieceMoved(coords, selectedPiece);

                }
                else 
                    OnSelectedPieceMoved(coords, selectedPiece);
            }
        }
        else
        {
            if (piece != null && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
        }
    }



    private void SelectPiece(Piece piece)
    {
        if (!isPromoting && !PauseMenu.GameIsPaused)
        {
            chessController.RemoveMovesEnablingAttackingOnPieceOfType<King>(piece);
            selectedPiece = piece;
            List<Vector2Int> selection = selectedPiece.avaliableMoves;
            ShowSelectionSquares(selection);
        }
    }

    private void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            squaresData.Add(position, isSquareFree);
        }
        squareSelector.ShowSelection(squaresData);
    }

    private void DeselectPiece()
    {
        selectedPiece = null;
        squareSelector.ClearSelection();
    }
    private void OnSelectedPieceMoved(Vector2Int coords, Piece piece)
    {
        TryToTakeOppositePiece(coords);
        UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
        selectedPiece.MovePiece(coords);
        selectedPiece.turnsSinceLastMove = 0;
        foreach (var pcs in FindObjectsByType<Piece>(FindObjectsSortMode.None)) 
        {
            if (pcs != selectedPiece) pcs.turnsSinceLastMove++;
        }
        DeselectPiece();
        EndTurn();
    }

    private void TryToTakeOppositePiece(Vector2Int coords) 
    {
        Piece piece = GetPieceOnSquare(coords);
        if (piece != null && !selectedPiece.IsFromSameTeam(piece)) 
        {
            TakePiece(piece);
        }
    }

    public void TakePiece(Piece piece) 
    {
        if (piece) 
        {
            grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
            chessController.OnPieceRemoved(piece);
        }
    }

    private void EndTurn()
    {
        chessController.EndTurn();
    }

    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            return grid[coords.x, coords.y];
        return null;
    }

    public bool CheckIfCoordinatesAreOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }

    public bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                    return true;
            }
        }
        return false;
    }

    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            grid[coords.x, coords.y] = piece;
    }


    public void PromotePiece(string piece)
    {
        //Vector3 originalPos = new Vector3(promotedPiece.transform.position.x, promotedPiece.transform.position.y, promotedPiece.transform.position.z);
        //promotedPiece.transform.DOMove(new Vector3(originalPos.x, originalPos.y- 5, originalPos.z), 0.1f);
        //await Task.Delay(100);

        TakePiece(promotedPiece);
        if (piece == "queen")
            chessController.CreatePieceAndInitialize(promotedPiece.occupiedSquare, promotedPiece.team, typeof(Queen));
        else if (piece == "rook")
            chessController.CreatePieceAndInitialize(promotedPiece.occupiedSquare, promotedPiece.team, typeof(Rook));
        else if (piece == "bishop")
            chessController.CreatePieceAndInitialize(promotedPiece.occupiedSquare, promotedPiece.team, typeof(Bishop));
        else if (piece == "knight")
            chessController.CreatePieceAndInitialize(promotedPiece.occupiedSquare, promotedPiece.team, typeof(Knight));


        queen.transform.position = new Vector3(-4f, -10, -10f);
        rook.transform.position = new Vector3(-2f, -10, -10F);
        bishop.transform.position = new Vector3(2F, -10, -10F);
        knight.transform.position = new Vector3(4F, -10, -10F);

        isPromoting = false;
        promotedPiece = null;
        chessController.EndTurn();
    }
    

}