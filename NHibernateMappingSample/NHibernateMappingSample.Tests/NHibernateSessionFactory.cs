namespace NHibernateMappingSample
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.SqlCommand;
    using NHibernate.Tool.hbm2ddl;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public class NHibernateSessionFactory
    {
        private ISessionFactory sessionFactory;
        private List<Assembly> mappingAssemblies;

        public ISession OpenSession()
        {
            return this.SessionFactory.OpenSession();
        }

        public void AddMappingAssembly(Assembly mappingAssembly)
        {
            this.mappingAssemblies = this.mappingAssemblies ?? new List<Assembly>();

            if (!this.mappingAssemblies.Contains(mappingAssembly))
            {
                this.mappingAssemblies.Add(mappingAssembly);
            }
        }

        public void CreateDb()
        {
            var configuration = GetConfiguration();
            new SchemaExport(configuration).Create(true, true);
        }

        public void DropDb()
        {
            var configuration = GetConfiguration();
            new SchemaExport(configuration).Drop(true, true);
        }

        public void CloseSessionFactory()
        {
            this.sessionFactory.Close();
        }

        private ISessionFactory SessionFactory
        {
            get
            {
                if (this.sessionFactory == null)
                {
                    var configuration = GetConfiguration();
                    this.sessionFactory = configuration.BuildSessionFactory();
                }

                return this.sessionFactory;
            }
        }
        private Configuration GetConfiguration()
        {
            var config = new Configuration().Configure();
            this.mappingAssemblies.ForEach(ma => config.AddAssembly(ma));
            config.SetInterceptor(new SqlQueryInterceptor());

            return config;
        }
    }

    internal class SqlQueryInterceptor : EmptyInterceptor, IInterceptor
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
