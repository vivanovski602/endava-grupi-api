namespace endavaRestApi.Repositories
{
    public interface IReportRepository
    {
        Task<int> GetOrderCountAsync();
    }
}
