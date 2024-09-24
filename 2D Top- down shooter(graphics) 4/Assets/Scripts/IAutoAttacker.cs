using UnityEngine;

public interface IAttacker
{
		float FireRate { get; }
		float AttackRange { get; }
		float BulletCooldown { get; }
		GameObject BulletPrefab { get; }
		Transform Transform { get; }

		void Attack();
}
