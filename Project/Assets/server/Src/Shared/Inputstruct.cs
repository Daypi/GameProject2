public class Inputstruct  {

	public float Horizontal;
	public float ChangeWeapon;
	public float ExecuteTime;
	public double dt;
	public bool Jump;
	public bool Shoot;
	
	public Inputstruct(float _horizontal, float _changeweapon, bool _jump, bool _shoot, float _ExecuteTime)
	{
		Horizontal = _horizontal;
		ChangeWeapon = _changeweapon;
		Jump = _jump;
		Shoot = _shoot;
		ExecuteTime = _ExecuteTime;
	}

	public bool isequal(Inputstruct inputi)
	{
		if (inputi.Horizontal == Horizontal &&
			inputi.ChangeWeapon == ChangeWeapon &&
			inputi.Jump == Jump &&
			inputi.Shoot == Shoot)
			return true;
		else
			return false;

	}
}
