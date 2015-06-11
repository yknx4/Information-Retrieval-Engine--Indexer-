namespace Engine.Database.Repositories
{
    class MySqlRepositoriesSync
    {
        private static volatile bool _termRepositoryWorking;
        private static volatile bool _weightRepositoryWorking;
        private static volatile bool _documentRepositoryWorking;

        public static bool IsTermRepositoryWorking
        {
            get { return _termRepositoryWorking; }
            set { _termRepositoryWorking = value; }
        }

        public static bool IsWeightRepositoryWorking
        {
            get { return _weightRepositoryWorking; }
            set { _weightRepositoryWorking = value; }
        }

        public static bool IsDocumentRepositoryWorking
        {
            get { return _documentRepositoryWorking; }
            set { _documentRepositoryWorking = value; }
        }
    }
}
