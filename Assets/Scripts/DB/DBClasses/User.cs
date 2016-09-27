using System.Collections;

public class User {

	public User(int idUser, string name, Language language)
	{
		this.idUser = idUser;
		this.name = name;
        this.language = language;
	}

	public int idUser { get; set; }
	public string name {get;set;}
    public Language language { get; set; }
}
