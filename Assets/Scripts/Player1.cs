
public class Player1 : Player
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Move(string characterType)
    {
        base.Move("player");
    }
}
