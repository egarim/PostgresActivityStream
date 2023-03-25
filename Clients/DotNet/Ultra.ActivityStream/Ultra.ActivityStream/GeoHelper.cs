using System;
using System.Linq;

namespace Ultra.ActivityStream
{
    
    /// <summary>
    /// A helper class for performing geographic calculations.
    /// </summary>
    public static class GeoHelper
    {
        private const double EarthRadiusInMeters = 6371000;

        /// <summary>
        /// Calculates the distance between two latitude and longitude coordinates using the Haversine formula.
        /// </summary>
        /// <param name="latitude1">The latitude of the first coordinate.</param>
        /// <param name="longitude1">The longitude of the first coordinate.</param>
        /// <param name="latitude2">The latitude of the second coordinate.</param>
        /// <param name="longitude2">The longitude of the second coordinate.</param>
        /// <returns>The distance between the two coordinates in meters.</returns>
        public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double latitude1InRadians = ConvertDegreesToRadians(latitude1);
            double latitude2InRadians = ConvertDegreesToRadians(latitude2);
            double longitude1InRadians = ConvertDegreesToRadians(longitude1);
            double longitude2InRadians = ConvertDegreesToRadians(longitude2);

            double latitudeDelta = latitude2InRadians - latitude1InRadians;
            double longitudeDelta = longitude2InRadians - longitude1InRadians;

            double a = Math.Pow(Math.Sin(latitudeDelta / 2), 2) + Math.Cos(latitude1InRadians) * Math.Cos(latitude2InRadians) * Math.Pow(Math.Sin(longitudeDelta / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusInMeters * c;
        }

        /// <summary>
        /// Determines if the distance between two latitude and longitude coordinates is within a specified radius in meters.
        /// </summary>
        /// <param name="latitude1">The latitude of the first coordinate.</param>
        /// <param name="longitude1">The longitude of the first coordinate.</param>
        /// <param name="latitude2">The latitude of the second coordinate.</param>
        /// <param name="longitude2">The longitude of the second coordinate.</param>
        /// <param name="radiusInMeters">The radius in meters to check if the coordinates are within.</param>
        /// <returns>True if the distance between the two coordinates is within the radius, otherwise false.</returns>
        public static bool IsWithinRadius(double latitude1, double longitude1, double latitude2, double longitude2, double radiusInMeters)
        {
            double distance = CalculateDistance(latitude1, longitude1, latitude2, longitude2);
            return distance <= radiusInMeters;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        private static double ConvertDegreesToRadians(double degrees)
        {
            return Math.PI / 180 * degrees;
        }
    }
}
