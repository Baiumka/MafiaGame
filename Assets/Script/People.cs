
public class People
{
    private string nickname;
    private int id;

    public People(int id, string nickname)
    {
        this.id = id;
        this.nickname = nickname;
    }

    public string Nickname { get => nickname; }
    public int Id { get => id; }

   
}
