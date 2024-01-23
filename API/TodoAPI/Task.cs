namespace TodoAPI
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
    }
}
