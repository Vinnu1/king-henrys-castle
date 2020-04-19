public class Archer : Enemy
{
    void Start()
    {
        base.Init();
    }

    override public bool Wait()
    {
        return false;
    }

    override public void Move(float dis, string enemyType)
    {
        base.Move(25f, enemyType = "Archer");
    }

}
