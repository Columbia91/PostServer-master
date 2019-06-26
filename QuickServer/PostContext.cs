namespace QuickServer
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class PostContext : DbContext
    {
        // Контекст настроен для использования строки подключения "PostContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "QuickServer.PostContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "PostContext" 
        // в файле конфигурации приложения.
        public PostContext()
            : base("name=PostContext")
        {
            Database.SetInitializer(new DataInitializer());
        }

        public DbSet<PostalRecord> PostalRecords { get; set; }
        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }
}