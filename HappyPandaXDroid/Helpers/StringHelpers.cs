using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using static HappyPandaXDroid.Core.Gallery;

namespace HappyPandaXDroid.Helpers
{
    public class StringHelpers
    {
        public static string NormalizeSortName(string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in input)
            {
                if (Char.IsUpper(c))
                {
                    sb.Append(" ");
                }
                sb.Append(c);
            }

            if (input != null && input.Length > 0 && Char.IsUpper(input[0]))
            {
                sb.Remove(0, 1);
            }

            String result = sb.ToString();
            return result;
        }

        public static string GetSortName(Sort sort)
        {
            switch (sort)
            {
                case Sort.ArtistName:
                    return "Artist";
                case Sort.CircleName:
                    return "Circle";
                case Sort.CollectionDate:
                    return "Collection Date";
                case Sort.CollectionGalleryCount:
                    return "Collection Gallery Count";
                case Sort.CollectionName:
                    return "Collection Name";
                case Sort.CollectionPublished:
                    return "Collection Published Date";
                case Sort.CollectionRandom:
                    return "Randon Collection";
                case Sort.GalleryArtist:
                    return "Gallery Artist";
                case Sort.GalleryDate:
                    return "Gallery Date";
                case Sort.GalleryPageCount:
                    return "Gallery Page Count";
                case Sort.GalleryPublished:
                    return "Gallery Published Date";
                case Sort.GalleryRandom:
                    return "Random Gallery";
                case Sort.GalleryRating:
                    return "Gallery Rating";
                case Sort.GalleryRead:
                    return "Gallery Read Status";
                case Sort.GalleryReadCount:
                    return "Gallery Read Count";
                case Sort.GalleryTitle:
                    return "Gallery Title";
                case Sort.GalleryUpdated:
                    return "Gallery Update Date";
                case Sort.NamespaceTagNamespace:
                    return "Namespace";
                case Sort.NamespaceTagTag:
                    return "Tag";
                case Sort.ParodyName:
                    return "Parody";
                default:
                    return "None";

            }
        }
    }
}