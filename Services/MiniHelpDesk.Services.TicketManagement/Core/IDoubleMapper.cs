namespace MiniHelpDesk.Services.TicketManagement.Core
{
    public interface IDoubleMapper<TSource, TDestination>
    {
        TDestination Map(TSource source, TDestination destination);
    }
}
