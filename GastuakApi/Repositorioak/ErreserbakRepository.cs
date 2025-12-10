using JatetxeaApi.Modeloak;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHSession = NHibernate.ISession;
using NHSessionFactory = NHibernate.ISessionFactory;

namespace JatetxeaApi.Repositorioak
{
    public class ErreserbakRepository
    {
        private readonly NHSession _session;

        public ErreserbakRepository(NHSessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        public void Add(Erreserbak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Save(item);
            tx.Commit();
        }

        public Erreserbak? Get(int id)
        {
            return _session.Query<Erreserbak>().SingleOrDefault(x => x.Id == id);
        }

        public IList<Erreserbak> GetAll()
        {
            return _session.Query<Erreserbak>().ToList();
        }

        public void Update(Erreserbak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Update(item);
            tx.Commit();
        }

        public void Delete(Erreserbak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Delete(item);
            tx.Commit();
        }
    }
}
