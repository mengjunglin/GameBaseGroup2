using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof( TubeRenderer))]
public class TubeController : MonoBehaviour {

    [SerializeField] List<Vector3> positionList;

    public static TubeController instance;
    TubeRenderer tube;
	public Material tubeMat;

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
		tube.crossSegments = 2;
        tube.SetPoints(positionList.ToArray(), 2, Color.green);
    }

	public void clearLine()
	{
		
		tube.Reset();
	}
		
}
