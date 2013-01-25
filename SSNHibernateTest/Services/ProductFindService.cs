using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using Contacts;

using NHibernate;

using ServiceStack.Common.Extensions;
using ServiceStack.ServiceInterface;


namespace Services
{
    public class ProductFindService : ServiceBase<ProductFind>
    {
        public ISessionFactory NHSessionFactory { get; set; }

        public ProductFindService(ISessionFactory sessionFactory)
        {
            NHSessionFactory = sessionFactory;
        }

        protected override object Run(ProductFind request)
        {

            var connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");

            using (connection)
            {
                connection.Open();
                CreateInMemoryTable(connection);

                using (var session = NHSessionFactory.OpenSession(connection))
                {
                    var result = session.Load<Models.Product>(request.Id);

                    return new ProductFindResponse
                               {
                                   Result = result.TranslateTo<ProductFindResponse.Product>()
                               };
                }
            }

        }

        protected void CreateInMemoryTable(SQLiteConnection conn)
        {

            SQLiteCommand cmd = new SQLiteCommand();
            try
            {


                string cmdstring = @"CREATE TABLE Product 
                            (Id INTEGER NOT NULL, 
                            Name TEXT, 
                            Description TEXT, 
                            PRIMARY KEY (Id));  
                            INSERT INTO Product (Name, Description) 
                            VALUES ('Raspberry pi', 'Credit card size computer.');";

                cmd.CommandText = cmdstring;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

            }
            catch (SQLiteException ex)
            {

                Debug.WriteLine(ex);
            }
            finally
            {
                cmd.Dispose();
            }

        }

    }
}