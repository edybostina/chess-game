using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Pawn : Piece
{

  
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        avaliableMoves.Clear();

        Vector2Int direction = team == TeamColor.White ? Vector2Int.up : Vector2Int.down;
        float range = hasMoved ? 1 : 2;
        for (int i = 1; i <= range; i++)
        {
            Vector2Int nextCoords = occupiedSquare + direction * i;
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                break;
            if (piece == null)
                TryToAddMove(nextCoords);
            else
                break;
        }
        int enPassantMoveDirection;
        
        if (direction == Vector2Int.up)
            enPassantMoveDirection = 1;
        else 
            enPassantMoveDirection = -1;

        Vector2Int[] takeDirections = new Vector2Int[] { new Vector2Int(1, direction.y), new Vector2Int(-1, direction.y), new Vector2Int(-1, direction.y - enPassantMoveDirection), new Vector2Int(1, direction.y- enPassantMoveDirection) };

        for (int i = 0; i < takeDirections.Length; i++)
        {


            if (i > 1)
            {

                Vector2Int nextCoords = occupiedSquare + takeDirections[i];
                Vector2Int enPassDir = new Vector2Int (takeDirections[i].x, 0);
                Piece piece = board.GetPieceOnSquare(nextCoords);

                TeamColor oppTeam = team == TeamColor.White ? TeamColor.Black : TeamColor.White;


                if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                    continue;


                if (piece != null)
                {
                    if (GetPieceInDirection<Pawn>(oppTeam, enPassDir) == piece)
                    {
                    
                        if ((piece.occupiedSquare.y == 3 && piece.team == TeamColor.White) || (piece.occupiedSquare.y == 4 && piece.team == TeamColor.Black))
                        {
                           

                            if (!piece.IsFromSameTeam(this) && piece.turnsSinceLastMove == 0)
                            {
                                TryToAddMove(nextCoords);
                            }
                        }
                    }
                }
            }
            else
            {
                Vector2Int nextCoords = occupiedSquare + takeDirections[i];
                Piece piece = board.GetPieceOnSquare(nextCoords);

                if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                    continue;
                if (piece != null && !piece.IsFromSameTeam(this))
                {
                    TryToAddMove(nextCoords);
                }
            }
        }
        return avaliableMoves;
    }

    public override void MovePiece(Vector2Int coords) 
    {
        base.MovePiece(coords);
        CheckPromotion();
    }

    private void CheckPromotion() 
    {
        int endOfBoardYCoord = team == TeamColor.White ? Board.BOARD_SIZE - 1 : 0;


        if (occupiedSquare.y == endOfBoardYCoord)
        {
           
            board.promotedPiece = this;
            Material teamMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
           

            board.queen.SetMaterial(teamMaterial);
            board.rook.SetMaterial(teamMaterial);
            board.bishop.SetMaterial(teamMaterial);
            board.knight.SetMaterial(teamMaterial);

            if (team == TeamColor.Black)
            {
                board.queen.transform.position = new Vector3(-3f, 0, -8.2f);
                board.rook.transform.position = new Vector3(-1f, 0, -8.2f);
                board.bishop.transform.position = new Vector3(1F, 0, -8.2f);
                board.knight.transform.position = new Vector3(3F, 0, -8.2f);
            }
            else
            {
                board.queen.transform.position = new Vector3(-3f, 0, 8.2f);
                board.rook.transform.position = new Vector3(-1f, 0, 8.2f);
                board.bishop.transform.position = new Vector3(1F, 0, 8.2f);
                board.knight.transform.position = new Vector3(3F, 0, 8.2f);
            }
            board.isPromoting = true;
        }
    }
}


//Debug.Log(grid[0, 0]);