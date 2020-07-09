namespace UAssetParser.Objects.Visitors
{
	public interface IStructObject
	{
		object Serialize();

		int GetSize();
	}
}
