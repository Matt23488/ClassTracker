using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example1
{
    class ExampleEntities : IDisposable
    {
        public HashSet<User> Users { get; }

        public ExampleEntities()
        {
            Users = new HashSet<User>
            {
                new User { Id = 1, Name = "Alice" },
                new User { Id = 2, Name = "Bob" },
                new User { Id = 3, Name = "Charlie" }
            };
        }

        public void Dispose()
        {
            // Nothing to do here
        }
    }
    
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
            => $"Id: {Id}; Name: {Name}";
    }
}
