using System;
using System.Linq;

namespace Ultra.ActivityStream
{
    public class CityCoordinates
    {
        public static Ultra.ActivityStream.Contracts.Location SantoDomingoLocation { get; private set; }
        public static Ultra.ActivityStream.Contracts.Location SantiagoDeChileLocation { get; private set; }
        public static Ultra.ActivityStream.Contracts.Location BuenosAiresLocation { get; private set; }
        public static Ultra.ActivityStream.Contracts.Location GlendaleLocation { get; private set; }
        public static Ultra.ActivityStream.Contracts.Location SanSalvadorLocation { get; private set; }
        public static Ultra.ActivityStream.Contracts.Location HavanaLocation { get; private set; }

        static CityCoordinates()
        {
            SantoDomingoLocation = new Ultra.ActivityStream.Contracts.Location(18.4861, -69.9312);
            SantiagoDeChileLocation = new Ultra.ActivityStream.Contracts.Location(-33.4489, -70.6693);
            BuenosAiresLocation = new Ultra.ActivityStream.Contracts.Location(-34.6037, -58.3816);
            GlendaleLocation = new Ultra.ActivityStream.Contracts.Location(33.5387, -112.1860);
            SanSalvadorLocation = new Ultra.ActivityStream.Contracts.Location(13.6930, -89.1910);
            HavanaLocation = new Ultra.ActivityStream.Contracts.Location(23.1350, -82.3589);
        }
    }
}
