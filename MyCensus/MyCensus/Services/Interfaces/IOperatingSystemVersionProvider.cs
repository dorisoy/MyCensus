namespace MyCensus.Services
{
    public interface IOperatingSystemVersionProvider
	{
		string GetOperatingSystemVersionString();
        void CheckUpdate();

        string GetVersion();

    }
}