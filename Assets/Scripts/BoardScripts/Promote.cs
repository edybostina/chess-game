using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Promote : MonoBehaviour
{
    [SerializeField] private MaterialSetter materialSetter;
    [SerializeField] private string piece;
    [SerializeField] private Board board;
    public Transform clickPosition;
    private string[] pieces = new string[] { "rook", "queen", "knight", "bishop" };




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
                    string name = clickPosition.ToString().Split()[0].ToLower();
                    if (pieces.Contains(name))
                    {
                        board.PromotePiece(name);
                    }
                }
            }
        }
    }


}
