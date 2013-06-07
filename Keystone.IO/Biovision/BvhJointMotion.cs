namespace Linearstar.Keystone.IO.Biovision
{
	public class BvhJointMotion
	{
		public float[] Position
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3
		/// </summary>
		public float[] Rotation
		{
			get;
			set;
		}

		public BvhJointMotion()
		{
		}

		public BvhJointMotion(float[] position, float[] rotation)
		{
			this.Position = position;
			this.Rotation = rotation;
		}
	}
}
