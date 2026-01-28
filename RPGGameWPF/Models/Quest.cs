using System;

namespace RPGGameWPF.Models
{
	public class Quest
	{
		public string Title { get; }
		public int TargetKills { get; }
		public int CurrentKills { get; private set; }
		public bool IsCompleted => CurrentKills >= TargetKills;

		public Quest(string title, int targetKills)
		{
			Title = title;
			TargetKills = targetKills;
			CurrentKills = 0;
		}

		public void AddKill()
		{		
				CurrentKills++;
		}
	}
}
