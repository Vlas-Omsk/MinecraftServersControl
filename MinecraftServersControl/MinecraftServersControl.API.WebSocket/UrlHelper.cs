using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftServersControl.API
{
    internal static class UrlHelper
    {
        public static bool CompareUrlSegments(string[] currentSegments, UrlSegment[] targetSegments)
        {
            var segmentsCount = Math.Min(currentSegments.Length, targetSegments.Length);

            if (targetSegments.Length > currentSegments.Length)
                return false;

            for (var i = 0; i < segmentsCount; i++)
            {
                var targetSegment = targetSegments[i];

                if (targetSegment.IsParameter)
                    continue;

                var currentSegment = currentSegments[i];

                if (targetSegment.Name != currentSegment)
                    return false;
            }

            return true;
        }

        public static IEnumerable<UrlSegment> GetTargetSegments(Uri uri)
        {
            return GetSegments(uri)
                .Select(x => new UrlSegment(x));
        }

        public static IEnumerable<string> GetSegments(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                return uri.OriginalString
                    .Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.ToLower());
            else
                return uri.Segments
                    .Where(x => x != "/")
                    .Select(x => x.TrimEnd('/').ToLower());
        }
    }
}
