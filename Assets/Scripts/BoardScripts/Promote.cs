using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promote : MonoBehaviour
{
    [SerializeField] private MaterialSetter materialSetter;
    [SerializeField] private string piece;
    [SerializeField] private Board board;
    public Transform clickPosition;
    

    
   
    public void SetMaterial(Material selectedMaterial)
    {
        materialSetter.SetSingleMaterial(selectedMaterial);
    }

    private void Update()
    {
        if (board.isPromoting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    clickPosition = hit.transform;
                    if (clickPosition.ToString() == "Rook (UnityEngine.Transform)")
                    {
                        board.PromotePiece("rook");
                    }
                    else if (clickPosition.ToString() == "Queen (UnityEngine.Transform)")
                    {
                        board.PromotePiece("queen");
                    }
                    else if (clickPosition.ToString() == "Knight (UnityEngine.Transform)")
                    {
                        board.PromotePiece("knight");
                    }
                    else if (clickPosition.ToString() == "Bishop (UnityEngine.Transform)")
                    {
                        board.PromotePiece("bishop");
                    }



                }
            }
        }
    }


}
