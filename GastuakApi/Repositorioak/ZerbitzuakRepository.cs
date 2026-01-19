using JatetxeaApi.Modeloak;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHSession = NHibernate.ISession;
using NHSessionFactory = NHibernate.ISessionFactory;

namespace JatetxeaApi.Repositorioak
{
    public class ZerbitzuakRepository
    {
        private readonly NHSession _session;

        public ZerbitzuakRepository(NHSessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        private bool TransakzioaAktibo()
        {
            return _session.Transaction != null && _session.Transaction.IsActive;
        }

        public void Add(Zerbitzuak item)
        {
            if (TransakzioaAktibo())
            {
                _session.Save(item);
                return;
            }

            using var tx = _session.BeginTransaction();
            _session.Save(item);
            tx.Commit();
        }

        public Zerbitzuak? Get(int id)
        {
            return _session.Query<Zerbitzuak>().SingleOrDefault(x => x.Id == id);
        }

        public IList<Zerbitzuak> GetAll()
        {
            return _session.Query<Zerbitzuak>().ToList();
        }

        public IList<Zerbitzuak> LortuErreserbaIdz(int erreserbaId)
        {
            return _session.Query<Zerbitzuak>()
                .Where(z => z.ErreserbaId.HasValue && z.ErreserbaId.Value == erreserbaId)
                .ToList();
        }

        public int ErreserbaLoturaKendu(int erreserbaId)
        {
            if (TransakzioaAktibo())
            {
                return _session.CreateQuery("update Zerbitzuak z set z.ErreserbaId = null where z.ErreserbaId = :id")
                    .SetParameter("id", erreserbaId)
                    .ExecuteUpdate();
            }

            using var tx = _session.BeginTransaction();
            var kop = _session.CreateQuery("update Zerbitzuak z set z.ErreserbaId = null where z.ErreserbaId = :id")
                .SetParameter("id", erreserbaId)
                .ExecuteUpdate();
            tx.Commit();
            return kop;
        }

        public void Update(Zerbitzuak item)
        {
            if (TransakzioaAktibo())
            {
                _session.Update(item);
                return;
            }

            using var tx = _session.BeginTransaction();
            _session.Update(item);
            tx.Commit();
        }

        public void Delete(Zerbitzuak item)
        {
            if (TransakzioaAktibo())
            {
                _session.Delete(item);
                return;
            }

            using var tx = _session.BeginTransaction();
            _session.Delete(item);
            tx.Commit();
        }
    }
}
