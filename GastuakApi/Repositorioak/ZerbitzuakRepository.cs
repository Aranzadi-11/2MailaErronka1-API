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

        public void Add(Zerbitzuak item)
        {
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

        public void Update(Zerbitzuak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Update(item);
            tx.Commit();
        }

        public void Delete(Zerbitzuak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Delete(item);
            tx.Commit();
        }
    }
}
