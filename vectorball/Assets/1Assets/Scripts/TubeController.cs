using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof( TubeRenderer))]
public class TubeController : MonoBehaviour {

    [SerializeField] List<Vector3> positionList;

    public static TubeController instance;
    TubeRenderer tube;

	void Awake ()
    {
        instance = this;
        tube = GetComponent<TubeRenderer>();
	}

    public void AddPosition(Vector3 position)
    {
        positionList.Add(position);
        UpdateLine();
    }

    public void AddPosition(Transform position)
    {
        AddPosition(position.position);
    }


    void UpdateLine()
    {
        tube.SetPoints(positionList.ToArray(), 1, Color.green);
    }

}
