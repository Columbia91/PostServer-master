using System.Collections.Generic;
using System.Data.Entity;

namespace QuickServer
{
    public class DataInitializer : CreateDatabaseIfNotExists<PostContext>
    {
        protected override void Seed(PostContext context)
        {
            context.PostalRecords.AddRange(new List<PostalRecord>
            {
                new PostalRecord
                {
                    Index = "Z05K6H5",
                    Street = "Brown st."
                },
                new PostalRecord
                {
                    Index = "Z05K6H5",
                    Street = "Central st."
                },
                new PostalRecord
                {
                    Index = "Z05K6H5",
                    Street = "Wall st."
                },
                new PostalRecord
                {
                    Index = "Z05K6H5",
                    Street = "Barclay st."
                }
            });
        }
    }
}