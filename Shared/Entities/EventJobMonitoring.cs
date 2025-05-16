namespace Shared.Entities
{
    public class EventJobMonitoring : BaseEntities
    {
        public int Id { get; set; }
        public string EventJobName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}