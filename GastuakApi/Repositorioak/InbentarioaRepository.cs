using JatetxeaApi.Modeloak;
using NHibernate;
using System.Linq;
using System.Collections.Generic;
using NHSession = NHibernate.ISession;
using NHSessionFactory = NHibernate.ISessionFactory;

namespace JatetxeaApi.Repositorioak
{
    public class InbentarioaRepository
    {
        private readonly NHSession _session;

        public InbentarioaRepository(NHSessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        public void Add(Inbentarioa item)
        {
            using var tx = _session.BeginTransaction();
            _session.Save(item);
            tx.Commit();
        }

        public Inbentarioa? Get(int id)
        {
            return _session.Query<Inbentarioa>()
                .SingleOrDefault(x => x.Id == id);
        }

        public IList<Inbentarioa> GetAll()
        {
            return _session.Query<Inbentarioa>().ToList();
        }

        public void Update(Inbentarioa item)
        {
            using var tx = _session.BeginTransaction();
            _session.Update(item);
            tx.Commit();
        }

        public void Delete(Inbentarioa item)
        {
            using var tx = _session.BeginTransaction();
            _session.Delete(item);
            tx.Commit();
        }
    }
}
