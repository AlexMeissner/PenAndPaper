namespace DataTransfer.WebSocket
{
    public class WebSocketUpdate(object entity)
    {
        public string Type { get; set; } = entity.GetType().Name;
        public object Entity { get; set; } = entity;
    }
}
