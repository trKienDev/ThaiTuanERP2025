namespace ThaiTuanERP2025.Domain.Common.ValueObjects
{
	public abstract class ValueObject
	{
		protected abstract IEnumerable<object> GetEqualityComponents();
		public override bool Equals(object? obj)
		{
			if (obj == null || obj.GetType() != GetType()) return false;

			var other = (ValueObject)obj;
			var thisValues = GetEqualityComponents().GetEnumerator();
			var otherValues = other.GetEqualityComponents().GetEnumerator();

			while (thisValues.MoveNext() && otherValues.MoveNext())
			{
				if (!Equals(thisValues.Current, otherValues.Current)) return false;
			}

			return !thisValues.MoveNext() && !otherValues.MoveNext();
		}

		public override int GetHashCode()
		{
			int hash = 0;
			foreach (var obj in GetEqualityComponents())
			{
				hash ^= obj?.GetHashCode() ?? 0;
			}
			return hash;
		}
	}
}