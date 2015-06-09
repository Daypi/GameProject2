using System;

enum character
{
	Vanguard, SKELETON, Kachujin, Alpha, Heraklios, Zoe
};

public class MeshCharacter
{
	public static string  GetnameCharacter(int num)
	{
		switch (num)
		{
		case (0) : 
			return "Vanguard";
		case (1) : 
			return "SKELETON";
		case (2) : 
			return "Kachujin";
		case (3) : 
			return "Alpha";
		case (4) : 
			return "Heraklios";
		case (5) : 
			return "Zoe";
		}
		return null;
	}
}
