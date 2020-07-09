namespace UAssetParser
{
	public class UAParserOptions
	{
		public bool Verbose
		{
			get;
			set;
		}

		public bool ForceArrays
		{
			get;
			set;
		}

		public bool PrintInstances
		{
			get;
			set;
		}

		public bool VeboseInstances
		{
			get;
			set;
		}

		public bool AllowNewEntries
		{
			get;
			set;
		}

		public UAParserOptions(bool verbose = false, bool forceArrays = false, bool inNumbers = false, bool verboseIn = false, bool newEntries = false)
		{
			Verbose = verbose;
			ForceArrays = forceArrays;
			PrintInstances = inNumbers;
			VeboseInstances = verboseIn;
			AllowNewEntries = newEntries;
		}
	}
}
