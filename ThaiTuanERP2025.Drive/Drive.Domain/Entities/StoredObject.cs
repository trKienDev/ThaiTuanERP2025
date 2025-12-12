namespace ThaiTuanERP2025.Domain.StoredFiles.Entities
{
	public class StoredObject 
	{
		#region Constructors
		private StoredObject() { }
		public StoredObject( string objectKey, string fileName, string contentType, long size)
		{
			Id = Guid.NewGuid();

			ObjectKey = objectKey.Trim();
			FileName = fileName.Trim();
			ContentType = contentType.Trim();
			Size = size;
			CreatedAt = DateTime.UtcNow;
		}
		#endregion

		#region Properties
		public Guid Id { get; private set;  }
		public string ObjectKey { get; private set; } = null!;
		public string FileName { get; private set; } = null!;
		public string ContentType { get; private set; } = null!;
		public long Size { get; private set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? ModifiedAt { get; set; }
		public bool IsDeleted { get; set; } = false;
		#endregion

		#region Domain Behaviors

		internal void UpdateMetadata(string newFileName, string newContentType)
		{
			FileName = newFileName.Trim();
			ContentType = newContentType.Trim();
		}

		#endregion
	}
}