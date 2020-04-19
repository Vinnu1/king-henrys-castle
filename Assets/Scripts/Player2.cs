using UnityEngine;

public class Player2 : Player
{
    public Animator animator;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Move(string characterType)
    {
        base.Move("enemy");
    }

    public override void Restart(string characterType)
    {
        base.Restart("enemy");
    }
}
