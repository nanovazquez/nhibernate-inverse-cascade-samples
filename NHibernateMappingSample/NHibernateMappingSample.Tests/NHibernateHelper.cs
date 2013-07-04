namespace NHibernateMappingsSample
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.SqlCommand;
    using NHibernate.Tool.hbm2ddl;
    using System.Diagnostics;

    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void CreateDb()
        {
            var configuration = GetConfiguration();
            new SchemaExport(configuration).Create(true, true);
        }

        public static void DropDb()
        {
            var configuration = GetConfiguration();
            new SchemaExport(configuration).Drop(true, true);
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    var configuration = GetConfiguration();
                    sessionFactory = configuration.BuildSessionFactory();
                }

                return sessionFactory;
            }
        }

        private static Configuration GetConfiguration()
        {
            var config = new Configuration().Configure();
            config.SetInterceptor(new SqlQueryInterceptor());
            return config;
        }
    }

    public class SqlQueryInterceptor : EmptyInterceptor, IInterceptor
    {
        SqlString IInterceptor.OnPrepareStatement(SqlString sql)
        {
            Debug.WriteLine("----- Sql sentence -----");
            Debug.WriteLine(sql);
            Debug.WriteLine("----- End Sql Sentence -----");

            return sql;
        }
    }

}
