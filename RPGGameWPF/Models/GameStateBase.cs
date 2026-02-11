namespace RPGGameWPF.Models
{
	public class GameStateBase
	{

		public Enemey? GetEnemeyAt(Position p)
		{
			for (int i = 0; i < Enemies.Count; i++)
			{
				if (Enemies[i].IsAlive && Enemies[i].Pos.X == p.X && Enemies[i].Pos.Y == p.Y) return Enemies[i];
			}
		}
	}
}