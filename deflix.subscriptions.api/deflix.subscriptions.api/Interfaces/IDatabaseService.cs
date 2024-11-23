namespace deflix.subscriptions.api.Interfaces
{
    public interface IDatabaseService
    {
        public int Execute(string query, object param = null);

        public T QueryFirst<T>(string query, object param = null);

        public List<T> Query<T>(string query, object param = null);
    }

}
