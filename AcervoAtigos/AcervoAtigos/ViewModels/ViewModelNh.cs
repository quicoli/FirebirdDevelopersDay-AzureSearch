using JulMar.Windows.Mvvm;
using NHibernate;

namespace AcervoAtigos.ViewModels
{
    public class ViewModelNh: ViewModel
    {
        private bool _estaOcupado;
        private ISession _session;
        private IStatelessSession _statelessSession;

        public bool EstaOcupado
        {
            get => _estaOcupado;
            set { _estaOcupado = value; RaisePropertyChanged(()=> EstaOcupado);}
        }


        protected ISession Session => _session ?? (_session = App.SessionFactory.OpenSession());

        protected IStatelessSession StatelessSession => _statelessSession ?? (_statelessSession = App.SessionFactory.OpenStatelessSession());

        public new virtual void Dispose()
        {
            _session?.Dispose();
            _statelessSession?.Dispose();
        }
    }
}