using System;
using System.Linq;

namespace Ultra.ActivityStream
{
    public class AccountCreator
    {

        public AccountCreator()
        {

        }
        public static List<Ultra.ActivityStream.Contracts.Account> CreateAccountsNearStPetersburg()
        {
            var accounts = new List<Ultra.ActivityStream.Contracts.Account>
    {
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            ObjectType = "user",
            DisplayName = "Ivan Ivanov",
            Latitude = 59.9310,
            Longitude = 30.3609,
            Slug = "Ivan"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("22222222-2222-2222-2222-222222222222"),
            ObjectType = "user",
            DisplayName = "Elena Petrova",
            Latitude = 59.9465,
            Longitude = 30.3820,
            Slug = "Elena"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("33333333-3333-3333-3333-333333333333"),
            ObjectType = "user",
            DisplayName = "Alexander Sidorov",
            Latitude = 59.9352,
            Longitude = 30.3129,
            Slug = "Alexander"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("44444444-4444-4444-4444-444444444444"),
            ObjectType = "user",
            DisplayName = "Olga Ivanova",
            Latitude = 59.9222,
            Longitude = 30.3454,
            Slug = "Olga"
        },
        new Ultra.ActivityStream.Contracts.Account
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
        public static List<Ultra.ActivityStream.Contracts.Account> CreateAccountsNearSanSalvador()
        {
            var accounts = new List<Ultra.ActivityStream.Contracts.Account>
    {
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("66666666-6666-6666-6666-666666666666"),
            ObjectType = "user",
            DisplayName = "Juan Perez",
            Latitude = 13.7034,
            Longitude = -89.2073,
            Slug = "Juan-Perez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("77777777-7777-7777-7777-777777777777"),
            ObjectType = "user",
            DisplayName = "Maria Garcia",
            Latitude = 13.6929,
            Longitude = -89.2386,
            Slug = "Maria-Garcia"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("88888888-8888-8888-8888-888888888888"),
            ObjectType = "user",
            DisplayName = "Pedro Hernandez",
            Latitude = 13.7057,
            Longitude = -89.2525,
            Slug = "Pedro-Hernandez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("99999999-9999-9999-9999-999999999999"),
            ObjectType = "user",
            DisplayName = "Ana Rodriguez",
            Latitude = 13.7123,
            Longitude = -89.2149,
            Slug = "Ana-Rodriguez"
        },
        new Ultra.ActivityStream.Contracts.Account
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
        public static List<Ultra.ActivityStream.Contracts.Account> CreateAccountsNearGlendale()
        {
            var accounts = new List<Ultra.ActivityStream.Contracts.Account>
    {
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aaa"),
            ObjectType = "user",
            DisplayName = "John Smith",
            Latitude = 33.5419,
            Longitude = -112.1850,
            Slug = "John-Smith"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aab"),
            ObjectType = "user",
            DisplayName = "Emma Johnson",
            Latitude = 33.5414,
            Longitude = -112.2374,
            Slug = "Emma-Johnson"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aac"),
            ObjectType = "user",
            DisplayName = "William Brown",
            Latitude = 33.5649,
            Longitude = -112.1925,
            Slug = "William-Brown"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("ba508de3-c3fb-4d9f-82b6-901bfe789aad"),
            ObjectType = "user",
            DisplayName = "Olivia Davis",
            Latitude = 33.5343,
            Longitude = -112.1954,
            Slug = "Olivia-Davis"
        },
        new Ultra.ActivityStream.Contracts.Account
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
        public static List<Ultra.ActivityStream.Contracts.Account> CreateAccountsNearSantoDomingo()
        {
            var accounts = new List<Ultra.ActivityStream.Contracts.Account>
    {
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("89e1755f-d75a-4f5c-913b-a6ff422533b2"),
            ObjectType = "user",
            DisplayName = "Juan Rodriguez",
            Latitude = 18.4897,
            Longitude = -69.8901,
            Slug = "juan-rodriguez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("2a055d7c-049c-4ead-984f-f1987c6e4deb"),
            ObjectType = "user",
            DisplayName = "Sofía Fernández",
            Latitude = 18.4816,
            Longitude = -69.9271,
            Slug = "sofia-fernandez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("95ee691b-2888-4397-8684-450080f09223"),
            ObjectType = "user",
            DisplayName = "Pedro Martínez",
            Latitude = 18.4640,
            Longitude = -69.9117,
            Slug = "pedro-martinez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("555596c1-3518-4917-8c89-463aa40c9ba1"),
            ObjectType = "user",
            DisplayName = "Ana Gómez",
            Latitude = 18.4758,
            Longitude = -69.9022,
            Slug = "ana-gomez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("65347759-d89b-4fa7-b016-46f0c7f29f5a"),
            ObjectType = "user",
            DisplayName = "Luis García",
            Latitude = 18.4876,
            Longitude = -69.9387,
            Slug = "luis-garcia"
        }
    };

            return accounts;
        }
        public static List<Ultra.ActivityStream.Contracts.Account> CreateAccountsNearSantiago()
        {
            //        var accounts = new List<Account>
            //{
            //    new Account
            //    {
            //        Id = new Guid("2d845728-6dff-4020-a841-69c84fdf8dfc"),
            //        ObjectType = "user",
            //        DisplayName = "Juan Pérez",
            //        Latitude = -33.4625,
            //        Longitude = -70.6684,
            //        Slug = "juan-perez"
            //    },
            //    new Account
            //    {
            //        Id = new Guid("6a7a9f3a-79ab-4641-87e3-8c2af4fa2c5a"),
            //        ObjectType = "user",
            //        DisplayName = "María González",
            //        Latitude = -33.4588,
            //        Longitude = -70.6506,
            //        Slug = "maria-gonzalez"
            //    },
            //    new Account
            //    {
            //        Id = new Guid("46b651ff-b95e-4a64-928a-073c2a858f3f"),
            //        ObjectType = "user",
            //        DisplayName = "Pedro Rodríguez",
            //        Latitude = -33.4714,
            //        Longitude = -70.6376,
            //        Slug = "pedro-rodriguez"
            //    },
            //    new Account
            //    {
            //        Id = new Guid("e9652272-3672-4071-972c-6f762277093f"),
            //        ObjectType = "user",
            //        DisplayName = "Ana Fernández",
            //        Latitude = -33.4393,
            //        Longitude = -70.6522,
            //        Slug = "ana-fernandez"
            //    },
            //    new Account
            //    {
            //        Id = new Guid("n38b058d6-e463-46f0-9e62-a0f9a4f44b64"),
            //        ObjectType = "user",
            //        DisplayName = "Luis Gómez",
            //        Latitude = -33.4434,
            //        Longitude = -70.6719,
            //        Slug = "luis-gomez"
            //    }
            //};
            var accounts = new List<Ultra.ActivityStream.Contracts.Account>();
            accounts.Add(new Ultra.ActivityStream.Contracts.Account
            {
                Id = new Guid("2d845728-6dff-4020-a841-69c84fdf8dfc"),
                ObjectType = "user",
                DisplayName = "Juan Pérez",
                Latitude = -33.4625,
                Longitude = -70.6684,
                Slug = "juan-perez"
            });

            accounts.Add(new Ultra.ActivityStream.Contracts.Account
            {
                Id = new Guid("6a7a9f3a-79ab-4641-87e3-8c2af4fa2c5a"),
                ObjectType = "user",
                DisplayName = "María González",
                Latitude = -33.4588,
                Longitude = -70.6506,
                Slug = "maria-gonzalez"
            });

            accounts.Add(new Ultra.ActivityStream.Contracts.Account
            {
                Id = new Guid("46b651ff-b95e-4a64-928a-073c2a858f3f"),
                ObjectType = "user",
                DisplayName = "Pedro Rodríguez",
                Latitude = -33.4714,
                Longitude = -70.6376,
                Slug = "pedro-rodriguez"
            });

            accounts.Add(new Ultra.ActivityStream.Contracts.Account
            {
                Id = new Guid("e9652272-3672-4071-972c-6f762277093f"),
                ObjectType = "user",
                DisplayName = "Ana Fernández",
                Latitude = -33.4393,
                Longitude = -70.6522,
                Slug = "ana-fernandez"
            });

            accounts.Add(new Ultra.ActivityStream.Contracts.Account
            {
                Id = new Guid("38b058d6-e463-46f0-9e62-a0f9a4f44b64"),
                ObjectType = "user",
                DisplayName = "Luis Gómez",
                Latitude = -33.4434,
                Longitude = -70.6719,
                Slug = "luis-gomez"
            });

            return accounts;
        }
        public static List<Ultra.ActivityStream.Contracts.Account> CreateAccountsNearBuenosAires()
        {
            var accounts = new List<Ultra.ActivityStream.Contracts.Account>
    {
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("c56d1a13-43b9-4519-93f3-c37ba79c8409"),
            ObjectType = "user",
            DisplayName = "Juan López",
            Latitude = -34.6037,
            Longitude = -58.3816,
            Slug = "juan-lopez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("38c227e0-1fda-459c-ace4-c4fd587395b4"),
            ObjectType = "user",
            DisplayName = "María García",
            Latitude = -34.6158,
            Longitude = -58.4333,
            Slug = "maria-garcia"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("e9c34ab5-4934-42a2-b410-a0515de1e864"),
            ObjectType = "user",
            DisplayName = "Pedro Martínez",
            Latitude = -34.5812,
            Longitude = -58.4059,
            Slug = "pedro-martinez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("063c76c8-5fb4-4416-92bd-6658fc42c71b"),
            ObjectType = "user",
            DisplayName = "Ana Rodríguez",
            Latitude = -34.6033,
            Longitude = -58.3724,
            Slug = "ana-rodriguez"
        },
        new Ultra.ActivityStream.Contracts.Account
        {
            Id = new Guid("5fa08483-b907-4c71-bc22-0054b438ceb3"),
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
