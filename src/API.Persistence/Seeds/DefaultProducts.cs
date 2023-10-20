using API.Domain.Entities;
using System.Collections.Generic;

namespace API.Persistence.Seeds
{
    public class DefaultProducts
    {
        public static IEnumerable<Product> ProductList()
        {
            return new List<Product> { 
                new Product {
                    Id = 1,
                    Name = "Taladro Percutor",
                    Description = @"Taladro Percutor Inalambrico 20V BRUSHLESS",
                    Price = 320,
                    Stock = 10
                },
                new Product {
                    Id = 2,
                    Name = "Dados Y Accesorios",
                    Description = "Set Dados Y Accesorios 1/4-1/2 108P Bahco",
                    Price = 144,
                    Stock = 230
                },
                new Product {
                    Id = 3,
                    Name = "Juego De Alicates",
                    Description = "Juego De Alicates 2 Piezas Redline",
                    Price = 86,
                    Stock = 110
                }
            };
        }
    }
}
