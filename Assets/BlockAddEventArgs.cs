using UnityEngine;

public class BlockAddEventArgs : System.EventArgs
{
    Vector3 _point;
    public int BlockTypeId { get; set; }
    public string BlockName { get; set; }

    public BlockAddEventArgs(Vector3 point, int blockTypeId, string blockName)
    {
        this._point = point;
        this.BlockTypeId = blockTypeId;
        this.BlockName = blockName;
    }

    public Vector3 BlockLocation { get { return _point; } }
}

public delegate void BlockAddEventHandler(object sender, BlockAddEventArgs e);