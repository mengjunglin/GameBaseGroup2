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
		positionList.Add (position);
		UpdateLine ();
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
		positionList.Clear ();
		tube.Reset();
	}

	public void SetTubeThickness(int thickness){
		tube.crossSegments = 2;
		tube.SetPoints(positionList.ToArray(), 1, Color.green);
	}

	public void CreateTube(int[] x){
		clearLine ();
		for (int i = 0; i < 8; i = i + 2) {
			AddPosition (FieldController.instance.GetAbsolutePosition (x[i], x[i+1]));
		}
	}
}
