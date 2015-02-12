using UnityEngine;

public struct GridCoord {
	public int x;
	public int y;
	
	public GridCoord(int x, int y) {		
		this.x = x;
		this.y = y;
	}
	
	public override bool Equals(object otherObject) {
		if (otherObject == null)
			return false;
		
		if (!(otherObject is GridCoord))
			return false;
		
		GridCoord other = (GridCoord)otherObject;	
		return this.x == other.x && this.y == other.y;
	}
	
	public static bool operator ==(GridCoord a, GridCoord b) {
		return a.Equals(b);
	}
	
	public static bool operator !=(GridCoord a, GridCoord b) {
		return !(a == b);
	}
	
	public override int GetHashCode()
	{
		return x ^ y;
	}
}