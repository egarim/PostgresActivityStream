using System;
using System.Linq;

namespace Ultra.ActivityStream
{
    public class CityCoordinates
    {
        public static Location SantoDomingoLocation { get; private set; }
        public static Location SantiagoDeChileLocation { get; private set; }
        public static Location BuenosAiresLocation { get; private set; }
        public static Location GlendaleLocation { get; private set; }
        public static Location SanSalvadorLocation { get; private set; }
        public static Location HavanaLocation { get; private set; }

        static CityCoordinates()
        {
            SantoDomingoLocation = new Location(18.4861, -69.9312);
            SantiagoDeChileLocation = new Location(-33.4489, -70.6693);
            BuenosAiresLocation = new Location(-34.6037, -58.3816);
            GlendaleLocation = new Location(33.5387, -112.1860);
            SanSalvadorLocation = new Location(13.6930, -89.1910);
            HavanaLocation = new Location(23.1350, -82.3589);
        }
    }
}
