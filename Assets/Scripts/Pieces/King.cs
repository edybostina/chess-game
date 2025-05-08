using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class King : Piece
{
    Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(-1,1),
        new Vector2Int(0,1),
        new Vector2Int(1,1),
        new Vector2Int(-1,0),
        new Vector2Int(1,0),
        new Vector2Int(-1,-1),
        new Vector2Int(0,-1),
        new Vector2Int(1,-1)
    };

    private Vector2Int leftCastlingMove;
    private Vector2Int rightCastlingMove;

    private Piece leftRook;
    private Piece rightRook;

    public override List<Vector2Int> SelectAvaliableSquares()
    {
        avaliableMoves.Clear();
        AssignStandardMoves();
        AssignCastlingMoves();
        return avaliableMoves;
    }

    private void AssignStandardMoves() 
    {
        float range = 1;
        foreach (var direction in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = occupiedSquare + direction * i;
                Piece piece = board.GetPieceOnSquare(nextCoords);

                if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                {
                    break;
                }
                if (piece == null)
                    TryToAddMove(nextCoords);
                else if (!piece.IsFromSameTeam(this))
                {
                    TryToAddMove(nextCoords);
                    break;
                }
                else if (piece.IsFromSameTeam(this))
                    break;
            }
        }
        
    }
    private void AssignCastlingMoves() 
    {
        if (hasMoved) return;

        leftRook = GetPieceInDirection<Rook>(team, Vector2Int.left);
        if (leftRook && !leftRook.hasMoved) 
        {

            leftCastlingMove = occupiedSquare + Vector2Int.left * 2;
            if(CheckIfKingCanCastle("stanga", team)) 
                avaliableMoves.Add(leftCastlingMove);
        }

        rightRook = GetPieceInDirection<Rook>(team, Vector2Int.right);
        if (rightRook && !rightRook.hasMoved)
        {   
            rightCastlingMove = occupiedSquare + Vector2Int.right * 2;
            if(CheckIfKingCanCastle("dreapta", team))
                avaliableMoves.Add(rightCastlingMove);
        }
    }

    public override void MovePiece(Vector2Int coords)
    {
        base.MovePiece(coords);
        if (coords == leftCastlingMove) 
        {
            board.UpdateBoardOnPieceMove(coords+ Vector2Int.right, leftRook.occupiedSquare, leftRook, null);
            leftRook.MovePiece(coords + Vector2Int.right);
        }
        else if (coords == rightCastlingMove)
        {
            board.UpdateBoardOnPieceMove(coords+ Vector2Int.left, rightRook.occupiedSquare, rightRook, null);
            rightRook.MovePiece(coords + Vector2Int.left);
        }
    }


    public bool CheckIfKingCanCastle(string direction, TeamColor echipa)
    {
       
        foreach (var piece in FindObjectsOfType<Piece>()) 
        {
            if (board.HasPiece(piece) && piece.team != echipa) 
            {
                foreach (var moves in piece.avaliableMoves) 
                {
                    if (direction == "dreapta")
                    {
                        if (moves == occupiedSquare + new Vector2Int(1, 0))
                            return false;
                    }
                    else 
                    {
                        if (moves == occupiedSquare + new Vector2Int(-1, 0))
                            return false;
                    }                   
                }      
            }
        }
        return true;
    }


}
