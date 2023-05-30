

using System;

namespace C3Apparel.Data.CustomTables
{
	/// <summary>
	/// Represents a content item of type PricingImportLogItem.
	/// </summary>
	public partial class PricingImportLogItem
	{

		public string MessageType
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public string Details
		{
			get;
			set;
		}

		public string ItemCreatedUser
		{
			get;
			set;
		}

		public DateTime ItemCreatedWhen { get; set; }
		public Guid ItemGUID { get; set; }
	}
}