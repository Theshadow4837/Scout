namespace Learning.Pages
{
    internal class Form
    {
        internal string _formTitleEntry;

        public string Id { get; set; }
        public List<string> Questions { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FormTitle { get; internal set; }
    }
}