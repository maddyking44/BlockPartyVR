using UnityEngine;

public class BlockAddEventArgs : System.EventArgs
{
    Vector3 _point;

    public BlockAddEventArgs(Vector3 point)
    {
        this._point = point;
    }

    public Vector3 BlockLocation { get { return _point; } }
}

public delegate void BlockAddEventHandler(object sender, BlockAddEventArgs e);