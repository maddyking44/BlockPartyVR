public class BlockRemoveEventArgs : System.EventArgs
{
    string _name;

    public BlockRemoveEventArgs(string name)
    {
        this._name = name;
    }

    public string Name
    {
        get { return _name; }
    }
}

public delegate void BlockRemoveEventHandler(object sender, BlockRemoveEventArgs e);