public class Slime : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Init(); 
    }

    // Update is called once per frame
    override public void Move(float dis, string enemyType = "general")
    {
        base.Move(3.3f);
    }
}
