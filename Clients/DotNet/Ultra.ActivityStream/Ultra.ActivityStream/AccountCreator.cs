using System;
using System.Linq;

namespace Brevitas.AppFramework
{
    public class AccountCreator
    {

        public AccountCreator()
        {

        }
        public static List<Account> CreateAccountsNearStPetersburg()
        {
            var accounts = new List<Account>
    {
        new Account
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            ObjectType = "user",
            DisplayName = "Ivan Ivanov",
            Latitude = 59.9310,
            Longitude = 30.3609,
            Slug = "Ivan"
        },
        new Account
        {
            Id = new Guid("22222222-2222-2222-2222-222222222222"),
            ObjectType = "user",
            DisplayName = "Elena Petrova",
            Latitude = 59.9465,
            Longitude = 30.3820,
            Slug = "Elena"
        },
        new Account
        {
            Id = new Guid("33333333-3333-3333-3333-333333333333"),
            ObjectType = "user",
            DisplayName = "Alexander Sidorov",
            Latitude = 59.9352,
            Longitude = 30.3129,
            Slug = "Alexander"
        },
        new Account
        {
            Id = new Guid("44444444-4444-4444-4444-444444444444"),
            ObjectType = "user",
            DisplayName = "Olga Ivanova",
            Latitude = 59.9222,
            Longitude = 30.3454,
            Slug = "Olga"
        },
        new Account
        {
            Id = new Guid("55555555-5555-5555-5555-555555555555"),
            ObjectType = "user",
            DisplayName = "Michael Kuznetsov",
            Latitude = 59.9167,
            Longitude = 30.4983,
            Slug = "Michael"
        }
    };

            return accounts;
        }
        public static List<Account> CreateAccountsNearSanSalvador()
        {
            var accounts = new List<Account>
    {
        new Account
        {
            Id = new Guid("66666666-6666-6666-6666-666666666666"),
            ObjectType = "user",
            DisplayName = "Juan Perez",
            Latitude = 13.7034,
            Longitude = -89.2073,
            Slug = "Juan-Perez"
        },
        new Account
        {
            Id = new Guid("77777777-7777-7777-7777-777777777777"),
            ObjectType = "user",
            DisplayName = "Maria Garcia",
            Latitude = 13.6929,
            Longitude = -89.2386,
            Slug = "Maria-Garcia"
        },
        new Account
        {
            Id = new Guid("88888888-8888-8888-8888-888888888888"),
            ObjectType = "user",
            DisplayName = "Pedro Hernandez",
            Latitude = 13.7057,
            Longitude = -89.2525,
            Slug = "Pedro-Hernandez"
        },
        new Account
        {
            Id = new Guid("99999999-9999-9999-9999-999999999999"),
            ObjectType = "user",
            DisplayName = "Ana Rodriguez",
            Latitude = 13.7123,
            Longitude = -89.2149,
            Slug = "Ana-Rodriguez"
        },
        new Account
        {
            Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            ObjectType = "user",
            DisplayName = "Carlos Martinez",
            Latitude = 13.7156,
            Longitude = -89.1550,
            Slug = "Carlos-Martinez"
        }
    };

            return accounts;
        }
        public static List<Account> CreateAccountsNearGlendale()
        {
            var accounts = new List<Account>
    {
        new Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aaa"),
            ObjectType = "user",
            DisplayName = "John Smith",
            Latitude = 33.5419,
            Longitude = -112.1850,
            Slug = "John-Smith"
        },
        new Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aab"),
            ObjectType = "user",
            DisplayName = "Emma Johnson",
            Latitude = 33.5414,
            Longitude = -112.2374,
            Slug = "Emma-Johnson"
        },
        new Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aac"),
            ObjectType = "user",
            DisplayName = "William Brown",
            Latitude = 33.5649,
            Longitude = -112.1925,
            Slug = "William-Brown"
        },
        new Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aad"),
            ObjectType = "user",
            DisplayName = "Olivia Davis",
            Latitude = 33.5343,
            Longitude = -112.1954,
            Slug = "Olivia-Davis"
        },
        new Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aae"),
            ObjectType = "user",
            DisplayName = "James Miller",
            Latitude = 33.5845,
            Longitude = -112.2156,
            Slug = "James-Miller"
        }
    };

            return accounts;
        }
        public static List<Account> CreateAccountsNearSantoDomingo()
        {
            var accounts = new List<Account>
    {
        new Account
        {
            Id = new Guid("faaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Juan Rodriguez",
            Latitude = 18.4897,
            Longitude = -69.8901,
            Slug = "juan-rodriguez"
        },
        new Account
        {
            Id = new Guid("gaaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Sofía Fernández",
            Latitude = 18.4816,
            Longitude = -69.9271,
            Slug = "sofia-fernandez"
        },
        new Account
        {
            Id = new Guid("haaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Pedro Martínez",
            Latitude = 18.4640,
            Longitude = -69.9117,
            Slug = "pedro-martinez"
        },
        new Account
        {
            Id = new Guid("iaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Ana Gómez",
            Latitude = 18.4758,
            Longitude = -69.9022,
            Slug = "ana-gomez"
        },
        new Account
        {
            Id = new Guid("jaaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Luis García",
            Latitude = 18.4876,
            Longitude = -69.9387,
            Slug = "luis-garcia"
        }
    };

            return accounts;
        }
        public static List<Account> CreateAccountsNearSantiago()
        {
            var accounts = new List<Account>
    {
        new Account
        {
            Id = new Guid("jaaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Juan Pérez",
            Latitude = -33.4625,
            Longitude = -70.6684,
            Slug = "juan-perez"
        },
        new Account
        {
            Id = new Guid("kaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "María González",
            Latitude = -33.4588,
            Longitude = -70.6506,
            Slug = "maria-gonzalez"
        },
        new Account
        {
            Id = new Guid("laaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Pedro Rodríguez",
            Latitude = -33.4714,
            Longitude = -70.6376,
            Slug = "pedro-rodriguez"
        },
        new Account
        {
            Id = new Guid("maaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Ana Fernández",
            Latitude = -33.4393,
            Longitude = -70.6522,
            Slug = "ana-fernandez"
        },
        new Account
        {
            Id = new Guid("naaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Luis Gómez",
            Latitude = -33.4434,
            Longitude = -70.6719,
            Slug = "luis-gomez"
        }
    };

            return accounts;
        }
        public static List<Account> CreateAccountsNearBuenosAires()
        {
            var accounts = new List<Account>
    {
        new Account
        {
            Id = new Guid("naaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Juan López",
            Latitude = -34.6037,
            Longitude = -58.3816,
            Slug = "juan-lopez"
        },
        new Account
        {
            Id = new Guid("obaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "María García",
            Latitude = -34.6158,
            Longitude = -58.4333,
            Slug = "maria-garcia"
        },
        new Account
        {
            Id = new Guid("pbaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Pedro Martínez",
            Latitude = -34.5812,
            Longitude = -58.4059,
            Slug = "pedro-martinez"
        },
        new Account
        {
            Id = new Guid("qbaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Ana Rodríguez",
            Latitude = -34.6033,
            Longitude = -58.3724,
            Slug = "ana-rodriguez"
        },
        new Account
        {
            Id = new Guid("rbaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            ObjectType = "user",
            DisplayName = "Luis Fernández",
            Latitude = -34.6345,
            Longitude = -58.3626,
            Slug = "luis-fernandez"
        }
    };

            return accounts;
        }

    }
}
