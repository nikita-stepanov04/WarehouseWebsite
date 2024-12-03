namespace WarehouseWebsite.Domain.Models.Emails
{
    public class EmailMetadata<T>
    {
        public required string ToAddress { get; set; }
        public required string Subject { get; set; }
        
        public string? Body { get; set; }

        public string? ViewName { get; set; }
        public T? Model { get; set; }
    }
}
