using System;

namespace RPGGameWPF.Models
{
	public class Enemy : Character
	{
		public EnemyType Type { get; }
		public Enemy(EnemyType type, Position pos) : base(pos)
		{
			Type = type;
			if (type == EnemyType.Goblin)
			{
				Name = "Goblin";
				MaxHp = 18;
				Hp = 18;
				BaseAttack = 5;
				BaseDefense = 1;
			}
			else
			{
				Name = "Skeleton";
				MaxHp = 24;
				Hp = 24;
				BaseAttack = 6;
				BaseDefense = 2;
			}
			
		}
	}
}
