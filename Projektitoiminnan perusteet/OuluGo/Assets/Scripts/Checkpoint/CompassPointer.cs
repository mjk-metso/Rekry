using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPointer : MonoBehaviour
{

    private List<Checkpoint> checkpoints;
    private Checkpoint _currpoint;
    // Start is called before the first frame update
    void Start()
    {
        checkpoints = CheckpointHolder.Instance.checkpoints;
    }

    // Update is called once per frame
    void Update()
    {
      if(!_currpoint) {
          _currpoint = closestCheckPoint();
      }
      Vector3 direction = _currpoint.transform.position - transform.position;
      float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
      transform.eulerAngles = new Vector3(0, angle, 0);
    }

    Checkpoint closestCheckPoint() {
        float _distance = float.PositiveInfinity;
        Checkpoint _point = null;
        foreach(var point in checkpoints) {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if(distance < _distance) {
                _distance = distance;
                _point = point;
            }
        }
        return _point;
    }
}
