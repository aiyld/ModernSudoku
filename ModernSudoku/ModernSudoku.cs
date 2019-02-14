using Contracts;
using Contracts.Plugins;
using ModernSudoku.Model;

namespace ModernSudoku
{
    public class ModernSudoku : IExtension
    {
        private IAppService service;

        /// <summary>
        /// Service.
        /// </summary>
        public IAppService Service
        {
            get { return this.service; }
            set
            {
                this.service = value;
                InternalService = value;
            }
        }

        /// <summary>
        /// Internal service.
        /// </summary>
        internal static IAppService InternalService { get; private set; }

        public void Start()
        {
            this.Service.Home.ShowWindow(new MainWindow());
        }
    }
}
