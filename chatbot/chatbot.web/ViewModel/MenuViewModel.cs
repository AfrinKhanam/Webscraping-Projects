namespace IndianBank_ChatBOT.ViewModel
{
    public class MenuViewModel
    {
        public string Text { get; set; }

        public string Url { get; set; }

        public MenuViewModel[] ChildItems { get; set; }

        public string[] Parents { get; set; }
    }
}
